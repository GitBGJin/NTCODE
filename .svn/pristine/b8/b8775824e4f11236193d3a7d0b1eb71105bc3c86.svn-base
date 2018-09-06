#region 命名空间
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
#endregion

namespace SmartEP.Service.DataAnalyze.AQIReport
{
    #region AQI报表管理类
    /// <summary>
    /// AQI报表管理类
    /// </summary>
    public class AQIReportManager
    {
        #region 私有变量
        private List<AQIFactorMapping> _lstAQIFactorMapping;
        private AQIReport _aqiReport;
        private DataView _dvwDataSource;
        private Enums.ReportType _reportType;
        #endregion

        #region 公共构造函数
        #region 有参构造函数
        public AQIReportManager(Enums.ReportType reportType)
        {
            _reportType = reportType;
            _lstAQIFactorMapping = GetInitAQIFactorMapping();
            _aqiReport = new AQIReport();
        }
        #endregion

        #region 有参构造函数
        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="lstAQIFactorMapping">映射列表</param>
        public AQIReportManager(List<AQIFactorMapping> lstAQIFactorMapping, Enums.ReportType reportType)
            : this(reportType)
        {
            _lstAQIFactorMapping = lstAQIFactorMapping;
        }
        #endregion

        #region 有参构造函数
        /// <summary>
        /// 有参构造函数
        /// <param name="dvwDataSource">数据源</param>
        /// </summary>
        public AQIReportManager(DataView dvwDataSource, Enums.ReportType reportType)
            : this(reportType)
        {
            _dvwDataSource = dvwDataSource;
        }
        #endregion

        #region 有参构造函数
        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="lstAQIFactorMapping">映射列表</param>
        /// <param name="dvwDataSource">数据源</param>
        public AQIReportManager(List<AQIFactorMapping> lstAQIFactorMapping, DataView dvwDataSource, Enums.ReportType reportType)
            : this(reportType)
        {
            _lstAQIFactorMapping = lstAQIFactorMapping;
            _dvwDataSource = dvwDataSource;
        }
        #endregion
        #endregion

        #region 公共属性
        #region AQI因子映射
        /// <summary>
        /// AQI因子映射
        /// </summary>
        public List<AQIFactorMapping> AQIFactorMappings
        {
            get
            {
                return _lstAQIFactorMapping;
            }
        }
        #endregion

        #region 数据源
        /// <summary>
        /// 数据源
        /// </summary>
        public DataView DataSource
        {
            get
            {
                if (_dvwDataSource == null)
                {
                    _dvwDataSource = new DataView();
                }
                return _dvwDataSource;
            }
            set { _dvwDataSource = value; }
        }
        #endregion
        #endregion

        #region 公共方法
        #region 添加映射
        /// <summary>
        /// 添加映射
        /// </summary>
        /// <param name="strFactorName">因子名称</param>
        /// <param name="lstAQIDataType">因子AQI的数据类型:如，8小时滑动平均、最大8小时平均</param>
        /// <param name="strMappingColumn">因子的映射列名</param>
        /// <param name="reportType">报表的类型</param>
        /// <param name="blnOverride">因子映射已经存在的时候是否要覆盖. 默认值是false，不覆盖</param>
        /// <returns></returns>
        public AQIFactorMapping AddFactorMapping(string strFactorName, List<AQIDataType> lstAQIDataType, string strMappingColumn, Enums.ReportType reportType, bool blnOverride = false)
        {
            AQIFactorMapping tmpAQIFactorMapping;
            tmpAQIFactorMapping = _lstAQIFactorMapping.Where(x => x.FactorName == strFactorName).FirstOrDefault();
            if (tmpAQIFactorMapping != null)//因子映射已经存在
            {
                if (blnOverride)//重写已经存在的映射关系
                {
                    tmpAQIFactorMapping.AQIDataTypes = lstAQIDataType;
                    tmpAQIFactorMapping.MappingColumn = strMappingColumn;
                    tmpAQIFactorMapping.ReportType = reportType;
                }
            }
            else//新的映射关系
            {
                tmpAQIFactorMapping = new AQIFactorMapping() { FactorName = strFactorName, AQIDataTypes = lstAQIDataType, MappingColumn = strMappingColumn, ReportType = reportType };

            }
            _lstAQIFactorMapping.Add(tmpAQIFactorMapping);
            return tmpAQIFactorMapping;
        }
        #endregion

        #region 获取AQI信息
        /// <summary>
        /// 获取AQI信息：如AQI值，因子浓度，污染等级等
        /// </summary>
        /// <returns></returns>
        public List<FactorAQI> GetAQI()
        {
            InitAQI();
            return _aqiReport.GetAQI(_reportType);
        }
        #endregion

        #region 获取首要污染物信息
        /// <summary>
        /// 获取首要污染物信息
        /// </summary>
        /// <returns></returns>
        public List<FactorAQI> GetPrimaryAQI()
        {
            //InitAQI();
            return _aqiReport.GetPrimaryAQI(_reportType);
        }
        #endregion
        #endregion

        #region 私有方法
        #region 获取默认的AQI映射
        /// <summary>
        /// 获取默认的AQI映射
        /// </summary>
        /// <returns></returns>
        private List<AQIFactorMapping> GetInitAQIFactorMapping()
        {
            return new List<AQIFactorMapping>() { 
                   #region 日报
                        #region SO2 24小时
                        new AQIFactorMapping(){ 
                            FactorName="SO2",
                            AQIDataTypes=new List<AQIDataType>(){
                                new AQIDataType(){ CaculatorType=Enums.CaculatorType.TwentyHours, PollutionIndexDataType=Enums.PollutionIndexDataType.Avg}
                            },
                            MappingColumn="ZAD_DATA36",
                            ReportType=Enums.ReportType.DayAQI
                        },
                        #endregion
                        #region NO2 24小时
                        new AQIFactorMapping(){ 
                            FactorName="NO2",
                            AQIDataTypes=new List<AQIDataType>(){
                                new AQIDataType(){ CaculatorType=Enums.CaculatorType.TwentyHours, PollutionIndexDataType=Enums.PollutionIndexDataType.Avg}
                            },
                            MappingColumn="ZAD_DATA34",
                            ReportType=Enums.ReportType.DayAQI
                        },
                        #endregion
                        #region PM10 24小时
                        new AQIFactorMapping(){ 
                            FactorName="PM10",
                            AQIDataTypes=new List<AQIDataType>(){
                                new AQIDataType(){ CaculatorType=Enums.CaculatorType.TwentyHours, PollutionIndexDataType=Enums.PollutionIndexDataType.Avg}
                            },
                            MappingColumn="ZAD_DATA93",
                            ReportType=Enums.ReportType.DayAQI
                        },
                        #endregion
                        #region CO 24小时
                        new AQIFactorMapping(){ 
                            FactorName="CO",
                            AQIDataTypes=new List<AQIDataType>(){
                                new AQIDataType(){ CaculatorType=Enums.CaculatorType.TwentyHours, PollutionIndexDataType=Enums.PollutionIndexDataType.Avg}
                            },
                            MappingColumn="ZAD_DATA35",
                            ReportType=Enums.ReportType.DayAQI
                        },
                        #endregion
                        #region O3 最大1小时,最大8小时
                        new AQIFactorMapping(){
                            FactorName="O3",
                            AQIDataTypes=new List<AQIDataType>(){
                                new AQIDataType(){ CaculatorType=Enums.CaculatorType.OneHour, PollutionIndexDataType=Enums.PollutionIndexDataType.MaxAvg},
                                new AQIDataType(){ CaculatorType=Enums.CaculatorType.EightHours, PollutionIndexDataType=Enums.PollutionIndexDataType.MaxAvg}
                            },
                            MappingColumn="ZAD_DATA31",
                            ReportType=Enums.ReportType.DayAQI
                        },
                        #endregion
                        #region PM2.5 24小时
                        new AQIFactorMapping(){ 
                            FactorName="PM2.5",
                            AQIDataTypes=new List<AQIDataType>(){
                                new AQIDataType(){ CaculatorType=Enums.CaculatorType.TwentyHours, PollutionIndexDataType=Enums.PollutionIndexDataType.Avg}
                            },
                            MappingColumn="ZAD_DATA94",
                            ReportType=Enums.ReportType.DayAQI
                        },
                        #endregion
                   #endregion
                   #region 实时报
                        #region SO2 1小时
                        new AQIFactorMapping(){ 
                            FactorName="SO2",
                            AQIDataTypes=new List<AQIDataType>(){
                                new AQIDataType(){ CaculatorType=Enums.CaculatorType.OneHour, PollutionIndexDataType=Enums.PollutionIndexDataType.Avg}
                            },
                            MappingColumn="ZAD_DATA36",
                            ReportType=Enums.ReportType.HourAQI
                        },
                        #endregion
                        #region NO2 1小时
                        new AQIFactorMapping(){ 
                            FactorName="NO2",
                            AQIDataTypes=new List<AQIDataType>(){
                                new AQIDataType(){ CaculatorType=Enums.CaculatorType.OneHour, PollutionIndexDataType=Enums.PollutionIndexDataType.Avg}
                            },
                            MappingColumn="ZAD_DATA34",
                            ReportType=Enums.ReportType.HourAQI
                        },
                        #endregion
                        #region PM10 1小时,24小时
                        new AQIFactorMapping(){ 
                            FactorName="PM10",
                            AQIDataTypes=new List<AQIDataType>(){
                                new AQIDataType(){ CaculatorType=Enums.CaculatorType.OneHour, PollutionIndexDataType=Enums.PollutionIndexDataType.Avg},
                                 new AQIDataType(){ CaculatorType=Enums.CaculatorType.TwentyHours, PollutionIndexDataType=Enums.PollutionIndexDataType.Avg}
                            },
                            MappingColumn="ZAD_DATA93",
                            ReportType=Enums.ReportType.HourAQI
                        },
                        #endregion
                        #region CO 1小时
                        new AQIFactorMapping(){ 
                            FactorName="CO",
                            AQIDataTypes=new List<AQIDataType>(){
                                new AQIDataType(){ CaculatorType=Enums.CaculatorType.OneHour, PollutionIndexDataType=Enums.PollutionIndexDataType.Avg}
                            },
                            MappingColumn="ZAD_DATA35",
                            ReportType=Enums.ReportType.HourAQI
                        },
                        #endregion
                        #region O3 1小时,8小时
                        new AQIFactorMapping(){ 
                            FactorName="O3",
                            AQIDataTypes=new List<AQIDataType>(){
                                new AQIDataType(){ CaculatorType=Enums.CaculatorType.OneHour, PollutionIndexDataType=Enums.PollutionIndexDataType.Avg},
                                 new AQIDataType(){ CaculatorType=Enums.CaculatorType.EightHours, PollutionIndexDataType=Enums.PollutionIndexDataType.Avg}
                            },
                            MappingColumn="ZAD_DATA31",
                            ReportType=Enums.ReportType.HourAQI
                        },
                        #endregion
                        #region PM2.5 1小时,24小时
                        new AQIFactorMapping(){ 
                            FactorName="PM2.5",
                            AQIDataTypes=new List<AQIDataType>(){
                                new AQIDataType(){ CaculatorType=Enums.CaculatorType.OneHour, PollutionIndexDataType=Enums.PollutionIndexDataType.Avg},
                                 new AQIDataType(){ CaculatorType=Enums.CaculatorType.TwentyHours, PollutionIndexDataType=Enums.PollutionIndexDataType.Avg}
                            },
                            MappingColumn="ZAD_DATA94",
                            ReportType=Enums.ReportType.HourAQI
                        }
                        #endregion
                   #endregion
            };
        }
        #endregion

        #region 初使化AQI
        /// <summary>
        /// 初使化AQI
        /// </summary>
        private void InitAQI()
        {
            List<AQIFactorMapping> lstAQIFactorMapping = this.AQIFactorMappings.Where(x => x.ReportType == _reportType).ToList();
            foreach (DataRowView drv in this.DataSource)
            {

                foreach (AQIFactorMapping aqiFactorMapping in lstAQIFactorMapping)
                {
                    AQIFactorData aqiFactorData = new AQIFactorData()
                    {
                        Time = System.Convert.ToDateTime(drv["Tstamp"]),
                        PortId = System.Convert.ToInt32(drv["PortId"]),
                        ReportType = _reportType
                    };
                    aqiFactorData.AQIDataTypes = aqiFactorMapping.AQIDataTypes;
                    aqiFactorData.FactorName = aqiFactorMapping.FactorName;
                    if (!Convert.IsDBNull(drv[aqiFactorMapping.MappingColumn]))
                    {
                        aqiFactorData.FactorValue = Convert.ToDecimal(drv[aqiFactorMapping.MappingColumn]);
                    }
                    aqiFactorData.ReportType = aqiFactorMapping.ReportType;
                    this._aqiReport.AddAQIFactorData(aqiFactorData);
                }
                
            }
        }
        #endregion
        #endregion
    }
    #endregion
}
