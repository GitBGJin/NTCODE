//加载审核Chart数据
function AjaxLoadingMutilFactor(facCode, StartTime, pointId, applicatironUID) {
    $.ajax({
        type: "POST", //用POST方式传输
        data: {
            "FactorCode": facCode,
            "StartTime": StartTime,
            "PointID": pointId,
            "ApplicationUID": applicatironUID
        },
        dataType: "", //数据格式:JSON                  
        url: "AuditAjaxHandler.ashx?DataType=MutilFactor",
        cache: false, //指令
        async: false, //取消同步

        beforeSend: function () {
        }, //发送数据之前
        success: function (msg) {
            var tsm = eval("(" + msg + ")");
            InitChart(tsm);
        },
        error: function (res) {
            //alert("无数据！");
        }
    });
}

//审核提交
function AjaxSubmitAudit(StartTime, pointId, applicationUID) {
    $.ajax({
        type: "POST", //用POST方式传输
        data: {
            "ApplicationUID": applicationUID,
            "StartTime": StartTime,
            "EndTime": StartTime,
            "PointID": pointId
        },
        dataType: "", //数据格式:JSON                  
        url: "AuditAjaxHandler.ashx?DataType=SubmitAudit",
        cache: true, //指令
        async: true, //取消同步
        beforeSend: function () {
        }, //发送数据之前
        success: function (msg) {
            //currentLoadingPanel.show();
            //Refresh_Grid(true);
            alert("提交审核成功！");
        },
        error: function (res) {
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
            document.getElementById('refreshData').click();
            LoadingData();
        },
        error: function (res) {
            //alert(res + "***");
        }
    });
}

////保存修改数据
//function AjaxModifyAuditData(flag, PointID, DataTime, FactorCode, NewData, applicationUID) {
//    $.ajax({
//        type: "POST", //用POST方式传输
//        data: {
//            "PointID": PointID.join(";"),
//            "DataTime": DataTime.join(";"),
//            "FactorCode": FactorCode.join(";"),
//            "NewData": NewData.join(";"),
//            "ApplicationUID": '<%=Session["applicationUID"]%>'
//        },
//        dataType: "", //数据格式:JSON                  
//        url: "AuditAjaxHandler.ashx?DataType=ModifyAuditData&flag=" + flag,
//        cache: false, //指令
//        async: false, //取消同步
//        beforeSend: function () {
//        }, //发送数据之前
//        success: function (msg) {
//            Refresh_Grid(true);
//            LoadingData();
//        },
//        error: function (res) {
//        }
//    });
//}

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
                        var date = new Date(params[1]);
                        data = +date.getDate() + '日'
                               + date.getHours() + '时';
                        return data + '<br/>' + params[0] + ":" + params[2];

                        //var date = new Date(params.value[0]);
                        //data = +date.getDate() + '日'
                        //       + date.getHours() + '时';
                        //return data + '<br/>' + params.seriesName + ":" + params.value[1];
                    }
                },
                toolbox: {
                    show: true,
                    orient: 'vertical',
                    x: 'right',
                    y: 'center',
                    feature: {
                        mark: { show: true },
                        dataView: { show: true, readOnly: false },
                        magicType: { show: true, type: ['line', 'bar', 'stack', 'tiled'] },
                        restore: { show: true },
                        saveAsImage: { show: true }
                    }
                },
                dataZoom: {
                    show: tsm.dataZoomShow
                    //start: 70
                },
                legend: {
                    //x: 'left',
                    data: tsm.legend
                },
                grid: {
                    y2: 80
                },
                xAxis: [
                                {
                                    type: 'category',
                                    data: tsm.category,
                                            calculable: true,
                                            axisLabel: {
                                                formatter: function (params) {
                                                    var time = new Date(params);
                                                    if (params == undefined || params =="") return "";
                                   
                                                 
                                                    return time.getDate() + '日'
                                                   + time.getHours() + '时';
                                                }
                                            }
                                }
                ],

                //xAxis: [
                //    {
                //        type: 'time',
                //        //splitNumber: 10,
                //        calculable: true,
                //        axisLabel: {
                //            formatter: function (params) {
                //                return params.getDate() + '日'
                //               + params.getHours() + '时';
                //            }
                //            //, rotate: 45
                //        }

                //    }
                //],
                //yAxis: [
                //    {
                //        type: 'value'
                //    }
                //],
                yAxis: tsm.yAxis,
                series: tsm.seriesList
            };
            // 为echarts对象加载数据 
            myChart.setOption(option);
        }
);
}
