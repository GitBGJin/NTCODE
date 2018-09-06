using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTDataProcessApplication
{
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
