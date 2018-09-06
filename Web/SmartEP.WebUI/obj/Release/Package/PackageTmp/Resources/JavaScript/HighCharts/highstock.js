/*
 Highstock JS v1.3.4 (2013-08-02)

 (c) 2009-2013 Torstein Hønsi

 License: www.highcharts.com/license
*/
(function () {
    function v(a, b) { var c; a || (a = {}); for (c in b) a[c] = b[c]; return a } function x() { var a, b = arguments.length, c = {}, d = function (a, b) { var c, h; typeof a !== "object" && (a = {}); for (h in b) b.hasOwnProperty(h) && (c = b[h], a[h] = c && typeof c === "object" && Object.prototype.toString.call(c) !== "[object Array]" && typeof c.nodeType !== "number" ? d(a[h] || {}, c) : b[h]); return a }; for (a = 0; a < b; a++) c = d(c, arguments[a]); return c } function lb() { for (var a = 0, b = arguments, c = b.length, d = {}; a < c; a++) d[b[a++]] = b[a]; return d } function z(a, b) {
        return parseInt(a,
        b || 10)
    } function ja(a) { return typeof a === "string" } function ba(a) { return typeof a === "object" } function Wa(a) { return Object.prototype.toString.call(a) === "[object Array]" } function ra(a) { return typeof a === "number" } function sa(a) { return S.log(a) / S.LN10 } function ka(a) { return S.pow(10, a) } function la(a, b) { for (var c = a.length; c--;) if (a[c] === b) { a.splice(c, 1); break } } function t(a) { return a !== r && a !== null } function C(a, b, c) {
        var d, e; if (ja(b)) t(c) ? a.setAttribute(b, c) : a && a.getAttribute && (e = a.getAttribute(b)); else if (t(b) &&
        ba(b)) for (d in b) a.setAttribute(d, b[d]); return e
    } function ha(a) { return Wa(a) ? a : [a] } function o() { var a = arguments, b, c, d = a.length; for (b = 0; b < d; b++) if (c = a[b], typeof c !== "undefined" && c !== null) return c } function F(a, b) { if (ya && b && b.opacity !== r) b.filter = "alpha(opacity=" + b.opacity * 100 + ")"; v(a.style, b) } function Z(a, b, c, d, e) { a = G.createElement(a); b && v(a, b); e && F(a, { padding: 0, border: $, margin: 0 }); c && F(a, c); d && d.appendChild(a); return a } function ca(a, b) { var c = function () { }; c.prototype = new a; v(c.prototype, b); return c }
    function za(a, b, c, d) { var e = N.lang, a = +a || 0, f = b === -1 ? (a.toString().split(".")[1] || "").length : isNaN(b = T(b)) ? 2 : b, b = c === void 0 ? e.decimalPoint : c, d = d === void 0 ? e.thousandsSep : d, e = a < 0 ? "-" : "", c = String(z(a = T(a).toFixed(f))), g = c.length > 3 ? c.length % 3 : 0; return e + (g ? c.substr(0, g) + d : "") + c.substr(g).replace(/(\d{3})(?=\d)/g, "$1" + d) + (f ? b + T(a - c).toFixed(f).slice(2) : "") } function La(a, b) { return Array((b || 2) + 1 - String(a).length).join(0) + a } function ma(a, b, c) {
        var d = a[b]; a[b] = function () {
            var a = Array.prototype.slice.call(arguments);
            a.unshift(d); return c.apply(this, a)
        }
    } function Ma(a, b) { for (var c = "{", d = !1, e, f, g, h, i, k = []; (c = a.indexOf(c)) !== -1;) { e = a.slice(0, c); if (d) { f = e.split(":"); g = f.shift().split("."); i = g.length; e = b; for (h = 0; h < i; h++) e = e[g[h]]; if (f.length) f = f.join(":"), g = /\.([0-9])/, h = N.lang, i = void 0, /f$/.test(f) ? (i = (i = f.match(g)) ? i[1] : -1, e = za(e, i, h.decimalPoint, f.indexOf(",") > -1 ? h.thousandsSep : "")) : e = Aa(f, e) } k.push(e); a = a.slice(c + 1); c = (d = !d) ? "}" : "{" } k.push(a); return k.join("") } function xb(a) { return S.pow(10, U(S.log(a) / S.LN10)) }
    function yb(a, b, c, d) { var e, c = o(c, 1); e = a / c; b || (b = [1, 2, 2.5, 5, 10], d && d.allowDecimals === !1 && (c === 1 ? b = [1, 2, 5, 10] : c <= 0.1 && (b = [1 / c]))); for (d = 0; d < b.length; d++) if (a = b[d], e <= (b[d] + (b[d + 1] || b[d])) / 2) break; a *= c; return a } function zb(a, b) {
        var c = b || [[mb, [1, 2, 5, 10, 20, 25, 50, 100, 200, 500]], [eb, [1, 2, 5, 10, 15, 30]], [Xa, [1, 2, 5, 10, 15, 30]], [Ba, [1, 2, 3, 4, 6, 8, 12]], [fa, [1, 2]], [Na, [1, 2]], [Oa, [1, 2, 3, 4, 6]], [na, null]], d = c[c.length - 1], e = H[d[0]], f = d[1], g; for (g = 0; g < c.length; g++) if (d = c[g], e = H[d[0]], f = d[1], c[g + 1] && a <= (e * f[f.length - 1] +
        H[c[g + 1][0]]) / 2) break; e === H[na] && a < 5 * e && (f = [1, 2, 5]); e === H[na] && a < 5 * e && (f = [1, 2, 5]); c = yb(a / e, f, d[0] === na ? xb(a / e) : 1); return { unitRange: e, count: c, unitName: d[0] }
    } function fb(a, b, c, d) {
        var e = [], f = {}, g = N.global.useUTC, h, i = new Date(b), k = a.unitRange, j = a.count; if (t(b)) {
            k >= H[eb] && (i.setMilliseconds(0), i.setSeconds(k >= H[Xa] ? 0 : j * U(i.getSeconds() / j))); if (k >= H[Xa]) i[Nb](k >= H[Ba] ? 0 : j * U(i[Ab]() / j)); if (k >= H[Ba]) i[Ob](k >= H[fa] ? 0 : j * U(i[Bb]() / j)); if (k >= H[fa]) i[Cb](k >= H[Oa] ? 1 : j * U(i[Pa]() / j)); k >= H[Oa] && (i[Pb](k >= H[na] ? 0 :
            j * U(i[nb]() / j)), h = i[ob]()); k >= H[na] && (h -= h % j, i[Qb](h)); if (k === H[Na]) i[Cb](i[Pa]() - i[Db]() + o(d, 1)); b = 1; h = i[ob](); for (var d = i.getTime(), l = i[nb](), m = i[Pa](), n = g ? 0 : (864E5 + i.getTimezoneOffset() * 6E4) % 864E5; d < c;) e.push(d), k === H[na] ? d = pb(h + b * j, 0) : k === H[Oa] ? d = pb(h, l + b * j) : !g && (k === H[fa] || k === H[Na]) ? d = pb(h, l, m + b * j * (k === H[fa] ? 1 : 7)) : d += k * j, b++; e.push(d); q(Eb(e, function (a) { return k <= H[Ba] && a % H[fa] === n }), function (a) { f[a] = fa })
        } e.info = v(a, { higherRanks: f, totalRange: k * j }); return e
    } function Rb() {
        this.symbol = this.color =
        0
    } function Sb(a, b) { var c = a.length, d, e; for (e = 0; e < c; e++) a[e].ss_i = e; a.sort(function (a, c) { d = b(a, c); return d === 0 ? a.ss_i - c.ss_i : d }); for (e = 0; e < c; e++) delete a[e].ss_i } function Qa(a) { for (var b = a.length, c = a[0]; b--;) a[b] < c && (c = a[b]); return c } function ta(a) { for (var b = a.length, c = a[0]; b--;) a[b] > c && (c = a[b]); return c } function Ca(a, b) { for (var c in a) a[c] && a[c] !== b && a[c].destroy && a[c].destroy(), delete a[c] } function Ya(a) { qb || (qb = Z(Ra)); a && qb.appendChild(a); qb.innerHTML = "" } function Da(a, b) {
        var c = "Highcharts error #" +
        a + ": www.highcharts.com/errors/" + a; if (b) throw c; else X.console && console.log(c)
    } function oa(a) { return parseFloat(a.toPrecision(14)) } function Za(a, b) { Sa = o(a, b.animation) } function Tb() { var a = N.global.useUTC, b = a ? "getUTC" : "get", c = a ? "setUTC" : "set"; pb = a ? Date.UTC : function (a, b, c, g, h, i) { return (new Date(a, b, o(c, 1), o(g, 0), o(h, 0), o(i, 0))).getTime() }; Ab = b + "Minutes"; Bb = b + "Hours"; Db = b + "Day"; Pa = b + "Date"; nb = b + "Month"; ob = b + "FullYear"; Nb = c + "Minutes"; Ob = c + "Hours"; Cb = c + "Date"; Pb = c + "Month"; Qb = c + "FullYear" } function Ea() { }
    function $a(a, b, c, d) { this.axis = a; this.pos = b; this.type = c || ""; this.isNew = !0; !c && !d && this.addLabel() } function Fb(a, b) { this.axis = a; if (b) this.options = b, this.id = b.id } function Ub(a, b, c, d, e, f) {
        var g = a.chart.inverted; this.axis = a; this.isNegative = c; this.options = b; this.x = d; this.total = 0; this.points = {}; this.stack = e; this.percent = f === "percent"; this.alignOptions = { align: b.align || (g ? c ? "left" : "right" : "center"), verticalAlign: b.verticalAlign || (g ? "middle" : c ? "bottom" : "top"), y: o(b.y, g ? 4 : c ? 14 : -6), x: o(b.x, g ? c ? -6 : 6 : 0) }; this.textAlign =
        b.textAlign || (g ? c ? "right" : "left" : "center")
    } function ua() { this.init.apply(this, arguments) } function Gb() { this.init.apply(this, arguments) } function rb(a, b) { this.init(a, b) } function Hb(a, b) { this.init(a, b) } function Ta() { this.init.apply(this, arguments) } function Ib(a) {
        var b = a.options, c = b.navigator, d = c.enabled, b = b.scrollbar, e = b.enabled, f = d ? c.height : 0, g = e ? b.height : 0; this.handles = []; this.scrollbarButtons = []; this.elementsToDestroy = []; this.chart = a; this.setBaseSeries(); this.height = f; this.scrollbarHeight = g; this.scrollbarEnabled =
        e; this.navigatorEnabled = d; this.navigatorOptions = c; this.scrollbarOptions = b; this.outlineHeight = f + g; this.init()
    } function Jb(a) { this.init(a) } var r, G = document, X = window, S = Math, s = S.round, U = S.floor, pa = S.ceil, u = S.max, D = S.min, T = S.abs, da = S.cos, ia = S.sin, ab = S.PI, gb = ab * 2 / 360, Ua = navigator.userAgent, Vb = X.opera, ya = /msie/i.test(Ua) && !Vb, sb = G.documentMode === 8, tb = /AppleWebKit/.test(Ua), ub = /Firefox/.test(Ua), hb = /(Mobile|Android|Windows Phone)/.test(Ua), Fa = "http://www.w3.org/2000/svg", ea = !!G.createElementNS && !!G.createElementNS(Fa,
    "svg").createSVGRect, cc = ub && parseInt(Ua.split("Firefox/")[1], 10) < 4, ga = !ea && !ya && !!G.createElement("canvas").getContext, bb, ib = G.documentElement.ontouchstart !== r, Wb = {}, Kb = 0, qb, N, Aa, Sa, Lb, H, qa = function () { }, Va = [], Ra = "div", $ = "none", Xb = "rgba(192,192,192," + (ea ? 1.0E-4 : 0.002) + ")", mb = "millisecond", eb = "second", Xa = "minute", Ba = "hour", fa = "day", Na = "week", Oa = "month", na = "year", Yb = "stroke-width", pb, Ab, Bb, Db, Pa, nb, ob, Nb, Ob, Cb, Pb, Qb, L = {}; X.Highcharts = X.Highcharts ? Da(16, !0) : {}; Aa = function (a, b, c) {
        if (!t(b) || isNaN(b)) return "Invalid date";
        var a = o(a, "%Y-%m-%d %H:%M:%S"), d = new Date(b), e, f = d[Bb](), g = d[Db](), h = d[Pa](), i = d[nb](), k = d[ob](), j = N.lang, l = j.weekdays, d = v({ a: l[g].substr(0, 3), A: l[g], d: La(h), e: h, b: j.shortMonths[i], B: j.months[i], m: La(i + 1), y: k.toString().substr(2, 2), Y: k, H: La(f), I: La(f % 12 || 12), l: f % 12 || 12, M: La(d[Ab]()), p: f < 12 ? "AM" : "PM", P: f < 12 ? "am" : "pm", S: La(d.getSeconds()), L: La(s(b % 1E3), 3) }, Highcharts.dateFormats); for (e in d) for (; a.indexOf("%" + e) !== -1;) a = a.replace("%" + e, typeof d[e] === "function" ? d[e](b) : d[e]); return c ? a.substr(0, 1).toUpperCase() +
        a.substr(1) : a
    }; Rb.prototype = { wrapColor: function (a) { if (this.color >= a) this.color = 0 }, wrapSymbol: function (a) { if (this.symbol >= a) this.symbol = 0 } }; H = lb(mb, 1, eb, 1E3, Xa, 6E4, Ba, 36E5, fa, 864E5, Na, 6048E5, Oa, 26784E5, na, 31556952E3); Lb = {
        init: function (a, b, c) {
            var b = b || "", d = a.shift, e = b.indexOf("C") > -1, f = e ? 7 : 3, g, b = b.split(" "), c = [].concat(c), h, i, k = function (a) { for (g = a.length; g--;) a[g] === "M" && a.splice(g + 1, 0, a[g + 1], a[g + 2], a[g + 1], a[g + 2]) }; e && (k(b), k(c)); a.isArea && (h = b.splice(b.length - 6, 6), i = c.splice(c.length - 6, 6)); if (d <=
            c.length / f) for (; d--;) c = [].concat(c).splice(0, f).concat(c); a.shift = 0; if (b.length) for (a = c.length; b.length < a;) d = [].concat(b).splice(b.length - f, f), e && (d[f - 6] = d[f - 2], d[f - 5] = d[f - 1]), b = b.concat(d); h && (b = b.concat(h), c = c.concat(i)); return [b, c]
        }, step: function (a, b, c, d) { var e = [], f = a.length; if (c === 1) e = d; else if (f === b.length && c < 1) for (; f--;) d = parseFloat(a[f]), e[f] = isNaN(d) ? a[f] : c * parseFloat(b[f] - d) + d; else e = b; return e }
    }; (function (a) {
        X.HighchartsAdapter = X.HighchartsAdapter || a && {
            init: function (b) {
                var c = a.fx, d = c.step,
                e, f = a.Tween, g = f && f.propHooks; e = a.cssHooks.opacity; a.extend(a.easing, { easeOutQuad: function (a, b, c, d, e) { return -d * (b /= e) * (b - 2) + c } }); a.each(["cur", "_default", "width", "height", "opacity"], function (a, b) { var e = d, j, l; b === "cur" ? e = c.prototype : b === "_default" && f && (e = g[b], b = "set"); (j = e[b]) && (e[b] = function (c) { c = a ? c : this; l = c.elem; return l.attr ? l.attr(c.prop, b === "cur" ? r : c.now) : j.apply(this, arguments) }) }); ma(e, "get", function (a, b, c) { return b.attr ? b.opacity || 0 : a.call(this, b, c) }); e = function (a) {
                    var c = a.elem, d; if (!a.started) d =
                    b.init(c, c.d, c.toD), a.start = d[0], a.end = d[1], a.started = !0; c.attr("d", b.step(a.start, a.end, a.pos, c.toD))
                }; f ? g.d = { set: e } : d.d = e; this.each = Array.prototype.forEach ? function (a, b) { return Array.prototype.forEach.call(a, b) } : function (a, b) { for (var c = 0, d = a.length; c < d; c++) if (b.call(a[c], a[c], c, a) === !1) return c }; a.fn.highcharts = function () {
                    var a = "Chart", b = arguments, c, d; ja(b[0]) && (a = b[0], b = Array.prototype.slice.call(b, 1)); c = b[0]; if (c !== r) c.chart = c.chart || {}, c.chart.renderTo = this[0], new Highcharts[a](c, b[1]), d = this;
                    c === r && (d = Va[C(this[0], "data-highcharts-chart")]); return d
                }
            }, getScript: a.getScript, inArray: a.inArray, adapterRun: function (b, c) { return a(b)[c]() }, grep: a.grep, map: function (a, c) { for (var d = [], e = 0, f = a.length; e < f; e++) d[e] = c.call(a[e], a[e], e, a); return d }, offset: function (b) { return a(b).offset() }, addEvent: function (b, c, d) { a(b).bind(c, d) }, removeEvent: function (b, c, d) { var e = G.removeEventListener ? "removeEventListener" : "detachEvent"; G[e] && b && !b[e] && (b[e] = function () { }); a(b).unbind(c, d) }, fireEvent: function (b, c, d, e) {
                var f =
                a.Event(c), g = "detached" + c, h; !ya && d && (delete d.layerX, delete d.layerY); v(f, d); b[c] && (b[g] = b[c], b[c] = null); a.each(["preventDefault", "stopPropagation"], function (a, b) { var c = f[b]; f[b] = function () { try { c.call(f) } catch (a) { b === "preventDefault" && (h = !0) } } }); a(b).trigger(f); b[g] && (b[c] = b[g], b[g] = null); e && !f.isDefaultPrevented() && !h && e(f)
            }, washMouseEvent: function (a) { var c = a.originalEvent || a; if (c.pageX === r) c.pageX = a.pageX, c.pageY = a.pageY; return c }, animate: function (b, c, d) {
                var e = a(b); if (!b.style) b.style = {}; if (c.d) b.toD =
                c.d, c.d = 1; e.stop(); e.animate(c, d)
            }, stop: function (b) { a(b).stop() }
        }
    })(X.jQuery); var P = X.HighchartsAdapter, I = P || {}; P && P.init.call(P, Lb); var vb = I.adapterRun, dc = I.getScript, va = I.inArray, q = I.each, Eb = I.grep, ec = I.offset, Ga = I.map, E = I.addEvent, V = I.removeEvent, B = I.fireEvent, Zb = I.washMouseEvent, Mb = I.animate, jb = I.stop, I = { enabled: !0, x: 0, y: 15, style: { color: "#666", cursor: "default", fontSize: "11px", lineHeight: "14px" } }; N = {
        colors: "#2f7ed8,#0d233a,#8bbc21,#910000,#1aadce,#492970,#f28f43,#77a1e5,#c42525,#a6c96a".split(","),
        symbols: ["circle", "diamond", "square", "triangle", "triangle-down"], lang: { loading: "Loading...", months: "January,February,March,April,May,June,July,August,September,October,November,December".split(","), shortMonths: "Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec".split(","), weekdays: "Sunday,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday".split(","), decimalPoint: ".", numericSymbols: "k,M,G,T,P,E".split(","), resetZoom: "Reset zoom", resetZoomTitle: "Reset zoom level 1:1", thousandsSep: "," }, global: {
            useUTC: !0,
            canvasToolsURL: "http://code.highcharts.com/stock/1.3.4/modules/canvas-tools.js", VMLRadialGradientURL: "http://code.highcharts.com/stock/1.3.4/gfx/vml-radial-gradient.png"
        }, chart: {
            borderColor: "#4572A7", borderRadius: 5, defaultSeriesType: "line", ignoreHiddenSeries: !0, spacingTop: 10, spacingRight: 10, spacingBottom: 15, spacingLeft: 10, style: { fontFamily: '"Lucida Grande", "Lucida Sans Unicode", Verdana, Arial, Helvetica, sans-serif', fontSize: "12px" }, backgroundColor: "#FFFFFF", plotBorderColor: "#C0C0C0", resetZoomButton: {
                theme: { zIndex: 20 },
                position: { align: "right", x: -10, y: 10 }
            }
        }, title: { text: "Chart title", align: "center", margin: 15, style: { color: "#274b6d", fontSize: "16px" } }, subtitle: { text: "", align: "center", style: { color: "#4d759e" } }, plotOptions: {
            line: {
                allowPointSelect: !1, showCheckbox: !1, animation: { duration: 1E3 }, events: {}, lineWidth: 2, marker: { enabled: !0, lineWidth: 0, radius: 4, lineColor: "#FFFFFF", states: { hover: { enabled: !0 }, select: { fillColor: "#FFFFFF", lineColor: "#000000", lineWidth: 2 } } }, point: { events: {} }, dataLabels: x(I, {
                    align: "center", enabled: !1, formatter: function () {
                        return this.y ===
                        null ? "" : za(this.y, -1)
                    }, verticalAlign: "bottom", y: 0
                }), cropThreshold: 300, pointRange: 0, showInLegend: !0, states: { hover: { marker: {} }, select: { marker: {} } }, stickyTracking: !0
            }
        }, labels: { style: { position: "absolute", color: "#3E576F" } }, legend: {
            enabled: !0, align: "center", layout: "horizontal", labelFormatter: function () { return this.name }, borderWidth: 1, borderColor: "#909090", borderRadius: 5, navigation: { activeColor: "#274b6d", inactiveColor: "#CCC" }, shadow: !1, itemStyle: { cursor: "pointer", color: "#274b6d", fontSize: "12px" }, itemHoverStyle: { color: "#000" },
            itemHiddenStyle: { color: "#CCC" }, itemCheckboxStyle: { position: "absolute", width: "13px", height: "13px" }, symbolWidth: 16, symbolPadding: 5, verticalAlign: "bottom", x: 0, y: 0, title: { style: { fontWeight: "bold" } }
        }, loading: { labelStyle: { fontWeight: "bold", position: "relative", top: "1em" }, style: { position: "absolute", backgroundColor: "white", opacity: 0.5, textAlign: "center" } }, tooltip: {
            enabled: !0, animation: ea, backgroundColor: "rgba(255, 255, 255, .85)", borderWidth: 1, borderRadius: 3, dateTimeLabelFormats: {
                millisecond: "%A, %b %e, %H:%M:%S.%L",
                second: "%A, %b %e, %H:%M:%S", minute: "%A, %b %e, %H:%M", hour: "%A, %b %e, %H:%M", day: "%A, %b %e, %Y", week: "Week from %A, %b %e, %Y", month: "%B %Y", year: "%Y"
            }, headerFormat: '<span style="font-size: 10px">{point.key}</span><br/>', pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b><br/>', shadow: !0, snap: hb ? 25 : 10, style: { color: "#333333", cursor: "default", fontSize: "12px", padding: "8px", whiteSpace: "nowrap" }
        }, credits: {
            enabled: !0, text: "Highcharts.com", href: "http://www.highcharts.com",
            position: { align: "right", x: -10, verticalAlign: "bottom", y: -5 }, style: { cursor: "pointer", color: "#909090", fontSize: "9px" }
        }
    }; var Q = N.plotOptions, P = Q.line; Tb(); var wa = function (a) {
        var b = [], c, d; (function (a) {
            a && a.stops ? d = Ga(a.stops, function (a) { return wa(a[1]) }) : (c = /rgba\(\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*,\s*([0-9]?(?:\.[0-9]+)?)\s*\)/.exec(a)) ? b = [z(c[1]), z(c[2]), z(c[3]), parseFloat(c[4], 10)] : (c = /#([a-fA-F0-9]{2})([a-fA-F0-9]{2})([a-fA-F0-9]{2})/.exec(a)) ? b = [z(c[1], 16), z(c[2], 16), z(c[3],
            16), 1] : (c = /rgb\(\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*\)/.exec(a)) && (b = [z(c[1]), z(c[2]), z(c[3]), 1])
        })(a); return {
            get: function (c) { var f; d ? (f = x(a), f.stops = [].concat(f.stops), q(d, function (a, b) { f.stops[b] = [f.stops[b][0], a.get(c)] })) : f = b && !isNaN(b[0]) ? c === "rgb" ? "rgb(" + b[0] + "," + b[1] + "," + b[2] + ")" : c === "a" ? b[3] : "rgba(" + b.join(",") + ")" : a; return f }, brighten: function (a) {
                if (d) q(d, function (b) { b.brighten(a) }); else if (ra(a) && a !== 0) {
                    var c; for (c = 0; c < 3; c++) b[c] += z(a * 255), b[c] < 0 && (b[c] = 0), b[c] > 255 &&
                    (b[c] = 255)
                } return this
            }, rgba: b, setOpacity: function (a) { b[3] = a; return this }
        }
    }; Ea.prototype = {
        init: function (a, b) { this.element = b === "span" ? Z(b) : G.createElementNS(Fa, b); this.renderer = a; this.attrSetters = {} }, opacity: 1, animate: function (a, b, c) { b = o(b, Sa, !0); jb(this); if (b) { b = x(b); if (c) b.complete = c; Mb(this, a, b) } else this.attr(a), c && c() }, attr: function (a, b) {
            var c, d, e, f, g = this.element, h = g.nodeName.toLowerCase(), i = this.renderer, k, j = this.attrSetters, l = this.shadows, m, n, p = this; ja(a) && t(b) && (c = a, a = {}, a[c] = b); if (ja(a)) c =
            a, h === "circle" ? c = { x: "cx", y: "cy" }[c] || c : c === "strokeWidth" && (c = "stroke-width"), p = C(g, c) || this[c] || 0, c !== "d" && c !== "visibility" && (p = parseFloat(p)); else {
                for (c in a) if (k = !1, d = a[c], e = j[c] && j[c].call(this, d, c), e !== !1) {
                    e !== r && (d = e); if (c === "d") d && d.join && (d = d.join(" ")), /(NaN| {2}|^$)/.test(d) && (d = "M 0 0"); else if (c === "x" && h === "text") for (e = 0; e < g.childNodes.length; e++) f = g.childNodes[e], C(f, "x") === C(g, "x") && C(f, "x", d); else if (this.rotation && (c === "x" || c === "y")) n = !0; else if (c === "fill") d = i.color(d, g, c); else if (h ===
                    "circle" && (c === "x" || c === "y")) c = { x: "cx", y: "cy" }[c] || c; else if (h === "rect" && c === "r") C(g, { rx: d, ry: d }), k = !0; else if (c === "translateX" || c === "translateY" || c === "rotation" || c === "verticalAlign" || c === "scaleX" || c === "scaleY") k = n = !0; else if (c === "stroke") d = i.color(d, g, c); else if (c === "dashstyle") if (c = "stroke-dasharray", d = d && d.toLowerCase(), d === "solid") d = $; else {
                        if (d) {
                            d = d.replace("shortdashdotdot", "3,1,1,1,1,1,").replace("shortdashdot", "3,1,1,1").replace("shortdot", "1,1,").replace("shortdash", "3,1,").replace("longdash",
                            "8,3,").replace(/dot/g, "1,3,").replace("dash", "4,3,").replace(/,$/, "").split(","); for (e = d.length; e--;) d[e] = z(d[e]) * o(a["stroke-width"], this["stroke-width"]); d = d.join(",")
                        }
                    } else if (c === "width") d = z(d); else if (c === "align") c = "text-anchor", d = { left: "start", center: "middle", right: "end" }[d]; else if (c === "title") e = g.getElementsByTagName("title")[0], e || (e = G.createElementNS(Fa, "title"), g.appendChild(e)), e.textContent = d; c === "strokeWidth" && (c = "stroke-width"); if (c === "stroke-width" || c === "stroke") {
                        this[c] = d; if (this.stroke &&
                        this["stroke-width"]) C(g, "stroke", this.stroke), C(g, "stroke-width", this["stroke-width"]), this.hasStroke = !0; else if (c === "stroke-width" && d === 0 && this.hasStroke) g.removeAttribute("stroke"), this.hasStroke = !1; k = !0
                    } this.symbolName && /^(x|y|width|height|r|start|end|innerR|anchorX|anchorY)/.test(c) && (m || (this.symbolAttr(a), m = !0), k = !0); if (l && /^(width|height|visibility|x|y|d|transform|cx|cy|r)$/.test(c)) for (e = l.length; e--;) C(l[e], c, c === "height" ? u(d - (l[e].cutHeight || 0), 0) : d); if ((c === "width" || c === "height") && h ===
                    "rect" && d < 0) d = 0; this[c] = d; c === "text" ? (d !== this.textStr && delete this.bBox, this.textStr = d, this.added && i.buildText(this)) : k || C(g, c, d)
                } n && this.updateTransform()
            } return p
        }, addClass: function (a) { var b = this.element, c = C(b, "class") || ""; c.indexOf(a) === -1 && C(b, "class", c + " " + a); return this }, symbolAttr: function (a) { var b = this; q("x,y,r,start,end,width,height,innerR,anchorX,anchorY".split(","), function (c) { b[c] = o(a[c], b[c]) }); b.attr({ d: b.renderer.symbols[b.symbolName](b.x, b.y, b.width, b.height, b) }) }, clip: function (a) {
            return this.attr("clip-path",
            a ? "url(" + this.renderer.url + "#" + a.id + ")" : $)
        }, crisp: function (a, b, c, d, e) { var f, g = {}, h = {}, i, a = a || this.strokeWidth || this.attr && this.attr("stroke-width") || 0; i = s(a) % 2 / 2; h.x = U(b || this.x || 0) + i; h.y = U(c || this.y || 0) + i; h.width = U((d || this.width || 0) - 2 * i); h.height = U((e || this.height || 0) - 2 * i); h.strokeWidth = a; for (f in h) this[f] !== h[f] && (this[f] = g[f] = h[f]); return g }, css: function (a) {
            var b = this.element, c = a && a.width && b.nodeName.toLowerCase() === "text", d, e = "", f = function (a, b) { return "-" + b.toLowerCase() }; if (a && a.color) a.fill =
            a.color; this.styles = a = v(this.styles, a); ga && c && delete a.width; if (ya && !ea) c && delete a.width, F(this.element, a); else { for (d in a) e += d.replace(/([A-Z])/g, f) + ":" + a[d] + ";"; C(b, "style", e) } c && this.added && this.renderer.buildText(this); return this
        }, on: function (a, b) { var c = this.element; if (ib && a === "click") c.ontouchstart = function (a) { a.preventDefault(); b.call(c, a) }; c["on" + a] = b; return this }, setRadialReference: function (a) { this.element.radialReference = a; return this }, translate: function (a, b) {
            return this.attr({
                translateX: a,
                translateY: b
            })
        }, invert: function () { this.inverted = !0; this.updateTransform(); return this }, htmlCss: function (a) { var b = this.element; if (b = a && b.tagName === "SPAN" && a.width) delete a.width, this.textWidth = b, this.updateTransform(); this.styles = v(this.styles, a); F(this.element, a); return this }, htmlGetBBox: function () { var a = this.element, b = this.bBox; if (!b) { if (a.nodeName === "text") a.style.position = "absolute"; b = this.bBox = { x: a.offsetLeft, y: a.offsetTop, width: a.offsetWidth, height: a.offsetHeight } } return b }, htmlUpdateTransform: function () {
            if (this.added) {
                var a =
                this.renderer, b = this.element, c = this.translateX || 0, d = this.translateY || 0, e = this.x || 0, f = this.y || 0, g = this.textAlign || "left", h = { left: 0, center: 0.5, right: 1 }[g], i = g && g !== "left", k = this.shadows; F(b, { marginLeft: c, marginTop: d }); k && q(k, function (a) { F(a, { marginLeft: c + 1, marginTop: d + 1 }) }); this.inverted && q(b.childNodes, function (c) { a.invertChild(c, b) }); if (b.tagName === "SPAN") {
                    var j, l, k = this.rotation, m; j = 0; var n = 1, p = 0, K; m = z(this.textWidth); var y = this.xCorr || 0, w = this.yCorr || 0, M = [k, g, b.innerHTML, this.textWidth].join(",");
                    if (M !== this.cTT) { t(k) && (j = k * gb, n = da(j), p = ia(j), this.setSpanRotation(k, p, n)); j = o(this.elemWidth, b.offsetWidth); l = o(this.elemHeight, b.offsetHeight); if (j > m && /[ \-]/.test(b.textContent || b.innerText)) F(b, { width: m + "px", display: "block", whiteSpace: "normal" }), j = m; m = a.fontMetrics(b.style.fontSize).b; y = n < 0 && -j; w = p < 0 && -l; K = n * p < 0; y += p * m * (K ? 1 - h : h); w -= n * m * (k ? K ? h : 1 - h : 1); i && (y -= j * h * (n < 0 ? -1 : 1), k && (w -= l * h * (p < 0 ? -1 : 1)), F(b, { textAlign: g })); this.xCorr = y; this.yCorr = w } F(b, { left: e + y + "px", top: f + w + "px" }); if (tb) l = b.offsetHeight;
                    this.cTT = M
                }
            } else this.alignOnAdd = !0
        }, setSpanRotation: function (a) { var b = {}; b[ya ? "-ms-transform" : tb ? "-webkit-transform" : ub ? "MozTransform" : Vb ? "-o-transform" : ""] = b.transform = "rotate(" + a + "deg)"; F(this.element, b) }, updateTransform: function () {
            var a = this.translateX || 0, b = this.translateY || 0, c = this.scaleX, d = this.scaleY, e = this.inverted, f = this.rotation; e && (a += this.attr("width"), b += this.attr("height")); a = ["translate(" + a + "," + b + ")"]; e ? a.push("rotate(90) scale(-1,1)") : f && a.push("rotate(" + f + " " + (this.x || 0) + " " + (this.y ||
            0) + ")"); (t(c) || t(d)) && a.push("scale(" + o(c, 1) + " " + o(d, 1) + ")"); a.length && C(this.element, "transform", a.join(" "))
        }, toFront: function () { var a = this.element; a.parentNode.appendChild(a); return this }, align: function (a, b, c) {
            var d, e, f, g, h = {}; e = this.renderer; f = e.alignedObjects; if (a) { if (this.alignOptions = a, this.alignByTranslate = b, !c || ja(c)) this.alignTo = d = c || "renderer", la(f, this), f.push(this), c = null } else a = this.alignOptions, b = this.alignByTranslate, d = this.alignTo; c = o(c, e[d], e); d = a.align; e = a.verticalAlign; f = (c.x ||
            0) + (a.x || 0); g = (c.y || 0) + (a.y || 0); if (d === "right" || d === "center") f += (c.width - (a.width || 0)) / { right: 1, center: 2 }[d]; h[b ? "translateX" : "x"] = s(f); if (e === "bottom" || e === "middle") g += (c.height - (a.height || 0)) / ({ bottom: 1, middle: 2 }[e] || 1); h[b ? "translateY" : "y"] = s(g); this[this.placed ? "animate" : "attr"](h); this.placed = !0; this.alignAttr = h; return this
        }, getBBox: function () {
            var a = this.bBox, b = this.renderer, c, d = this.rotation; c = this.element; var e = this.styles, f = d * gb; if (!a) {
                if (c.namespaceURI === Fa || b.forExport) {
                    try {
                        a = c.getBBox ?
                        v({}, c.getBBox()) : { width: c.offsetWidth, height: c.offsetHeight }
                    } catch (g) { } if (!a || a.width < 0) a = { width: 0, height: 0 }
                } else a = this.htmlGetBBox(); if (b.isSVG) { b = a.width; c = a.height; if (ya && e && e.fontSize === "11px" && c.toPrecision(3) === "22.7") a.height = c = 14; if (d) a.width = T(c * ia(f)) + T(b * da(f)), a.height = T(c * da(f)) + T(b * ia(f)) } this.bBox = a
            } return a
        }, show: function () { return this.attr({ visibility: "visible" }) }, hide: function () { return this.attr({ visibility: "hidden" }) }, fadeOut: function (a) {
            var b = this; b.animate({ opacity: 0 }, {
                duration: a ||
                150, complete: function () { b.hide() }
            })
        }, add: function (a) { var b = this.renderer, c = a || b, d = c.element || b.box, e = d.childNodes, f = this.element, g = C(f, "zIndex"), h; if (a) this.parentGroup = a; this.parentInverted = a && a.inverted; this.textStr !== void 0 && b.buildText(this); if (g) c.handleZ = !0, g = z(g); if (c.handleZ) for (c = 0; c < e.length; c++) if (a = e[c], b = C(a, "zIndex"), a !== f && (z(b) > g || !t(g) && t(b))) { d.insertBefore(f, a); h = !0; break } h || d.appendChild(f); this.added = !0; B(this, "add"); return this }, safeRemoveChild: function (a) {
            var b = a.parentNode;
            b && b.removeChild(a)
        }, destroy: function () {
            var a = this, b = a.element || {}, c = a.shadows, d = a.renderer.isSVG && b.nodeName === "SPAN" && b.parentNode, e, f; b.onclick = b.onmouseout = b.onmouseover = b.onmousemove = b.point = null; jb(a); if (a.clipPath) a.clipPath = a.clipPath.destroy(); if (a.stops) { for (f = 0; f < a.stops.length; f++) a.stops[f] = a.stops[f].destroy(); a.stops = null } a.safeRemoveChild(b); for (c && q(c, function (b) { a.safeRemoveChild(b) }) ; d && d.childNodes.length === 0;) b = d.parentNode, a.safeRemoveChild(d), d = b; a.alignTo && la(a.renderer.alignedObjects,
            a); for (e in a) delete a[e]; return null
        }, shadow: function (a, b, c) {
            var d = [], e, f, g = this.element, h, i, k, j; if (a) {
                i = o(a.width, 3); k = (a.opacity || 0.15) / i; j = this.parentInverted ? "(-1,-1)" : "(" + o(a.offsetX, 1) + ", " + o(a.offsetY, 1) + ")"; for (e = 1; e <= i; e++) { f = g.cloneNode(0); h = i * 2 + 1 - 2 * e; C(f, { isShadow: "true", stroke: a.color || "black", "stroke-opacity": k * e, "stroke-width": h, transform: "translate" + j, fill: $ }); if (c) C(f, "height", u(C(f, "height") - h, 0)), f.cutHeight = h; b ? b.element.appendChild(f) : g.parentNode.insertBefore(f, g); d.push(f) } this.shadows =
                d
            } return this
        }
    }; var Ha = function () { this.init.apply(this, arguments) }; Ha.prototype = {
        Element: Ea, init: function (a, b, c, d) {
            var e = location, f, g; f = this.createElement("svg").attr({ version: "1.1" }); g = f.element; a.appendChild(g); a.innerHTML.indexOf("xmlns") === -1 && C(g, "xmlns", Fa); this.isSVG = !0; this.box = g; this.boxWrapper = f; this.alignedObjects = []; this.url = (ub || tb) && G.getElementsByTagName("base").length ? e.href.replace(/#.*?$/, "").replace(/([\('\)])/g, "\\$1").replace(/ /g, "%20") : ""; this.createElement("desc").add().element.appendChild(G.createTextNode("Created with Highstock 1.3.4"));
            this.defs = this.createElement("defs").add(); this.forExport = d; this.gradients = {}; this.setSize(b, c, !1); var h; if (ub && a.getBoundingClientRect) this.subPixelFix = b = function () { F(a, { left: 0, top: 0 }); h = a.getBoundingClientRect(); F(a, { left: pa(h.left) - h.left + "px", top: pa(h.top) - h.top + "px" }) }, b(), E(X, "resize", b)
        }, isHidden: function () { return !this.boxWrapper.getBBox().width }, destroy: function () {
            var a = this.defs; this.box = null; this.boxWrapper = this.boxWrapper.destroy(); Ca(this.gradients || {}); this.gradients = null; if (a) this.defs =
            a.destroy(); this.subPixelFix && V(X, "resize", this.subPixelFix); return this.alignedObjects = null
        }, createElement: function (a) { var b = new this.Element; b.init(this, a); return b }, draw: function () { }, buildText: function (a) {
            for (var b = a.element, c = this, d = c.forExport, e = o(a.textStr, "").toString().replace(/<(b|strong)>/g, '<span style="font-weight:bold">').replace(/<(i|em)>/g, '<span style="font-style:italic">').replace(/<a/g, "<span").replace(/<\/(b|strong|i|em|a)>/g, "</span>").split(/<br.*?>/g), f = b.childNodes, g = /style="([^"]+)"/,
            h = /href="(http[^"]+)"/, i = C(b, "x"), k = a.styles, j = k && k.width && z(k.width), l = k && k.lineHeight, m = f.length; m--;) b.removeChild(f[m]); j && !a.added && this.box.appendChild(b); e[e.length - 1] === "" && e.pop(); q(e, function (e, f) {
                var m, y = 0, e = e.replace(/<span/g, "|||<span").replace(/<\/span>/g, "</span>|||"); m = e.split("|||"); q(m, function (e) {
                    if (e !== "" || m.length === 1) {
                        var n = {}, o = G.createElementNS(Fa, "tspan"), q; g.test(e) && (q = e.match(g)[1].replace(/(;| |^)color([ :])/, "$1fill$2"), C(o, "style", q)); h.test(e) && !d && (C(o, "onclick", 'location.href="' +
                        e.match(h)[1] + '"'), F(o, { cursor: "pointer" })); e = (e.replace(/<(.|\n)*?>/g, "") || " ").replace(/&lt;/g, "<").replace(/&gt;/g, ">"); if (e !== " " && (o.appendChild(G.createTextNode(e)), y ? n.dx = 0 : n.x = i, C(o, n), !y && f && (!ea && d && F(o, { display: "block" }), C(o, "dy", l || c.fontMetrics(/px$/.test(o.style.fontSize) ? o.style.fontSize : k.fontSize).h, tb && o.offsetHeight)), b.appendChild(o), y++, j)) for (var e = e.replace(/([^\^])-/g, "$1- ").split(" "), r, A = []; e.length || A.length;) delete a.bBox, r = a.getBBox().width, n = r > j, !n || e.length === 1 ? (e =
                        A, A = [], e.length && (o = G.createElementNS(Fa, "tspan"), C(o, { dy: l || 16, x: i }), q && C(o, "style", q), b.appendChild(o), r > j && (j = r))) : (o.removeChild(o.firstChild), A.unshift(e.pop())), e.length && o.appendChild(G.createTextNode(e.join(" ").replace(/- /g, "-")))
                    }
                })
            })
        }, button: function (a, b, c, d, e, f, g) {
            var h = this.label(a, b, c, null, null, null, null, null, "button"), i = 0, k, j, l, m, n, a = { x1: 0, y1: 0, x2: 0, y2: 1 }, e = x({ "stroke-width": 1, stroke: "#CCCCCC", fill: { linearGradient: a, stops: [[0, "#FEFEFE"], [1, "#F6F6F6"]] }, r: 2, padding: 5, style: { color: "black" } },
            e); l = e.style; delete e.style; f = x(e, { stroke: "#68A", fill: { linearGradient: a, stops: [[0, "#FFF"], [1, "#ACF"]] } }, f); m = f.style; delete f.style; g = x(e, { stroke: "#68A", fill: { linearGradient: a, stops: [[0, "#9BD"], [1, "#CDF"]] } }, g); n = g.style; delete g.style; E(h.element, ya ? "mouseover" : "mouseenter", function () { h.attr(f).css(m) }); E(h.element, ya ? "mouseout" : "mouseleave", function () { k = [e, f, g][i]; j = [l, m, n][i]; h.attr(k).css(j) }); h.setState = function (a) { (i = a) ? a === 2 && h.attr(g).css(n) : h.attr(e).css(l) }; return h.on("click", function () { d.call(h) }).attr(e).css(v({ cursor: "default" },
            l))
        }, crispLine: function (a, b) { a[1] === a[4] && (a[1] = a[4] = s(a[1]) - b % 2 / 2); a[2] === a[5] && (a[2] = a[5] = s(a[2]) + b % 2 / 2); return a }, path: function (a) { var b = { fill: $ }; Wa(a) ? b.d = a : ba(a) && v(b, a); return this.createElement("path").attr(b) }, circle: function (a, b, c) { a = ba(a) ? a : { x: a, y: b, r: c }; return this.createElement("circle").attr(a) }, arc: function (a, b, c, d, e, f) { if (ba(a)) b = a.y, c = a.r, d = a.innerR, e = a.start, f = a.end, a = a.x; a = this.symbol("arc", a || 0, b || 0, c || 0, c || 0, { innerR: d || 0, start: e || 0, end: f || 0 }); a.r = c; return a }, rect: function (a, b,
        c, d, e, f) { e = ba(a) ? a.r : e; e = this.createElement("rect").attr({ rx: e, ry: e, fill: $ }); return e.attr(ba(a) ? a : e.crisp(f, a, b, u(c, 0), u(d, 0))) }, setSize: function (a, b, c) { var d = this.alignedObjects, e = d.length; this.width = a; this.height = b; for (this.boxWrapper[o(c, !0) ? "animate" : "attr"]({ width: a, height: b }) ; e--;) d[e].align() }, g: function (a) { var b = this.createElement("g"); return t(a) ? b.attr({ "class": "highcharts-" + a }) : b }, image: function (a, b, c, d, e) {
            var f = { preserveAspectRatio: $ }; arguments.length > 1 && v(f, { x: b, y: c, width: d, height: e });
            f = this.createElement("image").attr(f); f.element.setAttributeNS ? f.element.setAttributeNS("http://www.w3.org/1999/xlink", "href", a) : f.element.setAttribute("hc-svg-href", a); return f
        }, symbol: function (a, b, c, d, e, f) {
            var g, h = this.symbols[a], h = h && h(s(b), s(c), d, e, f), i = /^url\((.*?)\)$/, k, j; if (h) g = this.path(h), v(g, { symbolName: a, x: b, y: c, width: d, height: e }), f && v(g, f); else if (i.test(a)) j = function (a, b) { a.element && (a.attr({ width: b[0], height: b[1] }), a.alignByTranslate || a.translate(s((d - b[0]) / 2), s((e - b[1]) / 2))) }, k = a.match(i)[1],
            a = Wb[k], g = this.image(k).attr({ x: b, y: c }), g.isImg = !0, a ? j(g, a) : (g.attr({ width: 0, height: 0 }), Z("img", { onload: function () { j(g, Wb[k] = [this.width, this.height]) }, src: k })); return g
        }, symbols: {
            circle: function (a, b, c, d) { var e = 0.166 * c; return ["M", a + c / 2, b, "C", a + c + e, b, a + c + e, b + d, a + c / 2, b + d, "C", a - e, b + d, a - e, b, a + c / 2, b, "Z"] }, square: function (a, b, c, d) { return ["M", a, b, "L", a + c, b, a + c, b + d, a, b + d, "Z"] }, triangle: function (a, b, c, d) { return ["M", a + c / 2, b, "L", a + c, b + d, a, b + d, "Z"] }, "triangle-down": function (a, b, c, d) {
                return ["M", a, b, "L", a + c, b,
                a + c / 2, b + d, "Z"]
            }, diamond: function (a, b, c, d) { return ["M", a + c / 2, b, "L", a + c, b + d / 2, a + c / 2, b + d, a, b + d / 2, "Z"] }, arc: function (a, b, c, d, e) { var f = e.start, c = e.r || c || d, g = e.end - 0.001, d = e.innerR, h = e.open, i = da(f), k = ia(f), j = da(g), g = ia(g), e = e.end - f < ab ? 0 : 1; return ["M", a + c * i, b + c * k, "A", c, c, 0, e, 1, a + c * j, b + c * g, h ? "M" : "L", a + d * j, b + d * g, "A", d, d, 0, e, 0, a + d * i, b + d * k, h ? "" : "Z"] }
        }, clipRect: function (a, b, c, d) {
            var e = "highcharts-" + Kb++, f = this.createElement("clipPath").attr({ id: e }).add(this.defs), a = this.rect(a, b, c, d, 0).add(f); a.id = e; a.clipPath =
            f; return a
        }, color: function (a, b, c) {
            var d = this, e, f = /^rgba/, g, h, i, k, j, l, m, n = []; a && a.linearGradient ? g = "linearGradient" : a && a.radialGradient && (g = "radialGradient"); if (g) {
                c = a[g]; h = d.gradients; k = a.stops; b = b.radialReference; Wa(c) && (a[g] = c = { x1: c[0], y1: c[1], x2: c[2], y2: c[3], gradientUnits: "userSpaceOnUse" }); g === "radialGradient" && b && !t(c.gradientUnits) && (c = x(c, { cx: b[0] - b[2] / 2 + c.cx * b[2], cy: b[1] - b[2] / 2 + c.cy * b[2], r: c.r * b[2], gradientUnits: "userSpaceOnUse" })); for (m in c) m !== "id" && n.push(m, c[m]); for (m in k) n.push(k[m]);
                n = n.join(","); h[n] ? a = h[n].id : (c.id = a = "highcharts-" + Kb++, h[n] = i = d.createElement(g).attr(c).add(d.defs), i.stops = [], q(k, function (a) { f.test(a[1]) ? (e = wa(a[1]), j = e.get("rgb"), l = e.get("a")) : (j = a[1], l = 1); a = d.createElement("stop").attr({ offset: a[0], "stop-color": j, "stop-opacity": l }).add(i); i.stops.push(a) })); return "url(" + d.url + "#" + a + ")"
            } else return f.test(a) ? (e = wa(a), C(b, c + "-opacity", e.get("a")), e.get("rgb")) : (b.removeAttribute(c + "-opacity"), a)
        }, text: function (a, b, c, d) {
            var e = N.chart.style, f = ga || !ea && this.forExport;
            if (d && !this.forExport) return this.html(a, b, c); b = s(o(b, 0)); c = s(o(c, 0)); a = this.createElement("text").attr({ x: b, y: c, text: a }).css({ fontFamily: e.fontFamily, fontSize: e.fontSize }); f && a.css({ position: "absolute" }); a.x = b; a.y = c; return a
        }, html: function (a, b, c) {
            var d = N.chart.style, e = this.createElement("span"), f = e.attrSetters, g = e.element, h = e.renderer; f.text = function (a) { a !== g.innerHTML && delete this.bBox; g.innerHTML = a; return !1 }; f.x = f.y = f.align = function (a, b) {
                b === "align" && (b = "textAlign"); e[b] = a; e.htmlUpdateTransform();
                return !1
            }; e.attr({ text: a, x: s(b), y: s(c) }).css({ position: "absolute", whiteSpace: "nowrap", fontFamily: d.fontFamily, fontSize: d.fontSize }); e.css = e.htmlCss; if (h.isSVG) e.add = function (a) {
                var b, c = h.box.parentNode, d = []; if (a) {
                    if (b = a.div, !b) {
                        for (; a;) d.push(a), a = a.parentGroup; q(d.reverse(), function (a) {
                            var d; b = a.div = a.div || Z(Ra, { className: C(a.element, "class") }, { position: "absolute", left: (a.translateX || 0) + "px", top: (a.translateY || 0) + "px" }, b || c); d = b.style; v(a.attrSetters, {
                                translateX: function (a) { d.left = a + "px" }, translateY: function (a) {
                                    d.top =
                                    a + "px"
                                }, visibility: function (a, b) { d[b] = a }
                            })
                        })
                    }
                } else b = c; b.appendChild(g); e.added = !0; e.alignOnAdd && e.htmlUpdateTransform(); return e
            }; return e
        }, fontMetrics: function (a) { var a = z(a || 11), a = a < 24 ? a + 4 : s(a * 1.2), b = s(a * 0.8); return { h: a, b: b } }, label: function (a, b, c, d, e, f, g, h, i) {
            function k() {
                var a, b; a = K.element.style; w = (cb === void 0 || A === void 0 || p.styles.textAlign) && K.getBBox(); p.width = (cb || w.width || 0) + 2 * aa + u; p.height = (A || w.height || 0) + 2 * aa; D = aa + n.fontMetrics(a && a.fontSize).b; if (z) {
                    if (!o) a = s(-M * aa), b = h ? -D : 0, p.box = o =
                    d ? n.symbol(d, a, b, p.width, p.height) : n.rect(a, b, p.width, p.height, 0, db[Yb]), o.add(p); o.isImg || o.attr(x({ width: p.width, height: p.height }, db)); db = null
                }
            } function j() { var a = p.styles, a = a && a.textAlign, b = u + aa * (1 - M), c; c = h ? 0 : D; if (t(cb) && (a === "center" || a === "right")) b += { center: 0.5, right: 1 }[a] * (cb - w.width); (b !== K.x || c !== K.y) && K.attr({ x: b, y: c }); K.x = b; K.y = c } function l(a, b) { o ? o.attr(a, b) : db[a] = b } function m() { K.add(p); p.attr({ text: a, x: b, y: c }); o && t(e) && p.attr({ anchorX: e, anchorY: f }) } var n = this, p = n.g(i), K = n.text("", 0, 0,
            g).attr({ zIndex: 1 }), o, w, M = 0, aa = 3, u = 0, cb, A, O, xa, J = 0, db = {}, D, g = p.attrSetters, z; E(p, "add", m); g.width = function (a) { cb = a; return !1 }; g.height = function (a) { A = a; return !1 }; g.padding = function (a) { t(a) && a !== aa && (aa = a, j()); return !1 }; g.paddingLeft = function (a) { t(a) && a !== u && (u = a, j()); return !1 }; g.align = function (a) { M = { left: 0, center: 0.5, right: 1 }[a]; return !1 }; g.text = function (a, b) { K.attr(b, a); k(); j(); return !1 }; g[Yb] = function (a, b) { z = !0; J = a % 2 / 2; l(b, a); return !1 }; g.stroke = g.fill = g.r = function (a, b) { b === "fill" && (z = !0); l(b, a); return !1 };
            g.anchorX = function (a, b) { e = a; l(b, a + J - O); return !1 }; g.anchorY = function (a, b) { f = a; l(b, a - xa); return !1 }; g.x = function (a) { p.x = a; a -= M * ((cb || w.width) + aa); O = s(a); p.attr("translateX", O); return !1 }; g.y = function (a) { xa = p.y = s(a); p.attr("translateY", xa); return !1 }; var C = p.css; return v(p, {
                css: function (a) { if (a) { var b = {}, a = x(a); q("fontSize,fontWeight,fontFamily,color,lineHeight,width,textDecoration".split(","), function (c) { a[c] !== r && (b[c] = a[c], delete a[c]) }); K.css(b) } return C.call(p, a) }, getBBox: function () {
                    return {
                        width: w.width +
                        2 * aa, height: w.height + 2 * aa, x: w.x - aa, y: w.y - aa
                    }
                }, shadow: function (a) { o && o.shadow(a); return p }, destroy: function () { V(p, "add", m); V(p.element, "mouseenter"); V(p.element, "mouseleave"); K && (K = K.destroy()); o && (o = o.destroy()); Ea.prototype.destroy.call(p); p = n = k = j = l = m = null }
            })
        }
    }; bb = Ha; var kb, Y; if (!ea && !ga) Highcharts.VMLElement = Y = {
        init: function (a, b) {
            var c = ["<", b, ' filled="f" stroked="f"'], d = ["position: ", "absolute", ";"], e = b === Ra; (b === "shape" || e) && d.push("left:0;top:0;width:1px;height:1px;"); d.push("visibility: ", e ?
            "hidden" : "visible"); c.push(' style="', d.join(""), '"/>'); if (b) c = e || b === "span" || b === "img" ? c.join("") : a.prepVML(c), this.element = Z(c); this.renderer = a; this.attrSetters = {}
        }, add: function (a) { var b = this.renderer, c = this.element, d = b.box, d = a ? a.element || a : d; a && a.inverted && b.invertChild(c, d); d.appendChild(c); this.added = !0; this.alignOnAdd && !this.deferUpdateTransform && this.updateTransform(); B(this, "add"); return this }, updateTransform: Ea.prototype.htmlUpdateTransform, setSpanRotation: function (a, b, c) {
            F(this.element,
            { filter: a ? ["progid:DXImageTransform.Microsoft.Matrix(M11=", c, ", M12=", -b, ", M21=", b, ", M22=", c, ", sizingMethod='auto expand')"].join("") : $ })
        }, attr: function (a, b) {
            var c, d, e, f = this.element || {}, g = f.style, h = f.nodeName, i = this.renderer, k = this.symbolName, j, l = this.shadows, m, n = this.attrSetters, p = this; ja(a) && t(b) && (c = a, a = {}, a[c] = b); if (ja(a)) c = a, p = c === "strokeWidth" || c === "stroke-width" ? this.strokeweight : this[c]; else for (c in a) if (d = a[c], m = !1, e = n[c] && n[c].call(this, d, c), e !== !1 && d !== null) {
                e !== r && (d = e); if (k && /^(x|y|r|start|end|width|height|innerR|anchorX|anchorY)/.test(c)) j ||
                (this.symbolAttr(a), j = !0), m = !0; else if (c === "d") { d = d || []; this.d = d.join(" "); e = d.length; m = []; for (var o; e--;) if (ra(d[e])) m[e] = s(d[e] * 10) - 5; else if (d[e] === "Z") m[e] = "x"; else if (m[e] = d[e], d.isArc && (d[e] === "wa" || d[e] === "at")) o = d[e] === "wa" ? 1 : -1, m[e + 5] === m[e + 7] && (m[e + 7] -= o), m[e + 6] === m[e + 8] && (m[e + 8] -= o); d = m.join(" ") || "x"; f.path = d; if (l) for (e = l.length; e--;) l[e].path = l[e].cutOff ? this.cutOffPath(d, l[e].cutOff) : d; m = !0 } else if (c === "visibility") {
                    if (l) for (e = l.length; e--;) l[e].style[c] = d; h === "DIV" && (d = d === "hidden" ?
                    "-999em" : 0, sb || (g[c] = d ? "visible" : "hidden"), c = "top"); g[c] = d; m = !0
                } else if (c === "zIndex") d && (g[c] = d), m = !0; else if (va(c, ["x", "y", "width", "height"]) !== -1) this[c] = d, c === "x" || c === "y" ? c = { x: "left", y: "top" }[c] : d = u(0, d), this.updateClipping ? (this[c] = d, this.updateClipping()) : g[c] = d, m = !0; else if (c === "class" && h === "DIV") f.className = d; else if (c === "stroke") d = i.color(d, f, c), c = "strokecolor"; else if (c === "stroke-width" || c === "strokeWidth") f.stroked = d ? !0 : !1, c = "strokeweight", this[c] = d, ra(d) && (d += "px"); else if (c === "dashstyle") (f.getElementsByTagName("stroke")[0] ||
                Z(i.prepVML(["<stroke/>"]), null, null, f))[c] = d || "solid", this.dashstyle = d, m = !0; else if (c === "fill") if (h === "SPAN") g.color = d; else { if (h !== "IMG") f.filled = d !== $ ? !0 : !1, d = i.color(d, f, c, this), c = "fillcolor" } else if (c === "opacity") m = !0; else if (h === "shape" && c === "rotation") this[c] = f.style[c] = d, f.style.left = -s(ia(d * gb) + 1) + "px", f.style.top = s(da(d * gb)) + "px"; else if (c === "translateX" || c === "translateY" || c === "rotation") this[c] = d, this.updateTransform(), m = !0; else if (c === "text") this.bBox = null, f.innerHTML = d, m = !0; m || (sb ? f[c] =
                    d : C(f, c, d))
            } return p
        }, clip: function (a) { var b = this, c; a ? (c = a.members, la(c, b), c.push(b), b.destroyClip = function () { la(c, b) }, a = a.getCSS(b)) : (b.destroyClip && b.destroyClip(), a = { clip: sb ? "inherit" : "rect(auto)" }); return b.css(a) }, css: Ea.prototype.htmlCss, safeRemoveChild: function (a) { a.parentNode && Ya(a) }, destroy: function () { this.destroyClip && this.destroyClip(); return Ea.prototype.destroy.apply(this) }, on: function (a, b) { this.element["on" + a] = function () { var a = X.event; a.target = a.srcElement; b(a) }; return this }, cutOffPath: function (a,
        b) { var c, a = a.split(/[ ,]/); c = a.length; if (c === 9 || c === 11) a[c - 4] = a[c - 2] = z(a[c - 2]) - 10 * b; return a.join(" ") }, shadow: function (a, b, c) {
            var d = [], e, f = this.element, g = this.renderer, h, i = f.style, k, j = f.path, l, m, n, p; j && typeof j.value !== "string" && (j = "x"); m = j; if (a) {
                n = o(a.width, 3); p = (a.opacity || 0.15) / n; for (e = 1; e <= 3; e++) {
                    l = n * 2 + 1 - 2 * e; c && (m = this.cutOffPath(j.value, l + 0.5)); k = ['<shape isShadow="true" strokeweight="', l, '" filled="false" path="', m, '" coordsize="10 10" style="', f.style.cssText, '" />']; h = Z(g.prepVML(k), null,
                    { left: z(i.left) + o(a.offsetX, 1), top: z(i.top) + o(a.offsetY, 1) }); if (c) h.cutOff = l + 1; k = ['<stroke color="', a.color || "black", '" opacity="', p * e, '"/>']; Z(g.prepVML(k), null, null, h); b ? b.element.appendChild(h) : f.parentNode.insertBefore(h, f); d.push(h)
                } this.shadows = d
            } return this
        }
    }, Y = ca(Ea, Y), Y = {
        Element: Y, isIE8: Ua.indexOf("MSIE 8.0") > -1, init: function (a, b, c) {
            var d, e; this.alignedObjects = []; d = this.createElement(Ra); e = d.element; e.style.position = "relative"; a.appendChild(d.element); this.isVML = !0; this.box = e; this.boxWrapper =
            d; this.setSize(b, c, !1); if (!G.namespaces.hcv) G.namespaces.add("hcv", "urn:schemas-microsoft-com:vml"), G.createStyleSheet().cssText = "hcv\\:fill, hcv\\:path, hcv\\:shape, hcv\\:stroke{ behavior:url(#default#VML); display: inline-block; } "
        }, isHidden: function () { return !this.box.offsetWidth }, clipRect: function (a, b, c, d) {
            var e = this.createElement(), f = ba(a); return v(e, {
                members: [], left: f ? a.x : a, top: f ? a.y : b, width: f ? a.width : c, height: f ? a.height : d, getCSS: function (a) {
                    var b = a.element, c = b.nodeName, a = a.inverted, d = this.top -
                    (c === "shape" ? b.offsetTop : 0), e = this.left, b = e + this.width, f = d + this.height, d = { clip: "rect(" + s(a ? e : d) + "px," + s(a ? f : b) + "px," + s(a ? b : f) + "px," + s(a ? d : e) + "px)" }; !a && sb && c === "DIV" && v(d, { width: b + "px", height: f + "px" }); return d
                }, updateClipping: function () { q(e.members, function (a) { a.css(e.getCSS(a)) }) }
            })
        }, color: function (a, b, c, d) {
            var e = this, f, g = /^rgba/, h, i, k = $; a && a.linearGradient ? i = "gradient" : a && a.radialGradient && (i = "pattern"); if (i) {
                var j, l, m = a.linearGradient || a.radialGradient, n, p, o, y, w, M = "", a = a.stops, aa, r = [], u = function () {
                    h =
                    ['<fill colors="' + r.join(",") + '" opacity="', o, '" o:opacity2="', p, '" type="', i, '" ', M, 'focus="100%" method="any" />']; Z(e.prepVML(h), null, null, b)
                }; n = a[0]; aa = a[a.length - 1]; n[0] > 0 && a.unshift([0, n[1]]); aa[0] < 1 && a.push([1, aa[1]]); q(a, function (a, b) { g.test(a[1]) ? (f = wa(a[1]), j = f.get("rgb"), l = f.get("a")) : (j = a[1], l = 1); r.push(a[0] * 100 + "% " + j); b ? (o = l, y = j) : (p = l, w = j) }); if (c === "fill") if (i === "gradient") c = m.x1 || m[0] || 0, a = m.y1 || m[1] || 0, n = m.x2 || m[2] || 0, m = m.y2 || m[3] || 0, M = 'angle="' + (90 - S.atan((m - a) / (n - c)) * 180 / ab) + '"',
                u(); else { var k = m.r, A = k * 2, O = k * 2, t = m.cx, s = m.cy, v = b.radialReference, x, k = function () { v && (x = d.getBBox(), t += (v[0] - x.x) / x.width - 0.5, s += (v[1] - x.y) / x.height - 0.5, A *= v[2] / x.width, O *= v[2] / x.height); M = 'src="' + N.global.VMLRadialGradientURL + '" size="' + A + "," + O + '" origin="0.5,0.5" position="' + t + "," + s + '" color2="' + w + '" '; u() }; d.added ? k() : E(d, "add", k); k = y } else k = j
            } else if (g.test(a) && b.tagName !== "IMG") f = wa(a), h = ["<", c, ' opacity="', f.get("a"), '"/>'], Z(this.prepVML(h), null, null, b), k = f.get("rgb"); else {
                k = b.getElementsByTagName(c);
                if (k.length) k[0].opacity = 1, k[0].type = "solid"; k = a
            } return k
        }, prepVML: function (a) { var b = this.isIE8, a = a.join(""); b ? (a = a.replace("/>", ' xmlns="urn:schemas-microsoft-com:vml" />'), a = a.indexOf('style="') === -1 ? a.replace("/>", ' style="display:inline-block;behavior:url(#default#VML);" />') : a.replace('style="', 'style="display:inline-block;behavior:url(#default#VML);')) : a = a.replace("<", "<hcv:"); return a }, text: Ha.prototype.html, path: function (a) { var b = { coordsize: "10 10" }; Wa(a) ? b.d = a : ba(a) && v(b, a); return this.createElement("shape").attr(b) },
        circle: function (a, b, c) { var d = this.symbol("circle"); if (ba(a)) c = a.r, b = a.y, a = a.x; d.isCircle = !0; return d.attr({ x: a, y: b, width: 2 * c, height: 2 * c }) }, g: function (a) { var b; a && (b = { className: "highcharts-" + a, "class": "highcharts-" + a }); return this.createElement(Ra).attr(b) }, image: function (a, b, c, d, e) { var f = this.createElement("img").attr({ src: a }); arguments.length > 1 && f.attr({ x: b, y: c, width: d, height: e }); return f }, rect: function (a, b, c, d, e, f) {
            if (ba(a)) b = a.y, c = a.width, d = a.height, f = a.strokeWidth, a = a.x; var g = this.symbol("rect");
            g.r = e; return g.attr(g.crisp(f, a, b, u(c, 0), u(d, 0)))
        }, invertChild: function (a, b) { var c = b.style; F(a, { flip: "x", left: z(c.width) - 1, top: z(c.height) - 1, rotation: -90 }) }, symbols: {
            arc: function (a, b, c, d, e) { var f = e.start, g = e.end, h = e.r || c || d, c = e.innerR, d = da(f), i = ia(f), k = da(g), j = ia(g); if (g - f === 0) return ["x"]; f = ["wa", a - h, b - h, a + h, b + h, a + h * d, b + h * i, a + h * k, b + h * j]; e.open && !c && f.push("e", "M", a, b); f.push("at", a - c, b - c, a + c, b + c, a + c * k, b + c * j, a + c * d, b + c * i, "x", "e"); f.isArc = !0; return f }, circle: function (a, b, c, d, e) {
                e && e.isCircle && (a -= c /
                2, b -= d / 2); return ["wa", a, b, a + c, b + d, a + c, b + d / 2, a + c, b + d / 2, "e"]
            }, rect: function (a, b, c, d, e) { var f = a + c, g = b + d, h; !t(e) || !e.r ? f = Ha.prototype.symbols.square.apply(0, arguments) : (h = D(e.r, c, d), f = ["M", a + h, b, "L", f - h, b, "wa", f - 2 * h, b, f, b + 2 * h, f - h, b, f, b + h, "L", f, g - h, "wa", f - 2 * h, g - 2 * h, f, g, f, g - h, f - h, g, "L", a + h, g, "wa", a, g - 2 * h, a + 2 * h, g, a + h, g, a, g - h, "L", a, b + h, "wa", a, b, a + 2 * h, b + 2 * h, a, b + h, a + h, b, "x", "e"]); return f }
        }
    }, Highcharts.VMLRenderer = kb = function () { this.init.apply(this, arguments) }, kb.prototype = x(Ha.prototype, Y), bb = kb; var $b; if (ga) Highcharts.CanVGRenderer =
    Y = function () { Fa = "http://www.w3.org/1999/xhtml" }, Y.prototype.symbols = {}, $b = function () { function a() { var a = b.length, d; for (d = 0; d < a; d++) b[d](); b = [] } var b = []; return { push: function (c, d) { b.length === 0 && dc(d, a); b.push(c) } } }(), bb = Y; $a.prototype = {
        addLabel: function () {
            var a = this.axis, b = a.options, c = a.chart, d = a.horiz, e = a.categories, f = a.series[0] && a.series[0].names, g = this.pos, h = b.labels, i = a.tickPositions, d = d && e && !h.step && !h.staggerLines && !h.rotation && c.plotWidth / i.length || !d && (c.optionsMarginLeft || c.chartWidth * 0.33),
            k = g === i[0], j = g === i[i.length - 1], f = e ? o(e[g], f && f[g], g) : g, e = this.label, i = i.info, l; a.isDatetimeAxis && i && (l = b.dateTimeLabelFormats[i.higherRanks[g] || i.unitName]); this.isFirst = k; this.isLast = j; b = a.labelFormatter.call({ axis: a, chart: c, isFirst: k, isLast: j, dateTimeLabelFormat: l, value: a.isLog ? oa(ka(f)) : f }); g = d && { width: u(1, s(d - 2 * (h.padding || 10))) + "px" }; g = v(g, h.style); if (t(e)) e && e.attr({ text: b }).css(g); else {
                d = { align: a.labelAlign }; if (ra(h.rotation)) d.rotation = h.rotation; this.label = t(b) && h.enabled ? c.renderer.text(b,
                0, 0, h.useHTML).attr(d).css(g).add(a.labelGroup) : null
            }
        }, getLabelSize: function () { var a = this.label, b = this.axis; return a ? (this.labelBBox = a.getBBox())[b.horiz ? "height" : "width"] : 0 }, getLabelSides: function () { var a = this.axis, b = this.labelBBox.width, a = b * { left: 0, center: 0.5, right: 1 }[a.labelAlign] - a.options.labels.x; return [-a, b - a] }, handleOverflow: function (a, b) {
            var c = !0, d = this.axis, e = d.chart, f = this.isFirst, g = this.isLast, h = b.x, i = d.reversed, k = d.tickPositions; if (f || g) {
                var j = this.getLabelSides(), l = j[0], j = j[1], e = e.plotLeft,
                m = e + d.len, k = (d = d.ticks[k[a + (f ? 1 : -1)]]) && d.label.xy && d.label.xy.x + d.getLabelSides()[f ? 0 : 1]; f && !i || g && i ? h + l < e && (h = e - l, d && h + j > k && (c = !1)) : h + j > m && (h = m - j, d && h + l < k && (c = !1)); b.x = h
            } return c
        }, getPosition: function (a, b, c, d) { var e = this.axis, f = e.chart, g = d && f.oldChartHeight || f.chartHeight; return { x: a ? e.translate(b + c, null, null, d) + e.transB : e.left + e.offset + (e.opposite ? (d && f.oldChartWidth || f.chartWidth) - e.right - e.left : 0), y: a ? g - e.bottom + e.offset - (e.opposite ? e.height : 0) : g - e.translate(b + c, null, null, d) - e.transB } }, getLabelPosition: function (a,
        b, c, d, e, f, g, h) { var i = this.axis, k = i.transA, j = i.reversed, l = i.staggerLines, m = i.chart.renderer.fontMetrics(e.style.fontSize).b, n = e.rotation, a = a + e.x - (f && d ? f * k * (j ? -1 : 1) : 0), b = b + e.y - (f && !d ? f * k * (j ? 1 : -1) : 0); n && i.side === 2 && (b -= m - m * da(n * gb)); !t(e.y) && !n && (b += m - c.getBBox().height / 2); l && (b += g / (h || 1) % l * (i.labelOffset / l)); return { x: a, y: b } }, getMarkPath: function (a, b, c, d, e, f) { return f.crispLine(["M", a, b, "L", a + (e ? 0 : -c), b + (e ? c : 0)], d) }, render: function (a, b, c) {
            var d = this.axis, e = d.options, f = d.chart.renderer, g = d.horiz, h = this.type,
            i = this.label, k = this.pos, j = e.labels, l = this.gridLine, m = h ? h + "Grid" : "grid", n = h ? h + "Tick" : "tick", p = e[m + "LineWidth"], K = e[m + "LineColor"], q = e[m + "LineDashStyle"], w = e[n + "Length"], m = e[n + "Width"] || 0, M = e[n + "Color"], u = e[n + "Position"], n = this.mark, t = j.step, s = !0, A = d.tickmarkOffset, O = this.getPosition(g, k, A, b), x = O.x, O = O.y, v = g && x === d.pos || !g && O === d.pos + d.len ? -1 : 1, db = d.staggerLines; this.isActive = !0; if (p) {
                k = d.getPlotLinePath(k + A, p * v, b, !0); if (l === r) {
                    l = { stroke: K, "stroke-width": p }; if (q) l.dashstyle = q; if (!h) l.zIndex = 1; if (b) l.opacity =
                    0; this.gridLine = l = p ? f.path(k).attr(l).add(d.gridGroup) : null
                } if (!b && l && k) l[this.isNew ? "attr" : "animate"]({ d: k, opacity: c })
            } if (m && w) u === "inside" && (w = -w), d.opposite && (w = -w), b = this.getMarkPath(x, O, w, m * v, g, f), n ? n.animate({ d: b, opacity: c }) : this.mark = f.path(b).attr({ stroke: M, "stroke-width": m, opacity: c }).add(d.axisGroup); if (i && !isNaN(x)) i.xy = O = this.getLabelPosition(x, O, i, g, j, A, a, t), this.isFirst && !o(e.showFirstLabel, 1) || this.isLast && !o(e.showLastLabel, 1) ? s = !1 : !db && g && j.overflow === "justify" && !this.handleOverflow(a,
            O) && (s = !1), t && a % t && (s = !1), s && !isNaN(O.y) ? (O.opacity = c, i[this.isNew ? "attr" : "animate"](O), this.isNew = !1) : i.attr("y", -9999)
        }, destroy: function () { Ca(this, this.axis) }
    }; Fb.prototype = {
        render: function () {
            var a = this, b = a.axis, c = b.horiz, d = (b.pointRange || 0) / 2, e = a.options, f = e.label, g = a.label, h = e.width, i = e.to, k = e.from, j = t(k) && t(i), l = e.value, m = e.dashStyle, n = a.svgElem, p = [], K, q = e.color, w = e.zIndex, M = e.events, r = b.chart.renderer; b.isLog && (k = sa(k), i = sa(i), l = sa(l)); if (h) {
                if (p = b.getPlotLinePath(l, h), d = { stroke: q, "stroke-width": h },
                m) d.dashstyle = m
            } else if (j) { if (k = u(k, b.min - d), i = D(i, b.max + d), p = b.getPlotBandPath(k, i, e), d = { fill: q }, e.borderWidth) d.stroke = e.borderColor, d["stroke-width"] = e.borderWidth } else return; if (t(w)) d.zIndex = w; if (n) p ? n.animate({ d: p }, null, n.onGetPath) : (n.hide(), n.onGetPath = function () { n.show() }); else if (p && p.length && (a.svgElem = n = r.path(p).attr(d).add(), M)) for (K in e = function (b) { n.on(b, function (c) { M[b].apply(a, [c]) }) }, M) e(K); if (f && t(f.text) && p && p.length && b.width > 0 && b.height > 0) {
                f = x({
                    align: c && j && "center", x: c ? !j &&
                    4 : 10, verticalAlign: !c && j && "middle", y: c ? j ? 16 : 10 : j ? 6 : -4, rotation: c && !j && 90
                }, f); if (!g) a.label = g = r.text(f.text, 0, 0, f.useHTML).attr({ align: f.textAlign || f.align, rotation: f.rotation, zIndex: w }).css(f.style).add(); b = [p[1], p[4], o(p[6], p[1])]; p = [p[2], p[5], o(p[7], p[2])]; c = Qa(b); j = Qa(p); g.align(f, !1, { x: c, y: j, width: ta(b) - c, height: ta(p) - j }); g.show()
            } else g && g.hide(); return a
        }, destroy: function () { la(this.axis.plotLinesAndBands, this); delete this.axis; Ca(this) }
    }; Ub.prototype = {
        destroy: function () { Ca(this, this.axis) },
        setTotal: function (a) { this.cum = this.total = a }, addValue: function (a) { this.setTotal(oa(this.total + a)) }, render: function (a) { var b = this.options, c = b.format, c = c ? Ma(c, this) : b.formatter.call(this); this.label ? this.label.attr({ text: c, visibility: "hidden" }) : this.label = this.axis.chart.renderer.text(c, 0, 0, b.useHTML).css(b.style).attr({ align: this.textAlign, rotation: b.rotation, visibility: "hidden" }).add(a) }, cacheExtremes: function (a, b) { this.points[a.index] = b }, setOffset: function (a, b) {
            var c = this.axis, d = c.chart, e = d.inverted,
            f = this.isNegative, g = c.translate(this.percent ? 100 : this.total, 0, 0, 0, 1), c = c.translate(0), c = T(g - c), h = d.xAxis[0].translate(this.x) + a, i = d.plotHeight, f = { x: e ? f ? g : g - c : h, y: e ? i - h - b : f ? i - g - c : i - g, width: e ? c : b, height: e ? b : c }; if (e = this.label) e.align(this.alignOptions, null, f), f = e.alignAttr, e.attr({ visibility: this.options.crop === !1 || d.isInsidePlot(f.x, f.y) ? ea ? "inherit" : "visible" : "hidden" })
        }
    }; ua.prototype = {
        defaultOptions: {
            dateTimeLabelFormats: {
                millisecond: "%H:%M:%S.%L", second: "%H:%M:%S", minute: "%H:%M", hour: "%H:%M", day: "%e. %b",
                week: "%e. %b", month: "%b '%y", year: "%Y"
            }, endOnTick: !1, gridLineColor: "#C0C0C0", labels: I, lineColor: "#C0D0E0", lineWidth: 1, minPadding: 0.01, maxPadding: 0.01, minorGridLineColor: "#E0E0E0", minorGridLineWidth: 1, minorTickColor: "#A0A0A0", minorTickLength: 2, minorTickPosition: "outside", startOfWeek: 1, startOnTick: !1, tickColor: "#C0D0E0", tickLength: 5, tickmarkPlacement: "between", tickPixelInterval: 100, tickPosition: "outside", tickWidth: 1, title: { align: "middle", style: { color: "#4d759e", fontWeight: "bold" } }, type: "linear"
        }, defaultYAxisOptions: {
            endOnTick: !0,
            gridLineWidth: 1, tickPixelInterval: 72, showLastLabel: !0, labels: { x: -8, y: 3 }, lineWidth: 0, maxPadding: 0.05, minPadding: 0.05, startOnTick: !0, tickWidth: 0, title: { rotation: 270, text: "Values" }, stackLabels: { enabled: !1, formatter: function () { return za(this.total, -1) }, style: I.style }
        }, defaultLeftAxisOptions: { labels: { x: -8, y: null }, title: { rotation: 270 } }, defaultRightAxisOptions: { labels: { x: 8, y: null }, title: { rotation: 90 } }, defaultBottomAxisOptions: { labels: { x: 0, y: 14 }, title: { rotation: 0 } }, defaultTopAxisOptions: {
            labels: { x: 0, y: -5 },
            title: { rotation: 0 }
        }, init: function (a, b) {
            var c = b.isX; this.horiz = a.inverted ? !c : c; this.xOrY = (this.isXAxis = c) ? "x" : "y"; this.opposite = b.opposite; this.side = this.horiz ? this.opposite ? 0 : 2 : this.opposite ? 1 : 3; this.setOptions(b); var d = this.options, e = d.type; this.labelFormatter = d.labels.formatter || this.defaultLabelFormatter; this.userOptions = b; this.minPixelPadding = 0; this.chart = a; this.reversed = d.reversed; this.zoomEnabled = d.zoomEnabled !== !1; this.categories = d.categories || e === "category"; this.isLog = e === "logarithmic"; this.isDatetimeAxis =
            e === "datetime"; this.isLinked = t(d.linkedTo); this.tickmarkOffset = this.categories && d.tickmarkPlacement === "between" ? 0.5 : 0; this.ticks = {}; this.minorTicks = {}; this.plotLinesAndBands = []; this.alternateBands = {}; this.len = 0; this.minRange = this.userMinRange = d.minRange || d.maxZoom; this.range = d.range; this.offset = d.offset || 0; this.stacks = {}; this.oldStacks = {}; this.stacksMax = {}; this._stacksTouched = 0; this.min = this.max = null; var f, d = this.options.events; va(this, a.axes) === -1 && (a.axes.push(this), a[c ? "xAxis" : "yAxis"].push(this));
            this.series = this.series || []; if (a.inverted && c && this.reversed === r) this.reversed = !0; this.removePlotLine = this.removePlotBand = this.removePlotBandOrLine; for (f in d) E(this, f, d[f]); if (this.isLog) this.val2lin = sa, this.lin2val = ka
        }, setOptions: function (a) { this.options = x(this.defaultOptions, this.isXAxis ? {} : this.defaultYAxisOptions, [this.defaultTopAxisOptions, this.defaultRightAxisOptions, this.defaultBottomAxisOptions, this.defaultLeftAxisOptions][this.side], x(N[this.isXAxis ? "xAxis" : "yAxis"], a)) }, update: function (a,
        b) { var c = this.chart, a = c.options[this.xOrY + "Axis"][this.options.index] = x(this.userOptions, a); this.destroy(!0); this._addedPlotLB = !1; this.init(c, v(a, { events: r })); c.isDirtyBox = !0; o(b, !0) && c.redraw() }, remove: function (a) { var b = this.chart, c = this.xOrY + "Axis"; q(this.series, function (a) { a.remove(!1) }); la(b.axes, this); la(b[c], this); b.options[c].splice(this.options.index, 1); q(b[c], function (a, b) { a.options.index = b }); this.destroy(); b.isDirtyBox = !0; o(a, !0) && b.redraw() }, defaultLabelFormatter: function () {
            var a = this.axis,
            b = this.value, c = a.categories, d = this.dateTimeLabelFormat, e = N.lang.numericSymbols, f = e && e.length, g, h = a.options.labels.format, a = a.isLog ? b : a.tickInterval; if (h) g = Ma(h, this); else if (c) g = b; else if (d) g = Aa(d, b); else if (f && a >= 1E3) for (; f-- && g === r;) c = Math.pow(1E3, f + 1), a >= c && e[f] !== null && (g = za(b / c, -1) + e[f]); g === r && (g = b >= 1E3 ? za(b, 0) : za(b, -1)); return g
        }, getSeriesExtremes: function () {
            var a = this, b = a.chart; a.hasVisibleSeries = !1; a.dataMin = a.dataMax = null; a.stacksMax = {}; a.buildStacks(); q(a.series, function (c) {
                if (c.visible ||
                !b.options.chart.ignoreHiddenSeries) {
                    var d = c.options, e; e = d.threshold; a.hasVisibleSeries = !0; a.isLog && e <= 0 && (e = null); if (a.isXAxis) { if (e = c.xData, e.length) a.dataMin = D(o(a.dataMin, e[0]), Qa(e)), a.dataMax = u(o(a.dataMax, e[0]), ta(e)) } else {
                        d = d.stacking; a.usePercentage = d === "percent"; if (a.usePercentage) a.dataMin = 0, a.dataMax = 99; c.getExtremes(); d = c.dataMax; c = c.dataMin; if (!a.usePercentage && t(c) && t(d)) a.dataMin = D(o(a.dataMin, c), c), a.dataMax = u(o(a.dataMax, d), d); if (t(e)) if (a.dataMin >= e) a.dataMin = e, a.ignoreMinPadding =
                        !0; else if (a.dataMax < e) a.dataMax = e, a.ignoreMaxPadding = !0
                    }
                }
            })
        }, translate: function (a, b, c, d, e, f) { var g = this.len, h = 1, i = 0, k = d ? this.oldTransA : this.transA, d = d ? this.oldMin : this.min, j = this.minPixelPadding, e = (this.options.ordinal || this.isLog && e) && this.lin2val; if (!k) k = this.transA; c && (h *= -1, i = g); this.reversed && (h *= -1, i -= h * g); b ? (a = a * h + i, a -= j, a = a / k + d, e && (a = this.lin2val(a))) : (e && (a = this.val2lin(a)), f === "between" && (f = 0.5), a = h * (a - d) * k + i + h * j + (ra(f) ? k * f * this.pointRange : 0)); return a }, toPixels: function (a, b) {
            return this.translate(a,
            !1, !this.horiz, null, !0) + (b ? 0 : this.pos)
        }, toValue: function (a, b) { return this.translate(a - (b ? 0 : this.pos), !0, !this.horiz, null, !0) }, getPlotLinePath: function (a, b, c, d) {
            var e = this.chart, f = this.left, g = this.top, h, i, k, a = this.translate(a, null, null, c), j = c && e.oldChartHeight || e.chartHeight, l = c && e.oldChartWidth || e.chartWidth, m; h = this.transB; c = i = s(a + h); h = k = s(j - a - h); if (isNaN(a)) m = !0; else if (this.horiz) { if (h = g, k = j - this.bottom, c < f || c > f + this.width) m = !0 } else if (c = f, i = l - this.right, h < g || h > g + this.height) m = !0; return m && !d ?
            null : e.renderer.crispLine(["M", c, h, "L", i, k], b || 0)
        }, getPlotBandPath: function (a, b) { var c = this.getPlotLinePath(b), d = this.getPlotLinePath(a); d && c ? d.push(c[4], c[5], c[1], c[2]) : d = null; return d }, getLinearTickPositions: function (a, b, c) { for (var d, b = oa(U(b / a) * a), c = oa(pa(c / a) * a), e = []; b <= c;) { e.push(b); b = oa(b + a); if (b === d) break; d = b } return e }, getLogTickPositions: function (a, b, c, d) {
            var e = this.options, f = this.len, g = []; if (!d) this._minorAutoInterval = null; if (a >= 0.5) a = s(a), g = this.getLinearTickPositions(a, b, c); else if (a >=
            0.08) for (var f = U(b), h, i, k, j, l, e = a > 0.3 ? [1, 2, 4] : a > 0.15 ? [1, 2, 4, 6, 8] : [1, 2, 3, 4, 5, 6, 7, 8, 9]; f < c + 1 && !l; f++) { i = e.length; for (h = 0; h < i && !l; h++) k = sa(ka(f) * e[h]), k > b && (!d || j <= c) && g.push(j), j > c && (l = !0), j = k } else if (b = ka(b), c = ka(c), a = e[d ? "minorTickInterval" : "tickInterval"], a = o(a === "auto" ? null : a, this._minorAutoInterval, (c - b) * (e.tickPixelInterval / (d ? 5 : 1)) / ((d ? f / this.tickPositions.length : f) || 1)), a = yb(a, null, xb(a)), g = Ga(this.getLinearTickPositions(a, b, c), sa), !d) this._minorAutoInterval = a / 5; if (!d) this.tickInterval = a; return g
        },
        getMinorTickPositions: function () { var a = this.options, b = this.tickPositions, c = this.minorTickInterval, d = [], e; if (this.isLog) { e = b.length; for (a = 1; a < e; a++) d = d.concat(this.getLogTickPositions(c, b[a - 1], b[a], !0)) } else if (this.isDatetimeAxis && a.minorTickInterval === "auto") d = d.concat(fb(zb(c), this.min, this.max, a.startOfWeek)), d[0] < this.min && d.shift(); else for (b = this.min + (b[0] - this.min) % c; b <= this.max; b += c) d.push(b); return d }, adjustForMinRange: function () {
            var a = this.options, b = this.min, c = this.max, d, e = this.dataMax -
            this.dataMin >= this.minRange, f, g, h, i, k; if (this.isXAxis && this.minRange === r && !this.isLog) t(a.min) || t(a.max) ? this.minRange = null : (q(this.series, function (a) { i = a.xData; for (g = k = a.xIncrement ? 1 : i.length - 1; g > 0; g--) if (h = i[g] - i[g - 1], f === r || h < f) f = h }), this.minRange = D(f * 5, this.dataMax - this.dataMin)); if (c - b < this.minRange) { var j = this.minRange; d = (j - c + b) / 2; d = [b - d, o(a.min, b - d)]; if (e) d[2] = this.dataMin; b = ta(d); c = [b + j, o(a.max, b + j)]; if (e) c[2] = this.dataMax; c = Qa(c); c - b < j && (d[0] = c - j, d[1] = o(a.min, c - j), b = ta(d)) } this.min = b; this.max =
            c
        }, setAxisTranslation: function (a) {
            var b = this.max - this.min, c = 0, d, e = 0, f = 0, g = this.linkedParent, h = this.transA; if (this.isXAxis) g ? (e = g.minPointOffset, f = g.pointRangePadding) : q(this.series, function (a) { var g = a.pointRange, h = a.options.pointPlacement, l = a.closestPointRange; g > b && (g = 0); c = u(c, g); e = u(e, ja(h) ? 0 : g / 2); f = u(f, h === "on" ? 0 : g); !a.noSharedTooltip && t(l) && (d = t(d) ? D(d, l) : l) }), g = this.ordinalSlope && d ? this.ordinalSlope / d : 1, this.minPointOffset = e *= g, this.pointRangePadding = f *= g, this.pointRange = D(c, b), this.closestPointRange =
            d; if (a) this.oldTransA = h; this.translationSlope = this.transA = h = this.len / (b + f || 1); this.transB = this.horiz ? this.left : this.bottom; this.minPixelPadding = h * e
        }, setTickPositions: function (a) {
            var b = this, c = b.chart, d = b.options, e = b.isLog, f = b.isDatetimeAxis, g = b.isXAxis, h = b.isLinked, i = b.options.tickPositioner, k = d.maxPadding, j = d.minPadding, l = d.tickInterval, m = d.minTickInterval, n = d.tickPixelInterval, p = b.categories; h ? (b.linkedParent = c[g ? "xAxis" : "yAxis"][d.linkedTo], c = b.linkedParent.getExtremes(), b.min = o(c.min, c.dataMin),
            b.max = o(c.max, c.dataMax), d.type !== b.linkedParent.options.type && Da(11, 1)) : (b.min = o(b.userMin, d.min, b.dataMin), b.max = o(b.userMax, d.max, b.dataMax)); if (e) !a && D(b.min, o(b.dataMin, b.min)) <= 0 && Da(10, 1), b.min = oa(sa(b.min)), b.max = oa(sa(b.max)); if (b.range && (b.userMin = b.min = u(b.min, b.max - b.range), b.userMax = b.max, a)) b.range = null; b.beforePadding && b.beforePadding(); b.adjustForMinRange(); if (!p && !b.usePercentage && !h && t(b.min) && t(b.max) && (c = b.max - b.min)) {
                if (!t(d.min) && !t(b.userMin) && j && (b.dataMin < 0 || !b.ignoreMinPadding)) b.min -=
                c * j; if (!t(d.max) && !t(b.userMax) && k && (b.dataMax > 0 || !b.ignoreMaxPadding)) b.max += c * k
            } b.tickInterval = b.min === b.max || b.min === void 0 || b.max === void 0 ? 1 : h && !l && n === b.linkedParent.options.tickPixelInterval ? b.linkedParent.tickInterval : o(l, p ? 1 : (b.max - b.min) * n / (b.len || 1)); g && !a && q(b.series, function (a) { a.processData(b.min !== b.oldMin || b.max !== b.oldMax) }); b.setAxisTranslation(!0); b.beforeSetTickPositions && b.beforeSetTickPositions(); if (b.postProcessTickInterval) b.tickInterval = b.postProcessTickInterval(b.tickInterval);
            if (b.pointRange) b.tickInterval = u(b.pointRange, b.tickInterval); if (!l && b.tickInterval < m) b.tickInterval = m; if (!f && !e && !l) b.tickInterval = yb(b.tickInterval, null, xb(b.tickInterval), d); b.minorTickInterval = d.minorTickInterval === "auto" && b.tickInterval ? b.tickInterval / 5 : d.minorTickInterval; b.tickPositions = a = d.tickPositions ? [].concat(d.tickPositions) : i && i.apply(b, [b.min, b.max]); if (!a) a = f ? (b.getNonLinearTimeTicks || fb)(zb(b.tickInterval, d.units), b.min, b.max, d.startOfWeek, b.ordinalPositions, b.closestPointRange, !0) :
            e ? b.getLogTickPositions(b.tickInterval, b.min, b.max) : b.getLinearTickPositions(b.tickInterval, b.min, b.max), b.tickPositions = a; if (!h) e = a[0], f = a[a.length - 1], h = b.minPointOffset || 0, d.startOnTick ? b.min = e : b.min - h > e && a.shift(), d.endOnTick ? b.max = f : b.max + h < f && a.pop(), a.length === 1 && (b.min -= 0.001, b.max += 0.001)
        }, setMaxTicks: function () {
            var a = this.chart, b = a.maxTicks || {}, c = this.tickPositions, d = this._maxTicksKey = [this.xOrY, this.pos, this.len].join("-"); if (!this.isLinked && !this.isDatetimeAxis && c && c.length > (b[d] || 0) &&
            this.options.alignTicks !== !1) b[d] = c.length; a.maxTicks = b
        }, adjustTickAmount: function () { var a = this._maxTicksKey, b = this.tickPositions, c = this.chart.maxTicks; if (c && c[a] && !this.isDatetimeAxis && !this.categories && !this.isLinked && this.options.alignTicks !== !1) { var d = this.tickAmount, e = b.length; this.tickAmount = a = c[a]; if (e < a) { for (; b.length < a;) b.push(oa(b[b.length - 1] + this.tickInterval)); this.transA *= (e - 1) / (a - 1); this.max = b[b.length - 1] } if (t(d) && a !== d) this.isDirty = !0 } }, setScale: function () {
            var a = this.stacks, b, c, d, e;
            this.oldMin = this.min; this.oldMax = this.max; this.oldAxisLength = this.len; this.setAxisSize(); e = this.len !== this.oldAxisLength; q(this.series, function (a) { if (a.isDirtyData || a.isDirty || a.xAxis.isDirty) d = !0 }); if (e || d || this.isLinked || this.forceRedraw || this.userMin !== this.oldUserMin || this.userMax !== this.oldUserMax) {
                if (!this.isXAxis) for (b in a) for (c in a[b]) a[b][c].total = null; this.forceRedraw = !1; this.getSeriesExtremes(); this.setTickPositions(); this.oldUserMin = this.userMin; this.oldUserMax = this.userMax; if (!this.isDirty) this.isDirty =
                e || this.min !== this.oldMin || this.max !== this.oldMax
            } else if (!this.isXAxis) { if (this.oldStacks) a = this.stacks = this.oldStacks; for (b in a) for (c in a[b]) a[b][c].cum = a[b][c].total } this.setMaxTicks()
        }, setExtremes: function (a, b, c, d, e) { var f = this, g = f.chart, c = o(c, !0), e = v(e, { min: a, max: b }); B(f, "setExtremes", e, function () { f.userMin = a; f.userMax = b; f.isDirtyExtremes = !0; c && g.redraw(d) }) }, zoom: function (a, b) {
            this.allowZoomOutside || (t(this.dataMin) && a <= this.dataMin && (a = r), t(this.dataMax) && b >= this.dataMax && (b = r)); this.displayBtn =
            a !== r || b !== r; this.setExtremes(a, b, !1, r, { trigger: "zoom" }); return !0
        }, setAxisSize: function () { var a = this.chart, b = this.options, c = b.offsetLeft || 0, d = b.offsetRight || 0, e = this.horiz, f, g; this.left = g = o(b.left, a.plotLeft + c); this.top = f = o(b.top, a.plotTop); this.width = c = o(b.width, a.plotWidth - c + d); this.height = b = o(b.height, a.plotHeight); this.bottom = a.chartHeight - b - f; this.right = a.chartWidth - c - g; this.len = u(e ? c : b, 0); this.pos = e ? g : f }, getExtremes: function () {
            var a = this.isLog; return {
                min: a ? oa(ka(this.min)) : this.min, max: a ? oa(ka(this.max)) :
                this.max, dataMin: this.dataMin, dataMax: this.dataMax, userMin: this.userMin, userMax: this.userMax
            }
        }, getThreshold: function (a) { var b = this.isLog, c = b ? ka(this.min) : this.min, b = b ? ka(this.max) : this.max; c > a || a === null ? a = c : b < a && (a = b); return this.translate(a, 0, 1, 0, 1) }, addPlotBand: function (a) { this.addPlotBandOrLine(a, "plotBands") }, addPlotLine: function (a) { this.addPlotBandOrLine(a, "plotLines") }, addPlotBandOrLine: function (a, b) {
            var c = (new Fb(this, a)).render(), d = this.userOptions; b && (d[b] = d[b] || [], d[b].push(a)); this.plotLinesAndBands.push(c);
            return c
        }, autoLabelAlign: function (a) { a = (o(a, 0) - this.side * 90 + 720) % 360; return a > 15 && a < 165 ? "right" : a > 195 && a < 345 ? "left" : "center" }, getOffset: function () {
            var a = this, b = a.chart, c = b.renderer, d = a.options, e = a.tickPositions, f = a.ticks, g = a.horiz, h = a.side, i = b.inverted ? [1, 0, 3, 2][h] : h, k, j = 0, l, m = 0, n = d.title, p = d.labels, K = 0, y = b.axisOffset, w = b.clipOffset, M = [-1, 1, 1, -1][h], s, x = 1, v = o(p.maxStaggerLines, 5), A, O, xa, J; a.hasData = k = a.hasVisibleSeries || t(a.min) && t(a.max) && !!e; a.showAxis = b = k || o(d.showEmpty, !0); a.staggerLines = a.horiz &&
            p.staggerLines; if (!a.axisGroup) a.gridGroup = c.g("grid").attr({ zIndex: d.gridZIndex || 1 }).add(), a.axisGroup = c.g("axis").attr({ zIndex: d.zIndex || 2 }).add(), a.labelGroup = c.g("axis-labels").attr({ zIndex: p.zIndex || 7 }).add(); if (k || a.isLinked) {
                a.labelAlign = o(p.align || a.autoLabelAlign(p.rotation)); q(e, function (b) { f[b] ? f[b].addLabel() : f[b] = new $a(a, b) }); if (a.horiz && !a.staggerLines && v && !p.rotation) {
                    for (s = a.reversed ? [].concat(e).reverse() : e; x < v;) {
                        k = []; A = !1; for (p = 0; p < s.length; p++) O = s[p], xa = (xa = f[O].label && f[O].label.bBox) ?
                        xa.width : 0, J = p % x, xa && (O = a.translate(O), k[J] !== r && O < k[J] && (A = !0), k[J] = O + xa); if (A) x++; else break
                    } if (x > 1) a.staggerLines = x
                } q(e, function (b) { if (h === 0 || h === 2 || { 1: "left", 3: "right" }[h] === a.labelAlign) K = u(f[b].getLabelSize(), K) }); if (a.staggerLines) K *= a.staggerLines, a.labelOffset = K
            } else for (s in f) f[s].destroy(), delete f[s]; if (n && n.text && n.enabled !== !1) {
                if (!a.axisTitle) a.axisTitle = c.text(n.text, 0, 0, n.useHTML).attr({ zIndex: 7, rotation: n.rotation || 0, align: n.textAlign || { low: "left", middle: "center", high: "right" }[n.align] }).css(n.style).add(a.axisGroup),
                a.axisTitle.isNew = !0; if (b) j = a.axisTitle.getBBox()[g ? "height" : "width"], m = o(n.margin, g ? 5 : 10), l = n.offset; a.axisTitle[b ? "show" : "hide"]()
            } a.offset = M * o(d.offset, y[h]); a.axisTitleMargin = o(l, K + m + (h !== 2 && K && M * d.labels[g ? "y" : "x"])); y[h] = u(y[h], a.axisTitleMargin + j + M * a.offset); w[i] = u(w[i], d.lineWidth)
        }, getLinePath: function (a) {
            var b = this.chart, c = this.opposite, d = this.offset, e = this.horiz, f = this.left + (c ? this.width : 0) + d; this.lineTop = d = b.chartHeight - this.bottom - (c ? this.height : 0) + d; c || (a *= -1); return b.renderer.crispLine(["M",
            e ? this.left : f, e ? d : this.top, "L", e ? b.chartWidth - this.right : f, e ? d : b.chartHeight - this.bottom], a)
        }, getTitlePosition: function () { var a = this.horiz, b = this.left, c = this.top, d = this.len, e = this.options.title, f = a ? b : c, g = this.opposite, h = this.offset, i = z(e.style.fontSize || 12), d = { low: f + (a ? 0 : d), middle: f + d / 2, high: f + (a ? d : 0) }[e.align], b = (a ? c + this.height : b) + (a ? 1 : -1) * (g ? -1 : 1) * this.axisTitleMargin + (this.side === 2 ? i : 0); return { x: a ? d : b + (g ? this.width : 0) + h + (e.x || 0), y: a ? b - (g ? this.height : 0) + h : d + (e.y || 0) } }, render: function () {
            var a = this,
            b = a.chart, c = b.renderer, d = a.options, e = a.isLog, f = a.isLinked, g = a.tickPositions, h = a.axisTitle, i = a.stacks, k = a.ticks, j = a.minorTicks, l = a.alternateBands, m = d.stackLabels, n = d.alternateGridColor, p = a.tickmarkOffset, o = d.lineWidth, y, w = b.hasRendered && t(a.oldMin) && !isNaN(a.oldMin); y = a.hasData; var M = a.showAxis, s, u; q([k, j, l], function (a) { for (var b in a) a[b].isActive = !1 }); if (y || f) if (a.minorTickInterval && !a.categories && q(a.getMinorTickPositions(), function (b) {
            j[b] || (j[b] = new $a(a, b, "minor")); w && j[b].isNew && j[b].render(null,
            !0); j[b].render(null, !1, 1)
            }), g.length && (q(g.slice(1).concat([g[0]]), function (b, c) { c = c === g.length - 1 ? 0 : c + 1; if (!f || b >= a.min && b <= a.max) k[b] || (k[b] = new $a(a, b)), w && k[b].isNew && k[b].render(c, !0), k[b].render(c, !1, 1) }), p && a.min === 0 && (k[-1] || (k[-1] = new $a(a, -1, null, !0)), k[-1].render(-1))), n && q(g, function (b, c) { if (c % 2 === 0 && b < a.max) l[b] || (l[b] = new Fb(a)), s = b + p, u = g[c + 1] !== r ? g[c + 1] + p : a.max, l[b].options = { from: e ? ka(s) : s, to: e ? ka(u) : u, color: n }, l[b].render(), l[b].isActive = !0 }), !a._addedPlotLB) q((d.plotLines || []).concat(d.plotBands ||
            []), function (b) { a.addPlotBandOrLine(b) }), a._addedPlotLB = !0; q([k, j, l], function (a) { var c, d, e = [], f = Sa ? Sa.duration || 500 : 0, g = function () { for (d = e.length; d--;) a[e[d]] && !a[e[d]].isActive && (a[e[d]].destroy(), delete a[e[d]]) }; for (c in a) if (!a[c].isActive) a[c].render(c, !1, 0), a[c].isActive = !1, e.push(c); a === l || !b.hasRendered || !f ? g() : f && setTimeout(g, f) }); if (o) y = a.getLinePath(o), a.axisLine ? a.axisLine.animate({ d: y }) : a.axisLine = c.path(y).attr({ stroke: d.lineColor, "stroke-width": o, zIndex: 7 }).add(a.axisGroup), a.axisLine[M ?
            "show" : "hide"](); if (h && M) h[h.isNew ? "attr" : "animate"](a.getTitlePosition()), h.isNew = !1; if (m && m.enabled) { var x, A, d = a.stackTotalGroup; if (!d) a.stackTotalGroup = d = c.g("stack-labels").attr({ visibility: "visible", zIndex: 6 }).add(); d.translate(b.plotLeft, b.plotTop); for (x in i) for (A in c = i[x], c) c[A].render(d) } a.isDirty = !1
        }, removePlotBandOrLine: function (a) {
            for (var b = this.plotLinesAndBands, c = this.options, d = this.userOptions, e = b.length; e--;) b[e].id === a && b[e].destroy(); q([c.plotLines || [], d.plotLines || [], c.plotBands ||
            [], d.plotBands || []], function (b) { for (e = b.length; e--;) b[e].id === a && la(b, b[e]) })
        }, setTitle: function (a, b) { this.update({ title: a }, b) }, redraw: function () { var a = this.chart.pointer; a.reset && a.reset(!0); this.render(); q(this.plotLinesAndBands, function (a) { a.render() }); q(this.series, function (a) { a.isDirty = !0 }) }, buildStacks: function () { this.isXAxis || q(this.series, function (a) { a.setStackedPoints() }) }, setCategories: function (a, b) { this.update({ categories: a }, b) }, destroy: function (a) {
            var b = this, c = b.stacks, d, e = b.plotLinesAndBands;
            a || V(b); for (d in c) Ca(c[d]), c[d] = null; q([b.ticks, b.minorTicks, b.alternateBands], function (a) { Ca(a) }); for (a = e.length; a--;) e[a].destroy(); q("stackTotalGroup,axisLine,axisGroup,gridGroup,labelGroup,axisTitle".split(","), function (a) { b[a] && (b[a] = b[a].destroy()) })
        }
    }; Gb.prototype = {
        init: function (a, b) {
            var c = b.borderWidth, d = b.style, e = z(d.padding); this.chart = a; this.options = b; this.crosshairs = []; this.now = { x: 0, y: 0 }; this.isHidden = !0; this.label = a.renderer.label("", 0, 0, b.shape, null, null, b.useHTML, null, "tooltip").attr({
                padding: e,
                fill: b.backgroundColor, "stroke-width": c, r: b.borderRadius, zIndex: 8
            }).css(d).css({ padding: 0 }).hide().add(); ga || this.label.shadow(b.shadow); this.shared = b.shared
        }, destroy: function () { q(this.crosshairs, function (a) { a && a.destroy() }); if (this.label) this.label = this.label.destroy(); clearTimeout(this.hideTimer); clearTimeout(this.tooltipTimeout) }, move: function (a, b, c, d) {
            var e = this, f = e.now, g = e.options.animation !== !1 && !e.isHidden; v(f, {
                x: g ? (2 * f.x + a) / 3 : a, y: g ? (f.y + b) / 2 : b, anchorX: g ? (2 * f.anchorX + c) / 3 : c, anchorY: g ? (f.anchorY +
                d) / 2 : d
            }); e.label.attr(f); if (g && (T(a - f.x) > 1 || T(b - f.y) > 1)) clearTimeout(this.tooltipTimeout), this.tooltipTimeout = setTimeout(function () { e && e.move(a, b, c, d) }, 32)
        }, hide: function () { var a = this, b; clearTimeout(this.hideTimer); if (!this.isHidden) b = this.chart.hoverPoints, this.hideTimer = setTimeout(function () { a.label.fadeOut(); a.isHidden = !0 }, o(this.options.hideDelay, 500)), b && q(b, function (a) { a.setState() }), this.chart.hoverPoints = null }, hideCrosshairs: function () { q(this.crosshairs, function (a) { a && a.hide() }) }, getAnchor: function (a,
        b) { var c, d = this.chart, e = d.inverted, f = d.plotTop, g = 0, h = 0, i, a = ha(a); c = a[0].tooltipPos; this.followPointer && b && (b.chartX === r && (b = d.pointer.normalize(b)), c = [b.chartX - d.plotLeft, b.chartY - f]); c || (q(a, function (a) { i = a.series.yAxis; g += a.plotX; h += (a.plotLow ? (a.plotLow + a.plotHigh) / 2 : a.plotY) + (!e && i ? i.top - f : 0) }), g /= a.length, h /= a.length, c = [e ? d.plotWidth - h : g, this.shared && !e && a.length > 1 && b ? b.chartY - f : e ? d.plotHeight - g : h]); return Ga(c, s) }, getPosition: function (a, b, c) {
            var d = this.chart, e = d.plotLeft, f = d.plotTop, g = d.plotWidth,
            h = d.plotHeight, i = o(this.options.distance, 12), k = c.plotX, c = c.plotY, d = k + e + (d.inverted ? i : -a - i), j = c - b + f + 15, l; d < 7 && (d = e + u(k, 0) + i); d + a > e + g && (d -= d + a - (e + g), j = c - b + f - i, l = !0); j < f + 5 && (j = f + 5, l && c >= j && c <= j + b && (j = c + f + i)); j + b > f + h && (j = u(f, f + h - b - i)); return { x: d, y: j }
        }, defaultFormatter: function (a) {
            var b = this.points || ha(this), c = b[0].series, d; d = [c.tooltipHeaderFormatter(b[0])]; q(b, function (a) { c = a.series; d.push(c.tooltipFormatter && c.tooltipFormatter(a) || a.point.tooltipFormatter(c.tooltipOptions.pointFormat)) }); d.push(a.options.footerFormat ||
            ""); return d.join("")
        }, refresh: function (a, b) {
            var c = this.chart, d = this.label, e = this.options, f, g, h, i = {}, k, j = []; k = e.formatter || this.defaultFormatter; var i = c.hoverPoints, l, m = e.crosshairs; h = this.shared; clearTimeout(this.hideTimer); this.followPointer = ha(a)[0].series.tooltipOptions.followPointer; g = this.getAnchor(a, b); f = g[0]; g = g[1]; h && (!a.series || !a.series.noSharedTooltip) ? (c.hoverPoints = a, i && q(i, function (a) { a.setState() }), q(a, function (a) { a.setState("hover"); j.push(a.getLabelConfig()) }), i = {
                x: a[0].category,
                y: a[0].y
            }, i.points = j, a = a[0]) : i = a.getLabelConfig(); k = k.call(i, this); i = a.series; h = h || !i.isCartesian || i.tooltipOutsidePlot || c.isInsidePlot(f, g); k === !1 || !h ? this.hide() : (this.isHidden && (jb(d), d.attr("opacity", 1).show()), d.attr({ text: k }), l = e.borderColor || a.color || i.color || "#606060", d.attr({ stroke: l }), this.updatePosition({ plotX: f, plotY: g }), this.isHidden = !1); if (m) {
                m = ha(m); for (d = m.length; d--;) if (i = a.series, e = i[d ? "yAxis" : "xAxis"], m[d] && e) if (h = d ? o(a.stackY, a.y) : a.x, e.isLog && (h = sa(h)), i.modifyValue && (h = i.modifyValue(h)),
                e = e.getPlotLinePath(h, 1), this.crosshairs[d]) this.crosshairs[d].attr({ d: e, visibility: "visible" }); else { h = { "stroke-width": m[d].width || 1, stroke: m[d].color || "#C0C0C0", zIndex: m[d].zIndex || 2 }; if (m[d].dashStyle) h.dashstyle = m[d].dashStyle; this.crosshairs[d] = c.renderer.path(e).attr(h).add() }
            } B(c, "tooltipRefresh", { text: k, x: f + c.plotLeft, y: g + c.plotTop, borderColor: l })
        }, updatePosition: function (a) {
            var b = this.chart, c = this.label, c = (this.options.positioner || this.getPosition).call(this, c.width, c.height, a); this.move(s(c.x),
            s(c.y), a.plotX + b.plotLeft, a.plotY + b.plotTop)
        }
    }; rb.prototype = {
        init: function (a, b) { var c = ga ? "" : b.chart.zoomType, d = a.inverted, e; this.options = b; this.chart = a; this.zoomX = e = /x/.test(c); this.zoomY = c = /y/.test(c); this.zoomHor = e && !d || c && d; this.zoomVert = c && !d || e && d; this.pinchDown = []; this.lastValidTouch = {}; if (b.tooltip.enabled) a.tooltip = new Gb(a, b.tooltip); this.setDOMEvents() }, normalize: function (a) {
            var b, c, a = a || X.event; if (!a.target) a.target = a.srcElement; a = Zb(a); c = a.touches ? a.touches.item(0) : a; this.chartPosition =
            b = ec(this.chart.container); return v(a, { chartX: s(o(c.pageX, c.clientX) - b.left), chartY: s(o(c.pageY, c.clientY) - b.top) })
        }, getCoordinates: function (a) { var b = { xAxis: [], yAxis: [] }; q(this.chart.axes, function (c) { b[c.isXAxis ? "xAxis" : "yAxis"].push({ axis: c, value: c.toValue(a[c.horiz ? "chartX" : "chartY"]) }) }); return b }, getIndex: function (a) { var b = this.chart; return b.inverted ? b.plotHeight + b.plotTop - a.chartY : a.chartX - b.plotLeft }, runPointActions: function (a) {
            var b = this.chart, c = b.series, d = b.tooltip, e, f = b.hoverPoint, g = b.hoverSeries,
            h, i, k = b.chartWidth, j = this.getIndex(a); if (d && this.options.tooltip.shared && (!g || !g.noSharedTooltip)) { e = []; h = c.length; for (i = 0; i < h; i++) if (c[i].visible && c[i].options.enableMouseTracking !== !1 && !c[i].noSharedTooltip && c[i].tooltipPoints.length && (b = c[i].tooltipPoints[j], b.series)) b._dist = T(j - b.clientX), k = D(k, b._dist), e.push(b); for (h = e.length; h--;) e[h]._dist > k && e.splice(h, 1); if (e.length && e[0].clientX !== this.hoverX) d.refresh(e, a), this.hoverX = e[0].clientX } if (g && g.tracker) { if ((b = g.tooltipPoints[j]) && b !== f) b.onMouseOver(a) } else d &&
            d.followPointer && !d.isHidden && (a = d.getAnchor([{}], a), d.updatePosition({ plotX: a[0], plotY: a[1] }))
        }, reset: function (a) { var b = this.chart, c = b.hoverSeries, d = b.hoverPoint, e = b.tooltip, b = e && e.shared ? b.hoverPoints : d; (a = a && e && b) && ha(b)[0].plotX === r && (a = !1); if (a) e.refresh(b); else { if (d) d.onMouseOut(); if (c) c.onMouseOut(); e && (e.hide(), e.hideCrosshairs()); this.hoverX = null } }, scaleGroups: function (a, b) {
            var c = this.chart, d; q(c.series, function (e) {
                d = a || e.getPlotBox(); e.xAxis && e.xAxis.zoomEnabled && (e.group.attr(d), e.markerGroup &&
                (e.markerGroup.attr(d), e.markerGroup.clip(b ? c.clipRect : null)), e.dataLabelsGroup && e.dataLabelsGroup.attr(d))
            }); c.clipRect.attr(b || c.clipBox)
        }, pinchTranslateDirection: function (a, b, c, d, e, f, g) {
            var h = this.chart, i = a ? "x" : "y", k = a ? "X" : "Y", j = "chart" + k, l = a ? "width" : "height", m = h["plot" + (a ? "Left" : "Top")], n, p, o = 1, q = h.inverted, w = h.bounds[a ? "h" : "v"], M = b.length === 1, s = b[0][j], u = c[0][j], r = !M && b[1][j], A = !M && c[1][j], t, c = function () { !M && T(s - r) > 20 && (o = T(u - A) / T(s - r)); p = (m - u) / o + s; n = h["plot" + (a ? "Width" : "Height")] / o }; c(); b = p;
            b < w.min ? (b = w.min, t = !0) : b + n > w.max && (b = w.max - n, t = !0); t ? (u -= 0.8 * (u - g[i][0]), M || (A -= 0.8 * (A - g[i][1])), c()) : g[i] = [u, A]; q || (f[i] = p - m, f[l] = n); f = q ? 1 / o : o; e[l] = n; e[i] = b; d[q ? a ? "scaleY" : "scaleX" : "scale" + k] = o; d["translate" + k] = f * m + (u - f * s)
        }, pinch: function (a) {
            var b = this, c = b.chart, d = b.pinchDown, e = c.tooltip && c.tooltip.options.followTouchMove, f = a.touches, g = f.length, h = b.lastValidTouch, i = b.zoomHor || b.pinchHor, k = b.zoomVert || b.pinchVert, j = i || k, l = b.selectionMarker, m = {}, n = {}; a.type === "touchstart" && (e || j) && a.preventDefault();
            Ga(f, function (a) { return b.normalize(a) }); if (a.type === "touchstart") q(f, function (a, b) { d[b] = { chartX: a.chartX, chartY: a.chartY } }), h.x = [d[0].chartX, d[1] && d[1].chartX], h.y = [d[0].chartY, d[1] && d[1].chartY], q(c.axes, function (a) { if (a.zoomEnabled) { var b = c.bounds[a.horiz ? "h" : "v"], d = a.minPixelPadding, e = a.toPixels(a.dataMin), f = a.toPixels(a.dataMax), g = D(e, f), e = u(e, f); b.min = D(a.pos, g - d); b.max = u(a.pos + a.len, e + d) } }); else if (d.length) {
                if (!l) b.selectionMarker = l = v({ destroy: qa }, c.plotBox); i && b.pinchTranslateDirection(!0,
                d, f, m, l, n, h); k && b.pinchTranslateDirection(!1, d, f, m, l, n, h); b.hasPinched = j; b.scaleGroups(m, n); !j && e && g === 1 && this.runPointActions(b.normalize(a))
            }
        }, dragStart: function (a) { var b = this.chart; b.mouseIsDown = a.type; b.cancelClick = !1; b.mouseDownX = this.mouseDownX = a.chartX; this.mouseDownY = a.chartY }, drag: function (a) {
            var b = this.chart, c = b.options.chart, d = a.chartX, a = a.chartY, e = this.zoomHor, f = this.zoomVert, g = b.plotLeft, h = b.plotTop, i = b.plotWidth, k = b.plotHeight, j, l = this.mouseDownX, m = this.mouseDownY; d < g ? d = g : d > g + i && (d =
            g + i); a < h ? a = h : a > h + k && (a = h + k); this.hasDragged = Math.sqrt(Math.pow(l - d, 2) + Math.pow(m - a, 2)); if (this.hasDragged > 10) {
                j = b.isInsidePlot(l - g, m - h); if (b.hasCartesianSeries && (this.zoomX || this.zoomY) && j && !this.selectionMarker) this.selectionMarker = b.renderer.rect(g, h, e ? 1 : i, f ? 1 : k, 0).attr({ fill: c.selectionMarkerFill || "rgba(69,114,167,0.25)", zIndex: 7 }).add(); this.selectionMarker && e && (e = d - l, this.selectionMarker.attr({ width: T(e), x: (e > 0 ? 0 : e) + l })); this.selectionMarker && f && (e = a - m, this.selectionMarker.attr({
                    height: T(e),
                    y: (e > 0 ? 0 : e) + m
                })); j && !this.selectionMarker && c.panning && b.pan(d)
            }
        }, drop: function (a) {
            var b = this.chart, c = this.hasPinched; if (this.selectionMarker) {
                var d = { xAxis: [], yAxis: [], originalEvent: a.originalEvent || a }, e = this.selectionMarker, f = e.x, g = e.y, h; if (this.hasDragged || c) q(b.axes, function (a) { if (a.zoomEnabled) { var b = a.horiz, c = a.toValue(b ? f : g), b = a.toValue(b ? f + e.width : g + e.height); !isNaN(c) && !isNaN(b) && (d[a.xOrY + "Axis"].push({ axis: a, min: D(c, b), max: u(c, b) }), h = !0) } }), h && B(b, "selection", d, function (a) {
                    b.zoom(v(a, c ? { animation: !1 } :
                    null))
                }); this.selectionMarker = this.selectionMarker.destroy(); c && this.scaleGroups()
            } if (b) F(b.container, { cursor: b._cursor }), b.cancelClick = this.hasDragged > 10, b.mouseIsDown = this.hasDragged = this.hasPinched = !1, this.pinchDown = []
        }, onContainerMouseDown: function (a) { a = this.normalize(a); a.preventDefault && a.preventDefault(); this.dragStart(a) }, onDocumentMouseUp: function (a) { this.drop(a) }, onDocumentMouseMove: function (a) {
            var b = this.chart, c = this.chartPosition, d = b.hoverSeries, a = Zb(a); c && d && d.isCartesian && !b.isInsidePlot(a.pageX -
            c.left - b.plotLeft, a.pageY - c.top - b.plotTop) && this.reset()
        }, onContainerMouseLeave: function () { this.reset(); this.chartPosition = null }, onContainerMouseMove: function (a) { var b = this.chart, a = this.normalize(a); a.returnValue = !1; b.mouseIsDown === "mousedown" && this.drag(a); b.isInsidePlot(a.chartX - b.plotLeft, a.chartY - b.plotTop) && !b.openMenu && this.runPointActions(a) }, inClass: function (a, b) { for (var c; a;) { if (c = C(a, "class")) if (c.indexOf(b) !== -1) return !0; else if (c.indexOf("highcharts-container") !== -1) return !1; a = a.parentNode } },
        onTrackerMouseOut: function (a) { var b = this.chart.hoverSeries; if (b && !b.options.stickyTracking && !this.inClass(a.toElement || a.relatedTarget, "highcharts-tooltip")) b.onMouseOut() }, onContainerClick: function (a) {
            var b = this.chart, c = b.hoverPoint, d = b.plotLeft, e = b.plotTop, f = b.inverted, g, h, i, a = this.normalize(a); a.cancelBubble = !0; if (!b.cancelClick) c && this.inClass(a.target, "highcharts-tracker") ? (g = this.chartPosition, h = c.plotX, i = c.plotY, v(c, { pageX: g.left + d + (f ? b.plotWidth - i : h), pageY: g.top + e + (f ? b.plotHeight - h : i) }), B(c.series,
            "click", v(a, { point: c })), b.hoverPoint && c.firePointEvent("click", a)) : (v(a, this.getCoordinates(a)), b.isInsidePlot(a.chartX - d, a.chartY - e) && B(b, "click", a))
        }, onContainerTouchStart: function (a) { var b = this.chart; a.touches.length === 1 ? (a = this.normalize(a), b.isInsidePlot(a.chartX - b.plotLeft, a.chartY - b.plotTop) ? (this.runPointActions(a), this.pinch(a)) : this.reset()) : a.touches.length === 2 && this.pinch(a) }, onContainerTouchMove: function (a) { (a.touches.length === 1 || a.touches.length === 2) && this.pinch(a) }, onDocumentTouchEnd: function (a) { this.drop(a) },
        setDOMEvents: function () {
            var a = this, b = a.chart.container, c; this._events = c = [[b, "onmousedown", "onContainerMouseDown"], [b, "onmousemove", "onContainerMouseMove"], [b, "onclick", "onContainerClick"], [b, "mouseleave", "onContainerMouseLeave"], [G, "mousemove", "onDocumentMouseMove"], [G, "mouseup", "onDocumentMouseUp"]]; ib && c.push([b, "ontouchstart", "onContainerTouchStart"], [b, "ontouchmove", "onContainerTouchMove"], [G, "touchend", "onDocumentTouchEnd"]); q(c, function (b) {
                a["_" + b[2]] = function (c) { a[b[2]](c) }; b[1].indexOf("on") ===
                0 ? b[0][b[1]] = a["_" + b[2]] : E(b[0], b[1], a["_" + b[2]])
            })
        }, destroy: function () { var a = this; q(a._events, function (b) { b[1].indexOf("on") === 0 ? b[0][b[1]] = null : V(b[0], b[1], a["_" + b[2]]) }); delete a._events; clearInterval(a.tooltipTimeout) }
    }; Hb.prototype = {
        init: function (a, b) {
            var c = this, d = b.itemStyle, e = o(b.padding, 8), f = b.itemMarginTop || 0; this.options = b; if (b.enabled) c.baseline = z(d.fontSize) + 3 + f, c.itemStyle = d, c.itemHiddenStyle = x(d, b.itemHiddenStyle), c.itemMarginTop = f, c.padding = e, c.initialItemX = e, c.initialItemY = e - 5, c.maxItemWidth =
            0, c.chart = a, c.itemHeight = 0, c.lastLineHeight = 0, c.render(), E(c.chart, "endResize", function () { c.positionCheckboxes() })
        }, colorizeItem: function (a, b) { var c = this.options, d = a.legendItem, e = a.legendLine, f = a.legendSymbol, g = this.itemHiddenStyle.color, c = b ? c.itemStyle.color : g, h = b ? a.color : g, g = a.options && a.options.marker, i = { stroke: h, fill: h }, k; d && d.css({ fill: c, color: c }); e && e.attr({ stroke: h }); if (f) { if (g && f.isMarker) for (k in g = a.convertAttribs(g), g) d = g[k], d !== r && (i[k] = d); f.attr(i) } }, positionItem: function (a) {
            var b = this.options,
            c = b.symbolPadding, b = !b.rtl, d = a._legendItemPos, e = d[0], d = d[1], f = a.checkbox; a.legendGroup && a.legendGroup.translate(b ? e : this.legendWidth - e - 2 * c - 4, d); if (f) f.x = e, f.y = d
        }, destroyItem: function (a) { var b = a.checkbox; q(["legendItem", "legendLine", "legendSymbol", "legendGroup"], function (b) { a[b] && (a[b] = a[b].destroy()) }); b && Ya(a.checkbox) }, destroy: function () { var a = this.group, b = this.box; if (b) this.box = b.destroy(); if (a) this.group = a.destroy() }, positionCheckboxes: function (a) {
            var b = this.group.alignAttr, c, d = this.clipHeight ||
            this.legendHeight; if (b) c = b.translateY, q(this.allItems, function (e) { var f = e.checkbox, g; f && (g = c + f.y + (a || 0) + 3, F(f, { left: b.translateX + e.legendItemWidth + f.x - 20 + "px", top: g + "px", display: g > c - 6 && g < c + d - 6 ? "" : $ })) })
        }, renderTitle: function () {
            var a = this.padding, b = this.options.title, c = 0; if (b.text) { if (!this.title) this.title = this.chart.renderer.label(b.text, a - 3, a - 4, null, null, null, null, null, "legend-title").attr({ zIndex: 1 }).css(b.style).add(this.group); a = this.title.getBBox(); c = a.height; this.offsetWidth = a.width; this.contentGroup.attr({ translateY: c }) } this.titleHeight =
            c
        }, renderItem: function (a) {
            var A; var b = this, c = b.chart, d = c.renderer, e = b.options, f = e.layout === "horizontal", g = e.symbolWidth, h = e.symbolPadding, i = b.itemStyle, k = b.itemHiddenStyle, j = b.padding, l = f ? o(e.itemDistance, 8) : 0, m = !e.rtl, n = e.width, p = e.itemMarginBottom || 0, q = b.itemMarginTop, y = b.initialItemX, w = a.legendItem, M = a.series || a, s = M.options, r = s.showCheckbox, t = e.useHTML; if (!w && (a.legendGroup = d.g("legend-item").attr({ zIndex: 1 }).add(b.scrollGroup), M.drawLegendSymbol(b, a), a.legendItem = w = d.text(e.labelFormat ? Ma(e.labelFormat,
            a) : e.labelFormatter.call(a), m ? g + h : -h, b.baseline, t).css(x(a.visible ? i : k)).attr({ align: m ? "left" : "right", zIndex: 2 }).add(a.legendGroup), (t ? w : a.legendGroup).on("mouseover", function () { a.setState("hover"); w.css(b.options.itemHoverStyle) }).on("mouseout", function () { w.css(a.visible ? i : k); a.setState() }).on("click", function (b) { var c = function () { a.setVisible() }, b = { browserEvent: b }; a.firePointEvent ? a.firePointEvent("legendItemClick", b, c) : B(a, "legendItemClick", b, c) }), b.colorizeItem(a, a.visible), s && r)) a.checkbox = Z("input",
            { type: "checkbox", checked: a.selected, defaultChecked: a.selected }, e.itemCheckboxStyle, c.container), E(a.checkbox, "click", function (b) { B(a, "checkboxClick", { checked: b.target.checked }, function () { a.select() }) }); d = w.getBBox(); A = a.legendItemWidth = e.itemWidth || g + h + d.width + l + (r ? 20 : 0), e = A; b.itemHeight = g = d.height; if (f && b.itemX - y + e > (n || c.chartWidth - 2 * j - y)) b.itemX = y, b.itemY += q + b.lastLineHeight + p, b.lastLineHeight = 0; b.maxItemWidth = u(b.maxItemWidth, e); b.lastItemY = q + b.itemY + p; b.lastLineHeight = u(g, b.lastLineHeight); a._legendItemPos =
            [b.itemX, b.itemY]; f ? b.itemX += e : (b.itemY += q + g + p, b.lastLineHeight = g); b.offsetWidth = n || u((f ? b.itemX - y - l : e) + j, b.offsetWidth)
        }, render: function () {
            var a = this, b = a.chart, c = b.renderer, d = a.group, e, f, g, h, i = a.box, k = a.options, j = a.padding, l = k.borderWidth, m = k.backgroundColor; a.itemX = a.initialItemX; a.itemY = a.initialItemY; a.offsetWidth = 0; a.lastItemY = 0; if (!d) a.group = d = c.g("legend").attr({ zIndex: 7 }).add(), a.contentGroup = c.g().attr({ zIndex: 1 }).add(d), a.scrollGroup = c.g().add(a.contentGroup); a.renderTitle(); e = []; q(b.series,
            function (a) { var b = a.options; b.showInLegend && !t(b.linkedTo) && (e = e.concat(a.legendItems || (b.legendType === "point" ? a.data : a))) }); Sb(e, function (a, b) { return (a.options && a.options.legendIndex || 0) - (b.options && b.options.legendIndex || 0) }); k.reversed && e.reverse(); a.allItems = e; a.display = f = !!e.length; q(e, function (b) { a.renderItem(b) }); g = k.width || a.offsetWidth; h = a.lastItemY + a.lastLineHeight + a.titleHeight; h = a.handleOverflow(h); if (l || m) {
                g += j; h += j; if (i) {
                    if (g > 0 && h > 0) i[i.isNew ? "attr" : "animate"](i.crisp(null, null, null,
                    g, h)), i.isNew = !1
                } else a.box = i = c.rect(0, 0, g, h, k.borderRadius, l || 0).attr({ stroke: k.borderColor, "stroke-width": l || 0, fill: m || $ }).add(d).shadow(k.shadow), i.isNew = !0; i[f ? "show" : "hide"]()
            } a.legendWidth = g; a.legendHeight = h; q(e, function (b) { a.positionItem(b) }); f && d.align(v({ width: g, height: h }, k), !0, "spacingBox"); b.isResizing || this.positionCheckboxes()
        }, handleOverflow: function (a) {
            var b = this, c = this.chart, d = c.renderer, e = this.options, f = e.y, f = c.spacingBox.height + (e.verticalAlign === "top" ? -f : f) - this.padding, g = e.maxHeight,
            h = this.clipRect, i = e.navigation, k = o(i.animation, !0), j = i.arrowSize || 12, l = this.nav; e.layout === "horizontal" && (f /= 2); g && (f = D(f, g)); if (a > f && !e.useHTML) {
                this.clipHeight = c = f - 20 - this.titleHeight; this.pageCount = pa(a / c); this.currentPage = o(this.currentPage, 1); this.fullHeight = a; if (!h) h = b.clipRect = d.clipRect(0, 0, 9999, 0), b.contentGroup.clip(h); h.attr({ height: c }); if (!l) this.nav = l = d.g().attr({ zIndex: 1 }).add(this.group), this.up = d.symbol("triangle", 0, 0, j, j).on("click", function () { b.scroll(-1, k) }).add(l), this.pager = d.text("",
                15, 10).css(i.style).add(l), this.down = d.symbol("triangle-down", 0, 0, j, j).on("click", function () { b.scroll(1, k) }).add(l); b.scroll(0); a = f
            } else if (l) h.attr({ height: c.chartHeight }), l.hide(), this.scrollGroup.attr({ translateY: 1 }), this.clipHeight = 0; return a
        }, scroll: function (a, b) {
            var c = this.pageCount, d = this.currentPage + a, e = this.clipHeight, f = this.options.navigation, g = f.activeColor, h = f.inactiveColor, f = this.pager, i = this.padding; d > c && (d = c); if (d > 0) b !== r && Za(b, this.chart), this.nav.attr({
                translateX: i, translateY: e + 7 +
                this.titleHeight, visibility: "visible"
            }), this.up.attr({ fill: d === 1 ? h : g }).css({ cursor: d === 1 ? "default" : "pointer" }), f.attr({ text: d + "/" + this.pageCount }), this.down.attr({ x: 18 + this.pager.getBBox().width, fill: d === c ? h : g }).css({ cursor: d === c ? "default" : "pointer" }), e = -D(e * (d - 1), this.fullHeight - e + i) + 1, this.scrollGroup.animate({ translateY: e }), f.attr({ text: d + "/" + c }), this.currentPage = d, this.positionCheckboxes(e)
        }
    }; Ta.prototype = {
        init: function (a, b) {
            var c, d = a.series; a.series = null; c = x(N, a); c.series = a.series = d; var d = c.chart,
            e = d.margin, e = ba(e) ? e : [e, e, e, e]; this.optionsMarginTop = o(d.marginTop, e[0]); this.optionsMarginRight = o(d.marginRight, e[1]); this.optionsMarginBottom = o(d.marginBottom, e[2]); this.optionsMarginLeft = o(d.marginLeft, e[3]); e = d.events; this.bounds = { h: {}, v: {} }; this.callback = b; this.isResizing = 0; this.options = c; this.axes = []; this.series = []; this.hasCartesianSeries = d.showAxes; var f = this, g; f.index = Va.length; Va.push(f); d.reflow !== !1 && E(f, "load", function () { f.initReflow() }); if (e) for (g in e) E(f, g, e[g]); f.xAxis = []; f.yAxis =
            []; f.animation = ga ? !1 : o(d.animation, !0); f.pointCount = 0; f.counters = new Rb; f.firstRender()
        }, initSeries: function (a) { var b = this.options.chart; (b = L[a.type || b.type || b.defaultSeriesType]) || Da(17, !0); b = new b; b.init(this, a); return b }, addSeries: function (a, b, c) { var d, e = this; a && (b = o(b, !0), B(e, "addSeries", { options: a }, function () { d = e.initSeries(a); e.isDirtyLegend = !0; b && e.redraw(c) })); return d }, addAxis: function (a, b, c, d) {
            var e = b ? "xAxis" : "yAxis", f = this.options; new ua(this, x(a, { index: this[e].length, isX: b })); f[e] = ha(f[e] ||
            {}); f[e].push(a); o(c, !0) && this.redraw(d)
        }, isInsidePlot: function (a, b, c) { var d = c ? b : a, a = c ? a : b; return d >= 0 && d <= this.plotWidth && a >= 0 && a <= this.plotHeight }, adjustTickAmounts: function () { this.options.chart.alignTicks !== !1 && q(this.axes, function (a) { a.adjustTickAmount() }); this.maxTicks = null }, redraw: function (a) {
            var b = this.axes, c = this.series, d = this.pointer, e = this.legend, f = this.isDirtyLegend, g, h, i = this.isDirtyBox, k = c.length, j = k, l = this.renderer, m = l.isHidden(), n = []; Za(a, this); m && this.cloneRenderTo(); for (this.layOutTitles() ; j--;) if (a =
            c[j], a.options.stacking && (g = !0, a.isDirty)) { h = !0; break } if (h) for (j = k; j--;) if (a = c[j], a.options.stacking) a.isDirty = !0; q(c, function (a) { a.isDirty && a.options.legendType === "point" && (f = !0) }); if (f && e.options.enabled) e.render(), this.isDirtyLegend = !1; g && this.getStacks(); if (this.hasCartesianSeries) {
                if (!this.isResizing) this.maxTicks = null, q(b, function (a) { a.setScale() }); this.adjustTickAmounts(); this.getMargins(); q(b, function (a) {
                    if (a.isDirtyExtremes) a.isDirtyExtremes = !1, n.push(function () { B(a, "afterSetExtremes", a.getExtremes()) });
                    if (a.isDirty || i || g) a.redraw(), i = !0
                })
            } i && this.drawChartBox(); q(c, function (a) { a.isDirty && a.visible && (!a.isCartesian || a.xAxis) && a.redraw() }); d && d.reset && d.reset(!0); l.draw(); B(this, "redraw"); m && this.cloneRenderTo(!0); q(n, function (a) { a.call() })
        }, showLoading: function (a) {
            var b = this.options, c = this.loadingDiv, d = b.loading; if (!c) this.loadingDiv = c = Z(Ra, { className: "highcharts-loading" }, v(d.style, { zIndex: 10, display: $ }), this.container), this.loadingSpan = Z("span", null, d.labelStyle, c); this.loadingSpan.innerHTML = a ||
            b.lang.loading; if (!this.loadingShown) F(c, { opacity: 0, display: "", left: this.plotLeft + "px", top: this.plotTop + "px", width: this.plotWidth + "px", height: this.plotHeight + "px" }), Mb(c, { opacity: d.style.opacity }, { duration: d.showDuration || 0 }), this.loadingShown = !0
        }, hideLoading: function () { var a = this.options, b = this.loadingDiv; b && Mb(b, { opacity: 0 }, { duration: a.loading.hideDuration || 100, complete: function () { F(b, { display: $ }) } }); this.loadingShown = !1 }, get: function (a) {
            var b = this.axes, c = this.series, d, e; for (d = 0; d < b.length; d++) if (b[d].options.id ===
            a) return b[d]; for (d = 0; d < c.length; d++) if (c[d].options.id === a) return c[d]; for (d = 0; d < c.length; d++) { e = c[d].points || []; for (b = 0; b < e.length; b++) if (e[b].id === a) return e[b] } return null
        }, getAxes: function () { var a = this, b = this.options, c = b.xAxis = ha(b.xAxis || {}), b = b.yAxis = ha(b.yAxis || {}); q(c, function (a, b) { a.index = b; a.isX = !0 }); q(b, function (a, b) { a.index = b }); c = c.concat(b); q(c, function (b) { new ua(a, b) }); a.adjustTickAmounts() }, getSelectedPoints: function () {
            var a = []; q(this.series, function (b) {
                a = a.concat(Eb(b.points || [],
                function (a) { return a.selected }))
            }); return a
        }, getSelectedSeries: function () { return Eb(this.series, function (a) { return a.selected }) }, getStacks: function () { var a = this; q(a.yAxis, function (a) { if (a.stacks && a.hasVisibleSeries) a.oldStacks = a.stacks }); q(a.series, function (b) { if (b.options.stacking && (b.visible === !0 || a.options.chart.ignoreHiddenSeries === !1)) b.stackKey = b.type + o(b.options.stack, "") }) }, showResetZoom: function () {
            var a = this, b = N.lang, c = a.options.chart.resetZoomButton, d = c.theme, e = d.states, f = c.relativeTo ===
            "chart" ? null : "plotBox"; this.resetZoomButton = a.renderer.button(b.resetZoom, null, null, function () { a.zoomOut() }, d, e && e.hover).attr({ align: c.position.align, title: b.resetZoomTitle }).add().align(c.position, !1, f)
        }, zoomOut: function () { var a = this; B(a, "selection", { resetSelection: !0 }, function () { a.zoom() }) }, zoom: function (a) {
            var b, c = this.pointer, d = !1, e; !a || a.resetSelection ? q(this.axes, function (a) { b = a.zoom() }) : q(a.xAxis.concat(a.yAxis), function (a) {
                var e = a.axis, h = e.isXAxis; if (c[h ? "zoomX" : "zoomY"] || c[h ? "pinchX" : "pinchY"]) b =
                e.zoom(a.min, a.max), e.displayBtn && (d = !0)
            }); e = this.resetZoomButton; if (d && !e) this.showResetZoom(); else if (!d && ba(e)) this.resetZoomButton = e.destroy(); b && this.redraw(o(this.options.chart.animation, a && a.animation, this.pointCount < 100))
        }, pan: function (a) {
            var b = this.xAxis[0], c = this.mouseDownX, d = b.pointRange / 2, e = b.getExtremes(), f = b.translate(c - a, !0) + d, c = b.translate(c + this.plotWidth - a, !0) - d; (d = this.hoverPoints) && q(d, function (a) { a.setState() }); b.series.length && f > D(e.dataMin, e.min) && c < u(e.dataMax, e.max) && b.setExtremes(f,
            c, !0, !1, { trigger: "pan" }); this.mouseDownX = a; F(this.container, { cursor: "move" })
        }, setTitle: function (a, b) { var f; var c = this, d = c.options, e; e = d.title = x(d.title, a); f = d.subtitle = x(d.subtitle, b), d = f; q([["title", a, e], ["subtitle", b, d]], function (a) { var b = a[0], d = c[b], e = a[1], a = a[2]; d && e && (c[b] = d = d.destroy()); a && a.text && !d && (c[b] = c.renderer.text(a.text, 0, 0, a.useHTML).attr({ align: a.align, "class": "highcharts-" + b, zIndex: a.zIndex || 4 }).css(a.style).add()) }); c.layOutTitles() }, layOutTitles: function () {
            var a = 0, b = this.title,
            c = this.subtitle, d = this.options, e = d.title, d = d.subtitle, f = this.spacingBox.width - 44; if (b && (b.css({ width: (e.width || f) + "px" }).align(v({ y: 15 }, e), !1, "spacingBox"), !e.floating && !e.verticalAlign)) a = b.getBBox().height, a >= 18 && a <= 25 && (a = 15); c && (c.css({ width: (d.width || f) + "px" }).align(v({ y: a + e.margin }, d), !1, "spacingBox"), !d.floating && !d.verticalAlign && (a = pa(a + c.getBBox().height))); this.titleOffset = a
        }, getChartSize: function () {
            var a = this.options.chart, b = this.renderToClone || this.renderTo; this.containerWidth = vb(b, "width");
            this.containerHeight = vb(b, "height"); this.chartWidth = u(0, a.width || this.containerWidth || 600); this.chartHeight = u(0, o(a.height, this.containerHeight > 19 ? this.containerHeight : 400))
        }, cloneRenderTo: function (a) {
            var b = this.renderToClone, c = this.container; a ? b && (this.renderTo.appendChild(c), Ya(b), delete this.renderToClone) : (c && c.parentNode === this.renderTo && this.renderTo.removeChild(c), this.renderToClone = b = this.renderTo.cloneNode(0), F(b, { position: "absolute", top: "-9999px", display: "block" }), G.body.appendChild(b),
            c && b.appendChild(c))
        }, getContainer: function () {
            var a, b = this.options.chart, c, d, e; this.renderTo = a = b.renderTo; e = "highcharts-" + Kb++; if (ja(a)) this.renderTo = a = G.getElementById(a); a || Da(13, !0); c = z(C(a, "data-highcharts-chart")); !isNaN(c) && Va[c] && Va[c].destroy(); C(a, "data-highcharts-chart", this.index); a.innerHTML = ""; a.offsetWidth || this.cloneRenderTo(); this.getChartSize(); c = this.chartWidth; d = this.chartHeight; this.container = a = Z(Ra, { className: "highcharts-container" + (b.className ? " " + b.className : ""), id: e }, v({
                position: "relative",
                overflow: "hidden", width: c + "px", height: d + "px", textAlign: "left", lineHeight: "normal", zIndex: 0, "-webkit-tap-highlight-color": "rgba(0,0,0,0)"
            }, b.style), this.renderToClone || a); this._cursor = a.style.cursor; this.renderer = b.forExport ? new Ha(a, c, d, !0) : new bb(a, c, d); ga && this.renderer.create(this, a, c, d)
        }, getMargins: function () {
            var a = this.options.chart, b = a.spacingTop, c = a.spacingRight, d = a.spacingBottom, a = a.spacingLeft, e, f = this.legend, g = this.optionsMarginTop, h = this.optionsMarginLeft, i = this.optionsMarginRight, k = this.optionsMarginBottom,
            j = this.options.legend, l = o(j.margin, 10), m = j.x, n = j.y, p = j.align, K = j.verticalAlign, y = this.titleOffset; this.resetMargins(); e = this.axisOffset; if (y && !t(g)) this.plotTop = u(this.plotTop, y + this.options.title.margin + b); if (f.display && !j.floating) if (p === "right") { if (!t(i)) this.marginRight = u(this.marginRight, f.legendWidth - m + l + c) } else if (p === "left") { if (!t(h)) this.plotLeft = u(this.plotLeft, f.legendWidth + m + l + a) } else if (K === "top") { if (!t(g)) this.plotTop = u(this.plotTop, f.legendHeight + n + l + b) } else if (K === "bottom" && !t(k)) this.marginBottom =
            u(this.marginBottom, f.legendHeight - n + l + d); this.extraBottomMargin && (this.marginBottom += this.extraBottomMargin); this.extraTopMargin && (this.plotTop += this.extraTopMargin); this.hasCartesianSeries && q(this.axes, function (a) { a.getOffset() }); t(h) || (this.plotLeft += e[3]); t(g) || (this.plotTop += e[0]); t(k) || (this.marginBottom += e[2]); t(i) || (this.marginRight += e[1]); this.setChartSize()
        }, initReflow: function () {
            function a(a) {
                var g = c.width || vb(d, "width"), h = c.height || vb(d, "height"), a = a ? a.target : X; if (!b.hasUserSize && g && h &&
                (a === X || a === G)) { if (g !== b.containerWidth || h !== b.containerHeight) clearTimeout(e), b.reflowTimeout = e = setTimeout(function () { if (b.container) b.setSize(g, h, !1), b.hasUserSize = null }, 100); b.containerWidth = g; b.containerHeight = h }
            } var b = this, c = b.options.chart, d = b.renderTo, e; E(X, "resize", a); E(b, "destroy", function () { V(X, "resize", a) })
        }, setSize: function (a, b, c) {
            var d = this, e, f, g; d.isResizing += 1; g = function () { d && B(d, "endResize", null, function () { d.isResizing -= 1 }) }; Za(c, d); d.oldChartHeight = d.chartHeight; d.oldChartWidth = d.chartWidth;
            if (t(a)) d.chartWidth = e = u(0, s(a)), d.hasUserSize = !!e; if (t(b)) d.chartHeight = f = u(0, s(b)); F(d.container, { width: e + "px", height: f + "px" }); d.setChartSize(!0); d.renderer.setSize(e, f, c); d.maxTicks = null; q(d.axes, function (a) { a.isDirty = !0; a.setScale() }); q(d.series, function (a) { a.isDirty = !0 }); d.isDirtyLegend = !0; d.isDirtyBox = !0; d.getMargins(); d.redraw(c); d.oldChartHeight = null; B(d, "resize"); Sa === !1 ? g() : setTimeout(g, Sa && Sa.duration || 500)
        }, setChartSize: function (a) {
            var b = this.inverted, c = this.renderer, d = this.chartWidth,
            e = this.chartHeight, f = this.options.chart, g = f.spacingTop, h = f.spacingRight, i = f.spacingBottom, k = f.spacingLeft, j = this.clipOffset, l, m, n, p; this.plotLeft = l = s(this.plotLeft); this.plotTop = m = s(this.plotTop); this.plotWidth = n = u(0, s(d - l - this.marginRight)); this.plotHeight = p = u(0, s(e - m - this.marginBottom)); this.plotSizeX = b ? p : n; this.plotSizeY = b ? n : p; this.plotBorderWidth = b = f.plotBorderWidth || 0; this.spacingBox = c.spacingBox = { x: k, y: g, width: d - k - h, height: e - g - i }; this.plotBox = c.plotBox = { x: l, y: m, width: n, height: p }; c = pa(u(b, j[3]) /
            2); d = pa(u(b, j[0]) / 2); this.clipBox = { x: c, y: d, width: U(this.plotSizeX - u(b, j[1]) / 2 - c), height: U(this.plotSizeY - u(b, j[2]) / 2 - d) }; a || q(this.axes, function (a) { a.setAxisSize(); a.setAxisTranslation() })
        }, resetMargins: function () {
            var a = this.options.chart, b = a.spacingRight, c = a.spacingBottom, d = a.spacingLeft; this.plotTop = o(this.optionsMarginTop, a.spacingTop); this.marginRight = o(this.optionsMarginRight, b); this.marginBottom = o(this.optionsMarginBottom, c); this.plotLeft = o(this.optionsMarginLeft, d); this.axisOffset = [0, 0, 0, 0];
            this.clipOffset = [0, 0, 0, 0]
        }, drawChartBox: function () {
            var a = this.options.chart, b = this.renderer, c = this.chartWidth, d = this.chartHeight, e = this.chartBackground, f = this.plotBackground, g = this.plotBorder, h = this.plotBGImage, i = a.borderWidth || 0, k = a.backgroundColor, j = a.plotBackgroundColor, l = a.plotBackgroundImage, m = a.plotBorderWidth || 0, n, p = this.plotLeft, o = this.plotTop, q = this.plotWidth, w = this.plotHeight, s = this.plotBox, u = this.clipRect, r = this.clipBox; n = i + (a.shadow ? 8 : 0); if (i || k) if (e) e.animate(e.crisp(null, null, null, c -
            n, d - n)); else { e = { fill: k || $ }; if (i) e.stroke = a.borderColor, e["stroke-width"] = i; this.chartBackground = b.rect(n / 2, n / 2, c - n, d - n, a.borderRadius, i).attr(e).add().shadow(a.shadow) } if (j) f ? f.animate(s) : this.plotBackground = b.rect(p, o, q, w, 0).attr({ fill: j }).add().shadow(a.plotShadow); if (l) h ? h.animate(s) : this.plotBGImage = b.image(l, p, o, q, w).add(); u ? u.animate({ width: r.width, height: r.height }) : this.clipRect = b.clipRect(r); if (m) g ? g.animate(g.crisp(null, p, o, q, w)) : this.plotBorder = b.rect(p, o, q, w, 0, m).attr({
                stroke: a.plotBorderColor,
                "stroke-width": m, zIndex: 1
            }).add(); this.isDirtyBox = !1
        }, propFromSeries: function () { var a = this, b = a.options.chart, c, d = a.options.series, e, f; q(["inverted", "angular", "polar"], function (g) { c = L[b.type || b.defaultSeriesType]; f = a[g] || b[g] || c && c.prototype[g]; for (e = d && d.length; !f && e--;) (c = L[d[e].type]) && c.prototype[g] && (f = !0); a[g] = f }) }, render: function () {
            var a = this, b = a.axes, c = a.renderer, d = a.options, e = d.labels, f = d.credits, g; a.setTitle(); a.legend = new Hb(a, d.legend); a.getStacks(); q(b, function (a) { a.setScale() }); a.getMargins();
            a.maxTicks = null; q(b, function (a) { a.setTickPositions(!0); a.setMaxTicks() }); a.adjustTickAmounts(); a.getMargins(); a.drawChartBox(); a.hasCartesianSeries && q(b, function (a) { a.render() }); if (!a.seriesGroup) a.seriesGroup = c.g("series-group").attr({ zIndex: 3 }).add(); q(a.series, function (a) { a.translate(); a.setTooltipPoints(); a.render() }); e.items && q(e.items, function (b) { var d = v(e.style, b.style), f = z(d.left) + a.plotLeft, g = z(d.top) + a.plotTop + 12; delete d.left; delete d.top; c.text(b.html, f, g).attr({ zIndex: 2 }).css(d).add() });
            if (f.enabled && !a.credits) g = f.href, a.credits = c.text(f.text, 0, 0).on("click", function () { if (g) location.href = g }).attr({ align: f.position.align, zIndex: 8 }).css(f.style).add().align(f.position); a.hasRendered = !0
        }, destroy: function () {
            var a = this, b = a.axes, c = a.series, d = a.container, e, f = d && d.parentNode; B(a, "destroy"); Va[a.index] = r; a.renderTo.removeAttribute("data-highcharts-chart"); V(a); for (e = b.length; e--;) b[e] = b[e].destroy(); for (e = c.length; e--;) c[e] = c[e].destroy(); q("title,subtitle,chartBackground,plotBackground,plotBGImage,plotBorder,seriesGroup,clipRect,credits,pointer,scroller,rangeSelector,legend,resetZoomButton,tooltip,renderer".split(","),
            function (b) { var c = a[b]; c && c.destroy && (a[b] = c.destroy()) }); if (d) d.innerHTML = "", V(d), f && Ya(d); for (e in a) delete a[e]
        }, isReadyToRender: function () { var a = this; return !ea && X == X.top && G.readyState !== "complete" || ga && !X.canvg ? (ga ? $b.push(function () { a.firstRender() }, a.options.global.canvasToolsURL) : G.attachEvent("onreadystatechange", function () { G.detachEvent("onreadystatechange", a.firstRender); G.readyState === "complete" && a.firstRender() }), !1) : !0 }, firstRender: function () {
            var a = this, b = a.options, c = a.callback; if (a.isReadyToRender()) a.getContainer(),
            B(a, "init"), a.resetMargins(), a.setChartSize(), a.propFromSeries(), a.getAxes(), q(b.series || [], function (b) { a.initSeries(b) }), B(a, "beforeRender"), a.pointer = new rb(a, b), a.render(), a.renderer.draw(), c && c.apply(a, [a]), q(a.callbacks, function (b) { b.apply(a, [a]) }), a.cloneRenderTo(!0), B(a, "load")
        }
    }; Ta.prototype.callbacks = []; var Ia = function () { }; Ia.prototype = {
        init: function (a, b, c) {
            this.series = a; this.applyOptions(b, c); this.pointAttr = {}; if (a.options.colorByPoint && (b = a.options.colors || a.chart.options.colors, this.color =
            this.color || b[a.colorCounter++], a.colorCounter === b.length)) a.colorCounter = 0; a.chart.pointCount++; return this
        }, applyOptions: function (a, b) { var c = this.series, d = c.pointValKey, a = Ia.prototype.optionsToObject.call(this, a); v(this, a); this.options = this.options ? v(this.options, a) : a; if (d) this.y = this[d]; if (this.x === r && c) this.x = b === r ? c.autoIncrement() : b; return this }, optionsToObject: function (a) {
            var b, c = this.series, d = c.pointArrayMap || ["y"], e = d.length, f = 0, g = 0; if (typeof a === "number" || a === null) b = { y: a }; else if (Wa(a)) {
                b =
                {}; if (a.length > e) { c = typeof a[0]; if (c === "string") b.name = a[0]; else if (c === "number") b.x = a[0]; f++ } for (; g < e;) b[d[g++]] = a[f++]
            } else if (typeof a === "object") { b = a; if (a.dataLabels) c._hasPointLabels = !0; if (a.marker) c._hasPointMarkers = !0 } return b
        }, destroy: function () {
            var a = this.series.chart, b = a.hoverPoints, c; a.pointCount--; if (b && (this.setState(), la(b, this), !b.length)) a.hoverPoints = null; if (this === a.hoverPoint) this.onMouseOut(); if (this.graphic || this.dataLabel) V(this), this.destroyElements(); this.legendItem && a.legend.destroyItem(this);
            for (c in this) this[c] = null
        }, destroyElements: function () { for (var a = "graphic,dataLabel,dataLabelUpper,group,connector,shadowGroup".split(","), b, c = 6; c--;) b = a[c], this[b] && (this[b] = this[b].destroy()) }, getLabelConfig: function () { return { x: this.category, y: this.y, key: this.name || this.category, series: this.series, point: this, percentage: this.percentage, total: this.total || this.stackTotal } }, select: function (a, b) {
            var c = this, d = c.series, e = d.chart, a = o(a, !c.selected); c.firePointEvent(a ? "select" : "unselect", { accumulate: b },
            function () { c.selected = c.options.selected = a; d.options.data[va(c, d.data)] = c.options; c.setState(a && "select"); b || q(e.getSelectedPoints(), function (a) { if (a.selected && a !== c) a.selected = a.options.selected = !1, d.options.data[va(a, d.data)] = a.options, a.setState(""), a.firePointEvent("unselect") }) })
        }, onMouseOver: function (a) {
            var b = this.series, c = b.chart, d = c.tooltip, e = c.hoverPoint; if (e && e !== this) e.onMouseOut(); this.firePointEvent("mouseOver"); d && (!d.shared || b.noSharedTooltip) && d.refresh(this, a); this.setState("hover");
            c.hoverPoint = this
        }, onMouseOut: function () { var a = this.series.chart, b = a.hoverPoints; if (!b || va(this, b) === -1) this.firePointEvent("mouseOut"), this.setState(), a.hoverPoint = null }, tooltipFormatter: function (a) { var b = this.series, c = b.tooltipOptions, d = o(c.valueDecimals, ""), e = c.valuePrefix || "", f = c.valueSuffix || ""; q(b.pointArrayMap || ["y"], function (b) { b = "{point." + b; if (e || f) a = a.replace(b + "}", e + b + "}" + f); a = a.replace(b + "}", b + ":,." + d + "f}") }); return Ma(a, { point: this, series: this.series }) }, update: function (a, b, c) {
            var d = this,
            e = d.series, f = d.graphic, g, h = e.data, i = e.chart, k = e.options, b = o(b, !0); d.firePointEvent("update", { options: a }, function () { d.applyOptions(a); ba(a) && (e.getAttribs(), f && f.attr(d.pointAttr[e.state])); g = va(d, h); e.xData[g] = d.x; e.yData[g] = e.toYData ? e.toYData(d) : d.y; e.zData[g] = d.z; k.data[g] = d.options; e.isDirty = e.isDirtyData = i.isDirtyBox = !0; k.legendType === "point" && i.legend.destroyItem(d); b && i.redraw(c) })
        }, remove: function (a, b) {
            var c = this, d = c.series, e = d.chart, f, g = d.data; Za(b, e); a = o(a, !0); c.firePointEvent("remove",
            null, function () { f = va(c, g); g.splice(f, 1); d.options.data.splice(f, 1); d.xData.splice(f, 1); d.yData.splice(f, 1); d.zData.splice(f, 1); c.destroy(); d.isDirty = !0; d.isDirtyData = !0; a && e.redraw() })
        }, firePointEvent: function (a, b, c) { var d = this, e = this.series.options; (e.point.events[a] || d.options && d.options.events && d.options.events[a]) && this.importEvents(); a === "click" && e.allowPointSelect && (c = function (a) { d.select(null, a.ctrlKey || a.metaKey || a.shiftKey) }); B(this, a, b, c) }, importEvents: function () {
            if (!this.hasImportedEvents) {
                var a =
                x(this.series.options.point, this.options).events, b; this.events = a; for (b in a) E(this, b, a[b]); this.hasImportedEvents = !0
            }
        }, setState: function (a) {
            var b = this.plotX, c = this.plotY, d = this.series, e = d.options.states, f = Q[d.type].marker && d.options.marker, g = f && !f.enabled, h = f && f.states[a], i = h && h.enabled === !1, k = d.stateMarkerGraphic, j = this.marker || {}, l = d.chart, m = this.pointAttr, a = a || ""; if (!(a === this.state || this.selected && a !== "select" || e[a] && e[a].enabled === !1 || a && (i || g && !h.enabled))) {
                if (this.graphic) e = f && this.graphic.symbolName &&
                m[a].r, this.graphic.attr(x(m[a], e ? { x: b - e, y: c - e, width: 2 * e, height: 2 * e } : {})); else { if (a && h) e = h.radius, j = j.symbol || d.symbol, k && k.currentSymbol !== j && (k = k.destroy()), k ? k.attr({ x: b - e, y: c - e }) : (d.stateMarkerGraphic = k = l.renderer.symbol(j, b - e, c - e, 2 * e, 2 * e).attr(m[a]).add(d.markerGroup), k.currentSymbol = j); if (k) k[a && l.isInsidePlot(b, c) ? "show" : "hide"]() } this.state = a
            }
        }
    }; var W = function () { }; W.prototype = {
        isCartesian: !0, type: "line", pointClass: Ia, sorted: !0, requireSorting: !0, pointAttrToOptions: {
            stroke: "lineColor", "stroke-width": "lineWidth",
            fill: "fillColor", r: "radius"
        }, colorCounter: 0, init: function (a, b) {
            var c, d, e = a.series; this.chart = a; this.options = b = this.setOptions(b); this.bindAxes(); v(this, { name: b.name, state: "", pointAttr: {}, visible: b.visible !== !1, selected: b.selected === !0 }); if (ga) b.animation = !1; d = b.events; for (c in d) E(this, c, d[c]); if (d && d.click || b.point && b.point.events && b.point.events.click || b.allowPointSelect) a.runTrackerClick = !0; this.getColor(); this.getSymbol(); this.setData(b.data, !1); if (this.isCartesian) a.hasCartesianSeries = !0; e.push(this);
            this._i = e.length - 1; Sb(e, function (a, b) { return o(a.options.index, a._i) - o(b.options.index, a._i) }); q(e, function (a, b) { a.index = b; a.name = a.name || "Series " + (b + 1) }); c = b.linkedTo; this.linkedSeries = []; if (ja(c) && (c = c === ":previous" ? e[this.index - 1] : a.get(c))) c.linkedSeries.push(this), this.linkedParent = c
        }, bindAxes: function () {
            var a = this, b = a.options, c = a.chart, d; a.isCartesian && q(["xAxis", "yAxis"], function (e) {
                q(c[e], function (c) {
                    d = c.options; if (b[e] === d.index || b[e] !== r && b[e] === d.id || b[e] === r && d.index === 0) c.series.push(a),
                    a[e] = c, c.isDirty = !0
                }); a[e] || Da(18, !0)
            })
        }, autoIncrement: function () { var a = this.options, b = this.xIncrement, b = o(b, a.pointStart, 0); this.pointInterval = o(this.pointInterval, a.pointInterval, 1); this.xIncrement = b + this.pointInterval; return b }, getSegments: function () {
            var a = -1, b = [], c, d = this.points, e = d.length; if (e) if (this.options.connectNulls) { for (c = e; c--;) d[c].y === null && d.splice(c, 1); d.length && (b = [d]) } else q(d, function (c, g) { c.y === null ? (g > a + 1 && b.push(d.slice(a + 1, g)), a = g) : g === e - 1 && b.push(d.slice(a + 1, g + 1)) }); this.segments =
            b
        }, setOptions: function (a) { var b = this.chart.options, c = b.plotOptions, d = c[this.type]; this.userOptions = a; a = x(d, c.series, a); this.tooltipOptions = x(b.tooltip, a.tooltip); d.marker === null && delete a.marker; return a }, getColor: function () { var a = this.options, b = this.userOptions, c = this.chart.options.colors, d = this.chart.counters, e; e = a.color || Q[this.type].color; if (!e && !a.colorByPoint) t(b._colorIndex) ? a = b._colorIndex : (b._colorIndex = d.color, a = d.color++), e = c[a]; this.color = e; d.wrapColor(c.length) }, getSymbol: function () {
            var a =
            this.userOptions, b = this.options.marker, c = this.chart, d = c.options.symbols, c = c.counters; this.symbol = b.symbol; if (!this.symbol) t(a._symbolIndex) ? a = a._symbolIndex : (a._symbolIndex = c.symbol, a = c.symbol++), this.symbol = d[a]; if (/^url/.test(this.symbol)) b.radius = 0; c.wrapSymbol(d.length)
        }, drawLegendSymbol: function (a) {
            var b = this.options, c = b.marker, d = a.options, e; e = d.symbolWidth; var f = this.chart.renderer, g = this.legendGroup, a = a.baseline - s(f.fontMetrics(d.itemStyle.fontSize).b * 0.3); if (b.lineWidth) {
                d = { "stroke-width": b.lineWidth };
                if (b.dashStyle) d.dashstyle = b.dashStyle; this.legendLine = f.path(["M", 0, a, "L", e, a]).attr(d).add(g)
            } if (c && c.enabled) b = c.radius, this.legendSymbol = e = f.symbol(this.symbol, e / 2 - b, a - b, 2 * b, 2 * b).add(g), e.isMarker = !0
        }, addPoint: function (a, b, c, d) {
            var e = this.options, f = this.data, g = this.graph, h = this.area, i = this.chart, k = this.xData, j = this.yData, l = this.zData, m = this.names, n = g && g.shift || 0, p = e.data; Za(d, i); c && q([g, h, this.graphNeg, this.areaNeg], function (a) { if (a) a.shift = n + 1 }); if (h) h.isArea = !0; b = o(b, !0); d = { series: this }; this.pointClass.prototype.applyOptions.apply(d,
            [a]); k.push(d.x); j.push(this.toYData ? this.toYData(d) : d.y); l.push(d.z); if (m) m[d.x] = d.name; p.push(a); e.legendType === "point" && this.generatePoints(); c && (f[0] && f[0].remove ? f[0].remove(!1) : (f.shift(), k.shift(), j.shift(), l.shift(), p.shift())); this.isDirtyData = this.isDirty = !0; b && (this.getAttribs(), i.redraw())
        }, setData: function (a, b) {
            var c = this.points, d = this.options, e = this.chart, f = null, g = this.xAxis, h = g && g.categories && !g.categories.length ? [] : null, i; this.xIncrement = null; this.pointRange = g && g.categories ? 1 : d.pointRange;
            this.colorCounter = 0; var k = [], j = [], l = [], m = a ? a.length : []; i = o(d.turboThreshold, 1E3); var n = this.pointArrayMap, n = n && n.length, p = !!this.toYData; if (i && m > i) { for (i = 0; f === null && i < m;) f = a[i], i++; if (ra(f)) { f = o(d.pointStart, 0); d = o(d.pointInterval, 1); for (i = 0; i < m; i++) k[i] = f, j[i] = a[i], f += d; this.xIncrement = f } else if (Wa(f)) if (n) for (i = 0; i < m; i++) d = a[i], k[i] = d[0], j[i] = d.slice(1, n + 1); else for (i = 0; i < m; i++) d = a[i], k[i] = d[0], j[i] = d[1] } else for (i = 0; i < m; i++) if (a[i] !== r && (d = { series: this }, this.pointClass.prototype.applyOptions.apply(d,
            [a[i]]), k[i] = d.x, j[i] = p ? this.toYData(d) : d.y, l[i] = d.z, h && d.name)) h[d.x] = d.name; ja(j[0]) && Da(14, !0); this.data = []; this.options.data = a; this.xData = k; this.yData = j; this.zData = l; this.names = h; for (i = c && c.length || 0; i--;) c[i] && c[i].destroy && c[i].destroy(); if (g) g.minRange = g.userMinRange; this.isDirty = this.isDirtyData = e.isDirtyBox = !0; o(b, !0) && e.redraw(!1)
        }, remove: function (a, b) {
            var c = this, d = c.chart, a = o(a, !0); if (!c.isRemoving) c.isRemoving = !0, B(c, "remove", null, function () {
                c.destroy(); d.isDirtyLegend = d.isDirtyBox = !0;
                a && d.redraw(b)
            }); c.isRemoving = !1
        }, processData: function (a) {
            var b = this.xData, c = this.yData, d = b.length, e; e = 0; var f, g, h = this.xAxis, i = this.options, k = i.cropThreshold, j = this.isCartesian; if (j && !this.isDirty && !h.isDirty && !this.yAxis.isDirty && !a) return !1; if (j && this.sorted && (!k || d > k || this.forceCrop)) if (a = h.min, h = h.max, b[d - 1] < a || b[0] > h) b = [], c = []; else if (b[0] < a || b[d - 1] > h) e = this.cropData(this.xData, this.yData, a, h), b = e.xData, c = e.yData, e = e.start, f = !0; for (h = b.length - 1; h >= 0; h--) d = b[h] - b[h - 1], d > 0 && (g === r || d < g) ? g = d : d <
            0 && this.requireSorting && Da(15); this.cropped = f; this.cropStart = e; this.processedXData = b; this.processedYData = c; if (i.pointRange === null) this.pointRange = g || 1; this.closestPointRange = g
        }, cropData: function (a, b, c, d) { var e = a.length, f = 0, g = e, h; for (h = 0; h < e; h++) if (a[h] >= c) { f = u(0, h - 1); break } for (; h < e; h++) if (a[h] > d) { g = h + 1; break } return { xData: a.slice(f, g), yData: b.slice(f, g), start: f, end: g } }, generatePoints: function () {
            var a = this.options.data, b = this.data, c, d = this.processedXData, e = this.processedYData, f = this.pointClass, g = d.length,
            h = this.cropStart || 0, i, k = this.hasGroupedData, j, l = [], m; if (!b && !k) b = [], b.length = a.length, b = this.data = b; for (m = 0; m < g; m++) i = h + m, k ? l[m] = (new f).init(this, [d[m]].concat(ha(e[m]))) : (b[i] ? j = b[i] : a[i] !== r && (b[i] = j = (new f).init(this, a[i], d[m])), l[m] = j); if (b && (g !== (c = b.length) || k)) for (m = 0; m < c; m++) if (m === h && !k && (m += g), b[m]) b[m].destroyElements(), b[m].plotX = r; this.data = b; this.points = l
        }, setStackedPoints: function () {
            if (this.options.stacking && !(this.visible !== !0 && this.chart.options.chart.ignoreHiddenSeries !== !1)) {
                var a =
                this.processedXData, b = this.processedYData, c = b.length, d = this.options, e = d.threshold, f = d.stack, d = d.stacking, g = this.stackKey, h = "-" + g, i = this.yAxis, k = i.stacks, j = i.oldStacks, l = i.stacksMax, m, n, p, o, q, w; for (q = 0; q < c; q++) {
                    n = a[q]; w = b[q]; o = (m = w < e) ? h : g; l[o] || (l[o] = w); k[o] || (k[o] = {}); if (!k[o][n]) j[o] && j[o][n] ? (k[o][n] = j[o][n], k[o][n].total = null) : k[o][n] = new Ub(i, i.options.stackLabels, m, n, f, d); p = k[o][n]; n = p.total; p.addValue(w); p.cacheExtremes(this, [n, n + w]); if (p.total > l[o] && !m) l[o] = p.total; else if (p.total < l[o] && m) l[o] =
                    p.total
                } i.oldStacks = {}
            }
        }, getExtremes: function () {
            var a = this.xAxis, b = this.yAxis, c = this.stackKey, d = this.options, e = d.threshold, f = this.processedXData, g = this.processedYData, h = g.length, i = [], k = 0, j = a.min, a = a.max, l, m, n; d.stacking && (m = b.stacksMax["-" + c] || e, n = b.stacksMax[c] || e); if (!t(m) || !t(n)) {
                for (d = 0; d < h; d++) if (l = f[d], c = g[d], e = c !== null && c !== r && (!b.isLog || c.length || c > 0), l = this.getExtremesFromAll || this.cropped || (f[d + 1] || l) >= j && (f[d - 1] || l) <= a, e && l) if (e = c.length) for (; e--;) c[e] !== null && (i[k++] = c[e]); else i[k++] =
                c; m = o(m, Qa(i)); n = o(n, ta(i))
            } this.dataMin = m; this.dataMax = n
        }, translate: function () {
            this.processedXData || this.processData(); this.generatePoints(); for (var a = this.options, b = a.stacking, c = this.xAxis, d = c.categories, e = this.yAxis, f = this.points, g = f.length, h = !!this.modifyValue, i = a.pointPlacement, k = i === "between" || ra(i), j = a.threshold, a = 0; a < g; a++) {
                var l = f[a], m = l.x, n = l.y, p = l.low, q = e.stacks[(n < j ? "-" : "") + this.stackKey], y; if (e.isLog && n <= 0) l.y = n = null; l.plotX = c.translate(m, 0, 0, 0, 1, i); if (b && this.visible && q && q[m]) q = q[m],
                y = q.total, q.cum = p = q.cum - n, n = p + n, q.cum === 0 && (p = o(j, e.min)), e.isLog && p <= 0 && (p = null), b === "percent" && (p = y ? p * 100 / y : 0, n = y ? n * 100 / y : 0), l.percentage = y ? l.y * 100 / y : 0, l.total = l.stackTotal = y, l.stackY = n, q.setOffset(this.pointXOffset || 0, this.barW || 0); l.yBottom = t(p) ? e.translate(p, 0, 1, 0, 1) : null; h && (n = this.modifyValue(n, l)); l.plotY = typeof n === "number" && n !== Infinity ? s(e.translate(n, 0, 1, 0, 1) * 10) / 10 : r; l.clientX = k ? c.translate(m, 0, 0, 0, 1) : l.plotX; l.negative = l.y < (j || 0); l.category = d && d[l.x] !== r ? d[l.x] : l.x
            } this.getSegments()
        },
        setTooltipPoints: function (a) { var b = [], c, d, e = (c = this.xAxis) ? c.tooltipLen || c.len : this.chart.plotSizeX, f, g, h, i = []; if (this.options.enableMouseTracking !== !1) { if (a) this.tooltipPoints = null; q(this.segments || this.points, function (a) { b = b.concat(a) }); c && c.reversed && (b = b.reverse()); this.orderTooltipPoints && this.orderTooltipPoints(b); a = b.length; for (h = 0; h < a; h++) { f = b[h]; g = b[h + 1]; c = b[h - 1] ? d + 1 : 0; for (d = b[h + 1] ? D(u(0, U((f.clientX + (g ? g.wrappedClientX || g.clientX : e)) / 2)), e) : e; c >= 0 && c <= d;) i[c++] = f } this.tooltipPoints = i } },
        tooltipHeaderFormatter: function (a) { var b = this.tooltipOptions, c = b.xDateFormat, d = b.dateTimeLabelFormats, e = this.xAxis, f = e && e.options.type === "datetime", b = b.headerFormat, e = e && e.closestPointRange, g; if (f && !c) if (e) for (g in H) { if (H[g] >= e) { c = d[g]; break } } else c = d.day; f && c && ra(a.key) && (b = b.replace("{point.key}", "{point.key:" + c + "}")); return Ma(b, { point: a, series: this }) }, onMouseOver: function () {
            var a = this.chart, b = a.hoverSeries; if (b && b !== this) b.onMouseOut(); this.options.events.mouseOver && B(this, "mouseOver"); this.setState("hover");
            a.hoverSeries = this
        }, onMouseOut: function () { var a = this.options, b = this.chart, c = b.tooltip, d = b.hoverPoint; if (d) d.onMouseOut(); this && a.events.mouseOut && B(this, "mouseOut"); c && !a.stickyTracking && (!c.shared || this.noSharedTooltip) && c.hide(); this.setState(); b.hoverSeries = null }, animate: function (a) {
            var b = this, c = b.chart, d = c.renderer, e; e = b.options.animation; var f = c.clipBox, g = c.inverted, h; if (e && !ba(e)) e = Q[b.type].animation; h = "_sharedClip" + e.duration + e.easing; if (a) a = c[h], e = c[h + "m"], a || (c[h] = a = d.clipRect(v(f, { width: 0 })),
            c[h + "m"] = e = d.clipRect(-99, g ? -c.plotLeft : -c.plotTop, 99, g ? c.chartWidth : c.chartHeight)), b.group.clip(a), b.markerGroup.clip(e), b.sharedClipKey = h; else { if (a = c[h]) a.animate({ width: c.plotSizeX }, e), c[h + "m"].animate({ width: c.plotSizeX + 99 }, e); b.animate = null; b.animationTimeout = setTimeout(function () { b.afterAnimate() }, e.duration) }
        }, afterAnimate: function () {
            var a = this.chart, b = this.sharedClipKey, c = this.group; c && this.options.clip !== !1 && (c.clip(a.clipRect), this.markerGroup.clip()); setTimeout(function () {
                b && a[b] && (a[b] =
                a[b].destroy(), a[b + "m"] = a[b + "m"].destroy())
            }, 100)
        }, drawPoints: function () {
            var a, b = this.points, c = this.chart, d, e, f, g, h, i, k, j, l = this.options.marker, m, n = this.markerGroup; if (l.enabled || this._hasPointMarkers) for (f = b.length; f--;) if (g = b[f], d = U(g.plotX), e = g.plotY, j = g.graphic, i = g.marker || {}, a = l.enabled && i.enabled === r || i.enabled, m = c.isInsidePlot(s(d), e, c.inverted), a && e !== r && !isNaN(e) && g.y !== null) if (a = g.pointAttr[g.selected ? "select" : ""], h = a.r, i = o(i.symbol, this.symbol), k = i.indexOf("url") === 0, j) j.attr({
                visibility: m ?
                ea ? "inherit" : "visible" : "hidden"
            }).animate(v({ x: d - h, y: e - h }, j.symbolName ? { width: 2 * h, height: 2 * h } : {})); else { if (m && (h > 0 || k)) g.graphic = c.renderer.symbol(i, d - h, e - h, 2 * h, 2 * h).attr(a).add(n) } else if (j) g.graphic = j.destroy()
        }, convertAttribs: function (a, b, c, d) { var e = this.pointAttrToOptions, f, g, h = {}, a = a || {}, b = b || {}, c = c || {}, d = d || {}; for (f in e) g = e[f], h[f] = o(a[g], b[f], c[f], d[f]); return h }, getAttribs: function () {
            var a = this, b = a.options, c = Q[a.type].marker ? b.marker : b, d = c.states, e = d.hover, f, g = a.color, h = { stroke: g, fill: g },
            i = a.points || [], k = [], j, l = a.pointAttrToOptions, m = b.negativeColor, n; b.marker ? (e.radius = e.radius || c.radius + 2, e.lineWidth = e.lineWidth || c.lineWidth + 1) : e.color = e.color || wa(e.color || g).brighten(e.brightness).get(); k[""] = a.convertAttribs(c, h); q(["hover", "select"], function (b) { k[b] = a.convertAttribs(d[b], k[""]) }); a.pointAttr = k; for (g = i.length; g--;) {
                h = i[g]; if ((c = h.options && h.options.marker || h.options) && c.enabled === !1) c.radius = 0; if (h.negative && m) h.color = h.fillColor = m; f = b.colorByPoint || h.color; if (h.options) for (n in l) t(c[l[n]]) &&
                (f = !0); if (f) { c = c || {}; j = []; d = c.states || {}; f = d.hover = d.hover || {}; if (!b.marker) f.color = wa(f.color || h.color).brighten(f.brightness || e.brightness).get(); j[""] = a.convertAttribs(v({ color: h.color }, c), k[""]); j.hover = a.convertAttribs(d.hover, k.hover, j[""]); j.select = a.convertAttribs(d.select, k.select, j[""]); if (h.negative && b.marker && m) j[""].fill = j.hover.fill = j.select.fill = a.convertAttribs({ fillColor: m }).fill } else j = k; h.pointAttr = j
            }
        }, update: function (a, b) {
            var c = this.chart, d = this.type, a = x(this.userOptions, {
                animation: !1,
                index: this.index, pointStart: this.xData[0]
            }, { data: this.options.data }, a); this.remove(!1); v(this, L[a.type || d].prototype); this.init(c, a); o(b, !0) && c.redraw(!1)
        }, destroy: function () {
            var a = this, b = a.chart, c = /AppleWebKit\/533/.test(Ua), d, e, f = a.data || [], g, h, i; B(a, "destroy"); V(a); q(["xAxis", "yAxis"], function (b) { if (i = a[b]) la(i.series, a), i.isDirty = i.forceRedraw = !0 }); a.legendItem && a.chart.legend.destroyItem(a); for (e = f.length; e--;) (g = f[e]) && g.destroy && g.destroy(); a.points = null; clearTimeout(a.animationTimeout); q("area,graph,dataLabelsGroup,group,markerGroup,tracker,graphNeg,areaNeg,posClip,negClip".split(","),
            function (b) { a[b] && (d = c && b === "group" ? "hide" : "destroy", a[b][d]()) }); if (b.hoverSeries === a) b.hoverSeries = null; la(b.series, a); for (h in a) delete a[h]
        }, drawDataLabels: function () {
            var a = this, b = a.options.dataLabels, c = a.points, d, e, f, g; if (b.enabled || a._hasPointLabels) a.dlProcessOptions && a.dlProcessOptions(b), g = a.plotGroup("dataLabelsGroup", "data-labels", a.visible ? "visible" : "hidden", b.zIndex || 6), e = b, q(c, function (c) {
                var i, k = c.dataLabel, j, l, m = c.connector, n = !0; d = c.options && c.options.dataLabels; i = e.enabled || d && d.enabled;
                if (k && !i) c.dataLabel = k.destroy(); else if (i) {
                    b = x(e, d); i = b.rotation; j = c.getLabelConfig(); f = b.format ? Ma(b.format, j) : b.formatter.call(j, b); b.style.color = o(b.color, b.style.color, a.color, "black"); if (k) if (t(f)) k.attr({ text: f }), n = !1; else { if (c.dataLabel = k = k.destroy(), m) c.connector = m.destroy() } else if (t(f)) {
                        k = { fill: b.backgroundColor, stroke: b.borderColor, "stroke-width": b.borderWidth, r: b.borderRadius || 0, rotation: i, padding: b.padding, zIndex: 1 }; for (l in k) k[l] === r && delete k[l]; k = c.dataLabel = a.chart.renderer[i ? "text" :
                        "label"](f, 0, -999, null, null, null, b.useHTML).attr(k).css(b.style).add(g).shadow(b.shadow)
                    } k && a.alignDataLabel(c, k, b, null, n)
                }
            })
        }, alignDataLabel: function (a, b, c, d, e) {
            var f = this.chart, g = f.inverted, h = o(a.plotX, -999), i = o(a.plotY, -999), a = b.getBBox(), d = v({ x: g ? f.plotWidth - i : h, y: s(g ? f.plotHeight - h : i), width: 0, height: 0 }, d); v(c, { width: a.width, height: a.height }); c.rotation ? (d = { align: c.align, x: d.x + c.x + d.width / 2, y: d.y + c.y + d.height / 2 }, b[e ? "attr" : "animate"](d)) : (b.align(c, null, d), d = b.alignAttr); b.attr({
                visibility: c.crop ===
                !1 || f.isInsidePlot(d.x, d.y) && f.isInsidePlot(d.x + a.width, d.y + a.height) ? f.renderer.isSVG ? "inherit" : "visible" : "hidden"
            })
        }, getSegmentPath: function (a) { var b = this, c = [], d = b.options.step; q(a, function (e, f) { var g = e.plotX, h = e.plotY, i; b.getPointSpline ? c.push.apply(c, b.getPointSpline(a, e, f)) : (c.push(f ? "L" : "M"), d && f && (i = a[f - 1], d === "right" ? c.push(i.plotX, h) : d === "center" ? c.push((i.plotX + g) / 2, i.plotY, (i.plotX + g) / 2, h) : c.push(g, i.plotY)), c.push(e.plotX, e.plotY)) }); return c }, getGraphPath: function () {
            var a = this, b = [], c,
            d = []; q(a.segments, function (e) { c = a.getSegmentPath(e); e.length > 1 ? b = b.concat(c) : d.push(e[0]) }); a.singlePoints = d; return a.graphPath = b
        }, drawGraph: function () {
            var a = this, b = this.options, c = [["graph", b.lineColor || this.color]], d = b.lineWidth, e = b.dashStyle, f = this.getGraphPath(), g = b.negativeColor; g && c.push(["graphNeg", g]); q(c, function (c, g) {
                var k = c[0], j = a[k]; if (j) jb(j), j.animate({ d: f }); else if (d && f.length) {
                    j = { stroke: c[1], "stroke-width": d, zIndex: 1 }; if (e) j.dashstyle = e; a[k] = a.chart.renderer.path(f).attr(j).add(a.group).shadow(!g &&
                    b.shadow)
                }
            })
        }, clipNeg: function () {
            var a = this.options, b = this.chart, c = b.renderer, d = a.negativeColor || a.negativeFillColor, e, f = this.graph, g = this.area, h = this.posClip, i = this.negClip; e = b.chartWidth; var k = b.chartHeight, j = u(e, k), l = this.yAxis; if (d && (f || g)) {
                d = s(l.toPixels(a.threshold || 0, !0)); a = { x: 0, y: 0, width: j, height: d }; j = { x: 0, y: d, width: j, height: j }; if (b.inverted) a.height = j.y = b.plotWidth - d, c.isVML && (a = { x: b.plotWidth - d - b.plotLeft, y: 0, width: e, height: k }, j = { x: d + b.plotLeft - e, y: 0, width: b.plotLeft + d, height: e }); l.reversed ?
                (b = j, e = a) : (b = a, e = j); h ? (h.animate(b), i.animate(e)) : (this.posClip = h = c.clipRect(b), this.negClip = i = c.clipRect(e), f && this.graphNeg && (f.clip(h), this.graphNeg.clip(i)), g && (g.clip(h), this.areaNeg.clip(i)))
            }
        }, invertGroups: function () { function a() { var a = { width: b.yAxis.len, height: b.xAxis.len }; q(["group", "markerGroup"], function (c) { b[c] && b[c].attr(a).invert() }) } var b = this, c = b.chart; if (b.xAxis) E(c, "resize", a), E(b, "destroy", function () { V(c, "resize", a) }), a(), b.invertGroups = a }, plotGroup: function (a, b, c, d, e) {
            var f = this[a],
            g = !f; g && (this[a] = f = this.chart.renderer.g(b).attr({ visibility: c, zIndex: d || 0.1 }).add(e)); f[g ? "attr" : "animate"](this.getPlotBox()); return f
        }, getPlotBox: function () { return { translateX: this.xAxis ? this.xAxis.left : this.chart.plotLeft, translateY: this.yAxis ? this.yAxis.top : this.chart.plotTop, scaleX: 1, scaleY: 1 } }, render: function () {
            var a = this.chart, b, c = this.options, d = c.animation && !!this.animate && a.renderer.isSVG, e = this.visible ? "visible" : "hidden", f = c.zIndex, g = this.hasRendered, h = a.seriesGroup; b = this.plotGroup("group",
            "series", e, f, h); this.markerGroup = this.plotGroup("markerGroup", "markers", e, f, h); d && this.animate(!0); this.getAttribs(); b.inverted = this.isCartesian ? a.inverted : !1; this.drawGraph && (this.drawGraph(), this.clipNeg()); this.drawDataLabels(); this.drawPoints(); this.options.enableMouseTracking !== !1 && this.drawTracker(); a.inverted && this.invertGroups(); c.clip !== !1 && !this.sharedClipKey && !g && b.clip(a.clipRect); d ? this.animate() : g || this.afterAnimate(); this.isDirty = this.isDirtyData = !1; this.hasRendered = !0
        }, redraw: function () {
            var a =
            this.chart, b = this.isDirtyData, c = this.group, d = this.xAxis, e = this.yAxis; c && (a.inverted && c.attr({ width: a.plotWidth, height: a.plotHeight }), c.animate({ translateX: o(d && d.left, a.plotLeft), translateY: o(e && e.top, a.plotTop) })); this.translate(); this.setTooltipPoints(!0); this.render(); b && B(this, "updatedData")
        }, setState: function (a) {
            var b = this.options, c = this.graph, d = this.graphNeg, e = b.states, b = b.lineWidth, a = a || ""; if (this.state !== a) this.state = a, e[a] && e[a].enabled === !1 || (a && (b = e[a].lineWidth || b + 1), c && !c.dashstyle &&
            (a = { "stroke-width": b }, c.attr(a), d && d.attr(a)))
        }, setVisible: function (a, b) {
            var c = this, d = c.chart, e = c.legendItem, f, g = d.options.chart.ignoreHiddenSeries, h = c.visible; f = (c.visible = a = c.userOptions.visible = a === r ? !h : a) ? "show" : "hide"; q(["group", "dataLabelsGroup", "markerGroup", "tracker"], function (a) { if (c[a]) c[a][f]() }); if (d.hoverSeries === c) c.onMouseOut(); e && d.legend.colorizeItem(c, a); c.isDirty = !0; c.options.stacking && q(d.series, function (a) { if (a.options.stacking && a.visible) a.isDirty = !0 }); q(c.linkedSeries, function (b) {
                b.setVisible(a,
                !1)
            }); if (g) d.isDirtyBox = !0; b !== !1 && d.redraw(); B(c, f)
        }, show: function () { this.setVisible(!0) }, hide: function () { this.setVisible(!1) }, select: function (a) { this.selected = a = a === r ? !this.selected : a; if (this.checkbox) this.checkbox.checked = a; B(this, a ? "select" : "unselect") }, drawTracker: function () {
            var a = this, b = a.options, c = b.trackByArea, d = [].concat(c ? a.areaPath : a.graphPath), e = d.length, f = a.chart, g = f.pointer, h = f.renderer, i = f.options.tooltip.snap, k = a.tracker, j = b.cursor, j = j && { cursor: j }, l = a.singlePoints, m, n = function () {
                if (f.hoverSeries !==
                a) a.onMouseOver()
            }; if (e && !c) for (m = e + 1; m--;) d[m] === "M" && d.splice(m + 1, 0, d[m + 1] - i, d[m + 2], "L"), (m && d[m] === "M" || m === e) && d.splice(m, 0, "L", d[m - 2] + i, d[m - 1]); for (m = 0; m < l.length; m++) e = l[m], d.push("M", e.plotX - i, e.plotY, "L", e.plotX + i, e.plotY); if (k) k.attr({ d: d }); else if (a.tracker = k = h.path(d).attr({ "class": "highcharts-tracker", "stroke-linejoin": "round", visibility: a.visible ? "visible" : "hidden", stroke: Xb, fill: c ? Xb : $, "stroke-width": b.lineWidth + (c ? 0 : 2 * i), zIndex: 2 }).addClass("highcharts-tracker").on("mouseover", n).on("mouseout",
            function (a) { g.onTrackerMouseOut(a) }).css(j).add(a.markerGroup), ib) k.on("touchstart", n)
        }
    }; I = ca(W); L.line = I; Q.area = x(P, { threshold: 0 }); I = ca(W, {
        type: "area", getSegments: function () {
            var a = [], b = [], c = [], d = this.xAxis, e = this.yAxis, f = e.stacks[this.stackKey], g = {}, h, i, k = this.points, j, l, m; if (this.options.stacking && !this.cropped) {
                for (l = 0; l < k.length; l++) g[k[l].x] = k[l]; for (m in f) c.push(+m); c.sort(function (a, b) { return a - b }); q(c, function (a) {
                    g[a] ? b.push(g[a]) : (h = d.translate(a), j = f[a].percent ? f[a].total ? f[a].cum * 100 /
                    f[a].total : 0 : f[a].cum, i = e.toPixels(j, !0), b.push({ y: null, plotX: h, clientX: h, plotY: i, yBottom: i, onMouseOver: qa }))
                }); b.length && a.push(b)
            } else W.prototype.getSegments.call(this), a = this.segments; this.segments = a
        }, getSegmentPath: function (a) {
            var b = W.prototype.getSegmentPath.call(this, a), c = [].concat(b), d, e = this.options; b.length === 3 && c.push("L", b[1], b[2]); if (e.stacking && !this.closedStacks) for (d = a.length - 1; d >= 0; d--) d < a.length - 1 && e.step && c.push(a[d + 1].plotX, a[d].yBottom), c.push(a[d].plotX, a[d].yBottom); else this.closeSegment(c,
            a); this.areaPath = this.areaPath.concat(c); return b
        }, closeSegment: function (a, b) { var c = this.yAxis.getThreshold(this.options.threshold); a.push("L", b[b.length - 1].plotX, c, "L", b[0].plotX, c) }, drawGraph: function () {
            this.areaPath = []; W.prototype.drawGraph.apply(this); var a = this, b = this.areaPath, c = this.options, d = c.negativeColor, e = c.negativeFillColor, f = [["area", this.color, c.fillColor]]; (d || e) && f.push(["areaNeg", d, e]); q(f, function (d) {
                var e = d[0], f = a[e]; f ? f.animate({ d: b }) : a[e] = a.chart.renderer.path(b).attr({
                    fill: o(d[2],
                    wa(d[1]).setOpacity(o(c.fillOpacity, 0.75)).get()), zIndex: 0
                }).add(a.group)
            })
        }, drawLegendSymbol: function (a, b) { b.legendSymbol = this.chart.renderer.rect(0, a.baseline - 11, a.options.symbolWidth, 12, 2).attr({ zIndex: 3 }).add(b.legendGroup) }
    }); L.area = I; Q.spline = x(P); Y = ca(W, {
        type: "spline", getPointSpline: function (a, b, c) {
            var d = b.plotX, e = b.plotY, f = a[c - 1], g = a[c + 1], h, i, k, j; if (f && g) {
                a = f.plotY; k = g.plotX; var g = g.plotY, l; h = (1.5 * d + f.plotX) / 2.5; i = (1.5 * e + a) / 2.5; k = (1.5 * d + k) / 2.5; j = (1.5 * e + g) / 2.5; l = (j - i) * (k - d) / (k - h) + e - j; i += l;
                j += l; i > a && i > e ? (i = u(a, e), j = 2 * e - i) : i < a && i < e && (i = D(a, e), j = 2 * e - i); j > g && j > e ? (j = u(g, e), i = 2 * e - j) : j < g && j < e && (j = D(g, e), i = 2 * e - j); b.rightContX = k; b.rightContY = j
            } c ? (b = ["C", f.rightContX || f.plotX, f.rightContY || f.plotY, h || d, i || e, d, e], f.rightContX = f.rightContY = null) : b = ["M", d, e]; return b
        }
    }); L.spline = Y; Q.areaspline = x(Q.area); var Ja = I.prototype; Y = ca(Y, { type: "areaspline", closedStacks: !0, getSegmentPath: Ja.getSegmentPath, closeSegment: Ja.closeSegment, drawGraph: Ja.drawGraph, drawLegendSymbol: Ja.drawLegendSymbol }); L.areaspline =
    Y; Q.column = x(P, { borderColor: "#FFFFFF", borderWidth: 1, borderRadius: 0, groupPadding: 0.2, marker: null, pointPadding: 0.1, minPointLength: 0, cropThreshold: 50, pointRange: null, states: { hover: { brightness: 0.1, shadow: !1 }, select: { color: "#C0C0C0", borderColor: "#000000", shadow: !1 } }, dataLabels: { align: null, verticalAlign: null, y: null }, stickyTracking: !1, threshold: 0 }); Y = ca(W, {
        type: "column", tooltipOutsidePlot: !0, pointAttrToOptions: { stroke: "borderColor", "stroke-width": "borderWidth", fill: "color", r: "borderRadius" }, trackerGroups: ["group",
        "dataLabelsGroup"], init: function () { W.prototype.init.apply(this, arguments); var a = this, b = a.chart; b.hasRendered && q(b.series, function (b) { if (b.type === a.type) b.isDirty = !0 }) }, getColumnMetrics: function () {
            var a = this, b = a.options, c = a.xAxis, d = a.yAxis, e = c.reversed, f, g = {}, h, i = 0; b.grouping === !1 ? i = 1 : q(a.chart.series, function (b) { var c = b.options, e = b.yAxis; if (b.type === a.type && b.visible && d.len === e.len && d.pos === e.pos) c.stacking ? (f = b.stackKey, g[f] === r && (g[f] = i++), h = g[f]) : c.grouping !== !1 && (h = i++), b.columnIndex = h }); var c =
            D(T(c.transA) * (c.ordinalSlope || b.pointRange || c.closestPointRange || 1), c.len), k = c * b.groupPadding, j = (c - 2 * k) / i, l = b.pointWidth, b = t(l) ? (j - l) / 2 : j * b.pointPadding, l = o(l, j - 2 * b); return a.columnMetrics = { width: l, offset: b + (k + ((e ? i - (a.columnIndex || 0) : a.columnIndex) || 0) * j - c / 2) * (e ? -1 : 1) }
        }, translate: function () {
            var a = this.chart, b = this.options, c = b.borderWidth, d = this.yAxis, e = this.translatedThreshold = d.getThreshold(b.threshold), f = o(b.minPointLength, 5), b = this.getColumnMetrics(), g = b.width, h = this.barW = pa(u(g, 1 + 2 * c)), i = this.pointXOffset =
            b.offset; W.prototype.translate.apply(this); q(this.points, function (b) { var j = D(u(-999, b.plotY), d.len + 999), l = o(b.yBottom, e), m = b.plotX + i, n = pa(D(j, l)), j = pa(u(j, l) - n); T(j) < f && f && (j = f, n = s(T(n - e) > f ? l - f : e - (d.translate(b.y, 0, 1, 0, 1) <= e ? f : 0))); b.barX = m; b.pointWidth = g; b.shapeType = "rect"; b.shapeArgs = b = a.renderer.Element.prototype.crisp.call(0, c, m, n, h, j); c % 2 && (b.y -= 1, b.height += 1) })
        }, getSymbol: qa, drawLegendSymbol: I.prototype.drawLegendSymbol, drawGraph: qa, drawPoints: function () {
            var a = this, b = a.options, c = a.chart.renderer,
            d; q(a.points, function (e) { var f = e.plotY, g = e.graphic; if (f !== r && !isNaN(f) && e.y !== null) d = e.shapeArgs, g ? (jb(g), g.animate(x(d))) : e.graphic = c[e.shapeType](d).attr(e.pointAttr[e.selected ? "select" : ""]).add(a.group).shadow(b.shadow, null, b.stacking && !b.borderRadius); else if (g) e.graphic = g.destroy() })
        }, drawTracker: function () {
            var a = this, b = a.chart, c = b.pointer, d = a.options.cursor, e = d && { cursor: d }, f = function (c) {
                var d = c.target, e; if (b.hoverSeries !== a) a.onMouseOver(); for (; d && !e;) e = d.point, d = d.parentNode; if (e !== r && e !==
                b.hoverPoint) e.onMouseOver(c)
            }; q(a.points, function (a) { if (a.graphic) a.graphic.element.point = a; if (a.dataLabel) a.dataLabel.element.point = a }); a._hasTracking ? a._hasTracking = !0 : q(a.trackerGroups, function (b) { if (a[b] && (a[b].addClass("highcharts-tracker").on("mouseover", f).on("mouseout", function (a) { c.onTrackerMouseOut(a) }).css(e), ib)) a[b].on("touchstart", f) })
        }, alignDataLabel: function (a, b, c, d, e) {
            var f = this.chart, g = f.inverted, h = a.dlBox || a.shapeArgs, i = a.below || a.plotY > o(this.translatedThreshold, f.plotSizeY),
            k = o(c.inside, !!this.options.stacking); if (h && (d = x(h), g && (d = { x: f.plotWidth - d.y - d.height, y: f.plotHeight - d.x - d.width, width: d.height, height: d.width }), !k)) g ? (d.x += i ? 0 : d.width, d.width = 0) : (d.y += i ? d.height : 0, d.height = 0); c.align = o(c.align, !g || k ? "center" : i ? "right" : "left"); c.verticalAlign = o(c.verticalAlign, g || k ? "middle" : i ? "top" : "bottom"); W.prototype.alignDataLabel.call(this, a, b, c, d, e)
        }, animate: function (a) {
            var b = this.yAxis, c = this.options, d = this.chart.inverted, e = {}; if (ea) a ? (e.scaleY = 0.001, a = D(b.pos + b.len, u(b.pos,
            b.toPixels(c.threshold))), d ? e.translateX = a - b.len : e.translateY = a, this.group.attr(e)) : (e.scaleY = 1, e[d ? "translateX" : "translateY"] = b.pos, this.group.animate(e, this.options.animation), this.animate = null)
        }, remove: function () { var a = this, b = a.chart; b.hasRendered && q(b.series, function (b) { if (b.type === a.type) b.isDirty = !0 }); W.prototype.remove.apply(a, arguments) }
    }); L.column = Y; Q.bar = x(Q.column); Ja = ca(Y, { type: "bar", inverted: !0 }); L.bar = Ja; Q.scatter = x(P, {
        lineWidth: 0, tooltip: {
            headerFormat: '<span style="font-size: 10px; color:{series.color}">{series.name}</span><br/>',
            pointFormat: "x: <b>{point.x}</b><br/>y: <b>{point.y}</b><br/>", followPointer: !0
        }, stickyTracking: !1
    }); Ja = ca(W, { type: "scatter", sorted: !1, requireSorting: !1, noSharedTooltip: !0, trackerGroups: ["markerGroup"], drawTracker: Y.prototype.drawTracker, setTooltipPoints: qa }); L.scatter = Ja; Q.pie = x(P, {
        borderColor: "#FFFFFF", borderWidth: 1, center: [null, null], clip: !1, colorByPoint: !0, dataLabels: { distance: 30, enabled: !0, formatter: function () { return this.point.name } }, ignoreHiddenPoint: !0, legendType: "point", marker: null, size: null,
        showInLegend: !1, slicedOffset: 10, states: { hover: { brightness: 0.1, shadow: !1 } }, stickyTracking: !1, tooltip: { followPointer: !0 }
    }); P = {
        type: "pie", isCartesian: !1, pointClass: ca(Ia, {
            init: function () { Ia.prototype.init.apply(this, arguments); var a = this, b; if (a.y < 0) a.y = null; v(a, { visible: a.visible !== !1, name: o(a.name, "Slice") }); b = function (b) { a.slice(b.type === "select") }; E(a, "select", b); E(a, "unselect", b); return a }, setVisible: function (a) {
                var b = this, c = b.series, d = c.chart, e; b.visible = b.options.visible = a = a === r ? !b.visible : a; c.options.data[va(b,
                c.data)] = b.options; e = a ? "show" : "hide"; q(["graphic", "dataLabel", "connector", "shadowGroup"], function (a) { if (b[a]) b[a][e]() }); b.legendItem && d.legend.colorizeItem(b, a); if (!c.isDirty && c.options.ignoreHiddenPoint) c.isDirty = !0, d.redraw()
            }, slice: function (a, b, c) { var d = this.series; Za(c, d.chart); o(b, !0); this.sliced = this.options.sliced = a = t(a) ? a : !this.sliced; d.options.data[va(this, d.data)] = this.options; a = a ? this.slicedTranslation : { translateX: 0, translateY: 0 }; this.graphic.animate(a); this.shadowGroup && this.shadowGroup.animate(a) }
        }),
        requireSorting: !1, noSharedTooltip: !0, trackerGroups: ["group", "dataLabelsGroup"], pointAttrToOptions: { stroke: "borderColor", "stroke-width": "borderWidth", fill: "color" }, getColor: qa, animate: function (a) { var b = this, c = b.points, d = b.startAngleRad; if (!a) q(c, function (a) { var c = a.graphic, a = a.shapeArgs; c && (c.attr({ r: b.center[3] / 2, start: d, end: d }), c.animate({ r: a.r, start: a.start, end: a.end }, b.options.animation)) }), b.animate = null }, setData: function (a, b) {
            W.prototype.setData.call(this, a, !1); this.processData(); this.generatePoints();
            o(b, !0) && this.chart.redraw()
        }, generatePoints: function () { var a, b = 0, c, d, e, f = this.options.ignoreHiddenPoint; W.prototype.generatePoints.call(this); c = this.points; d = c.length; for (a = 0; a < d; a++) e = c[a], b += f && !e.visible ? 0 : e.y; this.total = b; for (a = 0; a < d; a++) e = c[a], e.percentage = e.y / b * 100, e.total = b }, getCenter: function () {
            var a = this.options, b = this.chart, c = 2 * (a.slicedOffset || 0), d, e = b.plotWidth - 2 * c, f = b.plotHeight - 2 * c, b = a.center, a = [o(b[0], "50%"), o(b[1], "50%"), a.size || "100%", a.innerSize || 0], g = D(e, f), h; return Ga(a, function (a,
            b) { h = /%$/.test(a); d = b < 2 || b === 2 && h; return (h ? [e, f, g, g][b] * z(a) / 100 : a) + (d ? c : 0) })
        }, translate: function (a) {
            this.generatePoints(); var b = 0, c = this.options, d = c.slicedOffset, e = d + c.borderWidth, f, g, h, i = this.startAngleRad = ab / 180 * ((c.startAngle || 0) % 360 - 90), k = this.points, j = 2 * ab, l = c.dataLabels.distance, c = c.ignoreHiddenPoint, m, n = k.length, p; if (!a) this.center = a = this.getCenter(); this.getX = function (b, c) { h = S.asin((b - a[1]) / (a[2] / 2 + l)); return a[0] + (c ? -1 : 1) * da(h) * (a[2] / 2 + l) }; for (m = 0; m < n; m++) {
                p = k[m]; f = s((i + b * j) * 1E3) / 1E3; if (!c ||
                p.visible) b += p.percentage / 100; g = s((i + b * j) * 1E3) / 1E3; p.shapeType = "arc"; p.shapeArgs = { x: a[0], y: a[1], r: a[2] / 2, innerR: a[3] / 2, start: f, end: g }; h = (g + f) / 2; h > 0.75 * j && (h -= 2 * ab); p.slicedTranslation = { translateX: s(da(h) * d), translateY: s(ia(h) * d) }; f = da(h) * a[2] / 2; g = ia(h) * a[2] / 2; p.tooltipPos = [a[0] + f * 0.7, a[1] + g * 0.7]; p.half = h < j / 4 ? 0 : 1; p.angle = h; e = D(e, l / 2); p.labelPos = [a[0] + f + da(h) * l, a[1] + g + ia(h) * l, a[0] + f + da(h) * e, a[1] + g + ia(h) * e, a[0] + f, a[1] + g, l < 0 ? "center" : p.half ? "right" : "left", h]
            } this.setTooltipPoints()
        }, drawGraph: null, drawPoints: function () {
            var a =
            this, b = a.chart.renderer, c, d, e = a.options.shadow, f, g; if (e && !a.shadowGroup) a.shadowGroup = b.g("shadow").add(a.group); q(a.points, function (h) {
                d = h.graphic; g = h.shapeArgs; f = h.shadowGroup; if (e && !f) f = h.shadowGroup = b.g("shadow").add(a.shadowGroup); c = h.sliced ? h.slicedTranslation : { translateX: 0, translateY: 0 }; f && f.attr(c); d ? d.animate(v(g, c)) : h.graphic = d = b.arc(g).setRadialReference(a.center).attr(h.pointAttr[h.selected ? "select" : ""]).attr({ "stroke-linejoin": "round" }).attr(c).add(a.group).shadow(e, f); h.visible === !1 &&
                h.setVisible(!1)
            })
        }, drawDataLabels: function () {
            var a = this, b = a.data, c, d = a.chart, e = a.options.dataLabels, f = o(e.connectorPadding, 10), g = o(e.connectorWidth, 1), h = d.plotWidth, d = d.plotHeight, i, k, j = o(e.softConnector, !0), l = e.distance, m = a.center, n = m[2] / 2, p = m[1], K = l > 0, y, w, r, t, x = [[], []], v, A, D, z, J, C = [0, 0, 0, 0], E = function (a, b) { return b.y - a.y }, H = function (a, b) { a.sort(function (a, c) { return a.angle !== void 0 && (c.angle - a.angle) * b }) }; if (a.visible && (e.enabled || a._hasPointLabels)) {
                W.prototype.drawDataLabels.apply(a); q(b, function (a) {
                    a.dataLabel &&
                    x[a.half].push(a)
                }); for (z = 0; !t && b[z];) t = b[z] && b[z].dataLabel && (b[z].dataLabel.getBBox().height || 21), z++; for (z = 2; z--;) {
                    var b = [], G = [], I = x[z], F = I.length, B; H(I, z - 0.5); if (l > 0) {
                        for (J = p - n - l; J <= p + n + l; J += t) b.push(J); w = b.length; if (F > w) { c = [].concat(I); c.sort(E); for (J = F; J--;) c[J].rank = J; for (J = F; J--;) I[J].rank >= w && I.splice(J, 1); F = I.length } for (J = 0; J < F; J++) {
                            c = I[J]; r = c.labelPos; c = 9999; var N, L; for (L = 0; L < w; L++) N = T(b[L] - r[1]), N < c && (c = N, B = L); if (B < J && b[J] !== null) B = J; else for (w < F - J + B && b[J] !== null && (B = w - F + J) ; b[B] === null;) B++;
                            G.push({ i: B, y: b[B] }); b[B] = null
                        } G.sort(E)
                    } for (J = 0; J < F; J++) {
                        c = I[J]; r = c.labelPos; y = c.dataLabel; D = c.visible === !1 ? "hidden" : "visible"; c = r[1]; if (l > 0) { if (w = G.pop(), B = w.i, A = w.y, c > A && b[B + 1] !== null || c < A && b[B - 1] !== null) A = c } else A = c; v = e.justify ? m[0] + (z ? -1 : 1) * (n + l) : a.getX(B === 0 || B === b.length - 1 ? c : A, z); y._attr = { visibility: D, align: r[6] }; y._pos = { x: v + e.x + ({ left: f, right: -f }[r[6]] || 0), y: A + e.y - 10 }; y.connX = v; y.connY = A; if (this.options.size === null) w = y.width, v - w < f ? C[3] = u(s(w - v + f), C[3]) : v + w > h - f && (C[1] = u(s(v + w - h + f), C[1])), A -
                        t / 2 < 0 ? C[0] = u(s(-A + t / 2), C[0]) : A + t / 2 > d && (C[2] = u(s(A + t / 2 - d), C[2]))
                    }
                } if (ta(C) === 0 || this.verifyDataLabelOverflow(C)) this.placeDataLabels(), K && g && q(this.points, function (b) {
                    i = b.connector; r = b.labelPos; if ((y = b.dataLabel) && y._pos) D = y._attr.visibility, v = y.connX, A = y.connY, k = j ? ["M", v + (r[6] === "left" ? 5 : -5), A, "C", v, A, 2 * r[2] - r[4], 2 * r[3] - r[5], r[2], r[3], "L", r[4], r[5]] : ["M", v + (r[6] === "left" ? 5 : -5), A, "L", r[2], r[3], "L", r[4], r[5]], i ? (i.animate({ d: k }), i.attr("visibility", D)) : b.connector = i = a.chart.renderer.path(k).attr({
                        "stroke-width": g,
                        stroke: e.connectorColor || b.color || "#606060", visibility: D
                    }).add(a.group); else if (i) b.connector = i.destroy()
                })
            }
        }, verifyDataLabelOverflow: function (a) {
            var b = this.center, c = this.options, d = c.center, e = c = c.minSize || 80, f; d[0] !== null ? e = u(b[2] - u(a[1], a[3]), c) : (e = u(b[2] - a[1] - a[3], c), b[0] += (a[3] - a[1]) / 2); d[1] !== null ? e = u(D(e, b[2] - u(a[0], a[2])), c) : (e = u(D(e, b[2] - a[0] - a[2]), c), b[1] += (a[0] - a[2]) / 2); e < b[2] ? (b[2] = e, this.translate(b), q(this.points, function (a) { if (a.dataLabel) a.dataLabel._pos = null }), this.drawDataLabels()) :
            f = !0; return f
        }, placeDataLabels: function () { q(this.points, function (a) { var a = a.dataLabel, b; if (a) (b = a._pos) ? (a.attr(a._attr), a[a.moved ? "animate" : "attr"](b), a.moved = !0) : a && a.attr({ y: -999 }) }) }, alignDataLabel: qa, drawTracker: Y.prototype.drawTracker, drawLegendSymbol: I.prototype.drawLegendSymbol, getSymbol: qa
    }; P = ca(W, P); L.pie = P; var R = W.prototype, fc = R.processData, gc = R.generatePoints, hc = R.destroy, ic = R.tooltipHeaderFormatter, jc = {
        approximation: "average", groupPixelWidth: 2, dateTimeLabelFormats: lb(mb, ["%A, %b %e, %H:%M:%S.%L",
        "%A, %b %e, %H:%M:%S.%L", "-%H:%M:%S.%L"], eb, ["%A, %b %e, %H:%M:%S", "%A, %b %e, %H:%M:%S", "-%H:%M:%S"], Xa, ["%A, %b %e, %H:%M", "%A, %b %e, %H:%M", "-%H:%M"], Ba, ["%A, %b %e, %H:%M", "%A, %b %e, %H:%M", "-%H:%M"], fa, ["%A, %b %e, %Y", "%A, %b %e", "-%A, %b %e, %Y"], Na, ["Week from %A, %b %e, %Y", "%A, %b %e", "-%A, %b %e, %Y"], Oa, ["%B %Y", "%B", "-%B %Y"], na, ["%Y", "%Y", "-%Y"])
    }, ac = {
        line: {}, spline: {}, area: {}, areaspline: {}, column: { approximation: "sum", groupPixelWidth: 10 }, arearange: { approximation: "range" }, areasplinerange: { approximation: "range" },
        columnrange: { approximation: "range", groupPixelWidth: 10 }, candlestick: { approximation: "ohlc", groupPixelWidth: 10 }, ohlc: { approximation: "ohlc", groupPixelWidth: 5 }
    }, bc = [[mb, [1, 2, 5, 10, 20, 25, 50, 100, 200, 500]], [eb, [1, 2, 5, 10, 15, 30]], [Xa, [1, 2, 5, 10, 15, 30]], [Ba, [1, 2, 3, 4, 6, 8, 12]], [fa, [1]], [Na, [1]], [Oa, [1, 3, 6]], [na, null]], Ka = {
        sum: function (a) { var b = a.length, c; if (!b && a.hasNulls) c = null; else if (b) for (c = 0; b--;) c += a[b]; return c }, average: function (a) { var b = a.length, a = Ka.sum(a); typeof a === "number" && b && (a /= b); return a }, open: function (a) {
            return a.length ?
            a[0] : a.hasNulls ? null : r
        }, high: function (a) { return a.length ? ta(a) : a.hasNulls ? null : r }, low: function (a) { return a.length ? Qa(a) : a.hasNulls ? null : r }, close: function (a) { return a.length ? a[a.length - 1] : a.hasNulls ? null : r }, ohlc: function (a, b, c, d) { a = Ka.open(a); b = Ka.high(b); c = Ka.low(c); d = Ka.close(d); if (typeof a === "number" || typeof b === "number" || typeof c === "number" || typeof d === "number") return [a, b, c, d] }, range: function (a, b) { a = Ka.low(a); b = Ka.high(b); if (typeof a === "number" || typeof b === "number") return [a, b] }
    }; R.groupData = function (a,
    b, c, d) {
        var e = this.data, f = this.options.data, g = [], h = [], i = a.length, k, j, l = !!b, m = [[], [], [], []], d = typeof d === "function" ? d : Ka[d], n = this.pointArrayMap, p = n && n.length, o; for (o = 0; o <= i; o++) {
            for (; c[1] !== r && a[o] >= c[1] || o === i;) if (k = c.shift(), j = d.apply(0, m), j !== r && (g.push(k), h.push(j)), m[0] = [], m[1] = [], m[2] = [], m[3] = [], o === i) break; if (o === i) break; if (n) {
                k = this.cropStart + o; k = e && e[k] || this.pointClass.prototype.applyOptions.apply({ series: this }, [f[k]]); var q; for (j = 0; j < p; j++) if (q = k[n[j]], typeof q === "number") m[j].push(q); else if (q ===
                null) m[j].hasNulls = !0
            } else if (k = l ? b[o] : null, typeof k === "number") m[0].push(k); else if (k === null) m[0].hasNulls = !0
        } return [g, h]
    }; R.processData = function () {
        var a = this.chart, b = this.options, c = b.dataGrouping, d = c && o(c.enabled, a.options._stock), e; this.forceCrop = d; if (fc.apply(this, arguments) !== !1 && d) {
            this.destroyGroupedData(); var f = this.processedXData, g = this.processedYData, h = a.plotSizeX, i = this.xAxis, k = i.getGroupPixelWidth && i.getGroupPixelWidth(), a = this.pointRange; if (k) {
                e = !0; this.points = null; var d = i.getExtremes(),
                a = d.min, d = d.max, j = i.getGroupIntervalFactor && i.getGroupIntervalFactor(a, d, f) || 1, h = k * (d - a) / h * j, i = (i.getNonLinearTimeTicks || fb)(zb(h, c.units || bc), a, d, null, f, this.closestPointRange), g = R.groupData.apply(this, [f, g, i, c.approximation]), f = g[0], g = g[1]; if (c.smoothed) { c = f.length - 1; for (f[c] = d; c-- && c > 0;) f[c] += h / 2; f[0] = a } this.currentDataGrouping = i.info; if (b.pointRange === null) this.pointRange = i.info.totalRange; this.closestPointRange = i.info.totalRange; this.processedXData = f; this.processedYData = g
            } else this.currentDataGrouping =
            null, this.pointRange = a; this.hasGroupedData = e
        }
    }; R.destroyGroupedData = function () { var a = this.groupedData; q(a || [], function (b, c) { b && (a[c] = b.destroy ? b.destroy() : null) }); this.groupedData = null }; R.generatePoints = function () { gc.apply(this); this.destroyGroupedData(); this.groupedData = this.hasGroupedData ? this.points : null }; R.tooltipHeaderFormatter = function (a) {
        var b = this.tooltipOptions, c = this.options.dataGrouping, d = b.xDateFormat, e, f = this.xAxis, g, h; if (f && f.options.type === "datetime" && c && ra(a.key)) {
            g = this.currentDataGrouping;
            c = c.dateTimeLabelFormats; if (g) f = c[g.unitName], g.count === 1 ? d = f[0] : (d = f[1], e = f[2]); else if (!d && c) for (h in H) if (H[h] >= f.closestPointRange) { d = c[h][0]; break } d = Aa(d, a.key); e && (d += Aa(e, a.key + g.totalRange - 1)); a = b.headerFormat.replace("{point.key}", d)
        } else a = ic.call(this, a); return a
    }; R.destroy = function () { for (var a = this.groupedData || [], b = a.length; b--;) a[b] && a[b].destroy(); hc.apply(this) }; ma(R, "setOptions", function (a, b) {
        var c = a.call(this, b), d = this.type, e = this.chart.options.plotOptions, f = Q[d].dataGrouping; if (ac[d]) f ||
        (f = x(jc, ac[d])), c.dataGrouping = x(f, e.series && e.series.dataGrouping, e[d].dataGrouping, b.dataGrouping); if (this.chart.options._stock) this.requireSorting = !0; return c
    }); ua.prototype.getGroupPixelWidth = function () { var a = this.series, b = a.length, c, d = 0, e = !1, f; for (c = b; c--;) (f = a[c].options.dataGrouping) && (d = u(d, f.groupPixelWidth)); for (c = b; c--;) if (f = a[c].options.dataGrouping) if (b = (a[c].processedXData || a[c].data).length, a[c].hasGroupedData || b > this.chart.plotSizeX / d || b && f.forced) e = !0; return e ? d : 0 }; Q.ohlc = x(Q.column,
    { lineWidth: 1, tooltip: { pointFormat: '<span style="color:{series.color};font-weight:bold">{series.name}</span><br/>Open: {point.open}<br/>High: {point.high}<br/>Low: {point.low}<br/>Close: {point.close}<br/>' }, states: { hover: { lineWidth: 3 } }, threshold: null }); P = ca(L.column, {
        type: "ohlc", pointArrayMap: ["open", "high", "low", "close"], toYData: function (a) { return [a.open, a.high, a.low, a.close] }, pointValKey: "high", pointAttrToOptions: { stroke: "color", "stroke-width": "lineWidth" }, upColorProp: "stroke", getAttribs: function () {
            L.column.prototype.getAttribs.apply(this,
            arguments); var a = this.options, b = a.states, a = a.upColor || this.color, c = x(this.pointAttr), d = this.upColorProp; c[""][d] = a; c.hover[d] = b.hover.upColor || a; c.select[d] = b.select.upColor || a; q(this.points, function (a) { if (a.open < a.close) a.pointAttr = c })
        }, translate: function () { var a = this.yAxis; L.column.prototype.translate.apply(this); q(this.points, function (b) { if (b.open !== null) b.plotOpen = a.translate(b.open, 0, 1, 0, 1); if (b.close !== null) b.plotClose = a.translate(b.close, 0, 1, 0, 1) }) }, drawPoints: function () {
            var a = this, b = a.chart,
            c, d, e, f, g, h, i, k; q(a.points, function (j) { if (j.plotY !== r) i = j.graphic, c = j.pointAttr[j.selected ? "selected" : ""], f = c["stroke-width"] % 2 / 2, k = s(j.plotX) + f, g = s(j.shapeArgs.width / 2), h = ["M", k, s(j.yBottom), "L", k, s(j.plotY)], j.open !== null && (d = s(j.plotOpen) + f, h.push("M", k, d, "L", k - g, d)), j.close !== null && (e = s(j.plotClose) + f, h.push("M", k, e, "L", k + g, e)), i ? i.animate({ d: h }) : j.graphic = b.renderer.path(h).attr(c).add(a.group) })
        }, animate: null
    }); L.ohlc = P; Q.candlestick = x(Q.column, {
        lineColor: "black", lineWidth: 1, states: { hover: { lineWidth: 2 } },
        tooltip: Q.ohlc.tooltip, threshold: null, upColor: "white"
    }); P = ca(P, {
        type: "candlestick", pointAttrToOptions: { fill: "color", stroke: "lineColor", "stroke-width": "lineWidth" }, upColorProp: "fill", drawPoints: function () {
            var a = this, b = a.chart, c, d, e, f, g, h, i, k, j, l, m, n; q(a.points, function (p) {
                l = p.graphic; if (p.plotY !== r) c = p.pointAttr[p.selected ? "selected" : ""], k = c["stroke-width"] % 2 / 2, j = s(p.plotX) + k, d = p.plotOpen, e = p.plotClose, f = S.min(d, e), g = S.max(d, e), n = s(p.shapeArgs.width / 2), h = s(f) !== s(p.plotY), i = g !== p.yBottom, f = s(f) + k,
                g = s(g) + k, m = ["M", j - n, g, "L", j - n, f, "L", j + n, f, "L", j + n, g, "L", j - n, g, "M", j, f, "L", j, h ? s(p.plotY) : f, "M", j, g, "L", j, i ? s(p.yBottom) : g, "Z"], l ? l.animate({ d: m }) : p.graphic = b.renderer.path(m).attr(c).add(a.group).shadow(a.options.shadow)
            })
        }
    }); L.candlestick = P; var wb = Ha.prototype.symbols; Q.flags = x(Q.column, {
        dataGrouping: null, fillColor: "white", lineWidth: 1, pointRange: 0, shape: "flag", stackDistance: 12, states: { hover: { lineColor: "black", fillColor: "#FCFFC5" } }, style: { fontSize: "11px", fontWeight: "bold", textAlign: "center" }, tooltip: { pointFormat: "{point.text}<br/>" },
        threshold: null, y: -30
    }); L.flags = ca(L.column, {
        type: "flags", sorted: !1, noSharedTooltip: !0, takeOrdinalPosition: !1, forceCrop: !0, init: W.prototype.init, pointAttrToOptions: { fill: "fillColor", stroke: "color", "stroke-width": "lineWidth", r: "radius" }, translate: function () {
            L.column.prototype.translate.apply(this); var a = this.chart, b = this.points, c = b.length - 1, d, e, f = this.options.onSeries, f = (d = f && a.get(f)) && d.options.step, g = d && d.points, h = g && g.length, i = this.xAxis, k = i.getExtremes(), j, l, m; if (d && d.visible && h) {
                l = g[h - 1].x; for (b.sort(function (a,
                b) { return a.x - b.x }) ; h-- && b[c];) if (d = b[c], j = g[h], j.x <= d.x && j.plotY !== r) { if (d.x <= l) d.plotY = j.plotY, j.x < d.x && !f && (m = g[h + 1]) && m.plotY !== r && (d.plotY += (d.x - j.x) / (m.x - j.x) * (m.plotY - j.plotY)); c--; h++; if (c < 0) break }
            } q(b, function (c, d) { if (c.plotY === r) c.x >= k.min && c.x <= k.max ? c.plotY = i.lineTop - a.plotTop : c.shapeArgs = {}; if ((e = b[d - 1]) && e.plotX === c.plotX) { if (e.stackIndex === r) e.stackIndex = 0; c.stackIndex = e.stackIndex + 1 } })
        }, drawPoints: function () {
            var a, b = this.points, c = this.chart.renderer, d, e, f = this.options, g = f.y, h, i, k,
            j, l, m = f.lineWidth % 2 / 2, n; for (k = b.length; k--;) if (j = b[k], d = j.plotX + m, a = j.stackIndex, h = j.options.shape || f.shape, e = j.plotY, e !== r && (e = j.plotY + g + m - (a !== r && a * f.stackDistance)), i = a ? r : j.plotX + m, n = a ? r : j.plotY, l = j.graphic, e !== r) a = j.pointAttr[j.selected ? "select" : ""], l ? l.attr({ x: d, y: e, r: a.r, anchorX: i, anchorY: n }) : l = j.graphic = c.label(j.options.title || f.title || "A", d, e, h, i, n, f.useHTML).css(x(f.style, j.style)).attr(a).attr({ align: h === "flag" ? "left" : "center", width: f.width, height: f.height }).add(this.group).shadow(f.shadow),
            i = l.box, a = i.getBBox(), j.shapeArgs = v(a, { x: d - (h === "flag" ? 0 : i.attr("width") / 2), y: e }); else if (l) j.graphic = l.destroy()
        }, drawTracker: function () { var a = this.points; L.column.prototype.drawTracker.apply(this); q(a, function (b) { var c = b.graphic; c && E(c.element, "mouseover", function () { if (b.stackIndex > 0 && !b.raised) b._y = c.y, c.attr({ y: b._y - 8 }), b.raised = !0; q(a, function (a) { if (a !== b && a.raised && a.graphic) a.graphic.attr({ y: a._y }), a.raised = !1 }) }) }) }, animate: qa
    }); wb.flag = function (a, b, c, d, e) {
        var f = e && e.anchorX || a, e = e && e.anchorY ||
        b; return ["M", f, e, "L", a, b + d, a, b, a + c, b, a + c, b + d, a, b + d, "M", f, e, "Z"]
    }; q(["circle", "square"], function (a) { wb[a + "pin"] = function (b, c, d, e, f) { var g = f && f.anchorX, f = f && f.anchorY, b = wb[a](b, c, d, e); g && f && b.push("M", g, c > f ? c : c + e, "L", g, f); return b } }); bb === kb && q(["flag", "circlepin", "squarepin"], function (a) { kb.prototype.symbols[a] = wb[a] }); P = lb("linearGradient", { x1: 0, y1: 0, x2: 0, y2: 1 }, "stops", [[0, "#FFF"], [1, "#CCC"]]); I = [].concat(bc); I[4] = [fa, [1, 2, 3, 4]]; I[5] = [Na, [1, 2, 3]]; v(N, {
        navigator: {
            handles: { backgroundColor: "#FFF", borderColor: "#666" },
            height: 40, margin: 10, maskFill: "rgba(255, 255, 255, 0.75)", outlineColor: "#444", outlineWidth: 1, series: { type: "areaspline", color: "#4572A7", compare: null, fillOpacity: 0.4, dataGrouping: { approximation: "average", groupPixelWidth: 2, smoothed: !0, units: I }, dataLabels: { enabled: !1, zIndex: 2 }, id: "highcharts-navigator-series", lineColor: "#4572A7", lineWidth: 1, marker: { enabled: !1 }, pointRange: 0, shadow: !1, threshold: null }, xAxis: { tickWidth: 0, lineWidth: 0, gridLineWidth: 1, tickPixelInterval: 200, labels: { align: "left", x: 3, y: -4 } }, yAxis: {
                gridLineWidth: 0,
                startOnTick: !1, endOnTick: !1, minPadding: 0.1, maxPadding: 0.1, labels: { enabled: !1 }, title: { text: null }, tickWidth: 0
            }
        }, scrollbar: {
            height: hb ? 20 : 14, barBackgroundColor: P, barBorderRadius: 2, barBorderWidth: 1, barBorderColor: "#666", buttonArrowColor: "#666", buttonBackgroundColor: P, buttonBorderColor: "#666", buttonBorderRadius: 2, buttonBorderWidth: 1, minWidth: 6, rifleColor: "#666", trackBackgroundColor: lb("linearGradient", { x1: 0, y1: 0, x2: 0, y2: 1 }, "stops", [[0, "#EEE"], [1, "#FFF"]]), trackBorderColor: "#CCC", trackBorderWidth: 1, liveRedraw: ea &&
            !hb
        }
    }); Ib.prototype = {
        drawHandle: function (a, b) {
            var c = this.chart, d = c.renderer, e = this.elementsToDestroy, f = this.handles, g = this.navigatorOptions.handles, g = { fill: g.backgroundColor, stroke: g.borderColor, "stroke-width": 1 }, h; this.rendered || (f[b] = d.g().css({ cursor: "e-resize" }).attr({ zIndex: 4 - b }).add(), h = d.rect(-4.5, 0, 9, 16, 3, 1).attr(g).add(f[b]), e.push(h), h = d.path(["M", -1.5, 4, "L", -1.5, 12, "M", 0.5, 4, "L", 0.5, 12]).attr(g).add(f[b]), e.push(h)); f[b][c.isResizing ? "animate" : "attr"]({
                translateX: this.scrollerLeft + this.scrollbarHeight +
                parseInt(a, 10), translateY: this.top + this.height / 2 - 8
            })
        }, drawScrollbarButton: function (a) {
            var b = this.chart.renderer, c = this.elementsToDestroy, d = this.scrollbarButtons, e = this.scrollbarHeight, f = this.scrollbarOptions, g; this.rendered || (d[a] = b.g().add(this.scrollbarGroup), g = b.rect(-0.5, -0.5, e + 1, e + 1, f.buttonBorderRadius, f.buttonBorderWidth).attr({ stroke: f.buttonBorderColor, "stroke-width": f.buttonBorderWidth, fill: f.buttonBackgroundColor }).add(d[a]), c.push(g), g = b.path(["M", e / 2 + (a ? -1 : 1), e / 2 - 3, "L", e / 2 + (a ? -1 : 1), e /
            2 + 3, e / 2 + (a ? 2 : -2), e / 2]).attr({ fill: f.buttonArrowColor }).add(d[a]), c.push(g)); a && d[a].attr({ translateX: this.scrollerWidth - e })
        }, render: function (a, b, c, d) {
            var e = this.chart, f = e.renderer, g, h, i, k, j = this.scrollbarGroup, l = this.navigatorGroup, m = this.scrollbar, l = this.xAxis, n = this.scrollbarTrack, p = this.scrollbarHeight, q = this.scrollbarEnabled, y = this.navigatorOptions, w = this.scrollbarOptions, t = w.minWidth, x = this.height, v = this.top, C = this.navigatorEnabled, A = y.outlineWidth, B = A / 2, E = 0, J = this.outlineHeight, I = w.barBorderRadius,
            F = w.barBorderWidth, G = v + B; if (!isNaN(a)) {
                this.navigatorLeft = g = o(l.left, e.plotLeft + p); this.navigatorWidth = h = o(l.len, e.plotWidth - 2 * p); this.scrollerLeft = i = g - p; this.scrollerWidth = k = k = h + 2 * p; if (l.getExtremes) { var H = e.xAxis[0].getExtremes(), N = H.dataMin === null, L = l.getExtremes(), P = D(H.dataMin, L.dataMin), H = u(H.dataMax, L.dataMax); !N && (P !== L.min || H !== L.max) && l.setExtremes(P, H, !0, !1) } h === 0 || s(a) === s(b) && c === r ? (c = 0, d = k) : (c = o(c, l.translate(a)), d = o(d, l.translate(b))); this.zoomedMax = a = D(z(u(c, d)), h); this.zoomedMin =
                d = this.fixedWidth ? a - this.fixedWidth : u(z(D(c, d)), 0); this.range = c = a - d; if (!this.rendered) {
                    if (C) this.navigatorGroup = l = f.g("navigator").attr({ zIndex: 3 }).add(), this.leftShade = f.rect().attr({ fill: y.maskFill }).add(l), this.rightShade = f.rect().attr({ fill: y.maskFill }).add(l), this.outline = f.path().attr({ "stroke-width": A, stroke: y.outlineColor }).add(l); if (q) this.scrollbarGroup = j = f.g("scrollbar").add(), m = w.trackBorderWidth, this.scrollbarTrack = n = f.rect().attr({
                        y: -m % 2 / 2, fill: w.trackBackgroundColor, stroke: w.trackBorderColor,
                        "stroke-width": m, r: w.trackBorderRadius || 0, height: p
                    }).add(j), this.scrollbar = m = f.rect().attr({ y: -F % 2 / 2, height: p, fill: w.barBackgroundColor, stroke: w.barBorderColor, "stroke-width": F, r: I }).add(j), this.scrollbarRifles = f.path().attr({ stroke: w.rifleColor, "stroke-width": 1 }).add(j)
                } e = e.isResizing ? "animate" : "attr"; C && (this.leftShade[e]({ x: g, y: v, width: d, height: x }), this.rightShade[e]({ x: g + a, y: v, width: h - a, height: x }), this.outline[e]({ d: ["M", i, G, "L", g + d + B, G, g + d + B, G + J - p, "M", g + a - B, G + J - p, "L", g + a - B, G, i + k, G] }), this.drawHandle(d +
                B, 0), this.drawHandle(a + B, 1)); if (q && j) this.drawScrollbarButton(0), this.drawScrollbarButton(1), j[e]({ translateX: i, translateY: s(G + x) }), n[e]({ width: k }), g = p + d, h = c - F, h < t && (E = (t - h) / 2, h = t, g -= E), this.scrollbarPad = E, m[e]({ x: U(g) + F % 2 / 2, width: h }), t = p + d + c / 2 - 0.5, this.scrollbarRifles.attr({ visibility: c > 12 ? "visible" : "hidden" })[e]({ d: ["M", t - 3, p / 4, "L", t - 3, 2 * p / 3, "M", t, p / 4, "L", t, 2 * p / 3, "M", t + 3, p / 4, "L", t + 3, 2 * p / 3] }); this.scrollbarPad = E; this.rendered = !0
            }
        }, addEvents: function () {
            var a = this.chart.container, b = this.mouseDownHandler,
            c = this.mouseMoveHandler, d = this.mouseUpHandler, e; e = [[a, "mousedown", b], [a, "mousemove", c], [document, "mouseup", d]]; ib && e.push([a, "touchstart", b], [a, "touchmove", c], [document, "touchend", d]); q(e, function (a) { E.apply(null, a) }); this._events = e
        }, removeEvents: function () { q(this._events, function (a) { V.apply(null, a) }); this._events = r; this.navigatorEnabled && this.baseSeries && V(this.baseSeries, "updatedData", this.updatedDataHandler) }, init: function () {
            var a = this, b = a.chart, c, d, e = a.scrollbarHeight, f = a.navigatorOptions, g = a.height,
            h = a.top, i, k, j = document.body.style, l, m = a.baseSeries; a.mouseDownHandler = function (d) {
                var d = b.pointer.normalize(d), e = a.zoomedMin, f = a.zoomedMax, h = a.top, k = a.scrollbarHeight, m = a.scrollerLeft, n = a.scrollerWidth, p = a.navigatorLeft, o = a.navigatorWidth, q = a.scrollbarPad, r = a.range, t = d.chartX, s = d.chartY, d = b.xAxis[0], u = hb ? 10 : 7; if (s > h && s < h + g + k) if ((h = !a.scrollbarEnabled || s < h + g) && S.abs(t - e - p) < u) a.grabbedLeft = !0, a.otherHandlePos = f; else if (h && S.abs(t - f - p) < u) a.grabbedRight = !0, a.otherHandlePos = e; else if (t > p + e - q && t < p + f + q) {
                    a.grabbedCenter =
                    t; a.fixedWidth = r; if (b.renderer.isSVG) l = j.cursor, j.cursor = "ew-resize"; i = t - e
                } else if (t > m && t < m + n && (f = h ? t - p - r / 2 : t < p ? e - D(10, r) : t > m + n - k ? e + D(10, r) : t < p + e ? e - r : f, f < 0 ? f = 0 : f + r > o && (f = o - r), f !== e)) { a.fixedWidth = r; if (!d.ordinalPositions) b.fixedRange = d.max - d.min; e = c.translate(f, !0); d.setExtremes(e, b.fixedRange ? e + b.fixedRange : c.translate(f + r, !0), !0, !1, { trigger: "navigator" }) }
            }; a.mouseMoveHandler = function (c) {
                var d = a.scrollbarHeight, e = a.navigatorLeft, f = a.navigatorWidth, g = a.scrollerLeft, h = a.scrollerWidth, j = a.range, l; if (c.pageX !==
                0) c = b.pointer.normalize(c), l = c.chartX, l < e ? l = e : l > g + h - d && (l = g + h - d), a.grabbedLeft ? (k = !0, a.render(0, 0, l - e, a.otherHandlePos)) : a.grabbedRight ? (k = !0, a.render(0, 0, a.otherHandlePos, l - e)) : a.grabbedCenter && (k = !0, l < i ? l = i : l > f + i - j && (l = f + i - j), a.render(0, 0, l - i, l - i + j)), k && a.scrollbarOptions.liveRedraw && setTimeout(function () { a.mouseUpHandler(c) }, 0)
            }; a.mouseUpHandler = function (d) {
                k && b.xAxis[0].setExtremes(c.translate(a.zoomedMin, !0), c.translate(a.zoomedMax, !0), !0, !1, { trigger: "navigator", DOMEvent: d }); if (d.type !== "mousemove") a.grabbedLeft =
                a.grabbedRight = a.grabbedCenter = a.fixedWidth = k = i = null, j.cursor = l || ""
            }; var n = b.xAxis.length, p = b.yAxis.length; b.extraBottomMargin = a.outlineHeight + f.margin; a.navigatorEnabled ? (a.xAxis = c = new ua(b, x({ ordinal: m && m.xAxis.options.ordinal }, f.xAxis, { id: "navigator-x-axis", isX: !0, type: "datetime", index: n, height: g, offset: 0, offsetLeft: e, offsetRight: -e, startOnTick: !1, endOnTick: !1, minPadding: 0, maxPadding: 0, zoomEnabled: !1 })), a.yAxis = d = new ua(b, x(f.yAxis, {
                id: "navigator-y-axis", alignTicks: !1, height: g, offset: 0, index: p,
                zoomEnabled: !1
            })), m || f.series.data ? a.addBaseSeries() : b.series.length === 0 && ma(b, "redraw", function (c, d) { if (b.series.length > 0 && !a.series) a.setBaseSeries(), b.redraw = c; c.call(b, d) })) : a.xAxis = c = { translate: function (a, c) { var d = b.xAxis[0].getExtremes(), f = b.plotWidth - 2 * e, g = d.dataMin, d = d.dataMax - g; return c ? a * d / f + g : f * (a - g) / d } }; ma(b, "getMargins", function (b) {
                var e = this.legend, f = e.options; b.call(this); a.top = h = a.navigatorOptions.top || this.chartHeight - a.height - a.scrollbarHeight - this.options.chart.spacingBottom - (f.verticalAlign ===
                "bottom" && f.enabled && !f.floating ? e.legendHeight + o(f.margin, 10) : 0); if (c && d) c.options.top = d.options.top = h, c.setAxisSize(), d.setAxisSize()
            }); a.addEvents()
        }, setBaseSeries: function (a) { var b = this.chart, a = a || b.options.navigator.baseSeries; this.series && this.series.remove(); this.baseSeries = b.series[a] || typeof a === "string" && b.get(a) || b.series[0]; this.xAxis && this.addBaseSeries() }, addBaseSeries: function () {
            var a = this.baseSeries, b = a ? a.options : {}, c = b.data, d = this.navigatorOptions.series, e; e = d.data; this.hasNavigatorData =
            !!e; b = x(b, d, { clip: !1, enableMouseTracking: !1, group: "nav", padXAxis: !1, xAxis: "navigator-x-axis", yAxis: "navigator-y-axis", name: "Navigator", showInLegend: !1, isInternal: !0, visible: !0 }); b.data = e || c; this.series = this.chart.initSeries(b); if (a && this.navigatorOptions.adaptToUpdatedData !== !1) E(a, "updatedData", this.updatedDataHandler), a.userOptions.events = v(a.userOptions.event, { updatedData: this.updatedDataHandler })
        }, updatedDataHandler: function () {
            var a = this.chart.scroller, b = a.baseSeries, c = b.xAxis, d = c.getExtremes(),
            e = d.min, f = d.max, g = d.dataMin, d = d.dataMax, h = f - e, i, k, j, l, m, n = a.series; i = n.xData; var p = !!c.setExtremes; k = f >= i[i.length - 1] - (this.closestPointRange || 0); i = e <= g; if (!a.hasNavigatorData) n.options.pointStart = b.xData[0], n.setData(b.options.data, !1), m = !0; i && (l = g, j = l + h); k && (j = d, i || (l = u(j - h, n.xData[0]))); p && (i || k) ? isNaN(l) || c.setExtremes(l, j, !0, !1, { trigger: "updatedData" }) : (m && this.chart.redraw(!1), a.render(u(e, g), D(f, d)))
        }, destroy: function () {
            this.removeEvents(); q([this.xAxis, this.yAxis, this.leftShade, this.rightShade,
            this.outline, this.scrollbarTrack, this.scrollbarRifles, this.scrollbarGroup, this.scrollbar], function (a) { a && a.destroy && a.destroy() }); this.xAxis = this.yAxis = this.leftShade = this.rightShade = this.outline = this.scrollbarTrack = this.scrollbarRifles = this.scrollbarGroup = this.scrollbar = null; q([this.scrollbarButtons, this.handles, this.elementsToDestroy], function (a) { Ca(a) })
        }
    }; Highcharts.Scroller = Ib; ma(ua.prototype, "zoom", function (a, b, c) {
        var d = this.chart, e = d.options, f = e.chart.zoomType, g = e.navigator, e = e.rangeSelector,
        h; if (this.isXAxis && (g && g.enabled || e && e.enabled)) if (f === "x") d.resetZoomButton = "blocked"; else if (f === "y") h = !1; else if (f === "xy") d = this.previousZoom, t(b) ? this.previousZoom = [this.min, this.max] : d && (b = d[0], c = d[1], delete this.previousZoom); return h !== r ? h : a.call(this, b, c)
    }); ma(Ta.prototype, "init", function (a, b, c) { E(this, "beforeRender", function () { var a = this.options; if (a.navigator.enabled || a.scrollbar.enabled) this.scroller = new Ib(this) }); a.call(this, b, c) }); v(N, {
        rangeSelector: {
            buttonTheme: {
                width: 28, height: 16,
                padding: 1, r: 0, stroke: "#68A", zIndex: 7
            }, inputPosition: { align: "right" }, labelStyle: { color: "#666" }
        }
    }); N.lang = x(N.lang, { rangeSelectorZoom: "Zoom", rangeSelectorFrom: "From", rangeSelectorTo: "To" }); Jb.prototype = {
        clickButton: function (a, b, c) {
            var d = this, e = d.selected, f = d.chart, g = d.buttons, h = f.xAxis[0], i = h && h.getExtremes(), k = f.scroller && f.scroller.xAxis, j = k && k.getExtremes && k.getExtremes(), k = j && j.dataMin, j = j && j.dataMax, l = i && i.dataMin, m = i && i.dataMax, n = (t(l) && t(k) ? D : o)(l, k), p = (t(m) && t(j) ? u : o)(m, j), s, i = h && D(i.max, o(p,
            i.max)), k = new Date(i), j = b.type, l = b.count, y, w, m = { millisecond: 1, second: 1E3, minute: 6E4, hour: 36E5, day: 864E5, week: 6048E5 }; if (!(n === null || p === null || a === d.selected)) {
                if (m[j]) y = m[j] * l, s = u(i - y, n); else if (j === "month" || j === "year") y = { month: "Month", year: "FullYear" }[j], k["set" + y](k["get" + y]() - l), s = u(k.getTime(), o(n, Number.MIN_VALUE)), y = { month: 30, year: 365 }[j] * 864E5 * l; else if (j === "ytd") if (h) {
                    if (p === r) n = Number.MAX_VALUE, p = Number.MIN_VALUE, q(f.series, function (a) { a = a.xData; n = D(a[0], n); p = u(a[a.length - 1], p) }), c = !1; i =
                    new Date(p); w = i.getFullYear(); s = w = u(n || 0, Date.UTC(w, 0, 1)); i = i.getTime(); i = D(p || i, i)
                } else { E(f, "beforeRender", function () { d.clickButton(a, b) }); return } else j === "all" && h && (s = n, i = p); g[e] && g[e].setState(0); g[a] && g[a].setState(2); f.fixedRange = y; h ? h.setExtremes(s, i, o(c, 1), 0, { trigger: "rangeSelectorButton", rangeSelectorButton: b }) : (c = f.options.xAxis, c[0] = x(c[0], { range: y, min: w })); d.selected = a
            }
        }, defaultButtons: [{ type: "month", count: 1, text: "1m" }, { type: "month", count: 3, text: "3m" }, { type: "month", count: 6, text: "6m" }, {
            type: "ytd",
            text: "YTD"
        }, { type: "year", count: 1, text: "1y" }, { type: "all", text: "All" }], init: function (a) {
            var b = this, c = a.options.rangeSelector, d = c.buttons || [].concat(b.defaultButtons), e = b.buttons = [], c = c.selected, f = b.blurInputs = function () { var a = b.minInput, c = b.maxInput; a && a.blur(); c && c.blur() }; b.chart = a; a.extraTopMargin = 25; b.buttonOptions = d; E(a.container, "mousedown", f); E(a, "resize", f); c !== r && d[c] && this.clickButton(c, d[c], !1); E(a, "load", function () {
                E(a.xAxis[0], "afterSetExtremes", function () {
                    if (a.fixedRange !== this.max - this.min) e[b.selected] &&
                    !a.renderer.forExport && e[b.selected].setState(0), b.selected = a.fixedRange = null
                })
            })
        }, setInputValue: function (a, b) { var c = this.chart.options.rangeSelector; if (t(b)) this[a + "Input"].HCTime = b; this[a + "Input"].value = Aa(c.inputEditDateFormat || "%Y-%m-%d", this[a + "Input"].HCTime); this[a + "DateBox"].attr({ text: Aa(c.inputDateFormat || "%b %e, %Y", this[a + "Input"].HCTime) }) }, drawInput: function (a) {
            var b = this, c = b.chart, d = c.options.chart.style, e = c.renderer, f = c.options.rangeSelector, g = b.div, h = a === "min", i, k, j, l = this.inputGroup;
            this[a + "Label"] = k = e.label(N.lang[h ? "rangeSelectorFrom" : "rangeSelectorTo"], this.inputGroup.offset).attr({ padding: 1 }).css(x(d, f.labelStyle)).add(l); l.offset += k.width + 5; this[a + "DateBox"] = j = e.label("", l.offset).attr({ padding: 1, width: 90, height: 16, stroke: "silver", "stroke-width": 1 }).css(x({ textAlign: "center" }, d, f.inputStyle)).on("click", function () { b[a + "Input"].focus() }).add(l); l.offset += j.width + (h ? 10 : 0); this[a + "Input"] = i = Z("input", { name: a, className: "highcharts-range-selector", type: "text" }, v({
                position: "absolute",
                border: 0, width: "1px", height: "1px", padding: 0, textAlign: "center", fontSize: d.fontSize, fontFamily: d.fontFamily, top: c.plotTop + "px"
            }, f.inputStyle), g); i.onfocus = function () { F(this, { left: l.translateX + j.x + "px", top: l.translateY + "px", width: j.width - 2 + "px", height: j.height - 2 + "px", border: "2px solid silver" }) }; i.onblur = function () { F(this, { border: 0, width: "1px", height: "1px" }); b.setInputValue(a) }; i.onchange = function () {
                var a = i.value, d = (f.inputDateParser || Date.parse)(a), e = c.xAxis[0].getExtremes(); isNaN(d) && (d = a.split("-"),
                d = Date.UTC(z(d[0]), z(d[1]) - 1, z(d[2]))); if (!isNaN(d) && (N.global.useUTC || (d += (new Date).getTimezoneOffset() * 6E4), h && d >= e.dataMin && d <= b.maxInput.HCTime || !h && d <= e.dataMax && d >= b.minInput.HCTime)) c.xAxis[0].setExtremes(h ? d : e.min, h ? e.max : d, r, r, { trigger: "rangeSelectorInput" })
            }
        }, render: function (a, b) {
            var c = this, d = c.chart, e = d.renderer, f = d.container, g = d.options, h = g.exporting && g.navigation && g.navigation.buttonOptions, i = g.rangeSelector, k = c.buttons, j = N.lang, l = c.div, l = c.inputGroup, m = i.buttonTheme, n = i.inputEnabled !==
            !1, p = m && m.states, o = d.plotLeft, r; if (!c.rendered && (c.zoomText = e.text(j.rangeSelectorZoom, o, d.plotTop - 10).css(i.labelStyle).add(), r = o + c.zoomText.getBBox().width + 5, q(c.buttonOptions, function (a, b) { k[b] = e.button(a.text, r, d.plotTop - 25, function () { c.clickButton(b, a); c.isActive = !0 }, m, p && p.hover, p && p.select).css({ textAlign: "center" }).add(); r += k[b].width + (i.buttonSpacing || 0); c.selected === b && k[b].setState(2) }), n)) c.div = l = Z("div", null, { position: "relative", height: 0, zIndex: 1 }), f.parentNode.insertBefore(l, f), c.inputGroup =
            l = e.g("input-group").add(), l.offset = 0, c.drawInput("min"), c.drawInput("max"); n && (f = d.plotTop - 35, l.align(v({ y: f, width: l.offset, x: h && f < (h.y || 0) + h.height - g.chart.spacingTop ? -40 : 0 }, i.inputPosition), !0, d.spacingBox), c.setInputValue("min", a), c.setInputValue("max", b)); c.rendered = !0
        }, destroy: function () {
            var a = this.minInput, b = this.maxInput, c = this.chart, d = this.blurInputs, e; V(c.container, "mousedown", d); V(c, "resize", d); Ca(this.buttons); if (a) a.onfocus = a.onblur = a.onchange = null; if (b) b.onfocus = b.onblur = b.onchange =
            null; for (e in this) this[e] && e !== "chart" && (this[e].destroy ? this[e].destroy() : this[e].nodeType && Ya(this[e])), this[e] = null
        }
    }; ma(Ta.prototype, "init", function (a, b, c) { E(this, "init", function () { if (this.options.rangeSelector.enabled) this.rangeSelector = new Jb(this) }); a.call(this, b, c) }); Highcharts.RangeSelector = Jb; Ta.prototype.callbacks.push(function (a) {
        function b() { f = a.xAxis[0].getExtremes(); g.render(u(f.min, f.dataMin), D(f.max, o(f.dataMax, Number.MAX_VALUE))) } function c() {
            f = a.xAxis[0].getExtremes(); isNaN(f.min) ||
            h.render(f.min, f.max)
        } function d(a) { g.render(a.min, a.max) } function e(a) { h.render(a.min, a.max) } var f, g = a.scroller, h = a.rangeSelector; g && (E(a.xAxis[0], "afterSetExtremes", d), ma(a, "drawChartBox", function (a) { var c = this.isDirtyBox; a.call(this); c && b() }), b()); h && (E(a.xAxis[0], "afterSetExtremes", e), E(a, "resize", c), c()); E(a, "destroy", function () { g && V(a.xAxis[0], "afterSetExtremes", d); h && (V(a, "resize", c), V(a.xAxis[0], "afterSetExtremes", e)) })
    }); Highcharts.StockChart = function (a, b) {
        var c = a.series, d, e = {
            marker: {
                enabled: !1,
                states: { hover: { radius: 5 } }
            }, states: { hover: { lineWidth: 2 } }
        }, f = { shadow: !1, borderWidth: 0 }; a.xAxis = Ga(ha(a.xAxis || {}), function (a) { return x({ minPadding: 0, maxPadding: 0, ordinal: !0, title: { text: null }, labels: { overflow: "justify" }, showLastLabel: !0 }, a, { type: "datetime", categories: null }) }); a.yAxis = Ga(ha(a.yAxis || {}), function (a) { d = a.opposite; return x({ labels: { align: d ? "right" : "left", x: d ? -2 : 2, y: -2 }, showLastLabel: !1, title: { text: null } }, a) }); a.series = null; a = x({
            chart: { panning: !0, pinchType: "x" }, navigator: { enabled: !0 }, scrollbar: { enabled: !0 },
            rangeSelector: { enabled: !0 }, title: { text: null }, tooltip: { shared: !0, crosshairs: !0 }, legend: { enabled: !1 }, plotOptions: { line: e, spline: e, area: e, areaspline: e, arearange: e, areasplinerange: e, column: f, columnrange: f, candlestick: f, ohlc: f }
        }, a, { _stock: !0, chart: { inverted: !1 } }); a.series = c; return new Ta(a, b)
    }; ma(rb.prototype, "init", function (a, b, c) { var d = c.chart.pinchType || ""; a.call(this, b, c); this.pinchX = this.pinchHor = d.indexOf("x") !== -1; this.pinchY = this.pinchVert = d.indexOf("y") !== -1 }); var kc = R.init, lc = R.processData, mc =
    Ia.prototype.tooltipFormatter; R.init = function () { kc.apply(this, arguments); this.setCompare(this.options.compare) }; R.setCompare = function (a) { this.modifyValue = a === "value" || a === "percent" ? function (b, c) { var d = this.compareValue, b = a === "value" ? b - d : b = 100 * (b / d) - 100; if (c) c.change = b; return b } : null; if (this.chart.hasRendered) this.isDirty = !0 }; R.processData = function () {
        var a = 0, b, c, d; lc.apply(this, arguments); if (this.xAxis && this.processedYData) {
            b = this.processedXData; c = this.processedYData; for (d = c.length; a < d; a++) if (typeof c[a] ===
            "number" && b[a] >= this.xAxis.min) { this.compareValue = c[a]; break }
        }
    }; ma(R, "getExtremes", function (a) { a.call(this); if (this.modifyValue) this.dataMax = this.modifyValue(this.dataMax), this.dataMin = this.modifyValue(this.dataMin) }); ua.prototype.setCompare = function (a, b) { this.isXAxis || (q(this.series, function (b) { b.setCompare(a) }), o(b, !0) && this.chart.redraw()) }; Ia.prototype.tooltipFormatter = function (a) {
        a = a.replace("{point.change}", (this.change > 0 ? "+" : "") + za(this.change, o(this.series.tooltipOptions.changeDecimals, 2)));
        return mc.apply(this, [a])
    }; (function () {
        var a = R.init, b = R.getSegments; R.init = function () {
            var b, d; a.apply(this, arguments); b = this.chart; (d = this.xAxis) && d.options.ordinal && E(this, "updatedData", function () { delete d.ordinalIndex }); if (d && d.options.ordinal && !d.hasOrdinalExtension) {
                d.hasOrdinalExtension = !0; d.beforeSetTickPositions = function () {
                    var a, b = [], c = !1, e, k = this.getExtremes(), j = k.min, k = k.max, l; if (this.options.ordinal) {
                        q(this.series, function (c, d) {
                            if (c.visible !== !1 && c.takeOrdinalPosition !== !1 && (b = b.concat(c.processedXData),
                            a = b.length, b.sort(function (a, b) { return a - b }), a)) for (d = a - 1; d--;) b[d] === b[d + 1] && b.splice(d, 1)
                        }); a = b.length; if (a > 2) { e = b[1] - b[0]; for (l = a - 1; l-- && !c;) b[l + 1] - b[l] !== e && (c = !0) } c ? (this.ordinalPositions = b, c = d.val2lin(j, !0), e = d.val2lin(k, !0), this.ordinalSlope = k = (k - j) / (e - c), this.ordinalOffset = j - c * k) : this.ordinalPositions = this.ordinalSlope = this.ordinalOffset = r
                    }
                }; d.val2lin = function (a, b) {
                    var c = this.ordinalPositions; if (c) {
                        var d = c.length, e, j; for (e = d; e--;) if (c[e] === a) { j = e; break } for (e = d - 1; e--;) if (a > c[e] || e === 0) {
                            c = (a -
                            c[e]) / (c[e + 1] - c[e]); j = e + c; break
                        } return b ? j : this.ordinalSlope * (j || 0) + this.ordinalOffset
                    } else return a
                }; d.lin2val = function (a, b) { var c = this.ordinalPositions; if (c) { var d = this.ordinalSlope, e = this.ordinalOffset, j = c.length - 1, l, m; if (b) a < 0 ? a = c[0] : a > j ? a = c[j] : (j = U(a), m = a - j); else for (; j--;) if (l = d * j + e, a >= l) { d = d * (j + 1) + e; m = (a - l) / (d - l); break } return m !== r && c[j] !== r ? c[j] + (m ? m * (c[j + 1] - c[j]) : 0) : a } else return a }; d.getExtendedPositions = function () {
                    var a = d.series[0].currentDataGrouping, e = d.ordinalIndex, h = a ? a.count + a.unitName :
                    "raw", i = d.getExtremes(), k, j; if (!e) e = d.ordinalIndex = {}; if (!e[h]) k = { series: [], getExtremes: function () { return { min: i.dataMin, max: i.dataMax } }, options: { ordinal: !0 } }, q(d.series, function (d) { j = { xAxis: k, xData: d.xData, chart: b, destroyGroupedData: qa }; j.options = { dataGrouping: a ? { enabled: !0, forced: !0, approximation: "open", units: [[a.unitName, [a.count]]] } : { enabled: !1 } }; d.processData.apply(j); k.series.push(j) }), d.beforeSetTickPositions.apply(k), e[h] = k.ordinalPositions; return e[h]
                }; d.getGroupIntervalFactor = function (a,
                b, c) { for (var d = 0, e = c.length, j = []; d < e - 1; d++) j[d] = c[d + 1] - c[d]; j.sort(function (a, b) { return a - b }); d = j[U(e / 2)]; a = u(a, c[0]); b = D(b, c[e - 1]); return e * d / (b - a) }; d.postProcessTickInterval = function (a) { var b = this.ordinalSlope; return b ? a / (b / d.closestPointRange) : a }; d.getNonLinearTimeTicks = function (a, b, c, e, k, j, l) {
                    var m = 0, n = 0, o, q = {}, s, u, x, v = [], z = -Number.MAX_VALUE, B = d.options.tickPixelInterval; if (!k || k.length < 3 || b === r) return fb(a, b, c, e); for (u = k.length; n < u; n++) {
                        x = n && k[n - 1] > c; k[n] < b && (m = n); if (n === u - 1 || k[n + 1] - k[n] > j * 5 ||
                        x) { if (k[n] > z) { for (o = fb(a, k[m], k[n], e) ; o.length && o[0] <= z;) o.shift(); o.length && (z = o[o.length - 1]); v = v.concat(o) } m = n + 1 } if (x) break
                    } a = o.info; if (l && a.unitRange <= H[Ba]) { n = v.length - 1; for (m = 1; m < n; m++) (new Date(v[m]))[Pa]() !== (new Date(v[m - 1]))[Pa]() && (q[v[m]] = fa, s = !0); s && (q[v[0]] = fa); a.higherRanks = q } v.info = a; if (l && t(B)) {
                        var l = a = v.length, n = [], A; for (s = []; l--;) m = d.translate(v[l]), A && (s[l] = A - m), n[l] = A = m; s.sort(); s = s[U(s.length / 2)]; s < B * 0.6 && (s = null); l = v[a - 1] > c ? a - 1 : a; for (A = void 0; l--;) m = n[l], c = A - m, A && c < B * 0.8 && (s ===
                        null || c < s * 0.8) ? (q[v[l]] && !q[v[l + 1]] ? (c = l + 1, A = m) : c = l, v.splice(c, 1)) : A = m
                    } return v
                }; var e = b.pan; b.pan = function (a) {
                    var d = b.xAxis[0], h = !1; if (d.options.ordinal && d.series.length) {
                        var i = b.mouseDownX, k = d.getExtremes(), j = k.dataMax, l = k.min, m = k.max, n; n = b.hoverPoints; var o = d.closestPointRange, i = (i - a) / (d.translationSlope * (d.ordinalSlope || o)), r = { ordinalPositions: d.getExtendedPositions() }, s, o = d.lin2val, t = d.val2lin; if (r.ordinalPositions) {
                            if (T(i) > 1) n && q(n, function (a) { a.setState() }), i < 0 ? (n = r, r = d.ordinalPositions ? d :
                            r) : n = d.ordinalPositions ? d : r, s = r.ordinalPositions, j > s[s.length - 1] && s.push(j), n = o.apply(n, [t.apply(n, [l, !0]) + i, !0]), i = o.apply(r, [t.apply(r, [m, !0]) + i, !0]), n > D(k.dataMin, l) && i < u(j, m) && d.setExtremes(n, i, !0, !1, { trigger: "pan" }), b.mouseDownX = a, F(b.container, { cursor: "move" })
                        } else h = !0
                    } else h = !0; h && e.apply(b, arguments)
                }
            }
        }; R.getSegments = function () {
            var a, d = this.options.gapSize, e = this.xAxis; b.apply(this); if (e.options.ordinal && d) a = this.segments, q(a, function (b, g) {
                for (var h = b.length - 1; h--;) b[h + 1].x - b[h].x > e.closestPointRange *
                d && a.splice(g + 1, 0, b.splice(h + 1, b.length - h))
            })
        }
    })(); v(Highcharts, {
        Axis: ua, Chart: Ta, Color: wa, Legend: Hb, Pointer: rb, Point: Ia, Tick: $a, Tooltip: Gb, Renderer: bb, Series: W, SVGElement: Ea, SVGRenderer: Ha, arrayMin: Qa, arrayMax: ta, charts: Va, dateFormat: Aa, format: Ma, pathAnim: Lb, getOptions: function () { return N }, hasBidiBug: cc, isTouchDevice: hb, numberFormat: za, seriesTypes: L, setOptions: function (a) { N = x(N, a); Tb(); return N }, addEvent: E, removeEvent: V, createElement: Z, discardElement: Ya, css: F, each: q, extend: v, map: Ga, merge: x, pick: o,
        splat: ha, extendClass: ca, pInt: z, wrap: ma, svg: ea, canvas: ga, vml: !ea && !ga, product: "Highstock", version: "1.3.4"
    })
})();
