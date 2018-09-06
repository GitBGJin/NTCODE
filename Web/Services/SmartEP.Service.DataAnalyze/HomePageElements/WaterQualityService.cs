using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SmartEP.Service.DataAnalyze.HomePageElements
{
    /// <summary>
    /// 名称：WaterQualityService.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-09-16
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：首页元件水质达标类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class WaterQualityService
    {
        /// <summary>
        /// 获取水质达标信息
        /// </summary>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// DataTotal：数据总数
        /// StandardData：达标数据
        /// NotStandardData：不达标数据
        /// StandardRate：数据达标率
        /// </returns>
        public DataView GetWaterQualityStandard(DateTime dateStart, DateTime dateEnd)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DataTotal", typeof(string));
            dt.Columns.Add("StandardData", typeof(string));
            dt.Columns.Add("NotStandardData", typeof(string));
            dt.Columns.Add("StandardRate", typeof(string));

            DataRow dr = dt.NewRow();
            dr["DataTotal"] = "106";
            dr["StandardData"] = "82";
            dr["NotStandardData"] = "24";
            dr["StandardRate"] = "77%";
            dt.Rows.Add(dr);

            return dt.DefaultView;
        }
    }
}
