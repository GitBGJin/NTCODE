﻿var chartResult;

//加载审核Chart数据
function AjaxLoadingMutilFactor(facCode, StartTime, EndTime, pointId, applicatironUID) {
    $.ajax({
        type: "POST", //用POST方式传输
        data: {
            "FactorCode": facCode,
            "StartTime": StartTime,
            "EndTime": EndTime,
            "PointID": pointId,
            "ApplicationUID": applicatironUID
        },
        dataType: "", //数据格式:JSON                  
        url: "AuditAjaxHandler.ashx?DataType=MutilPointFactorSuper",
        cache: false, //指令
        async: false, //取消同步

        beforeSend: function () {
        }, //发送数据之前
        success: function (msg) {
            var tsm = eval("(" + msg + ")");
            InitChart(tsm);
            chartResult = tsm;
        },
        error: function (res) {
            //alert("无数据！");
        }
    });
}

//审核提交
function AjaxSubmitAudit(StartTime, EndTime, pointId, applicationUID, UserGuid, pointType) {
    $.ajax({
        type: "POST", //用POST方式传输
        data: {
            "ApplicationUID": applicationUID,
            "StartTime": StartTime,
            "EndTime": EndTime,
            "PointID": pointId,
            "UserGuid": UserGuid,
            "pointType": pointType
        },
        dataType: "", //数据格式:JSON                  
        url: "AuditAjaxHandler.ashx?DataType=SubmitAuditSuper",
        cache: false, //指令
        async: true, //取消同步
        beforeSend: function () {
        }, //发送数据之前
        success: function (msg) {
            if (msg == "True") {
                //currentLoadingPanel.show();
                //Refresh_Grid(true);            
                document.getElementById('submitButton').click();
                alert("提交审核成功！");
            } else alert("提交审核失败！");
            $('#AuditSubmitDiv').css("display", "none");
        },
        error: function (res) {
            $('#AuditSubmitDiv').css("display", "none");
        }
    });
}

//审核提交
function AjaxSubmitAuditWeibo(StartTime, EndTime, pointId, UserGuid) {
    $.ajax({
        type: "POST", //用POST方式传输
        data: {
            "StartTime": StartTime,
            "EndTime": EndTime,
            "PointID": pointId,
            "UserGuid": UserGuid
        },
        dataType: "", //数据格式:JSON                  
        url: "AuditAjaxHandler.ashx?DataType=SubmitAuditWeibo",
        cache: false, //指令
        async: true, //取消同步
        beforeSend: function () {
        }, //发送数据之前
        success: function (msg) {
            if (msg == "True") {
                //currentLoadingPanel.show();
                //Refresh_Grid(true);            
                document.getElementById('submitButton').click();
                alert("提交审核成功！");
            } else alert("提交审核失败！");
            $('#AuditSubmitDiv').css("display", "none");
        },
        error: function (res) {
            $('#AuditSubmitDiv').css("display", "none");
        }
    });
}

//审核提交
function AjaxSubmitAuditLijingpu(StartTime, EndTime, pointId, UserGuid) {
    $.ajax({
        type: "POST", //用POST方式传输
        data: {
            "StartTime": StartTime,
            "EndTime": EndTime,
            "PointID": pointId,
            "UserGuid": UserGuid
        },
        dataType: "", //数据格式:JSON                  
        url: "AuditAjaxHandler.ashx?DataType=SubmitAuditLijingpu",
        cache: false, //指令
        async: true, //取消同步
        beforeSend: function () {
        }, //发送数据之前
        success: function (msg) {
            if (msg == "True") {
                //currentLoadingPanel.show();
                //Refresh_Grid(true);            
                document.getElementById('submitButton').click();
                alert("提交审核成功！");
            } else alert("提交审核失败！");
            $('#AuditSubmitDiv').css("display", "none");
        },
        error: function (res) {
            $('#AuditSubmitDiv').css("display", "none");
        }
    });
}



//恢复到原始数据
function AjaxAuditOperateData(PointID, DataTime, FactorCode, NewData, application, Reason, UserGuid, url) {
    $.ajax({
        type: "POST", //用POST方式传输
        data: {
            "PointID": PointID,
            "DataTime": DataTime,
            "FactorCode": FactorCode,
            "NewData": NewData,
            "ApplicationUID": application,
            "Reason": Reason,
            "UserGuid": UserGuid
        },
        dataType: "", //数据格式:JSON                  
        url: url,
        cache: true, //指令
        async: true, //取消同步
        beforeSend: function () {
        }, //发送数据之前
        success: function (msg) {
            //Refresh_Grid(true);
            //LoadingData();
            document.getElementById('refreshData').click();
        },
        error: function (res) {
            //alert(res + "***");
        }
    });
}

//恢复到原始数据
function AjaxAuditOperateDataWeibo(PointID, DataTime, FactorCode, NewData, Pollutant, Reason, UserGuid, url) {
    $.ajax({
        type: "POST", //用POST方式传输
        data: {
            "PointID": PointID,
            "DataTime": DataTime,
            "FactorCode": FactorCode,
            "NewData": NewData,
            "Reason": Reason,
            "UserGuid": UserGuid,
            "Pollutant": Pollutant
        },
        dataType: "", //数据格式:JSON                  
        url: url,
        cache: true, //指令
        async: true, //取消同步
        beforeSend: function () {
        }, //发送数据之前
        success: function (msg) {
            //Refresh_Grid(true);
            //LoadingData();
            document.getElementById('refreshData').click();
        },
        error: function (res) {
            //alert(res + "***");
        }
    });
}

function AjaxPreData() {
    $.ajax({
        type: "POST", //用POST方式传输
        dataType: "", //数据格式:JSON                  
        url: "AuditAjaxHandler.ashx?DataType=PreData",
        cache: true, //指令
        async: true, //取消同步

        beforeSend: function () {
        }, //发送数据之前
        success: function (msg) {
        },
        error: function (res) {
            //alert("无数据！");
        }
    });
}

function InitChart(tsm) {
    // 路径配置
    require.config({
        paths: {
            echarts: '../../../Resources/JavaScript/Echarts/build/dist'
        }
    });

    // 使用
    require(
        [
            'echarts',
            'echarts/chart/line', // 使用柱状图就加载bar模块，按需加载
              'echarts/chart/bar' // 使用柱状图就加载bar模块，按需加载
        ],
        function (ec) {
            // 基于准备好的dom，初始化echarts图表
            var myChart = ec.init(document.getElementById('auditChart'), "macarons");

            var option = {
                title: {
                    text: '',
                    subtext: ''
                },
                tooltip: {
                    trigger: 'item',
                    formatter: function (params) {
                        var date = new Date(params[1])
                        try {
                            var date = new Date(params[1].replace(/-/g, "/"));
                        } catch (e) {
                        }
                        var partamname = params[5].name.split(';');
                        if (tsm.applicationType == 'Water') {
                            data = +date.getDate() + '日'
                                   + date.getHours() + '时'
                                   + date.getMinutes() + '分';
                            return data + '  ' + params[0] + "<br/>" + partamname[0] + ":" + params[2][1];
                        }
                        else {
                            data = +date.getDate() + '日'
                               + date.getHours() + '时' + date.getMinutes() + '分' + date.getSeconds() + '秒';
                            //data = +date.getDate() + '日'
                            //   + date.getHours() + '时'
                            //   + data.getMinutes() + '分';
                            return data + '  ' + params[0] + "<br/>" + partamname[0] + ":" + params[2];
                        }
                    }
                },
                toolbox: {
                    show: true,
                    orient: 'vertical',
                    x: 'right',
                    y: 'center',
                    feature: {
                        mark: { show: true },
                        dataView: { show: false, readOnly: false },
                        magicType: { show: true, type: ['line', 'bar'] },
                        restore: { show: true },
                        saveAsImage: { show: true }
                    }
                },
                dataZoom: {
                    show: tsm.dataZoomShow
                    //,start: 30
                },
                legend: {
                    //x: 'left',
                    data: tsm.legend,
                    y: 'bottom'
                },
                grid: {
                    y2: 50,
                    y: 20
                },
                xAxis: [
                                {
                                    type: (tsm.applicationType == 'Water') ? 'time' : 'category',
                                    data: (tsm.applicationType == 'Water') ? null : tsm.category,
                                    //type: 'time',
                                    calculable: true,
                                    axisLabel: {
                                        formatter: function (params) {
                                            var time = new Date(params);
                                            try {
                                                time = new Date(params.replace(/-/g, "/"));
                                            } catch (e) {
                                                time = new Date(params);
                                            }
                                            if (params == undefined || params == "") return "";
                                            if (tsm.applicationType == 'Water') {
                                                return time.getDate() + '日'
                                                        + time.getHours() + '时'
                                                     + time.getMinutes() + "分";
                                            }
                                            else
                                                // return time.getDate() + '日'
                                                //+ time.getHours() + '时';
                                                return time.getDate() + '日'
                                               + time.getHours() + '时' + time.getMinutes() + '分' + time.getSeconds() + '秒';
                                        }
                                    }
                                }
                ],
                yAxis: tsm.yAxis,
                series: tsm.seriesList
            };

            RegChartRightClickEvents()

            var ecConfig = require('echarts/config');
            //鼠标悬停
            function eConsole(param) {
                try {
                    var aa = param;
                    if (param.data.name.split(';')[4] == "0") {
                        if (tsm.applicationType == 'Water')
                            ChartPointSelect(param.data.name, param.value[1]);
                        else
                            ChartPointSelect(param.data.name, param.value);
                    }
                    else
                        ClearSelectedInfo();
                } catch (e) {
                }
            }

            ////鼠标双击修改值
            //function eDoubleClick(param) {
            //    ChartPointSelect(param.data.name, param.value);
            //    window.radopen("AuditModityData.aspx?data=" + param.value, "ModifyData")
            //}

            //AuditModityData

            //myChart.on(ecConfig.EVENT.CONTEXTMENU, eConsole);
            ////myChart.on(ecConfig.EVENT.CLICK, eConsole);
            //myChart.on(ecConfig.EVENT.DBLCLICK, eDoubleClick);
            myChart.on(ecConfig.EVENT.HOVER, eConsole);
            ////myChart.on(ecConfig.EVENT.DATA_ZOOM, eConsole);
            ////myChart.on(ecConfig.EVENT.LEGEND_SELECTED, eConsole);
            ////myChart.on(ecConfig.EVENT.MAGIC_TYPE_CHANGED, eConsole);
            ////myChart.on(ecConfig.EVENT.DATA_VIEW_CHANGED, eConsole);

            // 为echarts对象加载数据 
            myChart.setOption(option);
        }
);
}

function RedrawChart() {
    try {
        if (chartResult != null && chartResult != undefined) {
            var tsm = chartResult;
            // 路径配置
            require.config({
                paths: {
                    echarts: '../../../Resources/JavaScript/Echarts/build/dist'
                }
            });

            // 使用
            require(
                [
                    'echarts',
                    'echarts/chart/line', // 使用柱状图就加载bar模块，按需加载
                      'echarts/chart/bar' // 使用柱状图就加载bar模块，按需加载
                ],
                function (ec) {
                    // 基于准备好的dom，初始化echarts图表
                    var myChart = ec.init(document.getElementById('auditChart'), "macarons");

                    var option = {
                        title: {
                            text: '',
                            subtext: ''
                        },
                        tooltip: {
                            trigger: 'item',
                            formatter: function (params) {
                                var date = new Date(params[1])
                                try {
                                    var date = new Date(params[1].replace(/-/g, "/"));
                                } catch (e) {
                                }
                                var partamname = params[5].name.split(';');
                                if (tsm.applicationType == 'Water') {
                                    data = +date.getDate() + '日'
                                           + date.getHours() + '时'
                                           + date.getMinutes() + '分';
                                    return data + '  ' + params[0] + "<br/>" + partamname[0] + ":" + params[2][1];
                                }
                                else {
                                    data = +date.getDate() + '日'
                                       + date.getHours() + '时';
                                    return data + '  ' + params[0] + "<br/>" + partamname[0] + ":" + params[2];
                                }
                            }
                        },
                        toolbox: {
                            show: true,
                            orient: 'vertical',
                            x: 'right',
                            y: 'center',
                            feature: {
                                mark: { show: true },
                                dataView: { show: false, readOnly: false },
                                magicType: { show: true, type: ['line', 'bar'] },
                                restore: { show: true },
                                saveAsImage: { show: true }
                            }
                        },
                        dataZoom: {
                            show: tsm.dataZoomShow
                            //,start: 30
                        },
                        legend: {
                            //x: 'left',
                            data: tsm.legend,
                            y: 'bottom'
                        },
                        grid: {
                            y2: 50,
                            y: 20
                        },
                        xAxis: [
                                        {
                                            type: (tsm.applicationType == 'Water') ? 'time' : 'category',
                                            data: (tsm.applicationType == 'Water') ? null : tsm.category,
                                            //type: 'time',
                                            calculable: true,
                                            axisLabel: {
                                                formatter: function (params) {
                                                    var time = new Date(params);
                                                    try {
                                                        time = new Date(params.replace(/-/g, "/"));
                                                    } catch (e) {
                                                        time = new Date(params);
                                                    }
                                                    if (params == undefined || params == "") return "";
                                                    if (tsm.applicationType == 'Water') {
                                                        return time.getDate() + '日'
                                                                + time.getHours() + '时'
                                                             + time.getMinutes() + "分";
                                                    }
                                                    else
                                                        return time.getDate() + '日'
                                                       + time.getHours() + '时';
                                                }
                                            }
                                        }
                        ],
                        yAxis: tsm.yAxis,
                        series: tsm.seriesList
                    };

                    RegChartRightClickEvents()

                    var ecConfig = require('echarts/config');
                    //鼠标悬停
                    function eConsole(param) {
                        try {
                            var aa = param;
                            if (param.data.name.split(';')[4] == "0") {
                                if (tsm.applicationType == 'Water')
                                    ChartPointSelect(param.data.name, param.value[1]);
                                else
                                    ChartPointSelect(param.data.name, param.value);
                            }
                            else
                                ClearSelectedInfo();
                        } catch (e) {
                        }
                    }

                    ////鼠标双击修改值
                    //function eDoubleClick(param) {
                    //    ChartPointSelect(param.data.name, param.value);
                    //    window.radopen("AuditModityData.aspx?data=" + param.value, "ModifyData")
                    //}

                    //AuditModityData

                    //myChart.on(ecConfig.EVENT.CONTEXTMENU, eConsole);
                    ////myChart.on(ecConfig.EVENT.CLICK, eConsole);
                    //myChart.on(ecConfig.EVENT.DBLCLICK, eDoubleClick);
                    myChart.on(ecConfig.EVENT.HOVER, eConsole);
                    ////myChart.on(ecConfig.EVENT.DATA_ZOOM, eConsole);
                    ////myChart.on(ecConfig.EVENT.LEGEND_SELECTED, eConsole);
                    ////myChart.on(ecConfig.EVENT.MAGIC_TYPE_CHANGED, eConsole);
                    ////myChart.on(ecConfig.EVENT.DATA_VIEW_CHANGED, eConsole);

                    // 为echarts对象加载数据 
                    myChart.setOption(option);
                    //window.onresize = myChart.resize;
                }
                );
        }
    } catch (e) {
    }
}







