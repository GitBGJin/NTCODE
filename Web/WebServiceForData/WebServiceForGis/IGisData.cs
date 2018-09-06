using System;
using System.Collections.Generic;
using System.Text;

namespace WebServiceForGis
{
    public interface IGisData
    {
        /// <summary>
        /// 获取监测点关注程度
        /// </summary>
        /// <returns>
        /// json格式的字符串，例如：[{"AttentionDegree":[{"Name":"全市","Code":"City"},{"Name":"城区","Code":"District"},{"Name":"乡镇","Code":"Town"}]}]
        /// </returns>
        string GetAttentionDegree();

        /// <summary>
        /// 获取所有站点情况
        /// </summary>
        /// <returns>
        /// [{"PortInfo":[{"PortId":"2","PortName":"实验小学","ST":"22","PortType":"1","attentionType":"District","X":"120.9531","Y":"31.3875","isShowName":"1"},
        /// {"PortId":"4","PortName":"二水厂","ST":"22","PortType":"1","attentionType":"District","X":"120.9122","Y":"31.4017","isShowName":"1"},
        /// {"PortId":"5","PortName":"震川中学","ST":"22","PortType":"1","attentionType":"District","X":"120.9989","Y":"31.3814","isShowName":"1"}]}]
        /// </returns>
        string GetPortInfo();

        /// <summary>
        /// 获取API监测因子
        /// </summary>
        /// <returns>
        /// [{"ApiFactor":[{"Name":"二氧化硫","EName":"SO2","Code":"a21026"},{"Name":"二氧化氮","EName":"NO2","Code":"a21004"},{"Name":"臭氧1小时","EName":"O3_1","Code":"a05024_1"},{"Name":"臭氧8小时","EName":"O3_8","Code":"a05024_8"},{"Name":"一氧化碳","EName":"CO","Code":"a21005"},{"Name":"可吸入颗粒物","EName":"PM10","Code":"a34002"},{"Name":"细微颗粒物","EName":"PM25","Code":"a34004"},{"Name":"AQI","EName":"AQI","Code":"AQI"}]}]
        /// </returns>
        string GetAPIFactor();

        /// <summary>
        /// 获取AQI分级颜色
        /// </summary>
        /// <param name="factorEName"></param>
        /// <returns>
        /// [{"KPIColor":[{"KPIName":"AQI","Alpla":"0.6","ListColorItem":[{"Color":"0x00e400","MinVal":"0","MaxVal":"50"},{"Color":"0xffff00","MinVal":"50.0001","MaxVal":"100"},{"Color":"0xff7e00","MinVal":"100.0001","MaxVal":"150"},{"Color":"0xff0000","MinVal":"150.0001","MaxVal":"200"},{"Color":"0x99004c","MinVal":"200.0001","MaxVal":"300"},{"Color":"0x7e0023","MinVal":"300.0001","MaxVal":"9999"}]}]}]
        /// </returns>
        string GetKPIColor(string factorEName);

        /// <summary>
        /// 获取站点分类
        /// </summary>
        /// <param name="ST">系统类型（21：地表水，22：环境空气）</param>
        /// <returns>
        /// [{"PortTypes":[{"PortTypeName":"自动站","PortTypeValue":"00"},{"PortTypeName":"人工监测点","PortTypeValue":"01"}]}]
        /// </returns>
        string GetPortTypes(string ST);

        /// <summary>
        /// 获取站点最新一条数据
        /// </summary>
        /// <param name="portid">测点唯一标识</param>
        /// <param name="ST">系统类型（21：地表水，22：环境空气）</param>
        /// <returns>
        /// [{"LastestData":[{"Item":"时间","ItemValue":"2012-11-13 22:00","ItemUnit":"--"},{"Item":"PM2.5","ItemValue":"0.090","ItemUnit":"mg/m3"},{"Item":"二氧化硫","ItemValue":"0.117","ItemUnit":"mg/m3"},{"Item":"二氧化氮","ItemValue":"0.055","ItemUnit":"mg/m3"},{"Item":"PM10","ItemValue":"0.089","ItemUnit":"mg/m3"}]}]
        /// </returns>
        string GetLastestDataByPortid(string portid, string ST);


        /// <summary>
        /// 获取实时空气质量情况(AQI)
        /// </summary>
        /// <param name="attentionType">测点关注程度</param>
        /// <param name="factorEName">因子（SO2、NO2、O3_1、AQI）</param>
        /// <returns>
        /// [{"HourAQI":[{"PortId":"2","PortName":"实验小学","FactorValue":"--","AQI":"--","Level":"--","State":"--"},{"PortId":"5","PortName":"震川中学","FactorValue":"--","AQI":"123","Level":"三级","State":"轻微污染"},{"PortId":"4","PortName":"二水厂","FactorValue":"--","AQI":"169","Level":"三级","State":"轻度污染"},{"PortId":"6","PortName":"托普学院","FactorValue":"--","AQI":"82","Level":"二级","State":"良"},{"PortId":"7","PortName":"淀山湖党校","FactorValue":"--","AQI":"95","Level":"二级","State":"良"},{"PortId":"8","PortName":"张浦培训中心","FactorValue":"--","AQI":"124","Level":"三级","State":"轻微污染"},{"PortId":"9","PortName":"锦溪干家甸村委会","FactorValue":"--","AQI":"117","Level":"三级","State":"轻微污染"}]}]
        /// </returns>
        string GetHourAQI(string attentionType, string factorEName);

        /// <summary>
        /// 获取日空气质量情况(AQI)
        /// </summary>
        /// <param name="attentionType">测点关注程度</param>
        /// <returns>
        /// [{"DayAQIBySiteType":[{"PortId":"2","PortName":"实验小学","FactorValue":"--","AQI":"--","Level":"--","State":"--"},{"PortId":"5","PortName":"震川中学","FactorValue":"--","AQI":"123","Level":"三级","State":"轻微污染"},{"PortId":"4","PortName":"二水厂","FactorValue":"--","AQI":"169","Level":"三级","State":"轻度污染"},{"PortId":"6","PortName":"托普学院","FactorValue":"--","AQI":"82","Level":"二级","State":"良"},{"PortId":"7","PortName":"淀山湖党校","FactorValue":"--","AQI":"95","Level":"二级","State":"良"},{"PortId":"8","PortName":"张浦培训中心","FactorValue":"--","AQI":"124","Level":"三级","State":"轻微污染"},{"PortId":"9","PortName":"锦溪干家甸村委会","FactorValue":"--","AQI":"117","Level":"三级","State":"轻微污染"}]}]
        /// </returns>
        string GetDayAQI(string attentionType);

        /// <summary>
        /// 查询空气日报(API)
        /// </summary>
        /// <param name="fromDate">开始日期</param>
        /// <param name="toDate">结束日期</param>
        /// <param name="classes">等级情况（优、良、轻微、轻度、中度、中度重、重）</param>
        /// <returns>
        /// [{"PortAPIByDate":[{"Date":"2012-11-15","AQI":"63","Level":"二级","State":"良","MainPollute":"PM10"},{"Date":"2012-11-14","AQI":"75","Level":"二级","State":"良","MainPollute":"PM10"},{"Date":"2012-11-13","AQI":"77","Level":"二级","State":"良","MainPollute":"PM10"},{"Date":"2012-11-12","AQI":"60","Level":"二级","State":"良","MainPollute":"PM10"},{"Date":"2012-11-11","AQI":"60","Level":"二级","State":"良","MainPollute":"PM10"}]}]
        /// </returns>
        string GetDayAPIByDateAndClass(string fromDate, string toDate, string classes);

        /// <summary>
        ///  水质情况(dataType：hour为小时值，day日均值)
        /// </summary>
        /// <param name="siteType">站点类型</param>
        /// <param name="dataType">数据类型</param>
        /// <returns>
        /// ">[{"PortWaterQuality":[{"PortId":"28","PortName":"庙泾河二水厂","DateTime":"11-16 05:00","COD":"2.7","NH3":"0.110","TP":"0.072","ZAO":"201.0","WQ":"Ⅱ"},{"PortId":"25","PortName":"傀儡湖箱涵","DateTime":"11-16 04:00","COD":"--","NH3":"0.400","TP":"--","ZAO":"--","WQ":"Ⅱ"}]}]
        /// </returns>
        string GetWaterQualityBySiteType(string siteType, string dataType);
    }
}
