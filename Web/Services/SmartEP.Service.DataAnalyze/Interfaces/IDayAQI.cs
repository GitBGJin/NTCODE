using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Interfaces
{
    /// <summary>
    /// 名称：IDayAQI.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：环境空气日AQI接口
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public interface IDayAQI
    {
        /// <summary>
        /// 获取某段时间内某一监测点AQI
        /// </summary>
        /// <returns></returns>
        int GetTimePortAQI();

        /// <summary>
        /// 获取某段时间内苏州市区所有监测点AQI
        /// </summary>
        /// <returns></returns>
        int GetTimeSzPortsAQI();

        /// <summary>
        /// 获取某段时间内某一城区所有监测点AQI
        /// </summary>
        /// <returns></returns>
        int GetTimeAreaPortsAQI();

        /// <summary>
        /// 获取某段时间内某一监测点AQI污染等级统计
        /// </summary>
        /// <returns></returns>
        int GetTimePortAQIGrade();

        /// <summary>
        /// 获取某段时间内苏州市区所有监测点AQI污染等级统计
        /// </summary>
        /// <returns></returns>
        int GetTimeSzPortsAQIGrade();

        /// <summary>
        /// 获取某段时间内某一城区所有监测点AQI污染等级统计
        /// </summary>
        /// <returns></returns>
        int GetTimeAreaPortsAQIGrade();

        /// <summary>
        /// 获取某段时间内某一监测点首要污染物统计
        /// </summary>
        /// <returns></returns>
        int GetTimePortPrimaryPollutant();

        /// <summary>
        /// 获取某段时间内苏州市区所有监测点首要污染物统计
        /// </summary>
        /// <returns></returns>
        int GetTimeSzPortsPrimaryPollutant();

        /// <summary>
        /// 获取某段时间内某一城区所有监测点首要污染物统计
        /// </summary>
        /// <returns></returns>
        int GetTimeAreaPortsPrimaryPollutant();

        /// <summary>
        /// 获取某段时间内某一监测点超标天数统计
        /// </summary>
        /// <returns></returns>
        int GetTimePortOutDay();

        /// <summary>
        /// 获取某段时间内苏州市区所有监测点超标天数统计
        /// </summary>
        /// <returns></returns>
        int GetTimeSzPortsOutDay();

        /// <summary>
        /// 获取某段时间内某一城区所有监测点超标天数统计
        /// </summary>
        /// <returns></returns>
        int GetTimeAreaPortsOutDay();
    }
}