using SmartEP.Core.Enums;
using SmartEP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Service.AutoMonitoring.Interfaces
{
    /// <summary>
    /// 名称：IMonitoringData.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 实时监控数据公用接口（ORM）
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public interface IMonitoringData
    {
        /// <summary>
        /// 获取监测数据
        /// <summary>
        IQueryable<IBaseEntityProperty> RetrieveMonitoringData(System.Int32 portId, DateTime startTime, DateTime endTime, List<string> pollutantCodes);

        /// <summary>
        /// 保存监测数据
        /// <summary>
        void SaveMonitoringData(IBaseEntityProperty baseEntity);

        /// <summary>
        /// 监测数据是否存在
        /// <summary>
        bool IsExist(int portId, DateTime tstamp, string pollutantCode);

        /// <summary>
        /// 更新监测数据
        /// <summary>
        void UpdateMonitoringData();

        /// <summary>
        /// 根据时间段删除监测数据
        /// <summary>
        void DeleteMonitoringData(DateTime startTime, DateTime endTime);

        /// <summary>
        /// 根据监测点删除监测数据
        /// <summary>
        void DeleteMonitoringData(int portId, DateTime startTime, DateTime endTime);

        /// <summary>
        /// 根据污染物获取平均值
        /// <summary>
        decimal RetrieveAvgMonitoringDataByPollutant(int portId, DateTime startTime, DateTime endTime, string pollutantCode);

        /// <summary>
        /// 根据污染物获取最大值
        /// <summary>
        decimal RetrieveMaxMonitoringDataByPollutant(int portId, DateTime startTime, DateTime endTime, string pollutantCode);

        /// <summary>
        /// 根据污染物获取最小值
        /// <summary>
        decimal RetrieveMinMonitoringDataByPollutant(int portId, DateTime startTime, DateTime endTime, string pollutantCode);

        /// <summary>
        /// 获取最新一条监测数据
        /// <summary>
        IQueryable<IBaseEntityProperty> RetrieveNewestMonitoringData(int portId, List<string> pollutantCodes);

        /// <summary>
        /// 获取最新N条监测数据
        /// <summary>
        IQueryable<IBaseEntityProperty> RetrieveNewestMonitoringData(int portId, List<string> pollutantCodes, int number);

        /// <summary>
        /// 根据污染物获取记录数
        /// <summary>
        int RetrieveMonitoringDataRecordsByPollutant(int portId, DateTime startTime, DateTime endTime, string pollutantCode);
    }
}
