using SmartEP.Core.Enums;
using SmartEP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.AutoMonitoring.Interfaces
{
    /// <summary>
    /// 名称：IDataSamplingRate.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-08-22
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：数据捕获率接口
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public interface IDataSamplingRate
    {
        /// <summary>
        /// 点位某一小时总捕获率 
        /// </summary>
        /// <param name="portId">测点Id</param>
        /// <param name="dtmHour">日期（小时）</param>
        /// <returns></returns>
        decimal GetHourSamplingRate(int portId, DateTime dtmHour);

        /// <summary>
        /// 点位某天总捕获率
        /// </summary>
        /// <param name="portId">测点Id</param>
        /// <param name="dtmDay">日期（天）</param>
        /// <returns></returns>
        decimal GetDaySamplingRate(int portId, DateTime dtmDay);

        /// <summary>
        /// 点位某月总捕获率
        /// </summary>
        /// <param name="portId">测点Id</param>
        /// <param name="year">年</param>
        /// <param name="monthOfYear">月</param>
        /// <returns></returns>
        decimal GetMonthSamplingRate(int portId, int year, int monthOfYear);

        /// <summary>
        /// 点位某季度总捕获率
        /// </summary>
        /// <param name="portId">测点Id</param>
        /// <param name="year">年</param>
        /// <param name="seasonOfYear">季</param>
        /// <returns></returns>
        decimal GetSeasonSamplingRate(int portId, int year, int seasonOfYear);

        /// <summary>
        /// 点位某年总捕获率
        /// </summary>
        /// <param name="portId">测点Id</param>
        /// <param name="year">年</param>
        /// <returns></returns>
        decimal GetYearSamplingRate(int portId, int year);

        /// <summary>
        /// 点位某一小时各个污染物捕获率明细
        /// </summary>
        /// <param name="portId">测点Id</param>
        /// <param name="dtmHour">日期（小时）</param>
        /// <returns></returns>
        DataView GetHourSamplingRateDetail(int portId, DateTime dtmHour);

        /// <summary>
        /// 点位某天各个污染物捕获率明细
        /// </summary>
        /// <param name="portId">测点Id</param>
        /// <param name="dtmDay">日期（天）</param>
        /// <returns></returns>
        DataView GetDaySamplingRateDetail(int portId, DateTime dtmDay);

        /// <summary>
        /// 点位某月各个污染物捕获率明细
        /// </summary>
        /// <param name="portId">测点Id</param>
        /// <param name="year">年</param>
        /// <param name="monthOfYear">月</param>
        /// <returns></returns>
        DataView GetMonthSamplingRateDetail(int portId, int year, int monthOfYear);

        /// <summary>
        /// 点位某季度各个污染物捕获率明细
        /// </summary>
        /// <param name="portId">测点Id</param>
        /// <param name="year">年</param>
        /// <param name="seasonOfYear">季</param>
        /// <returns></returns>
        DataView GetSeasonSamplingRateDetail(int portId, int year, int seasonOfYear);

        /// <summary>
        /// 点位某年各个污染物捕获率明细
        /// </summary>
        /// <param name="portId">测点Id</param>
        /// <param name="year">年</param>
        /// <returns></returns>
        DataView GetYearSamplingRateDetail(int portId, int year);
    }
}
