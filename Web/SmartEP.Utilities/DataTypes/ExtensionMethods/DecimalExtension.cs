using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Utilities.DataTypes.ExtensionMethods
{
    /// <summary>
    /// 名称：DecimalExtension.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-25
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// decima数值格式化处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public static class DecimalExtension
    {
        /// <summary>
        /// 取得银行家算法取得的数据
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="decimalNum">小数位</param>
        /// <returns></returns>
        public static decimal GetRoundValue(decimal value, int decimalNum)
        {
            if (decimalNum < 0)
                return value;
            return Math.Round(value, decimalNum, MidpointRounding.ToEven);
        }

        /// <summary>
        /// 取得数据库中取得的因子浓度实际值
        /// 排除直接用银行家算法时因数据库小数位多、补零而导致的数据异常
        /// </summary>
        /// <param name="value">因子浓度</param>
        /// <param name="decimalNum">小数位</param>
        /// <returns></returns>
        public static decimal GetPollutantValue(decimal value, int decimalNum)
        {
            if (decimalNum < 0)
                return value;
            decimal valuePow = value * Convert.ToInt32(Math.Pow(10, decimalNum));
            if (valuePow - Convert.ToDecimal(Math.Floor(valuePow)) == 0M)
                return Math.Round(value, decimalNum);
            else
                return Math.Round(value, decimalNum, MidpointRounding.ToEven);
        }


    }
}
