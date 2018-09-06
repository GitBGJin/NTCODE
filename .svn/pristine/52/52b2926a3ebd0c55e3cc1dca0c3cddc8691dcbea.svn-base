using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Data.Enums;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.BaseData
{
    /// <summary>
    /// 名称：WaterQualityDAL.cs
    /// 创建人：李飞
    /// 创建日期：2015-09-06
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 水质标准处理DAL
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class WaterQualityDAL
    {
        #region << 变量 >>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.MonitoringBusiness);
        #endregion

        /// <summary>
        /// 计算水质污染指数
        /// </summary>
        /// <param name="pollutantCode">因子编码</param>
        /// <param name="pollutantValue">因子浓度</param>
        /// <param name="iEQI">评价水质类别</param>
        /// <param name="hourType">时间类型</param>
        /// <param name="waterPointCalWQType">地表水水质评价站点属性类型</param>
        /// <returns></returns>
        public decimal GetWQI(string pollutantCode, decimal pollutantValue, Int32 iEQI, string hourType, string waterPointCalWQType)
        {
            string sql = string.Format("SELECT dbo.F_GetWQI('{0}','{1}','{2}',{3},'{4}')", pollutantCode, hourType, iEQI, pollutantValue, waterPointCalWQType);
            object ret = g_DatabaseHelper.ExecuteScalar(sql, connection);
            if (ret != null && ret != DBNull.Value)
                return Convert.ToDecimal(ret);
            return 0M;
        }

        /// <summary>
        /// 获取首要染污物等(根据因子指数)
        /// </summary>
        /// <param name="ReturnType">返回类型（CODE、NAME、ENAME、VALUE、SIMPLE）</param>
        /// <param name="EvaluateFactorCodes">参与评价因子，以“;”分割</param>
        /// <param name="WQIValues">因子指数列表（Key:PollutantCode、Value:分指数）</param>
        /// <returns></returns>
        public string GetWQI_Max(string ReturnType, string EvaluateFactorCodes, Dictionary<string, Decimal> WQIValues)
        {
            // @WQI_PH：pH值 w01001
            string WQI_PH = WQIValues.ContainsKey("w01001") ? WQIValues["w01001"].ToString() : "NULL";
            // @WQI_NH3N：氨氮 w21003
            string WQI_NH3N = WQIValues.ContainsKey("w21003") ? WQIValues["w21003"].ToString() : "NULL";
            // @WQI_CODMN：高锰酸盐 w01019
            string WQI_CODMN = WQIValues.ContainsKey("w01019") ? WQIValues["w01019"].ToString() : "NULL";
            // @WQI_DOX：溶解氧 w01009
            string WQI_DOX = WQIValues.ContainsKey("w01009") ? WQIValues["w01009"].ToString() : "NULL";
            // @WQI_TP：总磷 w21011
            string WQI_TP = WQIValues.ContainsKey("w21011") ? WQIValues["w21011"].ToString() : "NULL";
            // @WQI_TN：总氮 w21001
            string WQI_TN = WQIValues.ContainsKey("w21001") ? WQIValues["w21001"].ToString() : "NULL";
            // @WQI_CODCR：化学需氧量（COD）w01018
            string WQI_CODCR = WQIValues.ContainsKey("w01018") ? WQIValues["w01018"].ToString() : "NULL";
            // @WQI_BOD5：五日生化需氧量w01017
            string WQI_BOD5 = WQIValues.ContainsKey("w01017") ? WQIValues["w01017"].ToString() : "NULL";
            // @WQI_CU：铜 w20122
            string WQI_CU = WQIValues.ContainsKey("w20122") ? WQIValues["w20122"].ToString() : "NULL";
            // @WQI_ZN：锌 w20123
            string WQI_ZN = WQIValues.ContainsKey("w20123") ? WQIValues["w20123"].ToString() : "NULL";
            // @WQI_F：氟化物 w21017
            string WQI_F = WQIValues.ContainsKey("w21017") ? WQIValues["w21017"].ToString() : "NULL";
            // @WQI_SE：硒 w20128
            string WQI_SE = WQIValues.ContainsKey("w20128") ? WQIValues["w20128"].ToString() : "NULL";
            // @WQI_ARS：砷 w20119
            string WQI_ARS = WQIValues.ContainsKey("w20119") ? WQIValues["w20119"].ToString() : "NULL";
            // @WQI_HG：汞 w20111
            string WQI_HG = WQIValues.ContainsKey("w20111") ? WQIValues["w20111"].ToString() : "NULL";
            // @WQI_CD：镉 w20115
            string WQI_CD = WQIValues.ContainsKey("w20115") ? WQIValues["w20115"].ToString() : "NULL";
            // @WQI_CR6：六价铬 w20117
            string WQI_CR6 = WQIValues.ContainsKey("w20117") ? WQIValues["w20117"].ToString() : "NULL";
            // @WQI_PB：铅 w20120
            string WQI_PB = WQIValues.ContainsKey("w20120") ? WQIValues["w20120"].ToString() : "NULL";
            // @WQI_CN：氰化物 w21016
            string WQI_CN = WQIValues.ContainsKey("w21016") ? WQIValues["w21016"].ToString() : "NULL";
            // @WQI_VLPH：挥发酚 w23002
            string WQI_VLPH = WQIValues.ContainsKey("w23002") ? WQIValues["w23002"].ToString() : "NULL";
            // @WQI_S2：硫化物 w21019
            string WQI_S2 = WQIValues.ContainsKey("w21019") ? WQIValues["w21019"].ToString() : "NULL";
            // @WQI_OILS：石油类 w22001
            string WQI_OILS = WQIValues.ContainsKey("w22001") ? WQIValues["w22001"].ToString() : "NULL";
            // @WQI_ASAA：阴离子表面活性剂 w19002
            string WQI_ASAA = WQIValues.ContainsKey("w19002") ? WQIValues["w19002"].ToString() : "NULL";
            // @WQI_FCG：粪大肠菌群（个/L）w02003
            string WQI_FCG = WQIValues.ContainsKey("w02003") ? WQIValues["w02003"].ToString() : "NULL";
            string sql = string.Format(@"SELECT dbo.F_GetWQI_Max('{0}','{1}',{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24})"
                , ReturnType, EvaluateFactorCodes, WQI_PH, WQI_NH3N, WQI_CODMN, WQI_DOX, WQI_TP, WQI_TN, WQI_CODCR, WQI_BOD5, WQI_CU
                , WQI_ZN, WQI_F, WQI_SE, WQI_ARS, WQI_HG, WQI_CD, WQI_CR6, WQI_PB, WQI_CN, WQI_VLPH, WQI_S2, WQI_OILS, WQI_ASAA, WQI_FCG);
            object ret = g_DatabaseHelper.ExecuteScalar(sql, connection);
            if (ret != null && ret != DBNull.Value)
                return ret.ToString();
            return string.Empty;
        }

        /// <summary>
        /// 计算综合污染指数(根据因子指数)
        /// </summary>
        /// <param name="EvaluateFactorCodes">参与评价因子，以“;”分割</param>
        /// <param name="WQIValues">因子指数列表（Key:PollutantCode、Value:分指数）</param>
        /// <returns></returns>
        public string GetWQI_Avg(string EvaluateFactorCodes, Dictionary<string, Decimal> WQIValues)
        {
            // @WQI_PH：pH值 w01001
            string WQI_PH = WQIValues.ContainsKey("w01001") ? WQIValues["w01001"].ToString() : "NULL";
            // @WQI_NH3N：氨氮 w21003
            string WQI_NH3N = WQIValues.ContainsKey("w21003") ? WQIValues["w21003"].ToString() : "NULL";
            // @WQI_CODMN：高锰酸盐 w01019
            string WQI_CODMN = WQIValues.ContainsKey("w01019") ? WQIValues["w01019"].ToString() : "NULL";
            // @WQI_DOX：溶解氧 w01009
            string WQI_DOX = WQIValues.ContainsKey("w01009") ? WQIValues["w01009"].ToString() : "NULL";
            // @WQI_TP：总磷 w21011
            string WQI_TP = WQIValues.ContainsKey("w21011") ? WQIValues["w21011"].ToString() : "NULL";
            // @WQI_TN：总氮 w21001
            string WQI_TN = WQIValues.ContainsKey("w21001") ? WQIValues["w21001"].ToString() : "NULL";
            // @WQI_CODCR：化学需氧量（COD）w01018
            string WQI_CODCR = WQIValues.ContainsKey("w01018") ? WQIValues["w01018"].ToString() : "NULL";
            // @WQI_BOD5：五日生化需氧量w01017
            string WQI_BOD5 = WQIValues.ContainsKey("w01017") ? WQIValues["w01017"].ToString() : "NULL";
            // @WQI_CU：铜 w20122
            string WQI_CU = WQIValues.ContainsKey("w20122") ? WQIValues["w20122"].ToString() : "NULL";
            // @WQI_ZN：锌 w20123
            string WQI_ZN = WQIValues.ContainsKey("w20123") ? WQIValues["w20123"].ToString() : "NULL";
            // @WQI_F：氟化物 w21017
            string WQI_F = WQIValues.ContainsKey("w21017") ? WQIValues["w21017"].ToString() : "NULL";
            // @WQI_SE：硒 w20128
            string WQI_SE = WQIValues.ContainsKey("w20128") ? WQIValues["w20128"].ToString() : "NULL";
            // @WQI_ARS：砷 w20119
            string WQI_ARS = WQIValues.ContainsKey("w20119") ? WQIValues["w20119"].ToString() : "NULL";
            // @WQI_HG：汞 w20111
            string WQI_HG = WQIValues.ContainsKey("w20111") ? WQIValues["w20111"].ToString() : "NULL";
            // @WQI_CD：镉 w20115
            string WQI_CD = WQIValues.ContainsKey("w20115") ? WQIValues["w20115"].ToString() : "NULL";
            // @WQI_CR6：六价铬 w20117
            string WQI_CR6 = WQIValues.ContainsKey("w20117") ? WQIValues["w20117"].ToString() : "NULL";
            // @WQI_PB：铅 w20120
            string WQI_PB = WQIValues.ContainsKey("w20120") ? WQIValues["w20120"].ToString() : "NULL";
            // @WQI_CN：氰化物 w21016
            string WQI_CN = WQIValues.ContainsKey("w21016") ? WQIValues["w21016"].ToString() : "NULL";
            // @WQI_VLPH：挥发酚 w23002
            string WQI_VLPH = WQIValues.ContainsKey("w23002") ? WQIValues["w23002"].ToString() : "NULL";
            // @WQI_S2：硫化物 w21019
            string WQI_S2 = WQIValues.ContainsKey("w21019") ? WQIValues["w21019"].ToString() : "NULL";
            // @WQI_OILS：石油类 w22001
            string WQI_OILS = WQIValues.ContainsKey("w22001") ? WQIValues["w22001"].ToString() : "NULL";
            // @WQI_ASAA：阴离子表面活性剂 w19002
            string WQI_ASAA = WQIValues.ContainsKey("w19002") ? WQIValues["w19002"].ToString() : "NULL";
            // @WQI_FCG：粪大肠菌群（个/L）w02003
            string WQI_FCG = WQIValues.ContainsKey("w02003") ? WQIValues["w02003"].ToString() : "NULL";
            string sql = string.Format(@"SELECT dbo.F_GetWQI_Avg('{0}',{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23})"
                , EvaluateFactorCodes, WQI_PH, WQI_NH3N, WQI_CODMN, WQI_DOX, WQI_TP, WQI_TN, WQI_CODCR, WQI_BOD5, WQI_CU
                , WQI_ZN, WQI_F, WQI_SE, WQI_ARS, WQI_HG, WQI_CD, WQI_CR6, WQI_PB, WQI_CN, WQI_VLPH, WQI_S2, WQI_OILS, WQI_ASAA, WQI_FCG);
            object ret = g_DatabaseHelper.ExecuteScalar(sql, connection);
            if (ret != null && ret != DBNull.Value)
                return ret.ToString();
            return string.Empty;
        }

        /// <summary>
        /// 计算水质等级
        /// </summary>
        /// <param name="pollutantCode">因子编码</param>
        /// <param name="pollutantValue">因子浓度</param>
        /// <param name="hourType">时间类型</param>
        /// <param name="waterPointCalWQType">地表水水质评价站点属性类型</param>
        /// <param name="returnType">返回值类型</param>
        /// <returns></returns>
        public string GetWQL(string pollutantCode, decimal pollutantValue, string hourType, string waterPointCalWQType, string returnType)
        {
            string sql = string.Format("SELECT dbo.F_GetWQL('{0}','{1}',{2},'{3}','{4}')", pollutantCode, hourType, pollutantValue, waterPointCalWQType, returnType);
            object ret = g_DatabaseHelper.ExecuteScalar(sql, connection);
            if (ret != null && ret != DBNull.Value)
                return ret.ToString();
            return string.Empty;
        }

        /// <summary>
        /// 获取首要染污物等(根据因子等级)
        /// </summary>
        /// <param name="ReturnType">返回类型（CODE、NAME、ENAME、VALUE、SIMPLE）</param>
        /// <param name="EvaluateFactorCodes">参与评价因子，以“;”分割</param>
        /// <param name="WQIValues">因子指数列表（Key:PollutantCode、Value:单因子等级）</param>
        /// <returns></returns>
        public string GetWQL_Max(string ReturnType, string EvaluateFactorCodes, Dictionary<string, Int32> WQIValues)
        {
            // @WQL_PH：pH值 w01001
            string WQL_PH = WQIValues.ContainsKey("w01001") ? WQIValues["w01001"].ToString() : "NULL";
            // @WQL_NH3N：氨氮 w21003
            string WQL_NH3N = WQIValues.ContainsKey("w21003") ? WQIValues["w21003"].ToString() : "NULL";
            // @WQL_CODMN：高锰酸盐 w01019
            string WQL_CODMN = WQIValues.ContainsKey("w01019") ? WQIValues["w01019"].ToString() : "NULL";
            // @WQL_DOX：溶解氧 w01009
            string WQL_DOX = WQIValues.ContainsKey("w01009") ? WQIValues["w01009"].ToString() : "NULL";
            // @WQL_TP：总磷 w21011
            string WQL_TP = WQIValues.ContainsKey("w21011") ? WQIValues["w21011"].ToString() : "NULL";
            // @WQL_TN：总氮 w21001
            string WQL_TN = WQIValues.ContainsKey("w21001") ? WQIValues["w21001"].ToString() : "NULL";
            // @WQL_CODCR：化学需氧量（COD）w01018
            string WQL_CODCR = WQIValues.ContainsKey("w01018") ? WQIValues["w01018"].ToString() : "NULL";
            // @WQL_BOD5：五日生化需氧量w01017
            string WQL_BOD5 = WQIValues.ContainsKey("w01017") ? WQIValues["w01017"].ToString() : "NULL";
            // @WQL_CU：铜 w20122
            string WQL_CU = WQIValues.ContainsKey("w20122") ? WQIValues["w20122"].ToString() : "NULL";
            // @WQL_ZN：锌 w20123
            string WQL_ZN = WQIValues.ContainsKey("w20123") ? WQIValues["w20123"].ToString() : "NULL";
            // @WQL_F：氟化物 w21017
            string WQL_F = WQIValues.ContainsKey("w21017") ? WQIValues["w21017"].ToString() : "NULL";
            // @WQL_SE：硒 w20128
            string WQL_SE = WQIValues.ContainsKey("w20128") ? WQIValues["w20128"].ToString() : "NULL";
            // @WQL_ARS：砷 w20119
            string WQL_ARS = WQIValues.ContainsKey("w20119") ? WQIValues["w20119"].ToString() : "NULL";
            // @WQL_HG：汞 w20111
            string WQL_HG = WQIValues.ContainsKey("w20111") ? WQIValues["w20111"].ToString() : "NULL";
            // @WQL_CD：镉 w20115
            string WQL_CD = WQIValues.ContainsKey("w20115") ? WQIValues["w20115"].ToString() : "NULL";
            // @WQL_CR6：六价铬 w20117
            string WQL_CR6 = WQIValues.ContainsKey("w20117") ? WQIValues["w20117"].ToString() : "NULL";
            // @WQL_PB：铅 w20120
            string WQL_PB = WQIValues.ContainsKey("w20120") ? WQIValues["w20120"].ToString() : "NULL";
            // @WQL_CN：氰化物 w21016
            string WQL_CN = WQIValues.ContainsKey("w21016") ? WQIValues["w21016"].ToString() : "NULL";
            // @WQL_VLPH：挥发酚 w23002
            string WQL_VLPH = WQIValues.ContainsKey("w23002") ? WQIValues["w23002"].ToString() : "NULL";
            // @WQL_S2：硫化物 w21019
            string WQL_S2 = WQIValues.ContainsKey("w21019") ? WQIValues["w21019"].ToString() : "NULL";
            // @WQL_OILS：石油类 w22001
            string WQL_OILS = WQIValues.ContainsKey("w22001") ? WQIValues["w22001"].ToString() : "NULL";
            // @WQL_ASAA：阴离子表面活性剂 w19002
            string WQL_ASAA = WQIValues.ContainsKey("w19002") ? WQIValues["w19002"].ToString() : "NULL";
            // @WQL_FCG：粪大肠菌群（个/L）w02003
            string WQL_FCG = WQIValues.ContainsKey("w02003") ? WQIValues["w02003"].ToString() : "NULL";
            string sql = string.Format(@"SELECT dbo.F_GetWQL_Max('{0}','{1}',{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24})"
                , ReturnType, EvaluateFactorCodes, WQL_PH, WQL_NH3N, WQL_CODMN, WQL_DOX, WQL_TP, WQL_TN, WQL_CODCR, WQL_BOD5, WQL_CU
                , WQL_ZN, WQL_F, WQL_SE, WQL_ARS, WQL_HG, WQL_CD, WQL_CR6, WQL_PB, WQL_CN, WQL_VLPH, WQL_S2, WQL_OILS, WQL_ASAA, WQL_FCG);
            object ret = g_DatabaseHelper.ExecuteScalar(sql, connection);
            if (ret != null && ret != DBNull.Value)
                return ret.ToString();
            return string.Empty;
        }
    }
}
