using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Interfaces
{
    /// <summary>
    /// 名称：IHourAQI.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：环境空气小时AQI接口
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public interface IHourAQI
    {
        /// <summary>
        /// 获取实时小时某一监测点AQI
        /// </summary>
        /// <returns></returns>
        int GetRTHourPortAQI();

        /// <summary>
        /// 获取实时小时苏州市区所有监测点AQI
        /// </summary>
        /// <returns></returns>
        int GetRTHourSzPortsAQI();

        /// <summary>
        /// 获取实时小时某一城区所有监测点AQI
        /// </summary>
        /// <returns></returns>
        int GetRTHourAreaPortsAQI();

        /// <summary>
        /// 获取实时小时某一监测点AQI污染等级统计
        /// </summary>
        /// <returns></returns>
        int GetRTHourPortAQIGrade();

        /// <summary>
        /// 获取实时小时苏州市区所有监测点AQI污染等级统计
        /// </summary>
        /// <returns></returns>
        int GetRTHourSzPortsAQIGrade();

        /// <summary>
        /// 获取实时小时某一城区所有监测点AQI污染等级统计
        /// </summary>
        /// <returns></returns>
        int GetRTHourAreaPortsAQIGrade();

        /// <summary>
        /// 获取实时小时某一监测点首要污染物统计
        /// </summary>
        /// <returns></returns>
        int GetRTHourPortPrimaryPollutant();

        /// <summary>
        /// 获取实时小时苏州市区所有监测点首要污染物统计
        /// </summary>
        /// <returns></returns>
        int GetRTHourSzPortsPrimaryPollutant();

        /// <summary>
        /// 获取实时小时某一城区所有监测点首要污染物统计
        /// </summary>
        /// <returns></returns>
        int GetRTHourAreaPortsPrimaryPollutant();
    }
}
