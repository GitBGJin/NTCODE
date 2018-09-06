using SmartEP.Core.Enums;
using SmartEP.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.BaseData.Alarm
{
    /// <summary>
    /// 名称：AlarmManagerService.cs
    /// 创建人：季柯
    /// 创建日期：2015-08-20
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：报警操作类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AlarmManagerService
    {

         #region << 构造函数 >>
        public AlarmManagerService(ApplicationType applicationType, PollutantDataType dataType)
        {
            switch (applicationType)
            {
                case ApplicationType.Air:
                    CreateAirAlarm(applicationType, dataType);
                    break;
                case ApplicationType.Water:
                    CreateWaterAlarm();
                    break;
            }
        }
        #endregion
        /// <summary>
        /// 生成空气报警
        /// </summary>
        public void CreateAirAlarm(ApplicationType applicationType, PollutantDataType dataType)
        {
            //离线
            new OfflineAlarmService(applicationType, dataType);
            //超标
            new ExcessiveAlarmService(applicationType, dataType);
            //突变
            new BreakAlarmService(applicationType, dataType);
            //重复
            new RepeatAlarmService(applicationType, dataType);
        }

       
        /// <summary>
        /// 生成地表水报警（只有小时数据）
        /// </summary>
        public void CreateWaterAlarm()
        {
            ApplicationType applicationType = ApplicationType.Water;
            PollutantDataType dataType = PollutantDataType.Min60;
            //离线
            new OfflineAlarmService(applicationType, dataType);
            //超标
            new ExcessiveAlarmService(applicationType, dataType);
            //突变
            new BreakAlarmService(applicationType, dataType);
            //重复
            new RepeatAlarmService(applicationType, dataType);
        }
    }
}
