using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SmartEP.Utilities.AdoData;
using SmartEP.Core.Generic;
using SmartEP.Data.Enums;

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Air
{
    public class SyncTransferMappingDAL
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        protected DatabaseHelper dbHelper = Singleton<DatabaseHelper>.GetInstance();

        private string AMS_BaseData_Conn = EnumMapping.GetConnectionName(DataConnectionType.BaseData);

        /// <summary>
        /// 取得空气测点关联
        /// </summary>
        /// <param name="sysType">关联类型（如：GuoJia）</param>
        /// <param name="LocalPortIds">本地站点列表，以“,”分割</param>
        /// <returns></returns>
        public DataTable GetPortMapping(string sysType, string LocalPortIds = null)
        {
            if (string.IsNullOrEmpty(sysType))
                return null;

            string sql = string.Empty;
            LocalPortIds = string.IsNullOrEmpty(LocalPortIds) ? LocalPortIds : LocalPortIds.Trim(';');
            if (string.IsNullOrEmpty(LocalPortIds))
            {
                sql = string.Format(@"
                    select distinct TransferSetGuid, LocalPort, SourcePort, SourceServerUrl 
                      from dbo.DT_TransferPortInfo AS port
                      left join MPInfo.TB_MonitoringPoint AS smp
	                    on port.LocalPort = smp.PointId
                     where SourceType = '{0}'
	                   and TransferFlag = 1
	                   and smp.PointId is not null
                ", sysType);
            }
            else
            {
                sql = string.Format(@"
                    select distinct TransferSetGuid, LocalPort, SourcePort, SourceServerUrl 
                      from dbo.DT_TransferPortInfo AS port
                      left join MPInfo.TB_MonitoringPoint AS smp
	                    on port.LocalPort = smp.PointId
                     where SourceType = '{0}'
	                   and TransferFlag = 1
                       and port.LocalPort in ({1}) 
	                   and smp.PointId is not null                    
                    ", sysType, LocalPortIds);
            }

            return dbHelper.ExecuteDataTable(sql, AMS_BaseData_Conn);
        }

        /// <summary>
        /// 取得空气单站点对应因子关联
        /// </summary>
        /// <returns></returns>
        public DataTable GetChannelMapping(string sysType)
        {
            if (string.IsNullOrEmpty(sysType))
                return null;

            string sql = string.Format(@"
                select distinct LocalPortid, LocalChannel, SourceChannel, UnitConversion  
                  from dbo.DT_TransferChannelMapping
                 where SysType = '{0}'
	               and TransferChannelFlag=1 
                 order by LocalChannel asc", sysType);

            return dbHelper.ExecuteDataTable(sql, AMS_BaseData_Conn);
        }

        /// <summary>
        /// 根据系统类型取得因子的名称信息
        /// </summary>
        /// <returns>LocalChannel：本地因子Code、PollutantName：因子名称、SourceChannel：第三方因子</returns>
        public DataTable GetChannelNameInfo(string sysType)
        {
            if (string.IsNullOrEmpty(sysType))
                return null;

            string sql = string.Format(@"
                    SELECT [LocalChannel]
                          ,[PollutantName]
                          ,[SourceChannel]
                      FROM [Standard].[TB_PollutantCode]
                     INNER JOIN [dbo].[DT_TransferChannelMapping]
                       ON PollutantCode = LocalChannel
                      AND SysType='{0}' 
                      AND TransferChannelFlag=1
                    ORDER BY OrderByNum DESC", sysType);

            return dbHelper.ExecuteDataTable(sql, AMS_BaseData_Conn);
        }


        /// <summary>
        /// 取得空气单站点对应因子关联
        /// </summary>
        /// <param name="sysType"></param>
        /// <param name="localFactors">多个因子以英文,分割</param>
        /// <returns>SourceChannel</returns>
        public DataTable GetSourceChannels(string sysType, string localFactors)
        {
            if (string.IsNullOrEmpty(sysType))
                return null;

            string sql = string.Format(@"
                select distinct SourceChannel  
                  from dbo.DT_TransferChannelMapping
                 where SysType = '{0}'
	               and LocalChannel in ('{1}')", sysType, localFactors);

            return dbHelper.ExecuteDataTable(sql, AMS_BaseData_Conn);
        }

        /// <summary>
        /// 获取状态映射
        /// </summary>
        /// <returns></returns>
        public DataTable GetStatusMapping(string sysType, string businessType)
        {
            string sql = string.Format(@"select distinct LocalStatus, SourceStatus 
                                           from dbo.DT_SyncStatusMapping 
                                          where SysType = '{0}' 
                                            and BusinessType = '{1}'"
                                          , sysType
                                          , businessType);

            return dbHelper.ExecuteDataTable(sql, AMS_BaseData_Conn);
        }

        /// <summary>
        /// 获取状态映射字典
        /// </summary>
        /// <param name="sysType">系统类型</param>
        /// <param name="businessType">业务类型</param>
        /// <returns></returns>
        public Dictionary<string, string> GetStatusMappingDic(string sysType, string businessType)
        {
            DataTable statusDt = GetStatusMapping(sysType, businessType);

            Dictionary<string, string> statusMap = new Dictionary<string, string>();

            foreach (DataRow drMap in statusDt.Rows)
            {
                statusMap.Add(drMap["SourceStatus"].ToString(), drMap["LocalStatus"].ToString());
            }

            return statusMap;
        }

        /// <summary>
        /// 获取转换状态
        /// </summary>
        /// <param name="statusDic">状态映射字典</param>
        /// <param name="sourceStatus">原始状态值</param>
        /// <param name="sourceSplitChar">原始状态分割字符</param>
        /// <param name="localSplitChar">本地状态存储分割字符</param>
        /// <returns>string</returns>
        public string GetConvertStatus(Dictionary<string, string> statusDic, string sourceStatus,
                                       char sourceSplitChar, char localSplitChar,
                                       string businessType)
        {
            if (string.IsNullOrEmpty(sourceStatus) || statusDic == null)
            {
                return sourceStatus;
            }

            string[] sourceStatuss = sourceStatus.Split(sourceSplitChar);

            StringBuilder statusSb = new StringBuilder();

            int index = 0;

            foreach (string srcStatus in sourceStatuss)
            {
                if (statusDic.ContainsKey(srcStatus))
                {
                    if (index != 0)
                    {// 存在多个状态映射，以本地分割符号分割
                        statusSb.Append(localSplitChar);
                    }

                    statusSb.Append(statusDic[srcStatus]);

                    index++;
                }
                else
                {
                    /*
                    CommonFunction.WriteInfoLog("When Sync Data Not Match Status=[" + srcStatus + "] BusinessType=[" + businessType
                                               + "], Please Add Status Map To (DT_SyncStatusMapping) Table.");
                     */
                }
            }

            return statusSb.ToString();
        }

        /// <summary>
        /// 将国家平台的因子编码转换为苏州新库的因子编码Case SQL语句
        /// </summary>
        /// <param name="channelInfo">因子映射DataTable</param>
        /// <returns>(case when PollutantCode = 112 then 'a01002' when PollutantCode = 110 then 'a01006' else PollutantCode end)</returns>
        public string ConvertPollutantCodeToCaseSql(DataTable channelInfo)
        {
            StringBuilder pollutantCodeSb = new StringBuilder();
            pollutantCodeSb.Append("(case");

            for (Int32 iRow = 0; iRow < channelInfo.Rows.Count; iRow++)
            {
                pollutantCodeSb.Append(string.Format(" when PollutantCode = '{0}' then '{1}'"
                                      , channelInfo.Rows[iRow]["SourceChannel"].ToString()
                                      , channelInfo.Rows[iRow]["LocalChannel"].ToString()
                                      ));
            }

            pollutantCodeSb.Append(" else PollutantCode end)");

            return pollutantCodeSb.ToString();
        }

        /// <summary>
        /// 将第三方平台的属性转换为新库因子的Case SQL语句
        /// </summary>
        /// <param name="channelInfo">因子映射DataTable</param>
        /// <param name="factorColumnName">第三方因子列存储名</param>
        /// <returns>(case when PollutantCode = 112 then 'a01002' when PollutantCode = 110 then 'a01006' else PollutantCode end)</returns>
        public string ConvertPollutantCodeToCaseSql(DataTable channelInfo, string factorColumnName)
        {
            StringBuilder pollutantCodeSb = new StringBuilder();
            pollutantCodeSb.Append("(case");

            for (Int32 iRow = 0; iRow < channelInfo.Rows.Count; iRow++)
            {
                pollutantCodeSb.Append(string.Format(" when {0} = '{1}' then '{2}'"
                                      , factorColumnName
                                      , channelInfo.Rows[iRow]["SourceChannel"].ToString()
                                      , channelInfo.Rows[iRow]["LocalChannel"].ToString()
                                      ));
            }

            pollutantCodeSb.Append(" else " + factorColumnName + " end)");

            return pollutantCodeSb.ToString();
        }
        /// <summary>
        /// 将第三方平台的属性转换为新库因子的Case SQL语句
        /// </summary>
        /// <param name="channelInfo">点位映射datatable</param>
        /// <param name="factorColumnName">第三方点位列存储名</param>
        /// <returns></returns>
        public string ConvertPointIdToCaseSql(DataTable channelInfo, string factorColumnName)
        {
            StringBuilder pollutantCodeSb = new StringBuilder();
            pollutantCodeSb.Append("(case");

            for (Int32 iRow = 0; iRow < channelInfo.Rows.Count; iRow++)
            {
                pollutantCodeSb.Append(string.Format(" when {0} = '{1}' then '{2}'"
                                      , factorColumnName
                                      , channelInfo.Rows[iRow]["SourcePort"].ToString()
                                      , channelInfo.Rows[iRow]["LocalPort"].ToString()
                                      ));
            }

            pollutantCodeSb.Append(" else " + factorColumnName + " end)");

            return pollutantCodeSb.ToString();
        }
        /// <summary>
        // 根据配置将因子单位转换为对应值
        /// </summary>
        /// <param name="dataDt">数据集合</param>
        /// <param name="pollutantClunName">因子列名</param>
        /// <param name="valueClunName">因子值列名</param>
        /// <param name="channelDt">因子集合</param>
        public void ConversionPollutantUnit(DataTable dataDt, string pollutantClunName,
                                            string valueClunName, DataTable channelDt)
        {
            string pollutantValue = null;

            double unitConvVal = 1;

            foreach (DataRow dataDrMap in dataDt.Rows)
            {
                foreach (DataRow channelDrRow in channelDt.Rows)
                {
                    if (channelDrRow["LocalChannel"].ToString().Equals(dataDrMap[pollutantClunName].ToString()))
                    {
                        pollutantValue = dataDrMap[valueClunName].ToString();

                        if (!string.IsNullOrEmpty(pollutantValue))
                        {
                            try
                            {
                                unitConvVal = Convert.ToDouble(channelDrRow["UnitConversion"].ToString());

                                if (unitConvVal == 1)
                                {
                                    break;
                                }
                                dataDrMap[valueClunName] = Convert.ToDouble(pollutantValue) * unitConvVal;
                            }
                            catch (Exception e)
                            {
                                dataDrMap[valueClunName] = DBNull.Value;
                            }
                        }

                        break;
                    }
                }
            }
        }
    }
}
