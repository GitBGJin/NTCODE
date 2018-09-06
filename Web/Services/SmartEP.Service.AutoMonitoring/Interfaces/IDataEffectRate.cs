using SmartEP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.AutoMonitoring.Interfaces
{
    /// <summary>
    /// 名称：IDataEffectRate.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-08-22
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：数据有效率接口
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public interface IDataEffectRate
    {
        /// <summary>
        /// 点位某一小时总有效率
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="dtmHour">日期（小时）</param>
        /// <returns></returns>
        decimal GetHourEffectRate(int portId, DateTime dtmHour);

        /// <summary>
        /// 点位某天总有效率
        /// </summary>
        /// <param name="portId">测点Id</param>
        /// <param name="dtmDay">日期（天）</param>
        /// <returns></returns>
        decimal GetDayEffectRate(int portId, DateTime dtmDay);

        /// <summary>
        /// 点位某月总有效率
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="year">年</param>
        /// <param name="monthOfYear">月</param>
        /// <returns></returns>
        decimal GetMonthEffectRate(int portId, int year, int monthOfYear);

        /// <summary>
        /// 点位某季度总有效率
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="year">年</param>
        /// <param name="seasonOfYear">季</param>
        /// <returns></returns>
        decimal GetSeasonEffectRate(int portId, int year, int seasonOfYear);

        /// <summary>
        /// 点位某年总有效率
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="year">年</param>
        /// <returns></returns>
        decimal GetYearEffectRate(int portId, int year);

        /// <summary>
        /// 点位某一小时各个污染物有效率明细
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="dtmHour">日期（小时）</param>
        /// <returns></returns>
        DataView GetHourEffectRateDetail(int portId, DateTime dtmHour);

        /// <summary>
        /// 点位某天各个污染物有效率明细
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="dtmDay">日期（天）</param>
        /// <returns></returns>
        DataView GetDayEffectRateDetail(int portId, DateTime dtmDay);

        /// <summary>
        /// 点位某月各个污染物有效率明细
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="year">年</param>
        /// <param name="monthOfYear">月</param>
        /// <returns></returns>
        DataView GetMonthEffectRateDetail(int portId, int year, int monthOfYear);

        /// <summary>
        /// 点位某季度各个污染物有效率明细
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="year">年</param>
        /// <param name="seasonOfYear">季</param>
        /// <returns></returns>
        DataView GetSeasonEffectRateDetail(int portId, int year, int seasonOfYear);

        /// <summary>
        /// 点位某年各个污染物有效率明细
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="year">年</param>
        /// <returns></returns>
        DataView GetYearEffectRateDetail(int portId, int year);
    }
}
