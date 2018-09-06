﻿using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SmartEP.Data.SqlServer.BaseData;
using System.IO;
using System.Threading;

namespace SmartEP.Data.SqlServer.MonitoringBusiness
{
    /// <summary>
    /// 名称：AuditDataDAL.cs
    /// 创建人：李飞
    /// 创建日期：2015-10-11
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境空气发布：审核数据处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AuditDataDAL
    {
        #region << 变量 >>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 存储过程处理类
        /// </summary>
        BaseDAHelper g_DBBiz = Singleton<BaseDAHelper>.GetInstance();
        BaseDAHelper g_DBBiz_A_dp = Singleton<BaseDAHelper>.GetInstance();
        BaseDAHelper g_DBBiz_A_hap = Singleton<BaseDAHelper>.GetInstance();
        BaseDAHelper g_DBBiz_A_dap = Singleton<BaseDAHelper>.GetInstance();
        BaseDAHelper g_DBBiz_W_hp = Singleton<BaseDAHelper>.GetInstance();
        BaseDAHelper g_DBBiz_W_dp = Singleton<BaseDAHelper>.GetInstance();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Air, PollutantDataType.Day);

        #endregion

        /// <summary>
        /// 生成审核数据(超级站区分仪器 Update By Rondo)
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="portIds">站点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateData(ApplicationType applicationType, string[] portIds, DateTime dateStart, DateTime dateEnd, string[] factor, out String errMsg)
        {
            errMsg = string.Empty;
            dateStart = DateTime.Parse(dateStart.ToString("yyyy-MM-dd 00:00:00"));
            if (applicationType == ApplicationType.Air)
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:00:00"));
            else
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:59:59.998"));

            try
            {
                if (portIds == null || portIds.Length == 0)
                    return false;
                g_DBBiz.ClearParameters();
                SqlParameter pramDateStart = new SqlParameter();
                pramDateStart = new SqlParameter();
                pramDateStart.SqlDbType = SqlDbType.DateTime;
                pramDateStart.ParameterName = "@m_begin";
                pramDateStart.Value = dateStart;
                g_DBBiz.SetProcedureParameters(pramDateStart);
                SqlParameter pramDateEnd = new SqlParameter();
                pramDateEnd = new SqlParameter();
                pramDateEnd.SqlDbType = SqlDbType.DateTime;
                pramDateEnd.ParameterName = "@m_end";
                pramDateEnd.Value = dateEnd;
                g_DBBiz.SetProcedureParameters(pramDateEnd);
                SqlParameter pramPortIds = new SqlParameter();
                pramPortIds = new SqlParameter();
                pramPortIds.SqlDbType = SqlDbType.NVarChar;
                pramPortIds.ParameterName = "@m_portlist";
                pramPortIds.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz.SetProcedureParameters(pramPortIds);
                SqlParameter pramFactors = new SqlParameter();
                pramFactors = new SqlParameter();
                pramFactors.SqlDbType = SqlDbType.NVarChar;
                pramFactors.ParameterName = "@m_factorlist";
                pramFactors.Value = StringExtensions.GetArrayStrNoEmpty(factor.ToList<string>(), ",");
                g_DBBiz.SetProcedureParameters(pramFactors);

                g_DBBiz_A_dp.ClearParameters();
                SqlParameter pramDateStart_A_dp = new SqlParameter();
                pramDateStart_A_dp = new SqlParameter();
                pramDateStart_A_dp.SqlDbType = SqlDbType.DateTime;
                pramDateStart_A_dp.ParameterName = "@m_begin";
                pramDateStart_A_dp.Value = dateStart;
                g_DBBiz_A_dp.SetProcedureParameters(pramDateStart_A_dp);
                SqlParameter pramDateEnd_A_dp = new SqlParameter();
                pramDateEnd_A_dp = new SqlParameter();
                pramDateEnd_A_dp.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_A_dp.ParameterName = "@m_end";
                pramDateEnd_A_dp.Value = dateEnd;
                g_DBBiz_A_dp.SetProcedureParameters(pramDateEnd_A_dp);
                SqlParameter pramPortIds_A_dp = new SqlParameter();
                pramPortIds_A_dp = new SqlParameter();
                pramPortIds_A_dp.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_A_dp.ParameterName = "@m_portlist";
                pramPortIds_A_dp.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_A_dp.SetProcedureParameters(pramPortIds_A_dp);
                SqlParameter pramFactors_A_dp = new SqlParameter();
                pramFactors_A_dp = new SqlParameter();
                pramFactors_A_dp.SqlDbType = SqlDbType.NVarChar;
                pramFactors_A_dp.ParameterName = "@m_factorlist";
                pramFactors_A_dp.Value = StringExtensions.GetArrayStrNoEmpty(factor.ToList<string>(), ",");
                g_DBBiz_A_dp.SetProcedureParameters(pramFactors_A_dp);

                g_DBBiz_A_hap.ClearParameters();
                SqlParameter pramDateStart_A_hap = new SqlParameter();
                pramDateStart_A_hap = new SqlParameter();
                pramDateStart_A_hap.SqlDbType = SqlDbType.DateTime;
                pramDateStart_A_hap.ParameterName = "@m_begin";
                pramDateStart_A_hap.Value = dateStart;
                g_DBBiz_A_hap.SetProcedureParameters(pramDateStart_A_hap);
                SqlParameter pramDateEnd_A_hap = new SqlParameter();
                pramDateEnd_A_hap = new SqlParameter();
                pramDateEnd_A_hap.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_A_hap.ParameterName = "@m_end";
                pramDateEnd_A_hap.Value = dateEnd;
                g_DBBiz_A_hap.SetProcedureParameters(pramDateEnd_A_hap);
                SqlParameter pramPortIds_A_hap = new SqlParameter();
                pramPortIds_A_hap = new SqlParameter();
                pramPortIds_A_hap.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_A_hap.ParameterName = "@m_portlist";
                pramPortIds_A_hap.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_A_hap.SetProcedureParameters(pramPortIds_A_hap);
                SqlParameter pramFactors_A_hap = new SqlParameter();
                pramFactors_A_hap = new SqlParameter();
                pramFactors_A_hap.SqlDbType = SqlDbType.NVarChar;
                pramFactors_A_hap.ParameterName = "@m_factorlist";
                pramFactors_A_hap.Value = StringExtensions.GetArrayStrNoEmpty(factor.ToList<string>(), ",");
                g_DBBiz_A_hap.SetProcedureParameters(pramFactors_A_hap);

                g_DBBiz_A_dap.ClearParameters();
                SqlParameter pramDateStart_A_dap = new SqlParameter();
                pramDateStart_A_dap = new SqlParameter();
                pramDateStart_A_dap.SqlDbType = SqlDbType.DateTime;
                pramDateStart_A_dap.ParameterName = "@m_begin";
                pramDateStart_A_dap.Value = dateStart;
                g_DBBiz_A_dap.SetProcedureParameters(pramDateStart_A_dap);
                SqlParameter pramDateEnd_A_dap = new SqlParameter();
                pramDateEnd_A_dap = new SqlParameter();
                pramDateEnd_A_dap.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_A_dap.ParameterName = "@m_end";
                pramDateEnd_A_dap.Value = dateEnd;
                g_DBBiz_A_dap.SetProcedureParameters(pramDateEnd_A_dap);
                SqlParameter pramPortIds_A_dap = new SqlParameter();
                pramPortIds_A_dap = new SqlParameter();
                pramPortIds_A_dap.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_A_dap.ParameterName = "@m_portlist";
                pramPortIds_A_dap.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_A_dap.SetProcedureParameters(pramPortIds_A_dap);
                SqlParameter pramFactors_A_dap = new SqlParameter();
                pramFactors_A_dap = new SqlParameter();
                pramFactors_A_dap.SqlDbType = SqlDbType.NVarChar;
                pramFactors_A_dap.ParameterName = "@m_factorlist";
                pramFactors_A_dap.Value = StringExtensions.GetArrayStrNoEmpty(factor.ToList<string>(), ",");
                g_DBBiz_A_dap.SetProcedureParameters(pramFactors_A_dap);

                g_DBBiz_W_hp.ClearParameters();
                SqlParameter pramDateStart_W_hp = new SqlParameter();
                pramDateStart_W_hp = new SqlParameter();
                pramDateStart_W_hp.SqlDbType = SqlDbType.DateTime;
                pramDateStart_W_hp.ParameterName = "@m_begin";
                pramDateStart_W_hp.Value = dateStart;
                g_DBBiz_W_hp.SetProcedureParameters(pramDateStart_W_hp);
                SqlParameter pramDateEnd_W_hp = new SqlParameter();
                pramDateEnd_W_hp = new SqlParameter();
                pramDateEnd_W_hp.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_W_hp.ParameterName = "@m_end";
                pramDateEnd_W_hp.Value = dateEnd;
                g_DBBiz_W_hp.SetProcedureParameters(pramDateEnd_W_hp);
                SqlParameter pramPortIds_W_hp = new SqlParameter();
                pramPortIds_W_hp = new SqlParameter();
                pramPortIds_W_hp.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_W_hp.ParameterName = "@m_portlist";
                pramPortIds_W_hp.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_W_hp.SetProcedureParameters(pramPortIds_W_hp);
                SqlParameter pramFactors_W_hp = new SqlParameter();
                pramFactors_W_hp = new SqlParameter();
                pramFactors_W_hp.SqlDbType = SqlDbType.NVarChar;
                pramFactors_W_hp.ParameterName = "@m_factorlist";
                pramFactors_W_hp.Value = StringExtensions.GetArrayStrNoEmpty(factor.ToList<string>(), ",");
                g_DBBiz_W_hp.SetProcedureParameters(pramFactors_W_hp);

                g_DBBiz_W_dp.ClearParameters();
                SqlParameter pramDateStart_W_dp = new SqlParameter();
                pramDateStart_W_dp = new SqlParameter();
                pramDateStart_W_dp.SqlDbType = SqlDbType.DateTime;
                pramDateStart_W_dp.ParameterName = "@m_begin";
                pramDateStart_W_dp.Value = dateStart;
                g_DBBiz_W_dp.SetProcedureParameters(pramDateStart_W_dp);
                SqlParameter pramDateEnd_W_dp = new SqlParameter();
                pramDateEnd_W_dp = new SqlParameter();
                pramDateEnd_W_dp.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_W_dp.ParameterName = "@m_end";
                pramDateEnd_W_dp.Value = dateEnd;
                g_DBBiz_W_dp.SetProcedureParameters(pramDateEnd_W_dp);
                SqlParameter pramPortIds_W_dp = new SqlParameter();
                pramPortIds_W_dp = new SqlParameter();
                pramPortIds_W_dp.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_W_dp.ParameterName = "@m_portlist";
                pramPortIds_W_dp.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_W_dp.SetProcedureParameters(pramPortIds_W_dp);
                SqlParameter pramFactors_W_dp = new SqlParameter();
                pramFactors_W_dp = new SqlParameter();
                pramFactors_W_dp.SqlDbType = SqlDbType.NVarChar;
                pramFactors_W_dp.ParameterName = "@m_factorlist";
                pramFactors_W_dp.Value = StringExtensions.GetArrayStrNoEmpty(factor.ToList<string>(), ",");
                g_DBBiz_W_dp.SetProcedureParameters(pramFactors_W_dp);

                if (applicationType == ApplicationType.Air)
                {
                    //生成小时、日、周、月、季、年数据
                    //小时计算表数据 修改 by lvyun
                    g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Hour_Calculate_Port_Mul_Super", connection);
                    //小时
                    g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Hour_Port_Mul_Super", connection);
                    //日
                    //g_DBBiz_A_dp.ExecuteProcNonQuery("UP_AirReport_Day_Port_Mul", connection);
                    ////周
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Week_Port_Mul", connection);
                    ////月
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Month_Port_Mul", connection);
                    ////季
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Season_Port_Mul", connection);
                    ////年
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Year_Port_Mul", connection);
                    //生成AQI数据
                    g_DBBiz_A_hap.ExecuteProcNonQuery("UP_AirReport_HourAQI_Port_Mul", connection);
                    //g_DBBiz_A_dap.ExecuteProcNonQuery("UP_AirReport_DayAQI_Port_Mul", connection);
                    //try
                    //{
                    //    //新增周报
                    //    g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Week_Port_Mul_New", connection);
                    //}
                    //catch
                    //{
                    //}
                }
                else
                {
                    //生成小时、日、周、月、季、年数据
                    //小时
                    g_DBBiz_W_hp.ExecuteProcNonQuery("UP_WaterReport_Hour_Port_Mul", connection);
                    //日
                    g_DBBiz_W_dp.ExecuteProcNonQuery("UP_WaterReport_Day_Port_Mul", connection);
                    //数据有效率生成
                    SetReportQualifiedRateByDay(applicationType, portIds, dateStart, dateEnd, out errMsg);
                    ////周
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Week_Port_Mul", connection);
                    ////月
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Month_Port_Mul", connection);
                    ////季
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Season_Port_Mul", connection);
                    ////年
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Year_Port_Mul", connection);

                    //try
                    //{
                    //    //新增日报
                    //    g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Day_Port_Mul_SZ", connection);
                    //}
                    //catch
                    //{
                    //}
                }
                //数据有效率生成
                //SetReportQualifiedRateByDay(applicationType, portIds, dateStart, dateEnd, out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 生成审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="portIds">站点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateData(ApplicationType applicationType, string[] portIds, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            dateStart = DateTime.Parse(dateStart.ToString("yyyy-MM-dd 00:00:00"));
            if (applicationType == ApplicationType.Air)
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:00:00"));
            else
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:59:59.998"));
            try
            {
                if (portIds == null || portIds.Length == 0)
                    return false;
                g_DBBiz.ClearParameters();
                SqlParameter pramDateStart = new SqlParameter();
                pramDateStart = new SqlParameter();
                pramDateStart.SqlDbType = SqlDbType.DateTime;
                pramDateStart.ParameterName = "@m_begin";
                pramDateStart.Value = dateStart;
                g_DBBiz.SetProcedureParameters(pramDateStart);
                SqlParameter pramDateEnd = new SqlParameter();
                pramDateEnd = new SqlParameter();
                pramDateEnd.SqlDbType = SqlDbType.DateTime;
                pramDateEnd.ParameterName = "@m_end";
                pramDateEnd.Value = dateEnd;
                g_DBBiz.SetProcedureParameters(pramDateEnd);
                SqlParameter pramPortIds = new SqlParameter();
                pramPortIds = new SqlParameter();
                pramPortIds.SqlDbType = SqlDbType.NVarChar;
                pramPortIds.ParameterName = "@m_portlist";
                pramPortIds.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz.SetProcedureParameters(pramPortIds);

                g_DBBiz_A_dp.ClearParameters();
                SqlParameter pramDateStart_A_dp = new SqlParameter();
                pramDateStart_A_dp = new SqlParameter();
                pramDateStart_A_dp.SqlDbType = SqlDbType.DateTime;
                pramDateStart_A_dp.ParameterName = "@m_begin";
                pramDateStart_A_dp.Value = dateStart;
                g_DBBiz_A_dp.SetProcedureParameters(pramDateStart_A_dp);
                SqlParameter pramDateEnd_A_dp = new SqlParameter();
                pramDateEnd_A_dp = new SqlParameter();
                pramDateEnd_A_dp.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_A_dp.ParameterName = "@m_end";
                pramDateEnd_A_dp.Value = dateEnd;
                g_DBBiz_A_dp.SetProcedureParameters(pramDateEnd_A_dp);
                SqlParameter pramPortIds_A_dp = new SqlParameter();
                pramPortIds_A_dp = new SqlParameter();
                pramPortIds_A_dp.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_A_dp.ParameterName = "@m_portlist";
                pramPortIds_A_dp.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_A_dp.SetProcedureParameters(pramPortIds_A_dp);

                g_DBBiz_A_hap.ClearParameters();
                SqlParameter pramDateStart_A_hap = new SqlParameter();
                pramDateStart_A_hap = new SqlParameter();
                pramDateStart_A_hap.SqlDbType = SqlDbType.DateTime;
                pramDateStart_A_hap.ParameterName = "@m_begin";
                pramDateStart_A_hap.Value = dateStart;
                g_DBBiz_A_hap.SetProcedureParameters(pramDateStart_A_hap);
                SqlParameter pramDateEnd_A_hap = new SqlParameter();
                pramDateEnd_A_hap = new SqlParameter();
                pramDateEnd_A_hap.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_A_hap.ParameterName = "@m_end";
                pramDateEnd_A_hap.Value = dateEnd;
                g_DBBiz_A_hap.SetProcedureParameters(pramDateEnd_A_hap);
                SqlParameter pramPortIds_A_hap = new SqlParameter();
                pramPortIds_A_hap = new SqlParameter();
                pramPortIds_A_hap.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_A_hap.ParameterName = "@m_portlist";
                pramPortIds_A_hap.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_A_hap.SetProcedureParameters(pramPortIds_A_hap);

                g_DBBiz_A_dap.ClearParameters();
                SqlParameter pramDateStart_A_dap = new SqlParameter();
                pramDateStart_A_dap = new SqlParameter();
                pramDateStart_A_dap.SqlDbType = SqlDbType.DateTime;
                pramDateStart_A_dap.ParameterName = "@m_begin";
                pramDateStart_A_dap.Value = dateStart;
                g_DBBiz_A_dap.SetProcedureParameters(pramDateStart_A_dap);
                SqlParameter pramDateEnd_A_dap = new SqlParameter();
                pramDateEnd_A_dap = new SqlParameter();
                pramDateEnd_A_dap.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_A_dap.ParameterName = "@m_end";
                pramDateEnd_A_dap.Value = dateEnd;
                g_DBBiz_A_dap.SetProcedureParameters(pramDateEnd_A_dap);
                SqlParameter pramPortIds_A_dap = new SqlParameter();
                pramPortIds_A_dap = new SqlParameter();
                pramPortIds_A_dap.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_A_dap.ParameterName = "@m_portlist";
                pramPortIds_A_dap.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_A_dap.SetProcedureParameters(pramPortIds_A_dap);

                g_DBBiz_W_hp.ClearParameters();
                SqlParameter pramDateStart_W_hp = new SqlParameter();
                pramDateStart_W_hp = new SqlParameter();
                pramDateStart_W_hp.SqlDbType = SqlDbType.DateTime;
                pramDateStart_W_hp.ParameterName = "@m_begin";
                pramDateStart_W_hp.Value = dateStart;
                g_DBBiz_W_hp.SetProcedureParameters(pramDateStart_W_hp);
                SqlParameter pramDateEnd_W_hp = new SqlParameter();
                pramDateEnd_W_hp = new SqlParameter();
                pramDateEnd_W_hp.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_W_hp.ParameterName = "@m_end";
                pramDateEnd_W_hp.Value = dateEnd;
                g_DBBiz_W_hp.SetProcedureParameters(pramDateEnd_W_hp);
                SqlParameter pramPortIds_W_hp = new SqlParameter();
                pramPortIds_W_hp = new SqlParameter();
                pramPortIds_W_hp.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_W_hp.ParameterName = "@m_portlist";
                pramPortIds_W_hp.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_W_hp.SetProcedureParameters(pramPortIds_W_hp);

                g_DBBiz_W_dp.ClearParameters();
                SqlParameter pramDateStart_W_dp = new SqlParameter();
                pramDateStart_W_dp = new SqlParameter();
                pramDateStart_W_dp.SqlDbType = SqlDbType.DateTime;
                pramDateStart_W_dp.ParameterName = "@m_begin";
                pramDateStart_W_dp.Value = dateStart;
                g_DBBiz_W_dp.SetProcedureParameters(pramDateStart_W_dp);
                SqlParameter pramDateEnd_W_dp = new SqlParameter();
                pramDateEnd_W_dp = new SqlParameter();
                pramDateEnd_W_dp.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_W_dp.ParameterName = "@m_end";
                pramDateEnd_W_dp.Value = dateEnd;
                g_DBBiz_W_dp.SetProcedureParameters(pramDateEnd_W_dp);
                SqlParameter pramPortIds_W_dp = new SqlParameter();
                pramPortIds_W_dp = new SqlParameter();
                pramPortIds_W_dp.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_W_dp.ParameterName = "@m_portlist";
                pramPortIds_W_dp.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_W_dp.SetProcedureParameters(pramPortIds_W_dp);

                if (applicationType == ApplicationType.Air)
                {
                    //生成小时、日、周、月、季、年数据
                    //小时计算表数据 修改 by lvyun
                    g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Hour_Calculate_Port_Mul", connection);
                    //小时
                    g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Hour_Port_Mul", connection);
                    //日
                    //g_DBBiz_A_dp.ExecuteProcNonQuery("UP_AirReport_Day_Port_Mul", connection);
                    ////周
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Week_Port_Mul", connection);
                    ////月
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Month_Port_Mul", connection);
                    ////季
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Season_Port_Mul", connection);
                    ////年
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Year_Port_Mul", connection);
                    //生成AQI数据
                    g_DBBiz_A_hap.ExecuteProcNonQuery("UP_AirReport_HourAQI_Port_Mul", connection);
                    //g_DBBiz_A_dap.ExecuteProcNonQuery("UP_AirReport_DayAQI_Port_Mul", connection);
                    //try
                    //{
                    //    //新增周报
                    //    g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Week_Port_Mul_New", connection);
                    //}
                    //catch
                    //{
                    //}
                }
                else
                {
                    //生成小时、日、周、月、季、年数据
                    //小时
                    g_DBBiz_W_hp.ExecuteProcNonQuery("UP_WaterReport_Hour_Port_Mul", connection);
                    //日
                    g_DBBiz_W_dp.ExecuteProcNonQuery("UP_WaterReport_Day_Port_Mul", connection);
                    //数据有效率生成
                    SetReportQualifiedRateByDay(applicationType, portIds, dateStart, dateEnd, out errMsg);
                    ////周
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Week_Port_Mul", connection);
                    ////月
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Month_Port_Mul", connection);
                    ////季
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Season_Port_Mul", connection);
                    ////年
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Year_Port_Mul", connection);

                    //try
                    //{
                    //    //新增日报
                    //    g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Day_Port_Mul_SZ", connection);
                    //}
                    //catch
                    //{
                    //}
                }
                //数据有效率生成
                //SetReportQualifiedRateByDay(applicationType, portIds, dateStart, dateEnd, out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 生成审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="portIds">站点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateDataDay(ApplicationType applicationType, string[] portIds, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            dateStart = DateTime.Parse(dateStart.ToString("yyyy-MM-dd 00:00:00"));
            if (applicationType == ApplicationType.Air)
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:00:00"));
            else
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:59:59.998"));
            try
            {
                if (portIds == null || portIds.Length == 0)
                    return false;
                g_DBBiz.ClearParameters();
                SqlParameter pramDateStart = new SqlParameter();
                pramDateStart = new SqlParameter();
                pramDateStart.SqlDbType = SqlDbType.DateTime;
                pramDateStart.ParameterName = "@m_begin";
                pramDateStart.Value = dateStart;
                g_DBBiz.SetProcedureParameters(pramDateStart);
                SqlParameter pramDateEnd = new SqlParameter();
                pramDateEnd = new SqlParameter();
                pramDateEnd.SqlDbType = SqlDbType.DateTime;
                pramDateEnd.ParameterName = "@m_end";
                pramDateEnd.Value = dateEnd;
                g_DBBiz.SetProcedureParameters(pramDateEnd);
                SqlParameter pramPortIds = new SqlParameter();
                pramPortIds = new SqlParameter();
                pramPortIds.SqlDbType = SqlDbType.NVarChar;
                pramPortIds.ParameterName = "@m_portlist";
                pramPortIds.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz.SetProcedureParameters(pramPortIds);

                g_DBBiz_A_dp.ClearParameters();
                SqlParameter pramDateStart_A_dp = new SqlParameter();
                pramDateStart_A_dp = new SqlParameter();
                pramDateStart_A_dp.SqlDbType = SqlDbType.DateTime;
                pramDateStart_A_dp.ParameterName = "@m_begin";
                pramDateStart_A_dp.Value = dateStart;
                g_DBBiz_A_dp.SetProcedureParameters(pramDateStart_A_dp);
                SqlParameter pramDateEnd_A_dp = new SqlParameter();
                pramDateEnd_A_dp = new SqlParameter();
                pramDateEnd_A_dp.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_A_dp.ParameterName = "@m_end";
                pramDateEnd_A_dp.Value = dateEnd;
                g_DBBiz_A_dp.SetProcedureParameters(pramDateEnd_A_dp);
                SqlParameter pramPortIds_A_dp = new SqlParameter();
                pramPortIds_A_dp = new SqlParameter();
                pramPortIds_A_dp.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_A_dp.ParameterName = "@m_portlist";
                pramPortIds_A_dp.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_A_dp.SetProcedureParameters(pramPortIds_A_dp);

                g_DBBiz_A_dap.ClearParameters();
                SqlParameter pramDateStart_A_dap = new SqlParameter();
                pramDateStart_A_dap = new SqlParameter();
                pramDateStart_A_dap.SqlDbType = SqlDbType.DateTime;
                pramDateStart_A_dap.ParameterName = "@m_begin";
                pramDateStart_A_dap.Value = dateStart;
                g_DBBiz_A_dap.SetProcedureParameters(pramDateStart_A_dap);
                SqlParameter pramDateEnd_A_dap = new SqlParameter();
                pramDateEnd_A_dap = new SqlParameter();
                pramDateEnd_A_dap.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_A_dap.ParameterName = "@m_end";
                pramDateEnd_A_dap.Value = dateEnd;
                g_DBBiz_A_dap.SetProcedureParameters(pramDateEnd_A_dap);
                SqlParameter pramPortIds_A_dap = new SqlParameter();
                pramPortIds_A_dap = new SqlParameter();
                pramPortIds_A_dap.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_A_dap.ParameterName = "@m_portlist";
                pramPortIds_A_dap.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_A_dap.SetProcedureParameters(pramPortIds_A_dap);

                g_DBBiz_W_hp.ClearParameters();
                SqlParameter pramDateStart_W_hp = new SqlParameter();
                pramDateStart_W_hp = new SqlParameter();
                pramDateStart_W_hp.SqlDbType = SqlDbType.DateTime;
                pramDateStart_W_hp.ParameterName = "@m_begin";
                pramDateStart_W_hp.Value = dateStart;
                g_DBBiz_W_hp.SetProcedureParameters(pramDateStart_W_hp);
                SqlParameter pramDateEnd_W_hp = new SqlParameter();
                pramDateEnd_W_hp = new SqlParameter();
                pramDateEnd_W_hp.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_W_hp.ParameterName = "@m_end";
                pramDateEnd_W_hp.Value = dateEnd;
                g_DBBiz_W_hp.SetProcedureParameters(pramDateEnd_W_hp);
                SqlParameter pramPortIds_W_hp = new SqlParameter();
                pramPortIds_W_hp = new SqlParameter();
                pramPortIds_W_hp.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_W_hp.ParameterName = "@m_portlist";
                pramPortIds_W_hp.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_W_hp.SetProcedureParameters(pramPortIds_W_hp);

                g_DBBiz_W_dp.ClearParameters();
                SqlParameter pramDateStart_W_dp = new SqlParameter();
                pramDateStart_W_dp = new SqlParameter();
                pramDateStart_W_dp.SqlDbType = SqlDbType.DateTime;
                pramDateStart_W_dp.ParameterName = "@m_begin";
                pramDateStart_W_dp.Value = dateStart;
                g_DBBiz_W_dp.SetProcedureParameters(pramDateStart_W_dp);
                SqlParameter pramDateEnd_W_dp = new SqlParameter();
                pramDateEnd_W_dp = new SqlParameter();
                pramDateEnd_W_dp.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_W_dp.ParameterName = "@m_end";
                pramDateEnd_W_dp.Value = dateEnd;
                g_DBBiz_W_dp.SetProcedureParameters(pramDateEnd_W_dp);
                SqlParameter pramPortIds_W_dp = new SqlParameter();
                pramPortIds_W_dp = new SqlParameter();
                pramPortIds_W_dp.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_W_dp.ParameterName = "@m_portlist";
                pramPortIds_W_dp.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_W_dp.SetProcedureParameters(pramPortIds_W_dp);

                if (applicationType == ApplicationType.Air)
                {
                    //生成小时、日、周、月、季、年数据
                    //小时
                    g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Hour_Port_Mul", connection);
                    //日
                    g_DBBiz_A_dp.ExecuteProcNonQuery("UP_AirReport_Day_Port_Mul", connection);
                    ////周
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Week_Port_Mul", connection);
                    ////月
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Month_Port_Mul", connection);
                    ////季
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Season_Port_Mul", connection);
                    ////年
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Year_Port_Mul", connection);
                    //生成AQI数据

                    g_DBBiz_A_dap.ExecuteProcNonQuery("UP_AirReport_DayAQI_Port_Mul", connection);
                    //try
                    //{
                    //    //新增周报
                    //    g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Week_Port_Mul_New", connection);
                    //}
                    //catch
                    //{
                    //}
                }
                else
                {
                    //生成小时、日、周、月、季、年数据
                    //小时
                    g_DBBiz_W_hp.ExecuteProcNonQuery("UP_WaterReport_Hour_Port_Mul", connection);
                    //日
                    g_DBBiz_W_dp.ExecuteProcNonQuery("UP_WaterReport_Day_Port_Mul", connection);
                    //数据有效率生成
                    SetReportQualifiedRateByDay(applicationType, portIds, dateStart, dateEnd, out errMsg);
                    ////周
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Week_Port_Mul", connection);
                    ////月
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Month_Port_Mul", connection);
                    ////季
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Season_Port_Mul", connection);
                    ////年
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Year_Port_Mul", connection);

                    //try
                    //{
                    //    //新增日报
                    //    g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Day_Port_Mul_SZ", connection);
                    //}
                    //catch
                    //{
                    //}
                }
                //数据有效率生成
                //SetReportQualifiedRateByDay(applicationType, portIds, dateStart, dateEnd, out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 生成审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="portIds">站点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateDataNew(ApplicationType applicationType, string[] portIds, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            dateStart = DateTime.Parse(dateStart.ToString("yyyy-MM-dd 00:00:00"));
            if (applicationType == ApplicationType.Air)
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:00:00"));
            else
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:59:59.998"));
            try
            {
                if (portIds == null || portIds.Length == 0)
                    return false;
                g_DBBiz.ClearParameters();
                SqlParameter pramDateStart = new SqlParameter();
                pramDateStart = new SqlParameter();
                pramDateStart.SqlDbType = SqlDbType.DateTime;
                pramDateStart.ParameterName = "@m_begin";
                pramDateStart.Value = dateStart;
                g_DBBiz.SetProcedureParameters(pramDateStart);
                SqlParameter pramDateEnd = new SqlParameter();
                pramDateEnd = new SqlParameter();
                pramDateEnd.SqlDbType = SqlDbType.DateTime;
                pramDateEnd.ParameterName = "@m_end";
                pramDateEnd.Value = dateEnd;
                g_DBBiz.SetProcedureParameters(pramDateEnd);
                SqlParameter pramPortIds = new SqlParameter();
                pramPortIds = new SqlParameter();
                pramPortIds.SqlDbType = SqlDbType.NVarChar;
                pramPortIds.ParameterName = "@m_portlist";
                pramPortIds.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz.SetProcedureParameters(pramPortIds);

                g_DBBiz_A_dp.ClearParameters();
                SqlParameter pramDateStart_A_dp = new SqlParameter();
                pramDateStart_A_dp = new SqlParameter();
                pramDateStart_A_dp.SqlDbType = SqlDbType.DateTime;
                pramDateStart_A_dp.ParameterName = "@m_begin";
                pramDateStart_A_dp.Value = dateStart;
                g_DBBiz_A_dp.SetProcedureParameters(pramDateStart_A_dp);
                SqlParameter pramDateEnd_A_dp = new SqlParameter();
                pramDateEnd_A_dp = new SqlParameter();
                pramDateEnd_A_dp.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_A_dp.ParameterName = "@m_end";
                pramDateEnd_A_dp.Value = dateEnd;
                g_DBBiz_A_dp.SetProcedureParameters(pramDateEnd_A_dp);
                SqlParameter pramPortIds_A_dp = new SqlParameter();
                pramPortIds_A_dp = new SqlParameter();
                pramPortIds_A_dp.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_A_dp.ParameterName = "@m_portlist";
                pramPortIds_A_dp.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_A_dp.SetProcedureParameters(pramPortIds_A_dp);

                g_DBBiz_A_hap.ClearParameters();
                SqlParameter pramDateStart_A_hap = new SqlParameter();
                pramDateStart_A_hap = new SqlParameter();
                pramDateStart_A_hap.SqlDbType = SqlDbType.DateTime;
                pramDateStart_A_hap.ParameterName = "@m_begin";
                pramDateStart_A_hap.Value = dateStart;
                g_DBBiz_A_hap.SetProcedureParameters(pramDateStart_A_hap);
                SqlParameter pramDateEnd_A_hap = new SqlParameter();
                pramDateEnd_A_hap = new SqlParameter();
                pramDateEnd_A_hap.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_A_hap.ParameterName = "@m_end";
                pramDateEnd_A_hap.Value = dateEnd;
                g_DBBiz_A_hap.SetProcedureParameters(pramDateEnd_A_hap);
                SqlParameter pramPortIds_A_hap = new SqlParameter();
                pramPortIds_A_hap = new SqlParameter();
                pramPortIds_A_hap.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_A_hap.ParameterName = "@m_portlist";
                pramPortIds_A_hap.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_A_hap.SetProcedureParameters(pramPortIds_A_hap);

                g_DBBiz_A_dap.ClearParameters();
                SqlParameter pramDateStart_A_dap = new SqlParameter();
                pramDateStart_A_dap = new SqlParameter();
                pramDateStart_A_dap.SqlDbType = SqlDbType.DateTime;
                pramDateStart_A_dap.ParameterName = "@m_begin";
                pramDateStart_A_dap.Value = dateStart;
                g_DBBiz_A_dap.SetProcedureParameters(pramDateStart_A_dap);
                SqlParameter pramDateEnd_A_dap = new SqlParameter();
                pramDateEnd_A_dap = new SqlParameter();
                pramDateEnd_A_dap.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_A_dap.ParameterName = "@m_end";
                pramDateEnd_A_dap.Value = dateEnd;
                g_DBBiz_A_dap.SetProcedureParameters(pramDateEnd_A_dap);
                SqlParameter pramPortIds_A_dap = new SqlParameter();
                pramPortIds_A_dap = new SqlParameter();
                pramPortIds_A_dap.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_A_dap.ParameterName = "@m_portlist";
                pramPortIds_A_dap.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_A_dap.SetProcedureParameters(pramPortIds_A_dap);

                g_DBBiz_W_hp.ClearParameters();
                SqlParameter pramDateStart_W_hp = new SqlParameter();
                pramDateStart_W_hp = new SqlParameter();
                pramDateStart_W_hp.SqlDbType = SqlDbType.DateTime;
                pramDateStart_W_hp.ParameterName = "@m_begin";
                pramDateStart_W_hp.Value = dateStart;
                g_DBBiz_W_hp.SetProcedureParameters(pramDateStart_W_hp);
                SqlParameter pramDateEnd_W_hp = new SqlParameter();
                pramDateEnd_W_hp = new SqlParameter();
                pramDateEnd_W_hp.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_W_hp.ParameterName = "@m_end";
                pramDateEnd_W_hp.Value = dateEnd;
                g_DBBiz_W_hp.SetProcedureParameters(pramDateEnd_W_hp);
                SqlParameter pramPortIds_W_hp = new SqlParameter();
                pramPortIds_W_hp = new SqlParameter();
                pramPortIds_W_hp.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_W_hp.ParameterName = "@m_portlist";
                pramPortIds_W_hp.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_W_hp.SetProcedureParameters(pramPortIds_W_hp);

                if (applicationType == ApplicationType.Air)
                {
                    //生成小时、日、周、月、季、年数据
                    //小时
                    g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Hour_Port_Mul", connection);
                    //日
                    g_DBBiz_A_dp.ExecuteProcNonQuery("UP_AirReport_Day_Port_Mul", connection);
                    ////周
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Week_Port_Mul", connection);
                    ////月
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Month_Port_Mul", connection);
                    ////季
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Season_Port_Mul", connection);
                    ////年
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Year_Port_Mul", connection);
                    //生成AQI数据
                    g_DBBiz_A_hap.ExecuteProcNonQuery("UP_AirReport_HourAQI_Port_Mul", connection);
                    g_DBBiz_A_dap.ExecuteProcNonQuery("UP_AirReport_DayAQI_Port_Mul", connection);
                    //try
                    //{
                    //    //新增周报
                    //    g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Week_Port_Mul_New", connection);
                    //}
                    //catch
                    //{
                    //}
                }
                else
                {
                    //生成小时、日、周、月、季、年数据
                    //小时
                    string hourDate = DateTime.Now.ToString();
                    g_DBBiz_W_hp.ExecuteProcNonQuery("UP_WaterReportNew_Hour_Port_Mul", connection);
                    TimeSpan waitTime = new TimeSpan(0, 0, 0, 1);
                    Thread.CurrentThread.Join(waitTime);
                    //日
                    string dayDate = DateTime.Now.ToString();
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Day_Port_Mul_SZ", connection);
                    ////Thread.CurrentThread.Join(waitTime);
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Day_Port_Mul", connection);

                    SetDayReportByHour("UP_WaterReport_Day_Port_Mul_SZ", applicationType, portIds, dateStart, dateEnd, out errMsg);
                    SetDayReportByHour("UP_WaterReport_Day_Port_Mul", applicationType, portIds, dateStart, dateEnd, out errMsg);
                    string qualifiedDate = DateTime.Now.ToString();

                    //数据有效率生成
                    SetReportQualifiedRateByDay(applicationType, portIds, dateStart, dateEnd, out errMsg);
                    string qualifiedDateafter = DateTime.Now.ToString();

                    WriteTextLog("(新)存储过程执行时间：", "小时数据：" + hourDate + "日数据：" + dayDate + "有效率：" + qualifiedDate + "结束：" + qualifiedDateafter, DateTime.Now);
                    WriteTextLog("(新)存储过程参数：", "点位" + StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",") + "开始时间" + dateStart.ToString() + "结束时间" + dateEnd.ToString(), DateTime.Now);


                    ////周
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Week_Port_Mul", connection);
                    ////月
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Month_Port_Mul", connection);
                    ////季
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Season_Port_Mul", connection);
                    ////年
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Year_Port_Mul", connection);

                    //try
                    //{
                    //    //新增日报
                    //    g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Day_Port_Mul_SZ", connection);
                    //}
                    //catch
                    //{
                    //}
                }
                //数据有效率生成
                //SetReportQualifiedRateByDay(applicationType, portIds, dateStart, dateEnd, out errMsg);
            }
            catch (Exception ex)
            {
                WriteTextLog("审核执行错误", ex.Message, DateTime.Now);
                errMsg = ex.Message;
                return false;
            }
            return true;
        }
        public bool SetDayReportByHour(string procName, ApplicationType applicationType, string[] portIds, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            dateStart = DateTime.Parse(dateStart.ToString("yyyy-MM-dd 00:00:00"));
            if (applicationType == ApplicationType.Air)
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:00:00"));
            else
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:59:59.998"));
            try
            {
                if (portIds == null || portIds.Length == 0)
                    return false;
                g_DBBiz.ClearParameters();
                SqlParameter pramDateStart = new SqlParameter();
                pramDateStart = new SqlParameter();
                pramDateStart.SqlDbType = SqlDbType.DateTime;
                pramDateStart.ParameterName = "@m_begin";
                pramDateStart.Value = dateStart;
                g_DBBiz.SetProcedureParameters(pramDateStart);

                SqlParameter pramDateEnd = new SqlParameter();
                pramDateEnd = new SqlParameter();
                pramDateEnd.SqlDbType = SqlDbType.DateTime;
                pramDateEnd.ParameterName = "@m_end";
                pramDateEnd.Value = dateEnd;
                g_DBBiz.SetProcedureParameters(pramDateEnd);

                SqlParameter pramPortIds = new SqlParameter();
                pramPortIds = new SqlParameter();
                pramPortIds.SqlDbType = SqlDbType.NVarChar;
                pramPortIds.ParameterName = "@m_portlist";
                pramPortIds.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz.SetProcedureParameters(pramPortIds);

                if (applicationType == ApplicationType.Air)
                {
                }
                else
                {
                    g_DBBiz.ExecuteProcNonQuery(procName, connection);
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
            return true;
        }
        /// <summary>  
        /// 写入日志到文本文件  
        /// </summary>  
        /// <param name="action">动作</param>  
        /// <param name="strMessage">日志内容</param>  
        /// <param name="time">时间</param>  
        public static void WriteTextLog(string action, string strMessage, DateTime time)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"System\Log\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileFullPath = path + time.ToString("yyyy-MM-dd") + ".System.txt";
            StringBuilder str = new StringBuilder();
            str.Append("Time:    " + time.ToString() + "\r\n");
            str.Append("Action:  " + action + "\r\n");
            str.Append("Message: " + strMessage + "\r\n");
            str.Append("-----------------------------------------------------------\r\n\r\n");
            StreamWriter sw;
            if (!File.Exists(fileFullPath))
            {
                sw = File.CreateText(fileFullPath);
            }
            else
            {
                sw = File.AppendText(fileFullPath);
            }
            sw.WriteLine(str.ToString());
            sw.Close();
        }
        /// <summary>
        /// 生成审核数据（超级站）
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="portIds">站点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateDataSuper(ApplicationType applicationType, string[] portIds, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            dateStart = DateTime.Parse(dateStart.ToString("yyyy-MM-dd 00:00:00"));
            if (applicationType == ApplicationType.Air)
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:59:59"));
            else
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:59:59.998"));
            try
            {
                if (portIds == null || portIds.Length == 0)
                    return false;
                g_DBBiz.ClearParameters();
                SqlParameter pramDateStart = new SqlParameter();
                pramDateStart = new SqlParameter();
                pramDateStart.SqlDbType = SqlDbType.DateTime;
                pramDateStart.ParameterName = "@m_begin";
                pramDateStart.Value = dateStart;
                g_DBBiz.SetProcedureParameters(pramDateStart);
                SqlParameter pramDateEnd = new SqlParameter();
                pramDateEnd = new SqlParameter();
                pramDateEnd.SqlDbType = SqlDbType.DateTime;
                pramDateEnd.ParameterName = "@m_end";
                pramDateEnd.Value = dateEnd;
                g_DBBiz.SetProcedureParameters(pramDateEnd);
                SqlParameter pramPortIds = new SqlParameter();
                pramPortIds = new SqlParameter();
                pramPortIds.SqlDbType = SqlDbType.NVarChar;
                pramPortIds.ParameterName = "@m_portlist";
                pramPortIds.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz.SetProcedureParameters(pramPortIds);

                g_DBBiz_A_dp.ClearParameters();
                SqlParameter pramDateStart_A_dp = new SqlParameter();
                pramDateStart_A_dp = new SqlParameter();
                pramDateStart_A_dp.SqlDbType = SqlDbType.DateTime;
                pramDateStart_A_dp.ParameterName = "@m_begin";
                pramDateStart_A_dp.Value = dateStart;
                g_DBBiz_A_dp.SetProcedureParameters(pramDateStart_A_dp);
                SqlParameter pramDateEnd_A_dp = new SqlParameter();
                pramDateEnd_A_dp = new SqlParameter();
                pramDateEnd_A_dp.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_A_dp.ParameterName = "@m_end";
                pramDateEnd_A_dp.Value = dateEnd;
                g_DBBiz_A_dp.SetProcedureParameters(pramDateEnd_A_dp);
                SqlParameter pramPortIds_A_dp = new SqlParameter();
                pramPortIds_A_dp = new SqlParameter();
                pramPortIds_A_dp.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_A_dp.ParameterName = "@m_portlist";
                pramPortIds_A_dp.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_A_dp.SetProcedureParameters(pramPortIds_A_dp);

                g_DBBiz_W_hp.ClearParameters();
                SqlParameter pramDateStart_W_hp = new SqlParameter();
                pramDateStart_W_hp = new SqlParameter();
                pramDateStart_W_hp.SqlDbType = SqlDbType.DateTime;
                pramDateStart_W_hp.ParameterName = "@m_begin";
                pramDateStart_W_hp.Value = dateStart;
                g_DBBiz_W_hp.SetProcedureParameters(pramDateStart_W_hp);
                SqlParameter pramDateEnd_W_hp = new SqlParameter();
                pramDateEnd_W_hp = new SqlParameter();
                pramDateEnd_W_hp.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_W_hp.ParameterName = "@m_end";
                pramDateEnd_W_hp.Value = dateEnd;
                g_DBBiz_W_hp.SetProcedureParameters(pramDateEnd_W_hp);
                SqlParameter pramPortIds_W_hp = new SqlParameter();
                pramPortIds_W_hp = new SqlParameter();
                pramPortIds_W_hp.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_W_hp.ParameterName = "@m_portlist";
                pramPortIds_W_hp.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_W_hp.SetProcedureParameters(pramPortIds_W_hp);

                g_DBBiz_W_dp.ClearParameters();
                SqlParameter pramDateStart_W_dp = new SqlParameter();
                pramDateStart_W_dp = new SqlParameter();
                pramDateStart_W_dp.SqlDbType = SqlDbType.DateTime;
                pramDateStart_W_dp.ParameterName = "@m_begin";
                pramDateStart_W_dp.Value = dateStart;
                g_DBBiz_W_dp.SetProcedureParameters(pramDateStart_W_dp);
                SqlParameter pramDateEnd_W_dp = new SqlParameter();
                pramDateEnd_W_dp = new SqlParameter();
                pramDateEnd_W_dp.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_W_dp.ParameterName = "@m_end";
                pramDateEnd_W_dp.Value = dateEnd;
                g_DBBiz_W_dp.SetProcedureParameters(pramDateEnd_W_dp);
                SqlParameter pramPortIds_W_dp = new SqlParameter();
                pramPortIds_W_dp = new SqlParameter();
                pramPortIds_W_dp.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_W_dp.ParameterName = "@m_portlist";
                pramPortIds_W_dp.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_W_dp.SetProcedureParameters(pramPortIds_W_dp);

                if (applicationType == ApplicationType.Air)
                {
                    //生成小时、日数据
                    //小时
                    g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Hour_Port_Mul", connection);
                    //日
                    g_DBBiz_A_dp.ExecuteProcNonQuery("UP_AirReport_Day_Port_Mul", connection);
                }
                else
                {
                    //生成小时、日、周、月、季、年数据
                    //小时
                    g_DBBiz_W_hp.ExecuteProcNonQuery("UP_WaterReport_Hour_Port_Mul", connection);
                    //日
                    g_DBBiz_W_dp.ExecuteProcNonQuery("UP_WaterReport_Day_Port_Mul", connection);
                    ////周
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Week_Port_Mul", connection);
                    ////月
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Month_Port_Mul", connection);
                    ////季
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Season_Port_Mul", connection);
                    ////年
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Year_Port_Mul", connection);

                    //try
                    //{
                    //    //新增日报
                    //    g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Day_Port_Mul_SZ", connection);
                    //}
                    //catch
                    //{
                    //}
                }
                //数据有效率生成
                //SetReportQualifiedRateByDay(applicationType, portIds, dateStart, dateEnd, out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
            return true;
        }


        /// <summary>
        /// 从审核后小时数据生成审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="portIds">站点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateDataFromHoursReport(ApplicationType applicationType, string[] portIds, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            dateStart = DateTime.Parse(dateStart.ToString("yyyy-MM-dd 00:00:00"));
            if (applicationType == ApplicationType.Air)
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:00:00"));
            else
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:59:59.998"));
            try
            {
                if (portIds == null || portIds.Length == 0)
                    return false;
                g_DBBiz.ClearParameters();
                SqlParameter pramDateStart = new SqlParameter();
                pramDateStart = new SqlParameter();
                pramDateStart.SqlDbType = SqlDbType.DateTime;
                pramDateStart.ParameterName = "@m_begin";
                pramDateStart.Value = dateStart;
                g_DBBiz.SetProcedureParameters(pramDateStart);
                SqlParameter pramDateEnd = new SqlParameter();
                pramDateEnd = new SqlParameter();
                pramDateEnd.SqlDbType = SqlDbType.DateTime;
                pramDateEnd.ParameterName = "@m_end";
                pramDateEnd.Value = dateEnd;
                g_DBBiz.SetProcedureParameters(pramDateEnd);
                SqlParameter pramPortIds = new SqlParameter();
                pramPortIds = new SqlParameter();
                pramPortIds.SqlDbType = SqlDbType.NVarChar;
                pramPortIds.ParameterName = "@m_portlist";
                pramPortIds.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz.SetProcedureParameters(pramPortIds);

                g_DBBiz_A_dap.ClearParameters();
                SqlParameter pramDateStart_A_dap = new SqlParameter();
                pramDateStart_A_dap = new SqlParameter();
                pramDateStart_A_dap.SqlDbType = SqlDbType.DateTime;
                pramDateStart_A_dap.ParameterName = "@m_begin";
                pramDateStart_A_dap.Value = dateStart;
                g_DBBiz_A_dap.SetProcedureParameters(pramDateStart_A_dap);
                SqlParameter pramDateEnd_A_dap = new SqlParameter();
                pramDateEnd_A_dap = new SqlParameter();
                pramDateEnd_A_dap.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_A_dap.ParameterName = "@m_end";
                pramDateEnd_A_dap.Value = dateEnd;
                g_DBBiz_A_dap.SetProcedureParameters(pramDateEnd_A_dap);
                SqlParameter pramPortIds_A_dap = new SqlParameter();
                pramPortIds_A_dap = new SqlParameter();
                pramPortIds_A_dap.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_A_dap.ParameterName = "@m_portlist";
                pramPortIds_A_dap.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_A_dap.SetProcedureParameters(pramPortIds_A_dap);

                g_DBBiz_W_dp.ClearParameters();
                SqlParameter pramDateStart_W_dp = new SqlParameter();
                pramDateStart_W_dp = new SqlParameter();
                pramDateStart_W_dp.SqlDbType = SqlDbType.DateTime;
                pramDateStart_W_dp.ParameterName = "@m_begin";
                pramDateStart_W_dp.Value = dateStart;
                g_DBBiz_W_dp.SetProcedureParameters(pramDateStart_W_dp);
                SqlParameter pramDateEnd_W_dp = new SqlParameter();
                pramDateEnd_W_dp = new SqlParameter();
                pramDateEnd_W_dp.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_W_dp.ParameterName = "@m_end";
                pramDateEnd_W_dp.Value = dateEnd;
                g_DBBiz_W_dp.SetProcedureParameters(pramDateEnd_W_dp);
                SqlParameter pramPortIds_W_dp = new SqlParameter();
                pramPortIds_W_dp = new SqlParameter();
                pramPortIds_W_dp.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_W_dp.ParameterName = "@m_portlist";
                pramPortIds_W_dp.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_W_dp.SetProcedureParameters(pramPortIds_W_dp);

                if (applicationType == ApplicationType.Air)
                {
                    //生成小时、日、周、月、季、年数据
                    //日
                    g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Day_Port_Mul", connection);
                    ////周
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Week_Port_Mul", connection);
                    ////月
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Month_Port_Mul", connection);
                    ////季
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Season_Port_Mul", connection);
                    ////年
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Year_Port_Mul", connection);
                    //生成AQI数据
                    g_DBBiz_A_dap.ExecuteProcNonQuery("UP_AirReport_DayAQI_Port_Mul", connection);
                    //try
                    //{
                    //    //新增周报
                    //    g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Week_Port_Mul_New", connection);
                    //}
                    //catch
                    //{
                    //}
                }
                else
                {
                    //生成日、周、月、季、年数据
                    //日
                    g_DBBiz_W_dp.ExecuteProcNonQuery("UP_WaterReport_Day_Port_Mul", connection);
                    ////周
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Week_Port_Mul", connection);
                    ////月
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Month_Port_Mul", connection);
                    ////季
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Season_Port_Mul", connection);
                    ////年
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Year_Port_Mul", connection);

                    //try
                    //{
                    //    //新增日报
                    //    g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Day_Port_Mul_SZ", connection);
                    //}
                    //catch
                    //{
                    //}
                }
                //数据有效率生成
                //SetReportQualifiedRateByDay(applicationType, portIds, dateStart, dateEnd, out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
            return true;
        }
        /// <summary>
        /// 从审核后小时数据生成审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="portIds">站点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateDataFromHourReport(ApplicationType applicationType, string[] portIds, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            dateStart = DateTime.Parse(dateStart.ToString("yyyy-MM-dd 00:00:00"));
            if (applicationType == ApplicationType.Air)
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:00:00"));
            else
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:59:59.998"));
            try
            {
                if (portIds == null || portIds.Length == 0)
                    return false;
                g_DBBiz.ClearParameters();
                SqlParameter pramDateStart = new SqlParameter();
                pramDateStart = new SqlParameter();
                pramDateStart.SqlDbType = SqlDbType.DateTime;
                pramDateStart.ParameterName = "@m_begin";
                pramDateStart.Value = dateStart;
                g_DBBiz.SetProcedureParameters(pramDateStart);
                SqlParameter pramDateEnd = new SqlParameter();
                pramDateEnd = new SqlParameter();
                pramDateEnd.SqlDbType = SqlDbType.DateTime;
                pramDateEnd.ParameterName = "@m_end";
                pramDateEnd.Value = dateEnd;
                g_DBBiz.SetProcedureParameters(pramDateEnd);
                SqlParameter pramPortIds = new SqlParameter();
                pramPortIds = new SqlParameter();
                pramPortIds.SqlDbType = SqlDbType.NVarChar;
                pramPortIds.ParameterName = "@m_portlist";
                pramPortIds.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz.SetProcedureParameters(pramPortIds);

                g_DBBiz_A_hap.ClearParameters();
                SqlParameter pramDateStart_A_hap = new SqlParameter();
                pramDateStart_A_hap = new SqlParameter();
                pramDateStart_A_hap.SqlDbType = SqlDbType.DateTime;
                pramDateStart_A_hap.ParameterName = "@m_begin";
                pramDateStart_A_hap.Value = dateStart;
                g_DBBiz_A_hap.SetProcedureParameters(pramDateStart_A_hap);
                SqlParameter pramDateEnd_A_hap = new SqlParameter();
                pramDateEnd_A_hap = new SqlParameter();
                pramDateEnd_A_hap.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_A_hap.ParameterName = "@m_end";
                pramDateEnd_A_hap.Value = dateEnd;
                g_DBBiz_A_hap.SetProcedureParameters(pramDateEnd_A_hap);
                SqlParameter pramPortIds_A_hap = new SqlParameter();
                pramPortIds_A_hap = new SqlParameter();
                pramPortIds_A_hap.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_A_hap.ParameterName = "@m_portlist";
                pramPortIds_A_hap.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_A_hap.SetProcedureParameters(pramPortIds_A_hap);

                g_DBBiz_A_dap.ClearParameters();
                SqlParameter pramDateStart_A_dap = new SqlParameter();
                pramDateStart_A_dap = new SqlParameter();
                pramDateStart_A_dap.SqlDbType = SqlDbType.DateTime;
                pramDateStart_A_dap.ParameterName = "@m_begin";
                pramDateStart_A_dap.Value = dateStart;
                g_DBBiz_A_dap.SetProcedureParameters(pramDateStart_A_dap);
                SqlParameter pramDateEnd_A_dap = new SqlParameter();
                pramDateEnd_A_dap = new SqlParameter();
                pramDateEnd_A_dap.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_A_dap.ParameterName = "@m_end";
                pramDateEnd_A_dap.Value = dateEnd;
                g_DBBiz_A_dap.SetProcedureParameters(pramDateEnd_A_dap);
                SqlParameter pramPortIds_A_dap = new SqlParameter();
                pramPortIds_A_dap = new SqlParameter();
                pramPortIds_A_dap.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_A_dap.ParameterName = "@m_portlist";
                pramPortIds_A_dap.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_A_dap.SetProcedureParameters(pramPortIds_A_dap);

                g_DBBiz_W_dp.ClearParameters();
                SqlParameter pramDateStart_W_dp = new SqlParameter();
                pramDateStart_W_dp = new SqlParameter();
                pramDateStart_W_dp.SqlDbType = SqlDbType.DateTime;
                pramDateStart_W_dp.ParameterName = "@m_begin";
                pramDateStart_W_dp.Value = dateStart;
                g_DBBiz_W_dp.SetProcedureParameters(pramDateStart_W_dp);
                SqlParameter pramDateEnd_W_dp = new SqlParameter();
                pramDateEnd_W_dp = new SqlParameter();
                pramDateEnd_W_dp.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_W_dp.ParameterName = "@m_end";
                pramDateEnd_W_dp.Value = dateEnd;
                g_DBBiz_W_dp.SetProcedureParameters(pramDateEnd_W_dp);
                SqlParameter pramPortIds_W_dp = new SqlParameter();
                pramPortIds_W_dp = new SqlParameter();
                pramPortIds_W_dp.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_W_dp.ParameterName = "@m_portlist";
                pramPortIds_W_dp.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_W_dp.SetProcedureParameters(pramPortIds_W_dp);

                if (applicationType == ApplicationType.Air)
                {
                    //生成小时、日、周、月、季、年数据
                    //日
                    g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Day_Port_Mul", connection);
                    ////周
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Week_Port_Mul", connection);
                    ////月
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Month_Port_Mul", connection);
                    ////季
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Season_Port_Mul", connection);
                    ////年
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Year_Port_Mul", connection);
                    //生成AQI数据
                    g_DBBiz_A_hap.ExecuteProcNonQuery("UP_AirReport_HourAQI_Port_Mul", connection);
                    g_DBBiz_A_dap.ExecuteProcNonQuery("UP_AirReport_DayAQI_Port_Mul", connection);
                    //try
                    //{
                    //    //新增周报
                    //    g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Week_Port_Mul_New", connection);
                    //}
                    //catch
                    //{
                    //}
                }
                else
                {
                    //生成日、周、月、季、年数据
                    //日
                    g_DBBiz_W_dp.ExecuteProcNonQuery("UP_WaterReport_Day_Port_Mul", connection);
                    ////周
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Week_Port_Mul", connection);
                    ////月
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Month_Port_Mul", connection);
                    ////季
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Season_Port_Mul", connection);
                    ////年
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Year_Port_Mul", connection);

                    //try
                    //{
                    //    //新增日报
                    //    g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Day_Port_Mul_SZ", connection);
                    //}
                    //catch
                    //{
                    //}
                }
                //数据有效率生成
                //SetReportQualifiedRateByDay(applicationType, portIds, dateStart, dateEnd, out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 从审核后日数据生成审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="portIds">站点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateDataFromDayReport(ApplicationType applicationType, string[] portIds, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            dateStart = DateTime.Parse(dateStart.ToString("yyyy-MM-dd 00:00:00"));
            if (applicationType == ApplicationType.Air)
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:00:00"));
            else
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:59:59.998"));
            try
            {
                if (portIds == null || portIds.Length == 0)
                    return false;
                g_DBBiz.ClearParameters();
                SqlParameter pramDateStart = new SqlParameter();
                pramDateStart = new SqlParameter();
                pramDateStart.SqlDbType = SqlDbType.DateTime;
                pramDateStart.ParameterName = "@m_begin";
                pramDateStart.Value = dateStart;
                g_DBBiz.SetProcedureParameters(pramDateStart);
                SqlParameter pramDateEnd = new SqlParameter();
                pramDateEnd = new SqlParameter();
                pramDateEnd.SqlDbType = SqlDbType.DateTime;
                pramDateEnd.ParameterName = "@m_end";
                pramDateEnd.Value = dateEnd;
                g_DBBiz.SetProcedureParameters(pramDateEnd);
                SqlParameter pramPortIds = new SqlParameter();
                pramPortIds = new SqlParameter();
                pramPortIds.SqlDbType = SqlDbType.NVarChar;
                pramPortIds.ParameterName = "@m_portlist";
                pramPortIds.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz.SetProcedureParameters(pramPortIds);

                g_DBBiz_A_dp.ClearParameters();
                SqlParameter pramDateStart_A_dp = new SqlParameter();
                pramDateStart_A_dp = new SqlParameter();
                pramDateStart_A_dp.SqlDbType = SqlDbType.DateTime;
                pramDateStart_A_dp.ParameterName = "@m_begin";
                pramDateStart_A_dp.Value = dateStart;
                g_DBBiz_A_dp.SetProcedureParameters(pramDateStart_A_dp);
                SqlParameter pramDateEnd_A_dp = new SqlParameter();
                pramDateEnd_A_dp = new SqlParameter();
                pramDateEnd_A_dp.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_A_dp.ParameterName = "@m_end";
                pramDateEnd_A_dp.Value = dateEnd;
                g_DBBiz_A_dp.SetProcedureParameters(pramDateEnd_A_dp);
                SqlParameter pramPortIds_A_dp = new SqlParameter();
                pramPortIds_A_dp = new SqlParameter();
                pramPortIds_A_dp.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_A_dp.ParameterName = "@m_portlist";
                pramPortIds_A_dp.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_A_dp.SetProcedureParameters(pramPortIds_A_dp);

                if (applicationType == ApplicationType.Air)
                {
                    //生成小时、日、周、月、季、年数据
                    ////周
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Week_Port_Mul", connection);
                    ////月
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Month_Port_Mul", connection);
                    ////季
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Season_Port_Mul", connection);
                    ////年
                    //g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Year_Port_Mul", connection);
                    //生成AQI数据
                    g_DBBiz.ExecuteProcNonQuery("UP_AirReport_HourAQI_Port_Mul", connection);
                    g_DBBiz_A_dp.ExecuteProcNonQuery("UP_AirReport_DayAQI_Port_Mul", connection);
                    //try
                    //{
                    //    //新增周报
                    //    g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Week_Port_Mul_New", connection);
                    //}
                    //catch
                    //{
                    //}
                }
                else
                {
                    //生成日、周、月、季、年数据
                    ////周
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Week_Port_Mul", connection);
                    ////月
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Month_Port_Mul", connection);
                    ////季
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Season_Port_Mul", connection);
                    ////年
                    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Year_Port_Mul", connection);

                    //try
                    //{
                    //    ////新增日报
                    //    //g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Day_Port_Mul_SZ", connection);
                    //}
                    //catch
                    //{
                    //}
                }
                //数据有效率生成
                //SetReportQualifiedRateByDay(applicationType, portIds, dateStart, dateEnd, out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 生成数据有效率
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="portIds">站点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool SetReportQualifiedRateByDay(ApplicationType applicationType, string[] portIds, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            dateStart = DateTime.Parse(dateStart.ToString("yyyy-MM-dd 00:00:00"));
            if (applicationType == ApplicationType.Air)
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:00:00"));
            else
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:59:59.998"));
            try
            {
                if (portIds == null || portIds.Length == 0)
                    return false;
                g_DBBiz.ClearParameters();
                SqlParameter pramDateStart = new SqlParameter();
                pramDateStart = new SqlParameter();
                pramDateStart.SqlDbType = SqlDbType.DateTime;
                pramDateStart.ParameterName = "@m_begin";
                pramDateStart.Value = dateStart;
                g_DBBiz.SetProcedureParameters(pramDateStart);
                SqlParameter pramDateEnd = new SqlParameter();
                pramDateEnd = new SqlParameter();
                pramDateEnd.SqlDbType = SqlDbType.DateTime;
                pramDateEnd.ParameterName = "@m_end";
                pramDateEnd.Value = dateEnd;
                g_DBBiz.SetProcedureParameters(pramDateEnd);
                SqlParameter pramPortIds = new SqlParameter();
                pramPortIds = new SqlParameter();
                pramPortIds.SqlDbType = SqlDbType.NVarChar;
                pramPortIds.ParameterName = "@m_portlist";
                pramPortIds.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz.SetProcedureParameters(pramPortIds);
                SqlParameter pramApplicationType = new SqlParameter();
                pramApplicationType = new SqlParameter();
                pramApplicationType.SqlDbType = SqlDbType.NVarChar;
                pramApplicationType.ParameterName = "@applicationType";

                g_DBBiz_A_dp.ClearParameters();
                SqlParameter pramDateStart_A_dp = new SqlParameter();
                pramDateStart_A_dp = new SqlParameter();
                pramDateStart_A_dp.SqlDbType = SqlDbType.DateTime;
                pramDateStart_A_dp.ParameterName = "@m_begin";
                pramDateStart_A_dp.Value = dateStart;
                g_DBBiz_A_dp.SetProcedureParameters(pramDateStart_A_dp);
                SqlParameter pramDateEnd_A_dp = new SqlParameter();
                pramDateEnd_A_dp = new SqlParameter();
                pramDateEnd_A_dp.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_A_dp.ParameterName = "@m_end";
                pramDateEnd_A_dp.Value = dateEnd;
                g_DBBiz_A_dp.SetProcedureParameters(pramDateEnd_A_dp);
                SqlParameter pramPortIds_A_dp = new SqlParameter();
                pramPortIds_A_dp = new SqlParameter();
                pramPortIds_A_dp.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_A_dp.ParameterName = "@m_portlist";
                pramPortIds_A_dp.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                g_DBBiz_A_dp.SetProcedureParameters(pramPortIds_A_dp);
                SqlParameter pramApplicationType_A_dp = new SqlParameter();
                pramApplicationType_A_dp = new SqlParameter();
                pramApplicationType_A_dp.SqlDbType = SqlDbType.NVarChar;
                pramApplicationType_A_dp.ParameterName = "@applicationType";

                if (applicationType == ApplicationType.Air)
                {
                    pramApplicationType.Value = "Air";
                    g_DBBiz.SetProcedureParameters(pramApplicationType);
                    g_DBBiz.ExecuteProcNonQuery("UP_ReportQualifiedRateByDay", connection);
                }
                else
                {
                    pramApplicationType_A_dp.Value = "Water";
                    g_DBBiz_A_dp.SetProcedureParameters(pramApplicationType_A_dp);
                    g_DBBiz_A_dp.ExecuteProcNonQuery("UP_ReportQualifiedRateByDay", connection);
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 生成区域审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="regionGuids">站点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateDataRegion(ApplicationType applicationType, string[] regionGuids, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            dateStart = DateTime.Parse(dateStart.ToString("yyyy-MM-dd 00:00:00"));
            if (applicationType == ApplicationType.Air)
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:00:00"));
            else
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:59:59.998"));
            try
            {
                if (regionGuids == null || regionGuids.Length == 0)
                    return false;
                g_DBBiz.ClearParameters();
                SqlParameter pramDateStart = new SqlParameter();
                pramDateStart = new SqlParameter();
                pramDateStart.SqlDbType = SqlDbType.DateTime;
                pramDateStart.ParameterName = "@m_begin";
                pramDateStart.Value = dateStart;
                g_DBBiz.SetProcedureParameters(pramDateStart);
                SqlParameter pramDateEnd = new SqlParameter();
                pramDateEnd = new SqlParameter();
                pramDateEnd.SqlDbType = SqlDbType.DateTime;
                pramDateEnd.ParameterName = "@m_end";
                pramDateEnd.Value = dateEnd;
                g_DBBiz.SetProcedureParameters(pramDateEnd);
                string strRegion = string.Empty;
                foreach (string regionGuid in regionGuids)
                {
                    if (strRegion.IndexOf(regionGuid) < 0)
                        strRegion += "," + regionGuid;
                }
                strRegion = strRegion.Trim(',');
                SqlParameter pramPortIds = new SqlParameter();
                pramPortIds = new SqlParameter();
                pramPortIds.SqlDbType = SqlDbType.NVarChar;
                pramPortIds.ParameterName = "@m_Regionlist";
                pramPortIds.Value = strRegion; //StringExtensions.GetArrayStrNoEmpty(regionGuids.ToList<string>(), ",");
                g_DBBiz.SetProcedureParameters(pramPortIds);

                g_DBBiz_A_dap.ClearParameters();
                SqlParameter pramDateStart_A_dap = new SqlParameter();
                pramDateStart_A_dap = new SqlParameter();
                pramDateStart_A_dap.SqlDbType = SqlDbType.DateTime;
                pramDateStart_A_dap.ParameterName = "@m_begin";
                pramDateStart_A_dap.Value = dateStart;
                g_DBBiz_A_dap.SetProcedureParameters(pramDateStart_A_dap);
                SqlParameter pramDateEnd_A_dap = new SqlParameter();
                pramDateEnd_A_dap = new SqlParameter();
                pramDateEnd_A_dap.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_A_dap.ParameterName = "@m_end";
                pramDateEnd_A_dap.Value = dateEnd;
                g_DBBiz_A_dap.SetProcedureParameters(pramDateEnd_A_dap);
                string strRegion_A_dap = string.Empty;
                foreach (string regionGuid in regionGuids)
                {
                    if (strRegion_A_dap.IndexOf(regionGuid) < 0)
                        strRegion_A_dap += "," + regionGuid;
                }
                strRegion_A_dap = strRegion_A_dap.Trim(',');
                SqlParameter pramPortIds_A_dap = new SqlParameter();
                pramPortIds_A_dap = new SqlParameter();
                pramPortIds_A_dap.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_A_dap.ParameterName = "@m_Regionlist";
                pramPortIds_A_dap.Value = strRegion_A_dap; //StringExtensions.GetArrayStrNoEmpty(regionGuids.ToList<string>(), ",");
                g_DBBiz_A_dap.SetProcedureParameters(pramPortIds_A_dap);

                g_DBBiz_A_hap.ClearParameters();
                SqlParameter pramDateStart_A_hap = new SqlParameter();
                pramDateStart_A_hap = new SqlParameter();
                pramDateStart_A_hap.SqlDbType = SqlDbType.DateTime;
                pramDateStart_A_hap.ParameterName = "@m_begin";
                pramDateStart_A_hap.Value = dateStart;
                g_DBBiz_A_hap.SetProcedureParameters(pramDateStart_A_hap);
                SqlParameter pramDateEnd_A_hap = new SqlParameter();
                pramDateEnd_A_hap = new SqlParameter();
                pramDateEnd_A_hap.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_A_hap.ParameterName = "@m_end";
                pramDateEnd_A_hap.Value = dateEnd;
                g_DBBiz_A_hap.SetProcedureParameters(pramDateEnd_A_hap);
                string strRegion_A_hap = string.Empty;
                foreach (string regionGuid in regionGuids)
                {
                    if (strRegion_A_hap.IndexOf(regionGuid) < 0)
                        strRegion_A_hap += "," + regionGuid;
                }
                strRegion_A_hap = strRegion_A_hap.Trim(',');
                SqlParameter pramPortIds_A_hap = new SqlParameter();
                pramPortIds_A_hap = new SqlParameter();
                pramPortIds_A_hap.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_A_hap.ParameterName = "@m_Regionlist";
                pramPortIds_A_hap.Value = strRegion_A_hap; //StringExtensions.GetArrayStrNoEmpty(regionGuids.ToList<string>(), ",");
                g_DBBiz_A_hap.SetProcedureParameters(pramPortIds_A_hap);

                g_DBBiz_A_dp.ClearParameters();
                SqlParameter pramDateStart_A_dp = new SqlParameter();
                pramDateStart_A_dp = new SqlParameter();
                pramDateStart_A_dp.SqlDbType = SqlDbType.DateTime;
                pramDateStart_A_dp.ParameterName = "@m_begin";
                pramDateStart_A_dp.Value = dateStart;
                g_DBBiz_A_dp.SetProcedureParameters(pramDateStart_A_dp);
                SqlParameter pramDateEnd_A_dp = new SqlParameter();
                pramDateEnd_A_dp = new SqlParameter();
                pramDateEnd_A_dp.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_A_dp.ParameterName = "@m_end";
                pramDateEnd_A_dp.Value = dateEnd;
                g_DBBiz_A_dp.SetProcedureParameters(pramDateEnd_A_dp);
                string strRegion_A_dp = string.Empty;
                foreach (string regionGuid in regionGuids)
                {
                    if (strRegion_A_dp.IndexOf(regionGuid) < 0)
                        strRegion_A_dp += "," + regionGuid;
                }
                strRegion_A_dp = strRegion_A_dp.Trim(',');
                SqlParameter pramPortIds_A_dp = new SqlParameter();
                pramPortIds_A_dp = new SqlParameter();
                pramPortIds_A_dp.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_A_dp.ParameterName = "@m_Regionlist";
                pramPortIds_A_dp.Value = strRegion_A_dp; //StringExtensions.GetArrayStrNoEmpty(regionGuids.ToList<string>(), ",");
                g_DBBiz_A_dp.SetProcedureParameters(pramPortIds_A_dp);

                if (applicationType == ApplicationType.Air)
                {
                    //区域小时AQI数据
                    g_DBBiz.ExecuteProcNonQuery("UP_AirReport_HourAQI_Region", connection);
                    //区域日AQI数据
                    g_DBBiz_A_dap.ExecuteProcNonQuery("UP_AirReport_DayAQI_Region", connection);
                    //大市区域小时AQI数据
                    g_DBBiz_A_hap.ExecuteProcNonQuery("UP_AirReport_HourAQI_DaShi", connection);
                    //大市区域日AQI数据
                    g_DBBiz_A_dp.ExecuteProcNonQuery("UP_AirReport_DayAQI_DaShi", connection);
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 生成区域审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="regionGuids">站点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateDataRegionFromDay(ApplicationType applicationType, string[] regionGuids, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            dateStart = DateTime.Parse(dateStart.ToString("yyyy-MM-dd 00:00:00"));
            if (applicationType == ApplicationType.Air)
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:00:00"));
            else
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:59:59.998"));
            try
            {
                if (regionGuids == null || regionGuids.Length == 0)
                    return false;
                g_DBBiz.ClearParameters();
                SqlParameter pramDateStart = new SqlParameter();
                pramDateStart = new SqlParameter();
                pramDateStart.SqlDbType = SqlDbType.DateTime;
                pramDateStart.ParameterName = "@m_begin";
                pramDateStart.Value = dateStart;
                g_DBBiz.SetProcedureParameters(pramDateStart);
                SqlParameter pramDateEnd = new SqlParameter();
                pramDateEnd = new SqlParameter();
                pramDateEnd.SqlDbType = SqlDbType.DateTime;
                pramDateEnd.ParameterName = "@m_end";
                pramDateEnd.Value = dateEnd;
                g_DBBiz.SetProcedureParameters(pramDateEnd);
                string strRegion = string.Empty;
                foreach (string regionGuid in regionGuids)
                {
                    if (strRegion.IndexOf(regionGuid) < 0)
                        strRegion += "," + regionGuid;
                }
                strRegion = strRegion.Trim(',');
                SqlParameter pramPortIds = new SqlParameter();
                pramPortIds = new SqlParameter();
                pramPortIds.SqlDbType = SqlDbType.NVarChar;
                pramPortIds.ParameterName = "@m_Regionlist";
                pramPortIds.Value = strRegion; //StringExtensions.GetArrayStrNoEmpty(regionGuids.ToList<string>(), ",");
                g_DBBiz.SetProcedureParameters(pramPortIds);

                g_DBBiz_A_dap.ClearParameters();
                SqlParameter pramDateStart_A_dap = new SqlParameter();
                pramDateStart_A_dap = new SqlParameter();
                pramDateStart_A_dap.SqlDbType = SqlDbType.DateTime;
                pramDateStart_A_dap.ParameterName = "@m_begin";
                pramDateStart_A_dap.Value = dateStart;
                g_DBBiz_A_dap.SetProcedureParameters(pramDateStart_A_dap);
                SqlParameter pramDateEnd_A_dap = new SqlParameter();
                pramDateEnd_A_dap = new SqlParameter();
                pramDateEnd_A_dap.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_A_dap.ParameterName = "@m_end";
                pramDateEnd_A_dap.Value = dateEnd;
                g_DBBiz_A_dap.SetProcedureParameters(pramDateEnd_A_dap);
                string strRegion_A_dap = string.Empty;
                foreach (string regionGuid in regionGuids)
                {
                    if (strRegion_A_dap.IndexOf(regionGuid) < 0)
                        strRegion_A_dap += "," + regionGuid;
                }
                strRegion_A_dap = strRegion_A_dap.Trim(',');
                SqlParameter pramPortIds_A_dap = new SqlParameter();
                pramPortIds_A_dap = new SqlParameter();
                pramPortIds_A_dap.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_A_dap.ParameterName = "@m_Regionlist";
                pramPortIds_A_dap.Value = strRegion_A_dap; //StringExtensions.GetArrayStrNoEmpty(regionGuids.ToList<string>(), ",");
                g_DBBiz_A_dap.SetProcedureParameters(pramPortIds_A_dap);

                if (applicationType == ApplicationType.Air)
                {
                    //区域日AQI数据
                    g_DBBiz.ExecuteProcNonQuery("UP_AirReport_DayAQI_Region", connection);
                    //大市区域日AQI数据
                    g_DBBiz_A_dap.ExecuteProcNonQuery("UP_AirReport_DayAQI_DaShi", connection);
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 生成城市审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="cityGuids">站点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateDataCity(ApplicationType applicationType, string[] cityGuids, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            dateStart = DateTime.Parse(dateStart.ToString("yyyy-MM-dd 00:00:00"));
            if (applicationType == ApplicationType.Air)
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:00:00"));
            else
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:59:59.998"));
            try
            {
                if (cityGuids == null || cityGuids.Length == 0)
                    return false;
                g_DBBiz.ClearParameters();
                SqlParameter pramDateStart = new SqlParameter();
                pramDateStart = new SqlParameter();
                pramDateStart.SqlDbType = SqlDbType.DateTime;
                pramDateStart.ParameterName = "@m_begin";
                pramDateStart.Value = dateStart;
                g_DBBiz.SetProcedureParameters(pramDateStart);
                SqlParameter pramDateEnd = new SqlParameter();
                pramDateEnd = new SqlParameter();
                pramDateEnd.SqlDbType = SqlDbType.DateTime;
                pramDateEnd.ParameterName = "@m_end";
                pramDateEnd.Value = dateEnd;
                g_DBBiz.SetProcedureParameters(pramDateEnd);
                string strCity = string.Empty;
                foreach (string cityGuid in cityGuids)
                {
                    if (strCity.IndexOf(cityGuid) < 0)
                        strCity += "," + cityGuid;
                }
                strCity = strCity.Trim(',');
                SqlParameter pramPortIds = new SqlParameter();
                pramPortIds = new SqlParameter();
                pramPortIds.SqlDbType = SqlDbType.NVarChar;
                pramPortIds.ParameterName = "@m_Regionlist";
                pramPortIds.Value = strCity;//StringExtensions.GetArrayStrNoEmpty(cityGuids.ToList<string>(), ",");
                g_DBBiz.SetProcedureParameters(pramPortIds);

                g_DBBiz_A_dap.ClearParameters();
                SqlParameter pramDateStart_A_dap = new SqlParameter();
                pramDateStart_A_dap = new SqlParameter();
                pramDateStart_A_dap.SqlDbType = SqlDbType.DateTime;
                pramDateStart_A_dap.ParameterName = "@m_begin";
                pramDateStart_A_dap.Value = dateStart;
                g_DBBiz_A_dap.SetProcedureParameters(pramDateStart_A_dap);
                SqlParameter pramDateEnd_A_dap = new SqlParameter();
                pramDateEnd_A_dap = new SqlParameter();
                pramDateEnd_A_dap.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_A_dap.ParameterName = "@m_end";
                pramDateEnd_A_dap.Value = dateEnd;
                g_DBBiz_A_dap.SetProcedureParameters(pramDateEnd_A_dap);
                string strCity_A_dap = string.Empty;
                foreach (string cityGuid in cityGuids)
                {
                    if (strCity_A_dap.IndexOf(cityGuid) < 0)
                        strCity_A_dap += "," + cityGuid;
                }
                strCity_A_dap = strCity_A_dap.Trim(',');
                SqlParameter pramPortIds_A_dap = new SqlParameter();
                pramPortIds_A_dap = new SqlParameter();
                pramPortIds_A_dap.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_A_dap.ParameterName = "@m_Regionlist";
                pramPortIds_A_dap.Value = strCity_A_dap;//StringExtensions.GetArrayStrNoEmpty(cityGuids.ToList<string>(), ",");
                g_DBBiz_A_dap.SetProcedureParameters(pramPortIds_A_dap);

                g_DBBiz_A_hap.ClearParameters();
                SqlParameter pramDateStart_A_hap = new SqlParameter();
                pramDateStart_A_hap = new SqlParameter();
                pramDateStart_A_hap.SqlDbType = SqlDbType.DateTime;
                pramDateStart_A_hap.ParameterName = "@m_begin";
                pramDateStart_A_hap.Value = dateStart;
                g_DBBiz_A_hap.SetProcedureParameters(pramDateStart_A_hap);
                SqlParameter pramDateEnd_A_hap = new SqlParameter();
                pramDateEnd_A_hap = new SqlParameter();
                pramDateEnd_A_hap.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_A_hap.ParameterName = "@m_end";
                pramDateEnd_A_hap.Value = dateEnd;
                g_DBBiz_A_hap.SetProcedureParameters(pramDateEnd_A_hap);
                string strCity_A_hap = string.Empty;
                foreach (string cityGuid in cityGuids)
                {
                    if (strCity_A_hap.IndexOf(cityGuid) < 0)
                        strCity_A_hap += "," + cityGuid;
                }
                strCity_A_hap = strCity_A_hap.Trim(',');
                SqlParameter pramPortIds_A_hap = new SqlParameter();
                pramPortIds_A_hap = new SqlParameter();
                pramPortIds_A_hap.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_A_hap.ParameterName = "@m_Regionlist";
                pramPortIds_A_hap.Value = strCity_A_hap;//StringExtensions.GetArrayStrNoEmpty(cityGuids.ToList<string>(), ",");
                g_DBBiz_A_hap.SetProcedureParameters(pramPortIds_A_hap);

                g_DBBiz_A_dp.ClearParameters();
                SqlParameter pramDateStart_A_dp = new SqlParameter();
                pramDateStart_A_dp = new SqlParameter();
                pramDateStart_A_dp.SqlDbType = SqlDbType.DateTime;
                pramDateStart_A_dp.ParameterName = "@m_begin";
                pramDateStart_A_dp.Value = dateStart;
                g_DBBiz_A_dp.SetProcedureParameters(pramDateStart_A_dp);
                SqlParameter pramDateEnd_A_dp = new SqlParameter();
                pramDateEnd_A_dp = new SqlParameter();
                pramDateEnd_A_dp.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_A_dp.ParameterName = "@m_end";
                pramDateEnd_A_dp.Value = dateEnd;
                g_DBBiz_A_dp.SetProcedureParameters(pramDateEnd_A_dp);
                string strCity_A_dp = string.Empty;
                foreach (string cityGuid in cityGuids)
                {
                    if (strCity_A_dp.IndexOf(cityGuid) < 0)
                        strCity_A_dp += "," + cityGuid;
                }
                strCity_A_dp = strCity_A_dp.Trim(',');
                SqlParameter pramPortIds_A_dp = new SqlParameter();
                pramPortIds_A_dp = new SqlParameter();
                pramPortIds_A_dp.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_A_dp.ParameterName = "@m_Regionlist";
                pramPortIds_A_dp.Value = strCity_A_dp;//StringExtensions.GetArrayStrNoEmpty(cityGuids.ToList<string>(), ",");
                g_DBBiz_A_dp.SetProcedureParameters(pramPortIds_A_dp);

                if (applicationType == ApplicationType.Air)
                {
                    //区域小时AQI数据
                    g_DBBiz.ExecuteProcNonQuery("UP_AirReport_HourAQI_City", connection);
                    //区域日AQI数据
                    g_DBBiz_A_dap.ExecuteProcNonQuery("UP_AirReport_DayAQI_City", connection);
                    //大市区域小时AQI数据
                    g_DBBiz_A_hap.ExecuteProcNonQuery("UP_AirReport_HourAQI_DaShi", connection);
                    //大市区域日AQI数据
                    g_DBBiz_A_dp.ExecuteProcNonQuery("UP_AirReport_DayAQI_DaShi", connection);
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 生成城市审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="cityGuids">站点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateDataCityFromDay(ApplicationType applicationType, string[] cityGuids, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            dateStart = DateTime.Parse(dateStart.ToString("yyyy-MM-dd 00:00:00"));
            if (applicationType == ApplicationType.Air)
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:00:00"));
            else
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:59:59.998"));
            try
            {
                if (cityGuids == null || cityGuids.Length == 0)
                    return false;
                g_DBBiz.ClearParameters();
                SqlParameter pramDateStart = new SqlParameter();
                pramDateStart = new SqlParameter();
                pramDateStart.SqlDbType = SqlDbType.DateTime;
                pramDateStart.ParameterName = "@m_begin";
                pramDateStart.Value = dateStart;
                g_DBBiz.SetProcedureParameters(pramDateStart);
                SqlParameter pramDateEnd = new SqlParameter();
                pramDateEnd = new SqlParameter();
                pramDateEnd.SqlDbType = SqlDbType.DateTime;
                pramDateEnd.ParameterName = "@m_end";
                pramDateEnd.Value = dateEnd;
                g_DBBiz.SetProcedureParameters(pramDateEnd);
                string strCity = string.Empty;
                foreach (string cityGuid in cityGuids)
                {
                    if (strCity.IndexOf(cityGuid) < 0)
                        strCity += "," + cityGuid;
                }
                strCity = strCity.Trim(',');
                SqlParameter pramPortIds = new SqlParameter();
                pramPortIds = new SqlParameter();
                pramPortIds.SqlDbType = SqlDbType.NVarChar;
                pramPortIds.ParameterName = "@m_Regionlist";
                pramPortIds.Value = strCity;//StringExtensions.GetArrayStrNoEmpty(cityGuids.ToList<string>(), ",");
                g_DBBiz.SetProcedureParameters(pramPortIds);

                g_DBBiz_A_dap.ClearParameters();
                SqlParameter pramDateStart_A_dap = new SqlParameter();
                pramDateStart_A_dap = new SqlParameter();
                pramDateStart_A_dap.SqlDbType = SqlDbType.DateTime;
                pramDateStart_A_dap.ParameterName = "@m_begin";
                pramDateStart_A_dap.Value = dateStart;
                g_DBBiz_A_dap.SetProcedureParameters(pramDateStart_A_dap);
                SqlParameter pramDateEnd_A_dap = new SqlParameter();
                pramDateEnd_A_dap = new SqlParameter();
                pramDateEnd_A_dap.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_A_dap.ParameterName = "@m_end";
                pramDateEnd_A_dap.Value = dateEnd;
                g_DBBiz_A_dap.SetProcedureParameters(pramDateEnd_A_dap);
                string strCity_A_dap = string.Empty;
                foreach (string cityGuid in cityGuids)
                {
                    if (strCity_A_dap.IndexOf(cityGuid) < 0)
                        strCity_A_dap += "," + cityGuid;
                }
                strCity_A_dap = strCity_A_dap.Trim(',');
                SqlParameter pramPortIds_A_dap = new SqlParameter();
                pramPortIds_A_dap = new SqlParameter();
                pramPortIds_A_dap.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_A_dap.ParameterName = "@m_Regionlist";
                pramPortIds_A_dap.Value = strCity_A_dap;//StringExtensions.GetArrayStrNoEmpty(cityGuids.ToList<string>(), ",");
                g_DBBiz_A_dap.SetProcedureParameters(pramPortIds_A_dap);

                if (applicationType == ApplicationType.Air)
                {
                    //区域日AQI数据
                    g_DBBiz.ExecuteProcNonQuery("UP_AirReport_DayAQI_City", connection);
                    //大市区域日AQI数据
                    g_DBBiz_A_dap.ExecuteProcNonQuery("UP_AirReport_DayAQI_DaShi", connection);
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 取得苏州上报日报数据
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public DataView GetAQIDataSZ(DateTime datetime)
        {
            string sql = string.Format(@"
                SELECT [STCODE]
                    ,STNAME
                    ,YEAR([DateTime]) AS [YYYY]
                    ,MONTH([DateTime]) AS [MM]
                    ,DAY([DateTime]) AS [DD]
                    ,[DWCODE]
                    ,port.MonitoringPointName AS [DWNAME]
                    ,[AQIValue] AS [AQI]
                    ,REPLACE(REPLACE(REPLACE([PrimaryPollutant],'-1',''),'-8',''),'O3,O3','O3') AS [SYWRW]
                    ,[Class] AS [KQZLZSJB]
                    ,dbo.F_GetAQI_Grade(AQIValue,'ROMAN') AS [LB] 
                 FROM AirRelease.TB_DayAQI AS data 
                 INNER JOIN 
                 (
	                 SELECT smp.PointId
		                 ,smp.MonitoringPointName 
		                 ,smpExt.stname
		                 ,smpExt.stcode
		                 ,dwcode
	                 FROM dbo.SY_MonitoringPoint AS smp
	                 LEFT JOIN dbo.SY_MonitoringPointExtensionForEQMSAir AS smpExt
		                ON smp.MonitoringPointUid = smpExt.MonitoringPointUid
	                 WHERE smpExt.StateUploadOrNot = 1
                 ) AS port  
                 ON data.PointId=port.PointId
                 WHERE [DateTime]=CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{0}',120),120)
                 ORDER BY [DateTime],[DWCODE] ", datetime.ToString("yyy-MM-dd 00:00:00"));

            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CurrentDT"></param>
        /// <param name="portIds"></param>
        /// <returns></returns>
        public DataView getDataViewJS(DateTime CurrentDT, string[] portIds)
        {
            string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
            String Sql = "";
            #region << 废旧 >>
            //            Sql = String.Format(@"
            //               SELECT 
            //	                a.STNAME
            //                    ,a.STCODE
            //	                ,p.monitoringpointname as DWNAME
            //                    ,a.DWCODE
            //	                ,YEAR([tstamp]) AS [YYYY]
            //	                ,MONTH([tstamp]) AS [MM]
            //	                ,DAY([tstamp]) AS [DD]
            //	                ,datepart(hour,[tstamp])+1 AS [HH]
            //			,MAX(CASE WHEN PollutantCode='a21026' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'SO2'
            //			,MAX(CASE WHEN PollutantCode='a21003' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'NO'
            //			,MAX(CASE WHEN PollutantCode='a21004' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'NO2'
            //			,MAX(CASE WHEN PollutantCode='a21002' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'NOX'
            //			,MAX(CASE WHEN PollutantCode='a34002' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'PM10'
            //			,MAX(CASE WHEN PollutantCode='a21005' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'CO'
            //			,MAX(CASE WHEN PollutantCode='a05024' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'O3'
            //			,MAX(CASE WHEN PollutantCode='a34004' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'PM25'
            //			,MAX(CASE WHEN PollutantCode='a01007' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),1) END) AS 'WS'
            //			,MAX(CASE WHEN PollutantCode='a01008' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),1) END) AS 'WD'
            //			,MAX(CASE WHEN PollutantCode='a01001' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),1) END) AS 'TEMP'
            //			,MAX(CASE WHEN PollutantCode='a01002' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),0) END) AS 'RH'
            //			,MAX(CASE WHEN PollutantCode='a01006' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),1) END) AS 'PRESS'
            //			,MAX(CASE WHEN PollutantCode='a01020' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),0) END) AS 'VISIBILITY'
            //		FROM AirReport.TB_HourReport as h
            //		left join dbo.SY_MonitoringPoint as p  on h.PointId = p.PointId
            //        left join dbo.SY_MonitoringPointExtensionForEQMSAir as a  on a.MonitoringPointUid = p.MonitoringPointUid
            //		where Tstamp >= '{0}' and Tstamp <= '{1}'
            //        --and p.sitetypeuid = '2efcf99d-ec8c-4e09-8f2d-ed74fa82c71b' 
            //            and a.StateUploadOrNot = 1
            //		GROUP BY p.monitoringpointname,Tstamp,a.STNAME,a.STCODE,a.DWCODE
            //        order by a.DWCODE,Tstamp", CurrentDT, CurrentDT.AddHours(23));
            #endregion
            Sql = String.Format(@"
                    SELECT allData.PointId
                        ,allData.Tstamp
                        ,a.STNAME
                        ,a.STCODE
                        ,'苏州'+p.monitoringpointname+'监测站' as DWNAME
                        ,a.DWCODE
                        ,YEAR(allData.[tstamp]) AS [YYYY]
                        ,MONTH(allData.[tstamp]) AS [MM]
                        ,DAY(allData.[tstamp]) AS [DD]
                        ,datepart(hour,allData.[tstamp])+1 AS [HH]
                        ,MAX(CASE WHEN PollutantCode='a21026'   THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'SO2'
                        ,MAX(CASE WHEN PollutantCode='a21003'   THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'NO'
                        ,MAX(CASE WHEN PollutantCode='a21004'   THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'NO2'
                        ,MAX(CASE WHEN PollutantCode='a21002'   THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'NOX'
                        ,MAX(CASE WHEN PollutantCode='a34002'   THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'PM10'
                        ,MAX(CASE WHEN PollutantCode='a21005'   THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'CO'
                        ,MAX(CASE WHEN PollutantCode='a05024'   THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'O3'
                        ,MAX(CASE WHEN PollutantCode='a34004'   THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'PM25'
                        ,MAX(CASE WHEN PollutantCode='a01007'   THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),1) END) AS 'WS'
                        ,MAX(CASE WHEN PollutantCode='a01008'   THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),1) END) AS 'WD'
                        ,MAX(CASE WHEN PollutantCode='a01001'   THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),1) END) AS 'TEMP'
                        ,MAX(CASE WHEN PollutantCode='a01002'   THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),0) END) AS 'RH'
                        ,MAX(CASE WHEN PollutantCode='a01006'   THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),1) END) AS 'PRESS'
                        ,MAX(CASE WHEN PollutantCode='a01020'   THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),0) END) AS 'VISIBILITY'
                    FROM dbo.F_GetAllDataByHour('{0}',',','{1}','{2}') AS allData
                    LEFT JOIN AirReport.TB_HourReport as h
	                    ON allData.PointID = h.PointId AND allData.tstamp = h.Tstamp
                    left join dbo.SY_MonitoringPoint as p  on allData.PointId = p.PointId
                    left join dbo.SY_MonitoringPointExtensionForEQMSAir as a  on a.MonitoringPointUid = p.MonitoringPointUid
                    where a.StateUploadOrNot = 1 
                        and p.applicationUid = 'airaaira-aira-aira-aira-airaairaaira'
                    GROUP BY allData.PointId,p.monitoringpointname,allData.Tstamp,a.STNAME,a.STCODE,a.DWCODE
                    order by DWCODE,allData.Tstamp
                ", portIdsStr, CurrentDT, CurrentDT.AddHours(23));
            return CreatDataView(Sql, connection);
        }

        public DataView getDataViewXJS(DateTime CurrentDT, string[] portIds)
        {
            string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
            String Sql = "";
            #region << 废弃 >>
            //            Sql = String.Format(@"
            //               SELECT 
            //					a.STNAME
            //                    ,a.STCODE
            //	                ,p.monitoringpointname as DWNAME
            //                    ,a.DWCODE
            //	                ,YEAR([tstamp]) AS [YYYY]
            //	                ,MONTH([tstamp]) AS [MM]
            //	                ,DAY([tstamp]) AS [DD]
            //	                ,datepart(hour,[tstamp])+1 AS [HH]
            //			,MAX(CASE WHEN PollutantCode='a21026' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'SO2'
            //			,MAX(CASE WHEN PollutantCode='a21003' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'NO'
            //			,MAX(CASE WHEN PollutantCode='a21004' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'NO2'
            //			,MAX(CASE WHEN PollutantCode='a21002' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'NOX'
            //			,MAX(CASE WHEN PollutantCode='a34002' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'PM10'
            //			,MAX(CASE WHEN PollutantCode='a21005' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'CO'
            //			,MAX(CASE WHEN PollutantCode='a05024' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'O3'
            //			,MAX(CASE WHEN PollutantCode='a34004' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'PM25'
            //			,MAX(CASE WHEN PollutantCode='a01007' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),1) END) AS 'WS'
            //			,MAX(CASE WHEN PollutantCode='a01008' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),1) END) AS 'WD'
            //			,MAX(CASE WHEN PollutantCode='a01001' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),1) END) AS 'TEMP'
            //			,MAX(CASE WHEN PollutantCode='a01002' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),0) END) AS 'RH'
            //			,MAX(CASE WHEN PollutantCode='a01006' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),1) END) AS 'PRESS'
            //			,MAX(CASE WHEN PollutantCode='a01020' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),0) END) AS 'VISIBILITY'
            //		FROM AirReport.TB_HourReport as h
            //		left join dbo.SY_MonitoringPoint as p  on h.PointId = p.PointId
            //        left join dbo.SY_MonitoringPointExtensionForEQMSAir as a  on a.MonitoringPointUid = p.MonitoringPointUid
            //		where Tstamp >= '{0}' and Tstamp <= '{1}'
            //            and a.ProvincialUploadOrNot = 1 --p.regionuid = '86b582c8-3c5a-49b1-b8b3-5a5057f1d29d' 
            //            and p.applicationUid = 'airaaira-aira-aira-aira-airaairaaira'
            //		GROUP BY p.monitoringpointname,Tstamp,a.STNAME,a.STCODE,a.DWCODE
            //        order by a.DWCODE,Tstamp", CurrentDT, CurrentDT.AddHours(23));
            #endregion

            Sql = String.Format(@"
                SELECT allData.PointId
                        ,allData.Tstamp
                        ,a.STNAME
                        ,a.STCODE
                        ,p.monitoringpointname as DWNAME
                        ,a.DWCODE
                        ,YEAR(allData.[tstamp]) AS [YYYY]
                        ,MONTH(allData.[tstamp]) AS [MM]
                        ,DAY(allData.[tstamp]) AS [DD]
                        ,datepart(hour,allData.[tstamp])+1 AS [HH]
                        ,MAX(CASE WHEN PollutantCode='a21026' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'SO2'
                        ,MAX(CASE WHEN PollutantCode='a21003' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'NO'
                        ,MAX(CASE WHEN PollutantCode='a21004' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'NO2'
                        ,MAX(CASE WHEN PollutantCode='a21002' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'NOX'
                        ,MAX(CASE WHEN PollutantCode='a34002' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'PM10'
						,MAX(CASE WHEN PollutantCode='a34004' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'PM25'
						,MAX(CASE WHEN PollutantCode='a05024' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'O3'
						,MAX(CASE WHEN PollutantCode='a21005' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'CO'
                        ,MAX(CASE WHEN PollutantCode='a01007' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),1) END) AS 'WS'
                        ,MAX(CASE WHEN PollutantCode='a01008' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),1) END) AS 'WD'
                        ,MAX(CASE WHEN PollutantCode='a01001' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),1) END) AS 'TEMP'
                        ,MAX(CASE WHEN PollutantCode='a01002' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),0) END) AS 'RH'
                        ,MAX(CASE WHEN PollutantCode='a01006' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),1) END) AS 'PRESS'
                        ,MAX(CASE WHEN PollutantCode='a01020' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),0) END) AS 'VISIBILITY'
                    FROM dbo.F_GetAllDataByHour('{0}',',','{1}','{2}') AS allData
                    LEFT JOIN AirReport.TB_HourReport as h
	                    ON allData.PointID = h.PointId AND allData.tstamp = h.Tstamp
                    left join dbo.SY_MonitoringPoint as p  on allData.PointId = p.PointId
                    left join dbo.SY_MonitoringPointExtensionForEQMSAir as a  on a.MonitoringPointUid = p.MonitoringPointUid
                    where p.applicationUid = 'airaaira-aira-aira-aira-airaairaaira' and h.PointId in({0})
                    GROUP BY allData.PointId,p.monitoringpointname,allData.Tstamp,a.STNAME,a.STCODE,a.DWCODE
                    order by STCODE desc,DWCODE,allData.Tstamp
                ", portIdsStr, CurrentDT, CurrentDT.AddHours(23));

            return CreatDataView(Sql, connection);
        }

        public DataView getDataViewDayXJS(DateTime CurrentDT, string[] portIds)
        {
            //string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
            String Sql = "";
            #region << 废弃 >>
            //            Sql = String.Format(@"
            //               SELECT 
            //					a.STNAME
            //                    ,a.STCODE
            //	                ,p.monitoringpointname as DWNAME
            //                    ,a.DWCODE
            //	                ,YEAR([tstamp]) AS [YYYY]
            //	                ,MONTH([tstamp]) AS [MM]
            //	                ,DAY([tstamp]) AS [DD]
            //	                ,datepart(hour,[tstamp])+1 AS [HH]
            //			,MAX(CASE WHEN PollutantCode='a21026' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'SO2'
            //			,MAX(CASE WHEN PollutantCode='a21003' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'NO'
            //			,MAX(CASE WHEN PollutantCode='a21004' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'NO2'
            //			,MAX(CASE WHEN PollutantCode='a21002' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'NOX'
            //			,MAX(CASE WHEN PollutantCode='a34002' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'PM10'
            //			,MAX(CASE WHEN PollutantCode='a21005' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'CO'
            //			,MAX(CASE WHEN PollutantCode='a05024' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'O3'
            //			,MAX(CASE WHEN PollutantCode='a34004' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) END) AS 'PM25'
            //			,MAX(CASE WHEN PollutantCode='a01007' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),1) END) AS 'WS'
            //			,MAX(CASE WHEN PollutantCode='a01008' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),1) END) AS 'WD'
            //			,MAX(CASE WHEN PollutantCode='a01001' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),1) END) AS 'TEMP'
            //			,MAX(CASE WHEN PollutantCode='a01002' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),0) END) AS 'RH'
            //			,MAX(CASE WHEN PollutantCode='a01006' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),1) END) AS 'PRESS'
            //			,MAX(CASE WHEN PollutantCode='a01020' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),0) END) AS 'VISIBILITY'
            //		FROM AirReport.TB_HourReport as h
            //		left join dbo.SY_MonitoringPoint as p  on h.PointId = p.PointId
            //        left join dbo.SY_MonitoringPointExtensionForEQMSAir as a  on a.MonitoringPointUid = p.MonitoringPointUid
            //		where Tstamp >= '{0}' and Tstamp <= '{1}'
            //            and a.ProvincialUploadOrNot = 1 --p.regionuid = '86b582c8-3c5a-49b1-b8b3-5a5057f1d29d' 
            //            and p.applicationUid = 'airaaira-aira-aira-aira-airaairaaira'
            //		GROUP BY p.monitoringpointname,Tstamp,a.STNAME,a.STCODE,a.DWCODE
            //        order by a.DWCODE,Tstamp", CurrentDT, CurrentDT.AddHours(23));
            #endregion

            //            Sql = String.Format(@"
            //                
            //                SELECT allData.PointId
            //                        ,allData.DateTime
            //                        ,a.STNAME
            //                        ,a.STCODE
            //                        ,p.monitoringpointname as DWNAME
            //                        ,a.DWCODE
            //                        ,YEAR(allData.[DateTime]) AS [YYYY]
            //                        ,MONTH(allData.[DateTime]) AS [MM]
            //                        ,DAY(allData.[DateTime]) AS [DD]
            //                        ,datepart(hour,allData.DateTime)+1 AS [HH]
            //                        ,MAX(CASE WHEN PollutantCode='a21026' AND a.CalAQIOrNot = 1  THEN [dbo].[F_Round](PollutantValue,3) END) AS 'SO2'
            //                        ,MAX(CASE WHEN PollutantCode='a21003' AND a.CalAQIOrNot = 1  THEN [dbo].[F_Round](PollutantValue,3) END) AS 'NO'
            //                        ,MAX(CASE WHEN PollutantCode='a21004' AND a.CalAQIOrNot = 1  THEN [dbo].[F_Round](PollutantValue,3) END) AS 'NO2'
            //                        ,MAX(CASE WHEN PollutantCode='a21002' AND a.CalAQIOrNot = 1  THEN [dbo].[F_Round](PollutantValue,3) END) AS 'NOX'
            //                        ,MAX(CASE WHEN PollutantCode='a34002' AND a.CalAQIOrNot = 1  THEN [dbo].[F_Round](PollutantValue,3) END) AS 'PM10'
            //                        ,MAX(CASE WHEN PollutantCode='a21005' AND a.CalAQIOrNot = 1  THEN [dbo].[F_Round](PollutantValue,3) END) AS 'CO'
            //                        ,MAX(CASE WHEN PollutantCode='a05024' AND a.CalAQIOrNot = 1  THEN [dbo].[F_Round](PollutantValue,3) END) AS 'O3'
            //                        ,MAX(CASE WHEN PollutantCode='a34004' AND a.CalAQIOrNot = 1  THEN [dbo].[F_Round](PollutantValue,3) END) AS 'PM25'
            //                        ,MAX(CASE WHEN PollutantCode='a01007' AND a.CalAQIOrNot = 1  THEN [dbo].[F_Round](PollutantValue,1) END) AS 'WS'
            //                        ,MAX(CASE WHEN PollutantCode='a01008' AND a.CalAQIOrNot = 1  THEN [dbo].[F_Round](PollutantValue,1) END) AS 'WD'
            //                        ,MAX(CASE WHEN PollutantCode='a01001' AND a.CalAQIOrNot = 1  THEN [dbo].[F_Round](PollutantValue,1) END) AS 'TEMP'
            //                        ,MAX(CASE WHEN PollutantCode='a01002' AND a.CalAQIOrNot = 1  THEN [dbo].[F_Round](PollutantValue,0) END) AS 'RH'
            //                        ,MAX(CASE WHEN PollutantCode='a01006' AND a.CalAQIOrNot = 1  THEN [dbo].[F_Round](PollutantValue,1) END) AS 'PRESS'
            //                        ,MAX(CASE WHEN PollutantCode='a01020' AND a.CalAQIOrNot = 1 THEN [dbo].[F_Round](PollutantValue,0) END) AS 'VISIBILITY'
            //                    FROM dbo.F_GetAllDataByDay('19,11,13,21,16',',','2016/4/30 0:00:00','2016/4/30 23:00:00') AS allData
            //                    LEFT JOIN AirReport.TB_DayReport as d
            //	                    ON allData.PointID = d.PointId AND allData.DateTime = d.DateTime
            //                    left join dbo.SY_MonitoringPoint as p  on allData.PointId = p.PointId
            //                    left join dbo.SY_MonitoringPointExtensionForEQMSAir as a  on a.MonitoringPointUid = p.MonitoringPointUid
            //                    where a.ProvincialUploadOrNot = 1 
            //                        and p.applicationUid = 'airaaira-aira-aira-aira-airaairaaira'
            //                    GROUP BY allData.PointId,p.monitoringpointname,allData.DateTime,a.STNAME,a.STCODE,a.DWCODE
            //                    order by STCODE desc,DWCODE,allData.DateTime
            //                ", portIdsStr, CurrentDT, CurrentDT.AddHours(23));
            Sql = string.Format(@"
                SELECT data.PointId
                    ,DateTime as Tstamp
                    ,[STCODE]
                    ,STNAME
                    ,YEAR([DateTime]) AS [YYYY]
                    ,MONTH([DateTime]) AS [MM]
                    ,DAY([DateTime]) AS [DD]
                    ,[DWCODE]
                    ,port.MonitoringPointName AS [DWNAME]
                    ,[AQIValue] AS [AQI]
                    ,REPLACE(REPLACE(REPLACE([PrimaryPollutant],'-1',''),'-8',''),'O3,O3','O3') AS [SYWRW]
                    ,[Class] AS [KQZLZSJB]
                    ,dbo.F_GetAQI_Grade(AQIValue,'ROMAN') AS [LB] 
                 FROM AirRelease.TB_DayAQI AS data 
                 INNER JOIN 
                 (
	                 SELECT smp.PointId
		                 ,smp.MonitoringPointName 
		                 ,smpExt.stname
		                 ,smpExt.stcode
		                 ,dwcode
	                 FROM dbo.SY_MonitoringPoint AS smp
	                 LEFT JOIN dbo.SY_MonitoringPointExtensionForEQMSAir AS smpExt
		                ON smp.MonitoringPointUid = smpExt.MonitoringPointUid
	                 WHERE smpExt.ProvincialUploadOrNot = 1
                 ) AS port  
                 ON data.PointId=port.PointId
                 WHERE [DateTime]=CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{0}',120),120)
                 ORDER BY [DateTime],[DWCODE] ", CurrentDT.ToString("yyy-MM-dd 00:00:00"));

            return CreatDataView(Sql, connection);
        }
        public DataView getDataViewDayXJSNew(DateTime dtBegin, DateTime dtEnd, string[] portIds)
        {
            string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
            if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
            {
                portIdsStr = " AND port.PointId =" + portIdsStr;
            }
            else if (!string.IsNullOrEmpty(portIdsStr))
            {
                portIdsStr = "AND port.PointId IN(" + portIdsStr + ")";
            }
            String Sql = "";
            Sql = string.Format(@"
                     SELECT STNAME
                    ,DateTime
                    ,port.PointId as PointId
                    ,port.MonitoringPointName AS PointName
                    ,SO2
                    ,NO2
                    ,PM10
                    ,PM25
                    ,Max8HourO3
                    ,CO
                 FROM AirRelease.TB_DayAQI AS data 
                 INNER JOIN 
                 (
	                 SELECT smp.PointId
		                 ,smp.MonitoringPointName 
		                 ,smpExt.stname
		                 ,smpExt.stcode
		                 ,dwcode
	                 FROM dbo.SY_MonitoringPoint AS smp
	                 LEFT JOIN dbo.SY_MonitoringPointExtensionForEQMSAir AS smpExt
		                ON smp.MonitoringPointUid = smpExt.MonitoringPointUid
                 ) AS port  
                 ON data.PointId=port.PointId
                 WHERE [DateTime]>='{0}' and [DateTime]<='{1}' {2}
                  ORDER BY STNAME desc,PointId,[DateTime]", dtBegin.ToString("yyy-MM-dd 00:00:00"), dtEnd.ToString("yyy-MM-dd 23:59:59"), portIdsStr);

            return CreatDataView(Sql, connection);
        }
        public DataView getDataViewSQDBF(DateTime CurrentDT, string[] portIds)
        {
            string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
            String Sql = "";
            Sql = String.Format(@"
                  SELECT PointId,
                       Tstamp
                        ,MAX(CASE WHEN PollutantCode='a21026' THEN PollutantValue END) AS 'SO2'
                        ,MAX(CASE WHEN PollutantCode='a21003' THEN PollutantValue END) AS 'NO'
                        ,MAX(CASE WHEN PollutantCode='a21004' THEN PollutantValue END) AS 'NO2'
                        ,MAX(CASE WHEN PollutantCode='a21002' THEN PollutantValue END) AS 'NOX'
                        ,MAX(CASE WHEN PollutantCode='a34002' THEN PollutantValue END) AS 'PM10'
						,MAX(CASE WHEN PollutantCode='a34004' THEN PollutantValue END) AS 'PM25'
						,MAX(CASE WHEN PollutantCode='a05024' THEN PollutantValue END) AS 'O3'
						,MAX(CASE WHEN PollutantCode='a21005' THEN PollutantValue END) AS 'CO'
                        ,MAX(CASE WHEN PollutantCode='a01007' THEN PollutantValue END) AS 'WS'
                        ,MAX(CASE WHEN PollutantCode='a01008' THEN PollutantValue END) AS 'WD'
                        ,MAX(CASE WHEN PollutantCode='a01001' THEN PollutantValue END) AS 'TEMP'
                        ,MAX(CASE WHEN PollutantCode='a01002' THEN PollutantValue END) AS 'RH'
                        ,MAX(CASE WHEN PollutantCode='a01006' THEN PollutantValue END) AS 'PRESS'
                        ,MAX(CASE WHEN PollutantCode='a01020' THEN PollutantValue END) AS 'VISIBILITY'
                     FROM [AMS_AirAutoMonitor].[Air].[TB_InfectantBy60]
	                 where PointId in({0}) AND Tstamp>='{1}' and Tstamp<='{2}'
                     group by PointId,Tstamp
                ", portIdsStr, CurrentDT, CurrentDT.AddHours(23));

            return CreatDataView(Sql, connection);
        }
        /// <summary>
        /// 获取DataView
        /// </summary>
        /// <param name="strSql">SQL 语句</param>
        /// <param name="strConnectString">连接字符串名称</param>
        /// <returns></returns>
        public DataView CreatDataView(String strSql, String strConnectString)
        {

            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings[strConnectString].ConnectionString);
            SqlDataAdapter myCommand = new SqlDataAdapter(strSql, myConn);
            DataTable dt = new DataTable();
            myCommand.Fill(dt);
            return dt.DefaultView;
            try
            {
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable getAQIDataWX(DateTime datetime)
        {
            string dateTime = datetime.ToShortDateString();
            MonitoringPointDAL DAL = new MonitoringPointDAL();
            string sql = string.Empty;
            DataTable dt = new DataTable();
            DataTable SourceDT = new DataTable();
            #region 创建数据源
            SourceDT.Columns.Add("PortName", typeof(string));
            SourceDT.Columns.Add("SO2", typeof(string));
            SourceDT.Columns.Add("SO2_IAQI", typeof(string));
            SourceDT.Columns.Add("NO2", typeof(string));
            SourceDT.Columns.Add("NO2_IAQI", typeof(string));
            SourceDT.Columns.Add("PM10", typeof(string));
            SourceDT.Columns.Add("PM10_IAQI", typeof(string));
            SourceDT.Columns.Add("CO", typeof(string));
            SourceDT.Columns.Add("CO_IAQI", typeof(string));
            SourceDT.Columns.Add("MaxOneHourO3", typeof(string));
            SourceDT.Columns.Add("MaxOneHourO3_IAQI", typeof(string));
            SourceDT.Columns.Add("Max8HourO3", typeof(string));
            SourceDT.Columns.Add("Max8HourO3_IAQI", typeof(string));
            SourceDT.Columns.Add("Pm25", typeof(string));
            SourceDT.Columns.Add("PM25_IAQI", typeof(string));
            SourceDT.Columns.Add("AQIValue", typeof(string));
            SourceDT.Columns.Add("PrimaryPollutant", typeof(string));
            SourceDT.Columns.Add("Grade", typeof(string));
            SourceDT.Columns.Add("Class", typeof(string));
            SourceDT.Columns.Add("RGBValue", typeof(string));
            #endregion
            sql = @"
                select p.PointId,p.monitoringpointname,a.CalAQIOrNot,stname,dwcode
                from dbo.SY_MonitoringPoint as p 
                left join dbo.SY_MonitoringPointExtensionForEQMSAir as a  
                    on a.MonitoringPointUid = p.MonitoringPointUid
                where a.StateUploadOrNot = 1 
                    and p.applicationUid = 'airaaira-aira-aira-aira-airaairaaira'
                ORDER BY a.DWCODE ";
            DataView dvPoint = CreatDataView(sql, connection);
            sql = "";
            if (dvPoint.Count > 0)
            {
                sql = "SELECT PortName,dwcode, "
           + "  CAST(1000 * CAST(SO2 AS decimal(11, 3)) AS int) AS SO2, SO2_IAQI, CAST(1000 * CAST(NO2 AS decimal(11, 3)) AS int) "
            + " AS NO2, NO2_IAQI, CAST(1000 * CAST(PM10 AS decimal(11, 3)) AS int) AS PM10, PM10_IAQI, CO, CO_IAQI, "
            + " CAST(1000 * CAST(MaxOneHourO3 AS decimal(11, 3)) AS int) AS MaxOneHourO3, MaxOneHourO3_IAQI, "
            + " CAST(1000 * CAST(Max8HourO3 AS decimal(11, 3)) AS int) AS Max8HourO3, Max8HourO3_IAQI, "
            + " CAST(1000 * CAST([PM25] AS decimal(11, 3)) AS int) AS [PM25], [PM25_IAQI], AQIValue, PrimaryPollutant, Grade, Class, "
           + "  RGBValue"
+ " FROM      (SELECT   p.PointId, a.stname as Name,a.dwcode as dwcode, p.monitoringPointName AS PortName, DATEADD(dd, - 1, r.dateTime) AS DT, r.SO2, r.SO2_IAQI, "
                             + " r.NO2, r.NO2_IAQI, r.PM10, r.PM10_IAQI, r.CO, r.CO_IAQI, r.MaxOneHourO3, r.MaxOneHourO3_IAQI, "
                             + " r.Max8HourO3, r.Max8HourO3_IAQI, r.[PM25] as Pm25, r.[PM25_IAQI] as [PM25_IAQI], r.AQIValue, r.PrimaryPollutant, r.Grade, r.Class, "
                             + "(case r.RGBValue when '#00e400' then '绿色' when '#ffff00' then '黄色' when '#ff7e00' then '橙色' when '#ff0000' then '红色' when '#99004c' then '紫色' when '#7e0023' then '褐红色' end)  as RGBValue"
             + " FROM      AirRelease.TB_DayAQI AS r INNER JOIN"
                             + " SY_MonitoringPoint AS p ON r.PointId = p.PointId "
                             + " inner join dbo.SY_MonitoringPointExtensionForEQMSAir as a  on a.MonitoringPointUid = p.MonitoringPointUid"
            + "  WHERE   (p.sitetypeuid = '2efcf99d-ec8c-4e09-8f2d-ed74fa82c71b' ) AND (r.dateTime = CONVERT(varchar(10), '" + dateTime + "', 120))"
            + "  UNION"
            + "  SELECT   999 AS PointId, '苏州市' AS Name,NULL as dwcode,'国控平均' AS PortName, DATEADD(dd, - 1, ReportDateTime) AS DT, SO2, "
                           + "   SO2_IAQI, NO2, NO2_IAQI, PM10, PM10_IAQI,Cast(CO as decimal(18,3)) as CO, CO_IAQI, MaxOneHourO3, MaxOneHourO3_IAQI, "
                            + "  Max8HourO3, Max8HourO3_IAQI, [PM25] as PM25, [PM25_IAQI] as[PM25_IAQI], AQIValue, PrimaryPollutant, Grade, Class, "
                           + "   (case RGBValue when '#00e400' then '绿色' when '#ffff00' then '黄色' when '#ff7e00' then '橙色' when '#ff0000' then '红色' when '#99004c' then '紫色' when '#7e0023' then '褐红色' end)  AS RGBValue"
             + " FROM      [AirReport].[TB_RegionDayAQIReport]"
            + "  WHERE   (ReportDateTime = CONVERT(varchar(10), '" + dateTime + "', 120)) and MonitoringRegionUid = '7e05b94c-bbd4-45c3-919c-42da2e63fd43') AS a"
+ " ORDER BY DT DESC";

                DataView dv = CreatDataView(sql, connection);
                int i = 0;
                foreach (DataRow dr in dvPoint.ToTable().Rows)
                {
                    DataRow sdr = SourceDT.NewRow();
                    dv.RowFilter = "PortName like '%" + dr["monitoringpointname"] + "%'";
                    if (dv.Count > 0 && dr["CalAQIOrNot"] != DBNull.Value && Convert.ToInt32(dr["CalAQIOrNot"]) == 1)
                    {
                        sdr["PortName"] = (dv[0]["dwcode"] != DBNull.Value ? dv[0]["dwcode"].ToString() : "") + "(" + dv[0]["PortName"].ToString() + ")";
                        //sdr["DT"] = dv[0]["DT"].ToString();
                        sdr["SO2"] = dv[0]["SO2"] != DBNull.Value ? dv[0]["SO2"].ToString() : "NA";
                        sdr["SO2_IAQI"] = dv[0]["SO2_IAQI"] != DBNull.Value ? dv[0]["SO2_IAQI"].ToString() : "NA";
                        sdr["NO2"] = dv[0]["NO2"] != DBNull.Value ? dv[0]["NO2"].ToString() : "NA";
                        sdr["NO2_IAQI"] = dv[0]["NO2_IAQI"] != DBNull.Value ? dv[0]["NO2_IAQI"].ToString() : "NA";
                        sdr["PM10"] = dv[0]["PM10"] != DBNull.Value ? dv[0]["PM10"].ToString() : "NA";
                        sdr["PM10_IAQI"] = dv[0]["PM10_IAQI"] != DBNull.Value ? dv[0]["PM10_IAQI"].ToString() : "NA";
                        sdr["CO"] = dv[0]["CO"] != DBNull.Value ? dv[0]["CO"].ToString() : "NA";
                        sdr["CO_IAQI"] = dv[0]["CO_IAQI"] != DBNull.Value ? dv[0]["CO_IAQI"].ToString() : "NA";
                        sdr["MaxOneHourO3"] = dv[0]["MaxOneHourO3"] != DBNull.Value ? dv[0]["MaxOneHourO3"].ToString() : "NA";
                        sdr["MaxOneHourO3_IAQI"] = dv[0]["MaxOneHourO3_IAQI"] != DBNull.Value ? dv[0]["MaxOneHourO3_IAQI"].ToString() : "NA";
                        sdr["Pm25"] = dv[0]["Pm25"] != DBNull.Value ? dv[0]["Pm25"].ToString() : "NA";
                        sdr["PM25_IAQI"] = dv[0]["PM25_IAQI"] != DBNull.Value ? dv[0]["PM25_IAQI"].ToString() : "NA";
                        sdr["Max8HourO3"] = dv[0]["Max8HourO3"] != DBNull.Value ? dv[0]["Max8HourO3"].ToString() : "NA";
                        sdr["Max8HourO3_IAQI"] = dv[0]["Max8HourO3_IAQI"] != DBNull.Value ? dv[0]["Max8HourO3_IAQI"].ToString() : "NA";
                        sdr["AQIValue"] = dv[0]["AQIValue"].ToString();
                        sdr["PrimaryPollutant"] = dv[0]["PrimaryPollutant"].ToString();
                        sdr["Grade"] = dv[0]["Grade"].ToString();
                        sdr["Class"] = dv[0]["Class"].ToString();
                        sdr["RGBValue"] = dv[0]["RGBValue"] != DBNull.Value ? dv[0]["RGBValue"].ToString() : "-";
                    }
                    else
                    {
                        sdr["PortName"] = (dr["dwcode"] != DBNull.Value ? dr["dwcode"].ToString() : "") + "(" + dr["monitoringpointname"].ToString() + ")";
                        //sdr["DT"] = "-";
                        sdr["SO2"] = "NA";
                        sdr["SO2_IAQI"] = "NA";
                        sdr["NO2"] = "NA";
                        sdr["NO2_IAQI"] = "NA";
                        sdr["PM10"] = "NA";
                        sdr["PM10_IAQI"] = "NA";
                        sdr["CO"] = "NA";
                        sdr["CO_IAQI"] = "NA";
                        sdr["MaxOneHourO3"] = "NA";
                        sdr["MaxOneHourO3_IAQI"] = "NA";
                        sdr["Pm25"] = "NA";
                        sdr["PM25_IAQI"] = "NA";
                        sdr["Max8HourO3"] = "NA";
                        sdr["Max8HourO3_IAQI"] = "NA";
                        sdr["AQIValue"] = "-";
                        sdr["PrimaryPollutant"] = "-";
                        sdr["Grade"] = "-";
                        sdr["Class"] = "-";
                        sdr["RGBValue"] = "-";
                    }
                    SourceDT.Rows.Add(sdr);
                    i++;
                }
                dv.RowFilter = "PortName = '国控平均'";
                DataRow sdrAvg = SourceDT.NewRow();
                if (dv.Count > 0)
                {
                    sdrAvg["PortName"] = "国控平均";
                    //sdrAvg["DT"] = dv[0]["DT"].ToString();
                    sdrAvg["SO2"] = dv[0]["SO2"] != DBNull.Value ? dv[0]["SO2"].ToString() : "NA";
                    sdrAvg["SO2_IAQI"] = dv[0]["SO2_IAQI"] != DBNull.Value ? dv[0]["SO2_IAQI"].ToString() : "NA";
                    sdrAvg["NO2"] = dv[0]["NO2"] != DBNull.Value ? dv[0]["NO2"].ToString() : "NA";
                    sdrAvg["NO2_IAQI"] = dv[0]["NO2_IAQI"] != DBNull.Value ? dv[0]["NO2_IAQI"].ToString() : "NA";
                    sdrAvg["PM10"] = dv[0]["PM10"] != DBNull.Value ? dv[0]["PM10"].ToString() : "NA";
                    sdrAvg["PM10_IAQI"] = dv[0]["PM10_IAQI"] != DBNull.Value ? dv[0]["PM10_IAQI"].ToString() : "NA";
                    sdrAvg["CO"] = dv[0]["CO"] != DBNull.Value ? dv[0]["CO"].ToString() : "NA";
                    sdrAvg["CO_IAQI"] = dv[0]["CO_IAQI"] != DBNull.Value ? dv[0]["CO_IAQI"].ToString() : "NA";
                    sdrAvg["MaxOneHourO3"] = dv[0]["MaxOneHourO3"] != DBNull.Value ? dv[0]["MaxOneHourO3"].ToString() : "NA";
                    sdrAvg["MaxOneHourO3_IAQI"] = dv[0]["MaxOneHourO3_IAQI"] != DBNull.Value ? dv[0]["MaxOneHourO3_IAQI"].ToString() : "NA";
                    sdrAvg["Pm25"] = dv[0]["Pm25"] != DBNull.Value ? dv[0]["Pm25"].ToString() : "NA";
                    sdrAvg["PM25_IAQI"] = dv[0]["PM25_IAQI"] != DBNull.Value ? dv[0]["PM25_IAQI"].ToString() : "NA";
                    sdrAvg["Max8HourO3"] = dv[0]["Max8HourO3"] != DBNull.Value ? dv[0]["Max8HourO3"].ToString() : "NA";
                    sdrAvg["Max8HourO3_IAQI"] = dv[0]["Max8HourO3_IAQI"] != DBNull.Value ? dv[0]["Max8HourO3_IAQI"].ToString() : "NA";
                    sdrAvg["AQIValue"] = dv[0]["AQIValue"].ToString();
                    sdrAvg["PrimaryPollutant"] = dv[0]["PrimaryPollutant"].ToString();
                    sdrAvg["Grade"] = dv[0]["Grade"].ToString();
                    sdrAvg["Class"] = dv[0]["Class"].ToString();
                    sdrAvg["RGBValue"] = dv[0]["RGBValue"] != DBNull.Value ? dv[0]["RGBValue"].ToString() : "-";
                }
                else
                {
                    sdrAvg["PortName"] = "国控平均";
                    //sdrAvg["DT"] = "-";
                    sdrAvg["SO2"] = "NA";
                    sdrAvg["SO2_IAQI"] = "NA";
                    sdrAvg["NO2"] = "NA";
                    sdrAvg["NO2_IAQI"] = "NA";
                    sdrAvg["PM10"] = "NA";
                    sdrAvg["PM10_IAQI"] = "NA";
                    sdrAvg["CO"] = "NA";
                    sdrAvg["CO_IAQI"] = "NA";
                    sdrAvg["MaxOneHourO3"] = "NA";
                    sdrAvg["MaxOneHourO3_IAQI"] = "NA";
                    sdrAvg["Pm25"] = "NA";
                    sdrAvg["PM25_IAQI"] = "NA";
                    sdrAvg["Max8HourO3"] = "NA";
                    sdrAvg["Max8HourO3_IAQI"] = "NA";
                    sdrAvg["AQIValue"] = "-";
                    sdrAvg["PrimaryPollutant"] = "-";
                    sdrAvg["Grade"] = "-";
                    sdrAvg["Class"] = "-";
                    sdrAvg["RGBValue"] = "-";
                }
                SourceDT.Rows.Add(sdrAvg);
            }
            return SourceDT;
        }

        /// <summary>
        /// 从审核历史表导入审核预处理数据
        /// </summary>
        /// <param name="applicationType"></param>
        /// <param name="portIds"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool GetDataFromHis(ApplicationType applicationType, string[] portIds, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            dateStart = DateTime.Parse(dateStart.ToString("yyyy-MM-dd 00:00:00"));
            if (applicationType == ApplicationType.Air)
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:00:00"));
            else
                dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 23:59:59.998"));
            try
            {
                if (portIds == null || portIds.Length == 0)
                    return false;
                g_DBBiz.ClearParameters();
                SqlParameter pramDateStart = new SqlParameter();
                pramDateStart = new SqlParameter();
                pramDateStart.SqlDbType = SqlDbType.DateTime;
                pramDateStart.ParameterName = "@m_begin";
                pramDateStart.Value = dateStart;
                g_DBBiz.SetProcedureParameters(pramDateStart);
                SqlParameter pramDateEnd = new SqlParameter();
                pramDateEnd = new SqlParameter();
                pramDateEnd.SqlDbType = SqlDbType.DateTime;
                pramDateEnd.ParameterName = "@m_end";
                pramDateEnd.Value = dateEnd;
                g_DBBiz.SetProcedureParameters(pramDateEnd);
                string strCity = string.Join(",", portIds);
                SqlParameter pramPortIds = new SqlParameter();
                pramPortIds = new SqlParameter();
                pramPortIds.SqlDbType = SqlDbType.NVarChar;
                pramPortIds.ParameterName = "@m_portlist";
                pramPortIds.Value = strCity;
                g_DBBiz.SetProcedureParameters(pramPortIds);

                g_DBBiz_W_hp.ClearParameters();
                SqlParameter pramDateStart_W_hp = new SqlParameter();
                pramDateStart_W_hp = new SqlParameter();
                pramDateStart_W_hp.SqlDbType = SqlDbType.DateTime;
                pramDateStart_W_hp.ParameterName = "@m_begin";
                pramDateStart_W_hp.Value = dateStart;
                g_DBBiz_W_hp.SetProcedureParameters(pramDateStart_W_hp);
                SqlParameter pramDateEnd_W_hp = new SqlParameter();
                pramDateEnd_W_hp = new SqlParameter();
                pramDateEnd_W_hp.SqlDbType = SqlDbType.DateTime;
                pramDateEnd_W_hp.ParameterName = "@m_end";
                pramDateEnd_W_hp.Value = dateEnd;
                g_DBBiz_W_hp.SetProcedureParameters(pramDateEnd_W_hp);
                string strCity_W_hp = string.Join(",", portIds);
                SqlParameter pramPortIds_W_hp = new SqlParameter();
                pramPortIds_W_hp = new SqlParameter();
                pramPortIds_W_hp.SqlDbType = SqlDbType.NVarChar;
                pramPortIds_W_hp.ParameterName = "@m_portlist";
                pramPortIds_W_hp.Value = strCity;
                g_DBBiz_W_hp.SetProcedureParameters(pramPortIds_W_hp);

                if (applicationType == ApplicationType.Air)
                {
                    //空气
                    g_DBBiz.ExecuteProcNonQuery("UP_GetAdtDataFromHisTB_Air", connection);
                }
                else
                {
                    //地表水
                    g_DBBiz_W_hp.ExecuteProcNonQuery("UP_GetAdtDataFromHisTB_Water", connection);
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
            return true;
        }
        /// <summary>
        /// 同步审核小时数据
        /// </summary>
        /// <param name="auditStatusUid">状态UID</param>
        /// <param name="appUid">应用类型UID</param>
        /// <param name="pointId">点位ID</param>
        /// <param name="date">数据时间</param>
        /// <param name="guoJiaTblName">国家库数据表完整表名</param>
        /// <param name="conn_db_str">数据库完整名</param>
        /// <param name="guoJiaFactors">国家因子列表(为空表示所有因子)</param>
        /// <param name="localFactors">本地因子列表(为空表示所有因子)</param>
        /// <param name="isPerYearLastHour">是否为同步每年最后一小时数据</param>
        public void SyncAuditAirInfectantByHourData(string pointId, DateTime date)
        {
            // 本地删除数据的时间条件
            string localStartTime = date.ToString("yyyy-MM-dd 00:00:00.000");
            string localEndTime = date.ToString("yyyy-MM-dd 23:00:00.000");
            //string portIdsStr = StringExtensions.GetArrayStrNoEmpty(pointId.ToList<string>(), ",");

            String DataSql = string.Format(@"
                                              SELECT PointId,
                       Tstamp
                        ,MAX(CASE WHEN PollutantCode='a21026' THEN PollutantValue END) AS 'SO2'
                        ,MAX(CASE WHEN PollutantCode='a21003' THEN PollutantValue END) AS 'NO'
                        ,MAX(CASE WHEN PollutantCode='a21004' THEN PollutantValue END) AS 'NO2'
                        ,MAX(CASE WHEN PollutantCode='a21002' THEN PollutantValue END) AS 'NOX'
                        ,MAX(CASE WHEN PollutantCode='a34002' THEN PollutantValue END) AS 'PM10'
						,MAX(CASE WHEN PollutantCode='a34004' THEN PollutantValue END) AS 'PM25'
						,MAX(CASE WHEN PollutantCode='a05024' THEN PollutantValue END) AS 'O3'
						,MAX(CASE WHEN PollutantCode='a21005' THEN PollutantValue END) AS 'CO'
                        ,MAX(CASE WHEN PollutantCode='a01007' THEN PollutantValue END) AS 'WS'
                        ,MAX(CASE WHEN PollutantCode='a01008' THEN PollutantValue END) AS 'WD'
                        ,MAX(CASE WHEN PollutantCode='a01001' THEN PollutantValue END) AS 'TEMP'
                        ,MAX(CASE WHEN PollutantCode='a01002' THEN PollutantValue END) AS 'RH'
                        ,MAX(CASE WHEN PollutantCode='a01006' THEN PollutantValue END) AS 'PRESS'
                        ,MAX(CASE WHEN PollutantCode='a01020' THEN PollutantValue END) AS 'VISIBILITY'
                     FROM [AirReport].[TB_HourReport] 
	                 where PointId ={0} AND Tstamp>='{1}' and Tstamp<='{2}'
                     group by PointId,Tstamp"
                                  , pointId
                                  , localStartTime, localEndTime);
            //先删除表中数据避免重复插入
            DataTable dtHour = g_DatabaseHelper.ExecuteDataTable(DataSql, connection);

            String queryDataSql = string.Format(@"                                            
                                          		SELECT 
										PointId
										,Tstamp 
										,PollutantCode
                                        ,CASE WHEN (0.985<=PollutantValue and  PollutantCode='a34002') THEN null else dbo.F_ValidValueByFlag(PollutantValue,Mark,',') END PollutantValue
										,Status dataFlag
										,Mark AuditFlag
										,CreatUser
									   FROM dbo.SY_Air_InfectantBy60
									   WHERE Tstamp >= '{1}'
										AND Tstamp <= '{2}'
										AND PointId ={0}
                                             order by PointId asc,Tstamp asc"
                                            , pointId
                                            , localStartTime
                                            , localEndTime);

            DataTable dtGuoJia = null;

            try
            {
                dtGuoJia = g_DatabaseHelper.ExecuteDataTable(queryDataSql, connection);
            }
            catch (Exception e)
            { }

            if (dtGuoJia == null || dtGuoJia.Rows.Count == 0)
            {
                return;
            }
            if (dtHour.Rows.Count < 24)
            {
                string preDelSql = string.Format(@"
                                               delete from [AirReport].[TB_HourReport] 
                                                where PointId ={0}
                                                  and Tstamp >= '{1}' 
                                                  and Tstamp <  '{2}'"
                                         , pointId
                                         , localStartTime
                                         , localEndTime
                                         );
                //先删除表中数据避免重复插入
                g_DatabaseHelper.ExecuteNonQuery(preDelSql, connection);
                // 添加数据
                string connStr = ConfigurationManager.ConnectionStrings[connection].ConnectionString;
                SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connStr, SqlBulkCopyOptions.UseInternalTransaction);

                sqlbulkcopy.DestinationTableName = "[AirReport].[TB_HourReport]";//目标表表名
                sqlbulkcopy.ColumnMappings.Add("[PointId]", "[PointId]");
                sqlbulkcopy.ColumnMappings.Add("[Tstamp]", "[Tstamp]");
                sqlbulkcopy.ColumnMappings.Add("[PollutantCode]", "[PollutantCode]");
                sqlbulkcopy.ColumnMappings.Add("[PollutantValue]", "[PollutantValue]");
                sqlbulkcopy.ColumnMappings.Add("[AuditFlag]", "[AuditFlag]");
                sqlbulkcopy.ColumnMappings.Add("[CreatUser]", "[CreatUser]");

                sqlbulkcopy.WriteToServer(dtGuoJia);
                sqlbulkcopy.Close();
            }
        }
    }
}
