using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.HomePageElements
{
    /// <summary>
    /// 名称：AlarmRemindService.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-09-15
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：首页元件报警提醒类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AlarmRemindService
    {
        /// <summary>
        /// 获取报警提醒信息
        /// </summary>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// OutOfLimits：超标
        /// Unusual：异常
        /// QualityControl：质控不合格
        /// InstrumentCheck：仪器期间检查
        /// </returns>
        public DataView GetAlarmRemindInfo(DateTime dateStart, DateTime dateEnd)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("OutOfLimits", typeof(string));
            dt.Columns.Add("Unusual", typeof(string));
            dt.Columns.Add("QualityControl", typeof(string));
            dt.Columns.Add("InstrumentCheck", typeof(string));

            DataRow dr = dt.NewRow();
            dr["OutOfLimits"] = "120";
            dr["Unusual"] = "36";
            dr["QualityControl"] = "52";
            dr["InstrumentCheck"] = "104";
            dt.Rows.Add(dr);

            return dt.DefaultView;
        }
    }
}
