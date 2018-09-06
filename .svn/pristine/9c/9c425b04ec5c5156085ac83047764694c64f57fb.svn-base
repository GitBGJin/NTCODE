using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SmartEP.MonitoringBusinessRepository.Air;
using SmartEP.MonitoringBusinessRepository.Water;
using SmartEP.AMSRepository.Air;
using SmartEP.AMSRepository.Water;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Core.Enums;
using System.IO;
using System.Data.OleDb;
using SmartEP.Data;
using Aspose.Cells;
using System.Configuration;
using System.Web;
using SmartEP.Data.SqlServer.MonitoringBusiness;
using SmartEP.Data.SqlServer.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.MonitoringBusinessRepository.Common;
using SmartEP.Utilities.AdoData;



namespace SmartEP.Service.DataAuditing.AuditInterfaces
{

    /// <summary>
    /// 名称：AuditDataService.cs
    /// 创建人：徐龙超
    /// 创建日期：2015-08-16
    /// 维护人员：
    /// 最新维护人员：吕云
    /// 最新维护日期：2017-6-20
    /// 功能摘要：
    /// 审核站点抽象类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AuditDataService
    {
        #region 变量

        AuditDataRepository g_AuditDataRepository = new AuditDataRepository();

        #region 空气
        AuditAirInfectantByHourRepository auditAirHourRep = new AuditAirInfectantByHourRepository();
        AMSRepository.Air.InfectantBy60Repository Infec60AirRep = new AMSRepository.Air.InfectantBy60Repository();
        #endregion

        #region 地表水
        AuditWaterInfectantByHourRepository waterHourRep = new AuditWaterInfectantByHourRepository();
        AMSRepository.Water.InfectantBy60Repository Infec60WaterRep = new AMSRepository.Water.InfectantBy60Repository();
        #endregion
        #endregion

        #region 方法
        #region 审核小时数据
        /// <summary>
        /// 获取空气审核小时数据列表（行转列）
        /// </summary>
        /// <param name="portId">点位ID</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pollutantCodes">因子</param>
        /// <param name="isAllDate">是否自动补缺失数据</param>
        /// <returns></returns>
        public DataView RetrieveAuditHourData(string applicationUID, string[] portId, string[] factor, DateTime startTime, DateTime endTime, bool isAllDate, bool isShowTotal = false, string PointType = "0")
        {
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))//环境空气
            {
                if (isShowTotal)
                {
                    return auditAirHourRep.GetStatisticalAllData(portId, factor, startTime, endTime, isAllDate);
                }
                return auditAirHourRep.GetDataView(portId, factor, startTime, endTime, isAllDate, PointType);
            }
            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))//地表水
            {
                if (isShowTotal)
                {
                    return waterHourRep.GetStatisticalAllData(portId, factor, startTime, endTime, false);
                }
                return waterHourRep.GetDataView(portId, factor, Convert.ToDateTime(startTime.ToString("yyyy-MM-dd 00:00:00")), Convert.ToDateTime(endTime.ToString("yyyy-MM-dd 23:59:59")), false);
            }
            return new DataView();

        }

        /// <summary>
        /// 获取空气审核小时数据列表（行转列）
        /// </summary>
        /// <param name="portId">点位ID</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pollutantCodes">因子</param>
        /// <param name="isAllDate">是否自动补缺失数据</param>
        /// <returns></returns>
        public DataView RetrieveAuditHourDataS(string applicationUID, string[] portId, string InsId, string[] factor, DateTime startTime, DateTime endTime, bool isAllDate, bool isShowTotal = false, string PointType = "0")
        {
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))//环境空气
            {
                if (isShowTotal)
                {
                    return auditAirHourRep.GetStatisticalAllData(portId, factor, startTime, endTime, isAllDate);
                }
                return auditAirHourRep.GetDataViewS(portId, InsId, factor, startTime, endTime, isAllDate, PointType);
            }
            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))//地表水
            {
                if (isShowTotal)
                {
                    return waterHourRep.GetStatisticalAllData(portId, factor, startTime, endTime, false);
                }
                return waterHourRep.GetDataView(portId, factor, Convert.ToDateTime(startTime.ToString("yyyy-MM-dd 00:00:00")), Convert.ToDateTime(endTime.ToString("yyyy-MM-dd 23:59:59")), false);
            }
            return new DataView();

        }

        /// <summary>
        /// 获取空气审核小时数据列表（行转列）超级站
        /// </summary>
        /// <param name="portId">点位ID</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pollutantCodes">因子</param>
        /// <param name="isAllDate">是否自动补缺失数据</param>
        /// <returns></returns>
        public DataView RetrieveAuditHourDataSuper(string applicationUID, string[] portId, string[] factor, DateTime startTime, DateTime endTime, bool isAllDate, bool isShowTotal = false, string PointType = "0")
        {
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))//环境空气
            {
                if (isShowTotal)
                {
                    return auditAirHourRep.GetStatisticalAllData(portId, factor, startTime, endTime, isAllDate);
                }
                return auditAirHourRep.GetDataViewSuper(portId, factor, startTime, endTime, isAllDate, PointType);
            }
            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))//地表水
            {
                if (isShowTotal)
                {
                    return waterHourRep.GetStatisticalAllData(portId, factor, startTime, endTime, false);
                }
                return waterHourRep.GetDataView(portId, factor, startTime, endTime, false);
            }
            return new DataView();

        }

        /// <summary>
        /// 多点审核数据列表
        /// </summary>
        /// <param name="applicationUID"></param>
        /// <param name="portId"></param>
        /// <param name="factor"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="isAllDate"></param>
        /// <param name="isShowTotal"></param>
        /// <returns></returns>
        public DataView RetrieveAuditHourDataMutilPoint(string applicationUID, string[] portId, string[] factor, DateTime startTime, DateTime endTime, bool isAllDate, bool isShowTotal = false)
        {
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))//环境空气
            {
                if (isShowTotal)
                {
                    return auditAirHourRep.GetStatisticalMutilPoint(portId, factor, startTime, endTime, isAllDate);
                }
                return auditAirHourRep.GetDataView(portId, factor, startTime, endTime, isAllDate);
            }
            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))//地表水
            {
                if (isShowTotal)
                {
                    return waterHourRep.GetStatisticalMutilPoint(portId, factor, startTime, endTime, false);
                }
                return waterHourRep.GetDataView(portId, factor, startTime, endTime, false);
            }
            return new DataView();

        }

        /// <summary>
        /// 根据点位、时间段获取审核数据列表【空气】
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="factor"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public IQueryable<AuditAirInfectantByHourEntity> RetrieveAirAuditHourDataByID(int portId, DateTime startTime, DateTime endTime)
        {
            IQueryable<AuditAirInfectantByHourEntity> auditHourList = auditAirHourRep.Retrieve(x => x.PointId == portId && x.DataDateTime >= startTime && x.DataDateTime <= endTime);
            return auditHourList;
        }

        /// <summary>
        /// 根据点位、时间段获取审核数据列表【地表水】
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="factor"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public IQueryable<AuditWaterInfectantByHourEntity> RetrieveWaterAuditHourDataByID(int portId, DateTime startTime, DateTime endTime)
        {
            IQueryable<AuditWaterInfectantByHourEntity> auditHourList = waterHourRep.Retrieve(x => x.PointId == portId && x.DataDateTime >= startTime && x.DataDateTime <= endTime);
            return auditHourList;
        }


        //public IQueryable<AuditAirInfectantByHourEntity> RetrieveAirAuditStatusHourByID(int portId, DateTime startTime, DateTime endTime)
        //{
        //    IQueryable<AuditAirInfectantByHourEntity> auditHourList = auditAirHourRep.Retrieve(x => x.PointId == portId && x.DataDateTime >= startTime && x.DataDateTime <= endTime);
        //    return auditHourList;
        //}


        /// <summary>
        /// 是否存在审核数据【空气】
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="factor"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public bool ExistAirAuditHourData(string[] portId, string[] factors, DateTime startTime, DateTime endTime)
        {
            return auditAirHourRep.Retrieve(x => portId.Contains(x.PointId.ToString()) && factors.Contains(x.PollutantCode) && x.DataDateTime >= startTime && x.DataDateTime <= endTime).Count() > 0 ? true : false;
        }

        /// <summary>
        /// 是否存在审核数据【地表水】
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="factor"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public bool ExistWaterAuditHourData(string[] portId, string[] factors, DateTime startTime, DateTime endTime)
        {
            return waterHourRep.Retrieve(x => portId.Contains(x.PointId.ToString()) && factors.Contains(x.PollutantCode) && x.DataDateTime >= startTime && x.DataDateTime <= endTime).Count() > 0 ? true : false;
        }


        /// <summary>
        /// 根据点位类型、时间段获取审核状态统计信息
        /// </summary>
        /// <param name="application">应用程序类型</param>
        /// <param name="SiteType">点位类型</param>
        /// <param name="beginTime">时间</param>
        /// <returns></returns>
        public DataTable AuditFlagStatisticsByPoint(ApplicationValue application, string SiteType, DateTime beginTime, DateTime endTime)
        {
            DataTable dt = new DataTable();
            if (application == ApplicationValue.Air)//环境空气
                dt = auditAirHourRep.AuditFlagStatisticsByPoint(EnumMapping.GetApplicationValue(application), SiteType, beginTime, endTime);
            else if (application == ApplicationValue.Water)//地表水
                dt = waterHourRep.AuditFlagStatisticsByPoint(EnumMapping.GetApplicationValue(application), SiteType, beginTime, endTime);
            return dt;
        }

        /// <summary>
        /// 取得统计数据固定因子（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="factor"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public DataView RetrieveAuditStatisticalData(string applicationUID, string[] portId, string[] factor, DateTime startTime, DateTime endTime)
        {
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))
                return auditAirHourRep.GetStatisticalData(portId, factor, startTime, endTime);
            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))
                return waterHourRep.GetStatisticalData(portId, factor, startTime, endTime);

            return null;
        }

        /// <summary>
        /// 取得统计数据所有因子（最大值、最小值、平均值）【空气】
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetAirStatisticalAllData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd, bool isAllDate = false)
        {
            return auditAirHourRep.GetStatisticalAllData(portIds, factors, dateStart, dateEnd, isAllDate);
        }
        #endregion

        #region 原始小时数据
        /// <summary>
        /// 获取原始值
        /// </summary>
        /// <param name="applicationUID"></param>
        /// <param name="DataDateTime"></param>
        /// <param name="PointId"></param>
        /// <param name="factorCode"></param>
        /// <returns></returns>
        public decimal GetSourcePolltantValue(string applicationUID, DateTime DataDateTime, int PointId, string factorCode)
        {
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))
            {
                string sql = "select [PollutantValue] from [Air].[TB_InfectantBy60] WITH(NOLOCK) where [Tstamp]='" + DataDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "' and [PointId]=" + PointId + " and PollutantCode='" + factorCode + "'";
                DatabaseHelper g_DatabaseHelper = new DatabaseHelper();
                DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
                if (dt.Rows.Count > 0)
                {
                    return Convert.ToDecimal(dt.Rows[0][0]);
                }
                //DomainModel.AirAutoMonitoring.InfectantBy60Entity infec60 = InfectantBy60AirInfo(DataDateTime, PointId, factorCode);
                //return infec60 != null ? infec60.PollutantValue.Value : -99999;
            }
            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))
            {
                string sql = "select [PollutantValue] from [Water].[TB_InfectantBy60] WITH(NOLOCK) where [Tstamp]='" + DataDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "' and [PointId]=" + PointId + " and PollutantCode='" + factorCode + "'";
                DatabaseHelper g_DatabaseHelper = new DatabaseHelper();
                DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_WaterAutoMonitorConnection");
                if (dt.Rows.Count > 0)
                {
                    return Convert.ToDecimal(dt.Rows[0][0]);
                }

                //DomainModel.WaterAutoMonitoring.InfectantBy60Entity infec60 = InfectantBy60WaterInfo(DataDateTime, PointId, factorCode);
                //return infec60 != null ? infec60.PollutantValue.Value : -99999;
            }
            return -99999;
        }

        /// <summary>
        /// 获取原始值
        /// </summary>
        /// <param name="applicationUID"></param>
        /// <param name="DataDateTime"></param>
        /// <param name="PointId"></param>
        /// <param name="factorCode"></param>
        /// <returns></returns>
        public decimal GetSourcePolltantValueWeibo(string applicationUID, DateTime DataDateTime, int PointId, string factorCode, string pollutant)
        {
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))
            {
                string sql = "select [" + factorCode + "] from [AMS_AirAutoMonitor].[dbo].[TB_SuperStation_Weibo] where DateTime='" + DataDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "' and pointid=" + PointId + " and PollutantCode='" + pollutant + "'";
                DatabaseHelper g_DatabaseHelper = new DatabaseHelper();
                DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
                if (dt.Rows.Count > 0)
                {
                    return Convert.ToDecimal(dt.Rows[0][0]);
                }
            }
            return -99999;
        }

        /// <summary>
        /// 获取原始值
        /// </summary>
        /// <param name="applicationUID"></param>
        /// <param name="DataDateTime"></param>
        /// <param name="PointId"></param>
        /// <param name="factorCode"></param>
        /// <returns></returns>
        public decimal GetSourcePolltantValueLijingpuAPS(string applicationUID, DateTime DataDateTime, int PointId, string factorCode, string pollutant)
        {
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))
            {
                string sql = "select [" + factorCode + "] from [AMS_AirAutoMonitor].[dbo].[TB_SuperStation_lijingpu_L] where DateTime='" + DataDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "' and pointid=" + PointId;
                DatabaseHelper g_DatabaseHelper = new DatabaseHelper();
                DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
                if (dt.Rows.Count > 0)
                {
                    return Convert.ToDecimal(dt.Rows[0][0]);
                }
            }
            return -99999;
        }

        /// <summary>
        /// 获取原始值
        /// </summary>
        /// <param name="applicationUID"></param>
        /// <param name="DataDateTime"></param>
        /// <param name="PointId"></param>
        /// <param name="factorCode"></param>
        /// <returns></returns>
        public decimal GetSourcePolltantValueLijingpuL72(string applicationUID, DateTime DataDateTime, int PointId, string factorCode, string pollutant)
        {
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))
            {
                string sql = "select [" + factorCode + "] from [AMS_AirAutoMonitor].[dbo].[TB_SuperStation_lijingpu_M] where DateTime='" + DataDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "' and pointid=" + PointId;
                DatabaseHelper g_DatabaseHelper = new DatabaseHelper();
                DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
                if (dt.Rows.Count > 0)
                {
                    return Convert.ToDecimal(dt.Rows[0][0]);
                }
            }
            return -99999;
        }

        #region 空气
        public DomainModel.AirAutoMonitoring.InfectantBy60Entity InfectantBy60AirInfo(DateTime time, int pointID, string factorCode)
        {
            DomainModel.AirAutoMonitoring.InfectantBy60Entity infec60 = Infec60AirRep.RetrieveFirstOrDefault(x => x.Tstamp == time && x.PointId == pointID && x.PollutantCode == factorCode);
            return infec60;
        }

        /// <summary>
        /// 是否存在原始数据
        /// </summary>
        /// <param name="pointID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public bool IsExitInfectantBy60Air(int pointID, DateTime beginTime, DateTime endTime)
        {
            return Infec60AirRep.RetrieveCount(x => x.Tstamp >= beginTime && x.Tstamp < endTime && x.PointId == pointID) > 0;
        }
        #endregion

        #region 地表水
        public DomainModel.WaterAutoMonitoring.InfectantBy60Entity InfectantBy60WaterInfo(DateTime time, int pointID, string factorCode)
        {
            DomainModel.WaterAutoMonitoring.InfectantBy60Entity infec60 = Infec60WaterRep.RetrieveFirstOrDefault(x => x.Tstamp == time && x.PointId == pointID && x.PollutantCode == factorCode);
            return infec60;
        }

        /// <summary>
        /// 是否存在原始数据
        /// </summary>
        /// <param name="pointID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public bool IsExitInfectantBy60Water(int pointID, DateTime beginTime, DateTime endTime)
        {
            return Infec60WaterRep.RetrieveCount(x => x.Tstamp >= beginTime && x.Tstamp < endTime && x.PointId == pointID) > 0;

        }
        #endregion
        #endregion

        #region << 上报数据生成 >>
        public bool CreateExportFile(DateTime DTdBegin, DateTime DTdEnd, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                #region  << 无锡 >>
                //CreateGuoKongAQIHourExportDBFFile(DTdBegin, DTdEnd);
                //CreateGuoKongAQIDayExportExcelFile(DTdBegin, DTdEnd);
                #endregion

                #region  << 苏州 >>
                CreateGuoKongAQIHourExportDBFFile(DTdBegin, DTdEnd);
                CreateGuoKongAQIDayExportDBFFile(DTdBegin, DTdEnd);
                #endregion
            }
            catch (Exception err)
            {
                errMsg = err.Message;
                return false;
            }
            return true;

            #region << 原有 >>
            //AuditDataDAL AuditDataDAL = new AuditDataDAL();
            //BasicInformationDAL BasicInformationDAL = new BasicInformationDAL();
            ////DateTime DTdBegin = datetime;
            ////DateTime DTdEnd = DTdBegin.AddDays(1);
            //#region 生成上报文件
            //try
            //{

            //    AuditDataDAL auditDataDAL = new AuditDataDAL();
            //    MonitoringPointAirService monitoringPointAirService = new MonitoringPointAirService();
            //    string[] portIdsGuoKong = monitoringPointAirService.Retrieve(x => x.MonitoringPointExtensionForEQMSAirEntity.StateUploadOrNot == true).Select(x => x.PointId.ToString()).ToArray<String>();
            //    string[] portIdsShengKong = monitoringPointAirService.Retrieve(x => x.MonitoringPointExtensionForEQMSAirEntity.ProvincialUploadOrNot == true).Select(x => x.PointId.ToString()).ToArray<String>();

            //    for (DateTime CurrentDT = DTdBegin; CurrentDT < DTdEnd; CurrentDT = CurrentDT.AddDays(1))
            //    {
            //        String appPath = HttpRuntime.AppDomainAppPath;
            //        String tempFilePath = ConfigurationManager.AppSettings["TempPath_Hour"].ToString();
            //        String dataFliePath = ConfigurationManager.AppSettings["DataPath_Hour"].ToString();
            //        String templateFilePath = ConfigurationManager.AppSettings["DBFTemplatePath_AQIHour"].ToString();

            //        //国控点小时DBF
            //        //系统配置路径
            //        //临时文件路径
            //        tempFilePath = appPath + "\\" + tempFilePath;
            //        //上报文件路径
            //        dataFliePath = appPath + "\\" + dataFliePath;
            //        //模板路径
            //        templateFilePath = appPath + templateFilePath;
            //        string dataType = "AQIHour";
            //        DataView data = AuditDataDAL.getDataViewJS(CurrentDT, portIdsGuoKong);
            //        string uploadName = ConfigurationManager.AppSettings["StateUploadName_hour"].ToString();
            //        CreateUploadDBF(dataType, CurrentDT, data, tempFilePath, dataFliePath, templateFilePath, uploadName);

            //        //省控点小时DBF
            //        dataType = "AQIHour";
            //        data = AuditDataDAL.getDataViewXJS(CurrentDT, portIdsShengKong);
            //        uploadName = ConfigurationManager.AppSettings["ProvincialUploadName_hour"].ToString();
            //        CreateUploadDBF(dataType, CurrentDT, data, tempFilePath, dataFliePath, templateFilePath, uploadName);

            //        //国控点日DBF
            //        tempFilePath = ConfigurationManager.AppSettings["TempPath_Day"].ToString();
            //        dataFliePath = ConfigurationManager.AppSettings["DataPath_Day"].ToString();
            //        templateFilePath = ConfigurationManager.AppSettings["DBFTemplatePath_AQIDay"].ToString();
            //        //临时文件路径
            //        tempFilePath = appPath + "\\" + tempFilePath;
            //        //上报文件路径
            //        dataFliePath = appPath + "\\" + dataFliePath;
            //        //模板路径
            //        templateFilePath = appPath + templateFilePath;
            //        dataType = "AQIDay";
            //        data = AuditDataDAL.GetAQIDataSZ(CurrentDT);
            //        uploadName = ConfigurationManager.AppSettings["StateUploadName_Day"].ToString();
            //        CreateUploadDBF(dataType, CurrentDT, data, tempFilePath, dataFliePath, templateFilePath, uploadName);

            //        //Excel
            //        templateFilePath = ConfigurationManager.AppSettings["ExcelTemplatePath_AQIDay"].ToString();
            //        //模板路径
            //        templateFilePath = appPath + templateFilePath;
            //        String excelFileName = ConfigurationManager.AppSettings["StateUploadName_Day_Excel"].ToString();
            //        excelFileName = string.Format(excelFileName, CurrentDT.ToString("yyyyMMdd"));//文件名
            //        tempFilePath = tempFilePath + @"\" + excelFileName;
            //        String excelSavePath = dataFliePath + @"\" + excelFileName;//完全路径
            //        data = AuditDataDAL.getAQIDataWX(CurrentDT).DefaultView;
            //        if (ExportExcel(CurrentDT, data, tempFilePath, templateFilePath))
            //        {
            //            excelSavePath = dataFliePath + "\\" + CurrentDT.ToString("yyyy") + "\\" + CurrentDT.ToString("MM");// 日报地址
            //            if (!Directory.Exists(excelSavePath)) Directory.CreateDirectory(excelSavePath); //如果不存在则创建文件夹
            //            File.Copy(tempFilePath, excelSavePath + "\\" + excelFileName, true);
            //            File.Delete(tempFilePath);
            //        }
            //        data.Dispose();
            //    }
            //}
            //catch (Exception err)
            //{
            //    errMsg = err.Message;
            //    return false;
            //}
            //#endregion
            #endregion
        }

        /// <summary>
        /// 生成国控点日上报数据Excel
        /// </summary>
        /// <param name="DTdBegin"></param>
        /// <param name="DTdEnd"></param>
        /// <param name="IsAbsolutePath">是否使用绝对路径</param>
        public void CreateGuoKongAQIDayExportExcelFile(DateTime DTdBegin, DateTime DTdEnd, bool IsAbsolutePath = false)
        {
            try
            {
                AuditDataDAL auditDataDAL = new AuditDataDAL();

                for (DateTime CurrentDT = DTdBegin; CurrentDT <= DTdEnd; CurrentDT = CurrentDT.AddDays(1))
                {
                    //国控点日
                    String tempFilePath = ConfigurationManager.AppSettings["TempPath_Day"].ToString();
                    String dataFliePath = ConfigurationManager.AppSettings["DataPath_Day"].ToString();
                    //Excel
                    String templateFilePath = ConfigurationManager.AppSettings["ExcelTemplatePath_AQIDay"].ToString();

                    if (!IsAbsolutePath)
                    {
                        String appPath = HttpRuntime.AppDomainAppPath;
                        //临时文件路径
                        tempFilePath = appPath + "\\" + tempFilePath;
                        //上报文件路径
                        dataFliePath = appPath + "\\" + dataFliePath;
                        //模板路径
                        templateFilePath = appPath + templateFilePath;
                    }
                    String excelFileName = ConfigurationManager.AppSettings["StateUploadName_Day_Excel"].ToString();
                    excelFileName = string.Format(excelFileName, CurrentDT.ToString("yyyyMMdd"));//文件名
                    tempFilePath = tempFilePath + @"\" + excelFileName;
                    String excelSavePath = dataFliePath + @"\" + excelFileName;//完全路径
                    DataView data = auditDataDAL.getAQIDataWX(CurrentDT).DefaultView;
                    if (ExportExcel(CurrentDT, data, tempFilePath, templateFilePath))
                    {
                        excelSavePath = dataFliePath + "\\" + CurrentDT.ToString("yyyy") + "\\" + CurrentDT.ToString("MM");// 日报地址
                        if (!Directory.Exists(excelSavePath)) Directory.CreateDirectory(excelSavePath); //如果不存在则创建文件夹
                        File.Copy(tempFilePath, excelSavePath + "\\" + excelFileName, true);
                        File.Delete(tempFilePath);
                    }
                    data.Dispose();
                }
            }
            catch (Exception err)
            {
                throw err;
            }

        }

        /// <summary>
        /// 生成国控点日上报数据DBF
        /// </summary>
        /// <param name="DTdBegin"></param>
        /// <param name="DTdEnd"></param>
        /// <param name="IsAbsolutePath">是否使用绝对路径</param>
        public void CreateGuoKongAQIDayExportDBFFile(DateTime DTdBegin, DateTime DTdEnd, bool IsAbsolutePath = false)
        {
            try
            {
                AuditDataDAL auditDataDAL = new AuditDataDAL();

                for (DateTime CurrentDT = DTdBegin; CurrentDT <= DTdEnd; CurrentDT = CurrentDT.AddDays(1))
                {
                    //国控点日DBF
                    String tempFilePath = ConfigurationManager.AppSettings["TempPath_Day"].ToString();
                    String dataFliePath = ConfigurationManager.AppSettings["DataPath_Day"].ToString();
                    String templateFilePath = ConfigurationManager.AppSettings["DBFTemplatePath_AQIDay"].ToString();
                    if (!IsAbsolutePath)
                    {
                        String appPath = HttpRuntime.AppDomainAppPath;
                        //临时文件路径
                        tempFilePath = appPath + "\\" + tempFilePath;
                        //上报文件路径
                        dataFliePath = appPath + "\\" + dataFliePath;
                        //模板路径
                        templateFilePath = appPath + templateFilePath;
                    }
                    String dataType = "AQIDay";
                    string dataName = "GK";
                    DataView data = auditDataDAL.GetAQIDataSZ(CurrentDT);
                    String uploadName = ConfigurationManager.AppSettings["StateUploadName_Day"].ToString();
                    CreateUploadDBF(dataName, dataType, CurrentDT, data, tempFilePath, dataFliePath, templateFilePath, uploadName);

                    data.Dispose();
                }
            }
            catch (Exception err)
            {
                throw err;
            }
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
        /// 生成省控点上报数据
        /// </summary>
        /// <param name="DTdBegin"></param>
        /// <param name="DTdEnd"></param>
        /// <param name="IsAbsolutePath">是否使用绝对路径</param>
        public string CreateShengKongAQIHourExportDBFFile(DateTime DTdBegin, DateTime DTdEnd, bool IsAbsolutePath = false)
        {
            string nameTemp = string.Empty;
            string dbfStr = string.Empty;
            try
            {
                AuditDataDAL auditDataDAL = new AuditDataDAL();
                MonitoringPointAirService monitoringPointAirService = new MonitoringPointAirService();
                //  string[] portIdsShengKong = monitoringPointAirService.Retrieve(x => x.MonitoringPointExtensionForEQMSAirEntity.ProvincialUploadOrNot == true).Select(x => x.PointId.ToString()).ToArray<String>();
                string[] portIdsShengKong = { "10", "11", "12", "13", "14", "15", "16", "17", "19", "20", "21", "22" };
                for (DateTime CurrentDT = DTdBegin; CurrentDT <= DTdEnd; CurrentDT = CurrentDT.AddDays(1))
                {
                    String tempFilePath = ConfigurationManager.AppSettings["TempPath_Hour"].ToString();
                    String dataFliePath = ConfigurationManager.AppSettings["DataPath_Hour"].ToString();
                    String templateFilePath = ConfigurationManager.AppSettings["DBFTemplatePathS_AQIHour"].ToString();
                    if (!IsAbsolutePath)
                    {
                        String appPath = HttpRuntime.AppDomainAppPath;
                        //系统配置路径
                        //临时文件路径
                        tempFilePath = appPath + "\\" + tempFilePath;
                        //上报文件路径
                        dataFliePath = appPath + "\\" + dataFliePath;
                        //模板路径
                        templateFilePath = appPath + templateFilePath;
                    }

                    //省控点小时DBF
                    string dataType = "AQIHour";
                    string dataName = "SK";
                    DataTable dt = auditDataDAL.getDataViewXJS(CurrentDT, portIdsShengKong).ToTable();
                    dt.Columns.Add("Number", typeof(int));
                    for (int j = 0; j < dt.Rows.Count; j++)//给DataType、portName字段赋值
                    {
                        int pointId = Convert.ToInt32(dt.Rows[j]["PointId"]);
                        switch (pointId)
                        {
                            case 15:
                                dt.Rows[j]["Number"] = 1;
                                dt.Rows[j]["DWNAME"] = "兴福子站";
                                break;
                            case 14:
                                dt.Rows[j]["Number"] = 2;
                                dt.Rows[j]["DWNAME"] = "菱塘子站";
                                break;
                            case 13:
                                dt.Rows[j]["Number"] = 3;
                                dt.Rows[j]["DWNAME"] = "海虞子站";
                                break;
                            case 21:
                                dt.Rows[j]["Number"] = 4;
                                dt.Rows[j]["DWNAME"] = "张家港市监测站";
                                break;
                            case 22:
                                dt.Rows[j]["Number"] = 5;
                                dt.Rows[j]["DWNAME"] = "城北小学";
                                break;
                            case 19:
                                dt.Rows[j]["Number"] = 6;
                                dt.Rows[j]["DWNAME"] = "昆山第二中学";
                                break;
                            case 20:
                                dt.Rows[j]["Number"] = 7;
                                dt.Rows[j]["DWNAME"] = "震川中学";
                                break;
                            case 10:
                                dt.Rows[j]["Number"] = 8;
                                dt.Rows[j]["DWNAME"] = "吴江环保局";
                                break;
                            case 11:
                                dt.Rows[j]["Number"] = 9;
                                dt.Rows[j]["DWNAME"] = "教师进修学校";
                                break;
                            case 12:
                                dt.Rows[j]["Number"] = 10;
                                dt.Rows[j]["DWNAME"] = "吴江开发区";
                                break;
                            case 16:
                                dt.Rows[j]["Number"] = 11;
                                dt.Rows[j]["DWNAME"] = "太仓监测站";
                                break;
                            case 17:
                                dt.Rows[j]["Number"] = 12;
                                dt.Rows[j]["DWNAME"] = "科教新城实小";
                                break;
                        }
                        nameTemp = dt.Rows[j]["DWNAME"].ToString();

                    }
                    DataView data = dt.DefaultView;
                    data.Sort = "Number,Tstamp";
                    string uploadName = ConfigurationManager.AppSettings["ProvincialUploadName_hour"].ToString();

                    if (CreateUploadDBF(dataName, dataType, CurrentDT, data, tempFilePath, dataFliePath, templateFilePath, uploadName))
                    {
                        data.Dispose();
                        dbfStr = "";
                    }
                    else
                        dbfStr = nameTemp + "DBF生成失败!";
                }
            }
            catch (Exception err)
            {
                WriteTextLog(nameTemp + "生成省控点上报数据", err.Message, DateTime.Now);
                return nameTemp + "DBF生成失败!";
            }
            return dbfStr;
        }
        /// <summary>
        /// 生成省控点上报数据
        /// </summary>
        /// <param name="DTdBegin"></param>
        /// <param name="DTdEnd"></param>
        /// <param name="IsAbsolutePath">是否使用绝对路径</param>
        public string CreateSKAQIHourExportDBFFile(DateTime DTdBegin, DateTime DTdEnd, bool IsAbsolutePath = false)
        {
            string nameTemp = string.Empty;
            string dbfStr = string.Empty;
            try
            {
                AuditDataDAL auditDataDAL = new AuditDataDAL();
                MonitoringPointAirService monitoringPointAirService = new MonitoringPointAirService();
                //  string[] portIdsShengKong = monitoringPointAirService.Retrieve(x => x.MonitoringPointExtensionForEQMSAirEntity.ProvincialUploadOrNot == true).Select(x => x.PointId.ToString()).ToArray<String>();
                string[] portIdsShengKong = { "26", "27", "28", "29", "33", "168", "179", "180", "181", "176", "25", "177", "178", "172", "173", "174", "175", "184", "182", "183" };
                for (DateTime CurrentDT = DTdBegin; CurrentDT <= DTdEnd; CurrentDT = CurrentDT.AddDays(1))
                {
                    String tempFilePath = ConfigurationManager.AppSettings["TempPathDBF_Hour"].ToString();
                    String dataFliePath = ConfigurationManager.AppSettings["DataPathDBF_Hour"].ToString();
                    String templateFilePath = ConfigurationManager.AppSettings["DBFTemplatePathS_AQIHour"].ToString();
                    if (!IsAbsolutePath)
                    {
                        String appPath = HttpRuntime.AppDomainAppPath;
                        //系统配置路径
                        //临时文件路径
                        tempFilePath = appPath + "\\" + tempFilePath;
                        //上报文件路径
                        dataFliePath = appPath + "\\" + dataFliePath;
                        //模板路径
                        templateFilePath = appPath + templateFilePath;
                    }

                    //省控点小时DBF
                    string dataType = "AQIHour";
                    string dataName = "SZ";
                    DataTable dt = auditDataDAL.getDataViewXJS(CurrentDT, portIdsShengKong).ToTable();
                    dt.Columns.Add("Number", typeof(int));
                    for (int j = 0; j < dt.Rows.Count; j++)//给DataType、portName字段赋值
                    {
                        int pointId = Convert.ToInt32(dt.Rows[j]["PointId"]);
                        switch (pointId)
                        {
                            case 29:
                                dt.Rows[j]["Number"] = 1;
                                dt.Rows[j]["DWNAME"] = "东山站";
                                break;
                            case 28:
                                dt.Rows[j]["Number"] = 2;
                                dt.Rows[j]["DWNAME"] = "方洲公园";
                                break;
                            case 27:
                                dt.Rows[j]["Number"] = 3;
                                dt.Rows[j]["DWNAME"] = "昆山花桥";
                                break;
                            case 166:
                                dt.Rows[j]["Number"] = 4;
                                dt.Rows[j]["DWNAME"] = "国土分局子站";
                                break;
                            case 26:
                                dt.Rows[j]["Number"] = 5;
                                dt.Rows[j]["DWNAME"] = "文昌中学";
                                break;
                            case 33:
                                dt.Rows[j]["Number"] = 6;
                                dt.Rows[j]["DWNAME"] = "拙政园";
                                break;
                            case 179:
                                dt.Rows[j]["Number"] = 7;
                                dt.Rows[j]["DWNAME"] = "东南开发区子站";
                                break;
                            case 180:
                                dt.Rows[j]["Number"] = 8;
                                dt.Rows[j]["DWNAME"] = "氟化工业园";
                                break;
                            case 181:
                                dt.Rows[j]["Number"] = 9;
                                dt.Rows[j]["DWNAME"] = "沿江开发区";
                                break;
                            case 176:
                                dt.Rows[j]["Number"] = 10;
                                dt.Rows[j]["DWNAME"] = "乐余广电站";
                                break;
                            case 25:
                                dt.Rows[j]["Number"] = 11;
                                dt.Rows[j]["DWNAME"] = "张家港农业示范园";
                                break;
                            case 177:
                                dt.Rows[j]["Number"] = 12;
                                dt.Rows[j]["DWNAME"] = "托普学院";
                                break;
                            case 178:
                                dt.Rows[j]["Number"] = 13;
                                dt.Rows[j]["DWNAME"] = "淀山湖党校";
                                break;
                            case 172:
                                dt.Rows[j]["Number"] = 14;
                                dt.Rows[j]["DWNAME"] = "太仓三水厂";
                                break;
                            case 173:
                                dt.Rows[j]["Number"] = 15;
                                dt.Rows[j]["DWNAME"] = "太仓气象观测站";
                                break;
                            case 174:
                                dt.Rows[j]["Number"] = 16;
                                dt.Rows[j]["DWNAME"] = "双凤生态园";
                                break;
                            case 175:
                                dt.Rows[j]["Number"] = 17;
                                dt.Rows[j]["DWNAME"] = "荣文学校";
                                break;
                            case 184:
                                dt.Rows[j]["Number"] = 18;
                                dt.Rows[j]["DWNAME"] = "青剑湖";
                                break;
                            case 182:
                                dt.Rows[j]["Number"] = 19;
                                dt.Rows[j]["DWNAME"] = "苏州大学高教区";
                                break;
                            case 183:
                                dt.Rows[j]["Number"] = 20;
                                dt.Rows[j]["DWNAME"] = "东部工业区";
                                break;
                        }
                        nameTemp = dt.Rows[j]["DWNAME"].ToString();

                    }
                    DataView data = dt.DefaultView;
                    data.Sort = "Number,Tstamp";
                    string uploadName = ConfigurationManager.AppSettings["ProvincialUploadNameDBF_hour"].ToString();

                    if (CreateUploadDBF(dataName, dataType, CurrentDT, data, tempFilePath, dataFliePath, templateFilePath, uploadName))
                    {
                        data.Dispose();
                        dbfStr = "";
                    }
                    else
                        dbfStr = nameTemp + "DBF生成失败!";
                }
            }
            catch (Exception err)
            {
                WriteTextLog(nameTemp + "生成省控点上报数据", err.Message, DateTime.Now);
                return nameTemp + "DBF生成失败!";
            }
            return dbfStr;
        }
        /// <summary>
        /// 生成省控点日上报数据
        /// </summary>
        /// <param name="DTdBegin"></param>
        /// <param name="DTdEnd"></param>
        /// <param name="IsAbsolutePath">是否使用绝对路径</param>
        public string CreateShengKongAQIDayExportDBFFile(DateTime DTdBegin, DateTime DTdEnd, bool IsAbsolutePath = false)
        {
            string DBFstr = string.Empty;
            try
            {
                AuditDataDAL auditDataDAL = new AuditDataDAL();
                MonitoringPointAirService monitoringPointAirService = new MonitoringPointAirService();
                // string[] portIdsShengKong = monitoringPointAirService.Retrieve(x => x.MonitoringPointExtensionForEQMSAirEntity.ProvincialUploadOrNot == true).Select(x => x.PointId.ToString()).ToArray<String>();
                string[] portIdsShengKong = { "10", "11", "12", "13", "14", "15", "16", "17", "19", "20", "21", "22" };
                for (DateTime CurrentDT = DTdBegin; CurrentDT <= DTdEnd; CurrentDT = CurrentDT.AddDays(1))
                {
                    String tempFilePath = ConfigurationManager.AppSettings["TempPath_Day"].ToString();
                    String dataFliePath = ConfigurationManager.AppSettings["DataPath_Day"].ToString();
                    String templateFilePath = ConfigurationManager.AppSettings["DBFTemplatePathS_AQIDay"].ToString();
                    if (!IsAbsolutePath)
                    {
                        String appPath = HttpRuntime.AppDomainAppPath;
                        //系统配置路径
                        //临时文件路径
                        tempFilePath = appPath + "\\" + tempFilePath;
                        //上报文件路径
                        dataFliePath = appPath + "\\" + dataFliePath;
                        //模板路径
                        templateFilePath = appPath + templateFilePath;
                    }

                    //省控点小时DBF
                    string dataType = "AQIDay";
                    string dataName = "SK";
                    DataTable dt = auditDataDAL.getDataViewDayXJS(CurrentDT, portIdsShengKong).ToTable();
                    dt.Columns.Add("Number", typeof(int));
                    for (int j = 0; j < dt.Rows.Count; j++)//给DataType、portName字段赋值
                    {
                        int pointId = Convert.ToInt32(dt.Rows[j]["PointId"]);
                        switch (pointId)
                        {
                            case 15:
                                dt.Rows[j]["Number"] = 1;
                                dt.Rows[j]["DWNAME"] = "兴福子站";
                                break;
                            case 14:
                                dt.Rows[j]["Number"] = 2;
                                dt.Rows[j]["DWNAME"] = "菱塘子站";
                                break;
                            case 13:
                                dt.Rows[j]["Number"] = 3;
                                dt.Rows[j]["DWNAME"] = "海虞子站";
                                break;
                            case 21:
                                dt.Rows[j]["Number"] = 4;
                                dt.Rows[j]["DWNAME"] = "张家港市监测站";
                                break;
                            case 22:
                                dt.Rows[j]["Number"] = 5;
                                dt.Rows[j]["DWNAME"] = "城北小学";
                                break;
                            case 19:
                                dt.Rows[j]["Number"] = 6;
                                dt.Rows[j]["DWNAME"] = "昆山第二中学";
                                break;
                            case 20:
                                dt.Rows[j]["Number"] = 7;
                                dt.Rows[j]["DWNAME"] = "震川中学";
                                break;
                            case 10:
                                dt.Rows[j]["Number"] = 8;
                                dt.Rows[j]["DWNAME"] = "吴江环保局";
                                break;
                            case 11:
                                dt.Rows[j]["Number"] = 9;
                                dt.Rows[j]["DWNAME"] = "教师进修学校";
                                break;
                            case 12:
                                dt.Rows[j]["Number"] = 10;
                                dt.Rows[j]["DWNAME"] = "吴江开发区";
                                break;
                            case 16:
                                dt.Rows[j]["Number"] = 11;
                                dt.Rows[j]["DWNAME"] = "太仓监测站";
                                break;
                            case 17:
                                dt.Rows[j]["Number"] = 12;
                                dt.Rows[j]["DWNAME"] = "科教新城实小";
                                break;
                        }
                    }
                    DataView data = dt.DefaultView;
                    data.Sort = "Number,Tstamp";
                    string uploadName = ConfigurationManager.AppSettings["ProvincialUploadName_Day"].ToString();
                    if (CreateUploadDBF(dataName, dataType, CurrentDT, data, tempFilePath, dataFliePath, templateFilePath, uploadName))
                    {
                        data.Dispose();
                    }
                    else
                    {
                        DBFstr = "DBF生成报错";
                    }
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            return DBFstr;
        }
        /// <summary>
        /// 生成国控点上报数据
        /// </summary>
        /// <param name="DTdBegin"></param>
        /// <param name="DTdEnd"></param>
        /// <param name="IsAbsolutePath">是否使用绝对路径</param>
        public void CreateGuoKongAQIHourExportDBFFile(DateTime DTdBegin, DateTime DTdEnd, bool IsAbsolutePath = false)
        {
            try
            {
                AuditDataDAL auditDataDAL = new AuditDataDAL();
                MonitoringPointAirService monitoringPointAirService = new MonitoringPointAirService();
                string[] portIdsGuoKong = monitoringPointAirService.Retrieve(x => x.MonitoringPointExtensionForEQMSAirEntity.StateUploadOrNot == true).Select(x => x.PointId.ToString()).ToArray<String>();

                for (DateTime CurrentDT = DTdBegin; CurrentDT <= DTdEnd; CurrentDT = CurrentDT.AddDays(1))
                {
                    String tempFilePath = ConfigurationManager.AppSettings["TempPath_Hour"].ToString();
                    String dataFliePath = ConfigurationManager.AppSettings["DataPath_Hour"].ToString();
                    String templateFilePath = ConfigurationManager.AppSettings["DBFTemplatePath_AQIHour"].ToString();
                    if (!IsAbsolutePath)
                    {
                        String appPath = HttpRuntime.AppDomainAppPath;
                        //国控点小时DBF
                        //系统配置路径
                        //临时文件路径
                        tempFilePath = appPath + "\\" + tempFilePath;
                        //上报文件路径
                        dataFliePath = appPath + "\\" + dataFliePath;
                        //模板路径
                        templateFilePath = appPath + templateFilePath;
                    }
                    string dataType = "AQIHour";
                    string dataName = "GK";
                    DataView data = auditDataDAL.getDataViewJS(CurrentDT, portIdsGuoKong);
                    string uploadName = ConfigurationManager.AppSettings["StateUploadName_hour"].ToString();
                    CreateUploadDBF(dataName, dataType, CurrentDT, data, tempFilePath, dataFliePath, templateFilePath, uploadName);

                    data.Dispose();
                }
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        /// <summary>
        /// 生成DBF文件
        /// </summary>
        /// <param name="dataType">
        /// 数据类型：AQIDay、AQIHour
        /// </param>
        /// <param name="CurrentDT">日期</param>
        /// <param name="data">数据</param>
        /// <param name="tempFilePath">临时文件路径</param>
        /// <param name="dataFliePath">上报文件路径</param>
        /// <param name="templateFilePath">模板文件路径</param>
        /// <param name="StateUploadName">文件前缀</param>
        /// <returns></returns>
        public bool CreateUploadDBF(String dataName, String dataType, DateTime CurrentDT, DataView data, String tempFilePath, String dataFliePath, String templateFilePath, String StateUploadName)
        {
            try
            {

                //如果不存在则创建文件夹
                if (!Directory.Exists(tempFilePath))
                    Directory.CreateDirectory(tempFilePath);
                if (!Directory.Exists(dataFliePath))
                    Directory.CreateDirectory(dataFliePath);

                //文件名
                String DBFFileName = dataName + CurrentDT.ToString("yyMMdd") + ".DBF";
                //完全路径
                String DBFSavePath = tempFilePath + @"\" + DBFFileName;

                if (CreateDBF(dataType, data, templateFilePath, tempFilePath, DBFFileName, DBFSavePath))
                {
                    StateUploadName = "\\" + StateUploadName + CurrentDT.ToString("yyyyMMdd") + ".DBF";
                    dataFliePath = dataFliePath + "\\" + CurrentDT.ToString("yyyy") + "\\" + CurrentDT.ToString("MM");// +"\\省站";//文件夹名称
                    if (!Directory.Exists(dataFliePath)) Directory.CreateDirectory(dataFliePath); //如果不存在则创建文件夹
                    File.Copy(DBFSavePath, dataFliePath + StateUploadName, true);
                    File.Delete(DBFSavePath);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                WriteTextLog("生成DBF文件", ex.Message, DateTime.Now);
                return false;
            }
        }

        #endregion
        /// <summary>   
        /// Datatable转换为Json   
        /// </summary>   
        /// <param name="table">Datatable对象</param>   
        /// <returns>Json字符串</returns>   
        public string ToJsonBySplitJoint(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    string strValue = drc[i][j].ToString();
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            if (jsonString.ToString() != "[") jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }
        /// <summary>  
        /// 格式化字符型、日期型、布尔型  
        /// </summary>  
        /// <param name="str"></param>  
        /// <param name="type"></param>  
        /// <returns></returns>  
        private string StringFormat(string str, Type type)
        {
            if (type == typeof(string) || type == typeof(Guid))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            else if (string.IsNullOrEmpty(str))
            {
                str = "null";
            }
            return str;
        }

        /// <summary>  
        /// 过滤特殊字符  
        /// </summary>  
        /// <param name="s"></param>  
        /// <returns></returns>  
        private string String2Json(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }
        #region DBF生成
        public Boolean CreateDBF(String DataType, DataView myView, String TemplatePath, String FolderPath, String FileName, String SavePath)
        {
            try
            {
                if (File.Exists(SavePath)) File.Delete(SavePath);//删除现在有文件
                File.Copy(TemplatePath, SavePath, true);//不覆盖文件更改为false
            }
            catch (Exception err)
            {
                WriteTextLog("DBF生成", err.Message, DateTime.Now);
                return false;
            }
            WriteTextLog("DBF生成数据：", ToJsonBySplitJoint(myView.ToTable()), DateTime.Now);
            #region 写DBF数据库
            String strConnectionString = "Provider=Microsoft.JET.OLEDB.4.0;Extended Properties=dBASE IV;data source=" + FolderPath;
            String STCODE, STNAME, YYYY, MM, DD, HH, DWCODE, DWNAME, SO2, NO, NO2, NOX, PM10, PM25, O3, CO, AQI, SYWRW, KQZLZSJB, LB, TEMP, RH, PRESS, WD, WS, VISIBILITY;
            try
            {
                for (int i = 0; i < myView.Count; i++)
                {
                    #region 赋值
                    STCODE = myView[i]["STCODE"] != DBNull.Value ? myView[i]["STCODE"].ToString() : "";
                    STNAME = myView[i]["STNAME"] != DBNull.Value ? myView[i]["STNAME"].ToString() : "";
                    YYYY = myView[i]["YYYY"].ToString();
                    MM = myView[i]["MM"].ToString();
                    DD = myView[i]["DD"].ToString();
                    DWCODE = myView[i]["DWCODE"] != DBNull.Value ? myView[i]["DWCODE"].ToString() : "";
                    DWNAME = myView[i]["DWNAME"] != DBNull.Value ? myView[i]["DWNAME"].ToString() : "";
                    HH = "-1"; SO2 = "-1"; NO = "-1"; NO2 = "-1"; NOX = "-1"; PM10 = "-1"; CO = "-1"; O3 = "-1"; PM25 = "-1";
                    AQI = "-1"; SYWRW = "--"; KQZLZSJB = "-1"; LB = "-1"; TEMP = "-1"; RH = "-1"; PRESS = "-1"; WD = "-1"; WS = "-1"; VISIBILITY = "-1";

                    if (DataType == "AQIDay")
                    {
                        if (myView[i]["AQI"] != DBNull.Value && myView[i]["AQI"].ToString() != "") AQI = myView[i]["AQI"].ToString();
                        if (myView[i]["SYWRW"] != DBNull.Value && myView[i]["SYWRW"].ToString() != "") SYWRW = myView[i]["SYWRW"].ToString();
                        if (myView[i]["KQZLZSJB"] != DBNull.Value && myView[i]["KQZLZSJB"].ToString() != "") KQZLZSJB = myView[i]["KQZLZSJB"].ToString();
                        if (myView[i]["LB"] != DBNull.Value && myView[i]["LB"].ToString() != "") LB = myView[i]["LB"].ToString();
                    }
                    else if (DataType == "AQIHour")
                    {
                        HH = myView[i]["HH"].ToString();
                        if (myView[i]["SO2"] != DBNull.Value && myView[i]["SO2"].ToString() != "") SO2 = Convert.ToDouble(myView[i]["SO2"]).ToString("#0.000");
                        if (myView[i]["NO"] != DBNull.Value && myView[i]["NO"].ToString() != "") NO = Convert.ToDouble(myView[i]["NO"]).ToString("#0.000");
                        if (myView[i]["NO2"] != DBNull.Value && myView[i]["NO2"].ToString() != "") NO2 = Convert.ToDouble(myView[i]["NO2"]).ToString("#0.000");
                        if (myView[i]["NOX"] != DBNull.Value && myView[i]["NOX"].ToString() != "") NOX = Convert.ToDouble(myView[i]["NOX"]).ToString("#0.000");
                        if (myView[i]["PM10"] != DBNull.Value && myView[i]["PM10"].ToString() != "") PM10 = Convert.ToDouble(myView[i]["PM10"]).ToString("#0.000");
                        if (myView[i]["PM25"] != DBNull.Value && myView[i]["PM25"].ToString() != "") PM25 = Convert.ToDouble(myView[i]["PM25"]).ToString("#0.000");
                        if (myView[i]["CO"] != DBNull.Value && myView[i]["CO"].ToString() != "") CO = Convert.ToDouble(myView[i]["CO"]).ToString("#0.000");
                        if (myView[i]["O3"] != DBNull.Value && myView[i]["O3"].ToString() != "") O3 = Convert.ToDouble(myView[i]["O3"]).ToString("#0.000");
                        if (myView[i]["DWNAME"].ToString().Contains("南门"))
                        {
                            if (myView[i]["TEMP"] != DBNull.Value && myView[i]["TEMP"].ToString() != "") TEMP = Convert.ToDouble(myView[i]["TEMP"]).ToString("#0.0");
                            if (myView[i]["RH"] != DBNull.Value && myView[i]["RH"].ToString() != "") RH = Convert.ToDouble(myView[i]["RH"]).ToString();
                            if (myView[i]["PRESS"] != DBNull.Value && myView[i]["PRESS"].ToString() != "") PRESS = Convert.ToDouble(myView[i]["PRESS"]).ToString();
                            if (myView[i]["WD"] != DBNull.Value && myView[i]["WD"].ToString() != "") WD = Convert.ToDouble(myView[i]["WD"]).ToString();
                            if (myView[i]["WS"] != DBNull.Value && myView[i]["WS"].ToString() != "") WS = Convert.ToDouble(myView[i]["WS"]).ToString("#0.0");
                            if (myView[i]["VISIBILITY"] != DBNull.Value && myView[i]["VISIBILITY"].ToString() != "") VISIBILITY = Convert.ToDouble(myView[i]["VISIBILITY"]).ToString("#0.0");
                        }

                    }

                    DBF_Insert(DataType, strConnectionString, FileName, STCODE, STNAME, YYYY, MM, DD, HH, DWCODE, DWNAME, SO2, NO, NO2, NOX, PM10, PM25, CO, O3, AQI, SYWRW, KQZLZSJB, LB, TEMP, RH, PRESS, WD, WS, VISIBILITY);

                    #endregion
                }
                return true;
            }
            catch (Exception err)
            {
                WriteTextLog("生成中", err.Message, DateTime.Now);
                return false;
            }
            #endregion
        }

        public void DBF_Insert(string DataType, string dbfConn, string tabName, string STCODE, string STNAME, string YYYY, string MM, string DD, string HH, string DWCODE, string DWNAME
            , string SO2, string NO, string NO2, string NOX, string PM10, string PM25, string CO, string O3, string AQI, string SYWRW, string KQZLZSJB, string LB, string TEMP, string RH, string PRESS, string WD, string WS, string VISIBILITY)
        {//表名最多8位
            OleDbConnection myConn = new OleDbConnection(dbfConn);
            OleDbCommand myCmd = new OleDbCommand();
            DataSet myST = new DataSet();
            switch (DataType)
            {
                case "AQIHour":
                    myCmd.CommandText = "INSERT INTO " + tabName.Replace(".DBF", "") + " ([STCODE],[STNAME],[YYYY],[MM],[DD],[HH],[DWCODE],[DWNAME],[SO2],[NO],[NO2],[NOX],[PM10],[PM25],[O3],[CO],[TEMP],[RH],[PRESS],[WD],[WS],[VISIBILITY])"
                                + " VALUES (@m_STCODE,@m_STNAME,@m_YYYY,@m_MM,@m_DD,@m_HH,@m_DWCODE,@m_DWNAME,@m_SO2,@m_NO,@m_NO2,@m_NOX,@m_PM10,@m_PM25,@m_O3,@m_CO,@m_TEMP,@m_RH,@m_PRESS,@m_WD,@m_WS,@m_VISIBILITY)";
                    break;
                case "AQIDay":
                    myCmd.CommandText = "INSERT INTO " + tabName.Replace(".DBF", "") + " (STCODE,STNAME,YYYY,MM,DD,DWCODE,DWNAME,AQI,SYWRW,KQZLZSJB,LB)"
                                + " VALUES (@m_STCODE,@m_STNAME,@m_YYYY,@m_MM,@m_DD,@m_DWCODE,@m_DWNAME,@m_AQI,@m_SYWRW,@m_KQZLZSJB,@m_LB)";//
                    break;
            }

            myCmd.Connection = myConn;
            myCmd.Parameters.Add(new OleDbParameter("@m_STCODE", OleDbType.VarChar, 6)).Value = STCODE;
            myCmd.Parameters.Add(new OleDbParameter("@m_STNAME", OleDbType.VarChar, 8)).Value = STNAME;

            myCmd.Parameters.Add(new OleDbParameter("@m_YYYY", OleDbType.VarChar, 4)).Value = YYYY;
            myCmd.Parameters.Add(new OleDbParameter("@m_MM", OleDbType.VarChar, 2)).Value = MM;
            myCmd.Parameters.Add(new OleDbParameter("@m_DD", OleDbType.VarChar, 2)).Value = DD;
            if (DataType.Equals("AQIDay"))
            {
                myCmd.Parameters.Add(new OleDbParameter("@m_DWCODE", OleDbType.VarChar, 4)).Value = DWCODE;
                myCmd.Parameters.Add(new OleDbParameter("@m_DWNAME", OleDbType.VarChar, 20)).Value = DWNAME;
                myCmd.Parameters.Add(new OleDbParameter("@m_AQI", OleDbType.VarChar, 8)).Value = AQI;
                myCmd.Parameters.Add(new OleDbParameter("@m_SYWRW", OleDbType.VarChar, 20)).Value = SYWRW;
                myCmd.Parameters.Add(new OleDbParameter("@m_KQZLZSJB", OleDbType.VarChar, 8)).Value = KQZLZSJB;
                myCmd.Parameters.Add(new OleDbParameter("@m_LB", OleDbType.VarChar, 8)).Value = LB;
            }
            else if (DataType.Equals("AQIHour"))
            {
                myCmd.Parameters.Add(new OleDbParameter("@m_HH", OleDbType.VarChar, 2)).Value = HH;//新增
                myCmd.Parameters.Add(new OleDbParameter("@m_DWCODE", OleDbType.VarChar, 4)).Value = DWCODE;
                myCmd.Parameters.Add(new OleDbParameter("@m_DWNAME", OleDbType.VarChar, 20)).Value = DWNAME;
                myCmd.Parameters.Add(new OleDbParameter("@m_SO2", OleDbType.VarChar, 8)).Value = SO2;
                myCmd.Parameters.Add(new OleDbParameter("@m_NO", OleDbType.VarChar, 8)).Value = NO;
                myCmd.Parameters.Add(new OleDbParameter("@m_NO2", OleDbType.VarChar, 8)).Value = NO2;
                myCmd.Parameters.Add(new OleDbParameter("@m_NOX", OleDbType.VarChar, 8)).Value = NOX;
                myCmd.Parameters.Add(new OleDbParameter("@m_PM10", OleDbType.VarChar, 8)).Value = PM10;
                myCmd.Parameters.Add(new OleDbParameter("@m_PM25", OleDbType.VarChar, 8)).Value = PM25;
                myCmd.Parameters.Add(new OleDbParameter("@m_O3", OleDbType.VarChar, 8)).Value = O3;
                myCmd.Parameters.Add(new OleDbParameter("@m_CO", OleDbType.VarChar, 8)).Value = CO;

                myCmd.Parameters.Add(new OleDbParameter("@m_TEMP", OleDbType.VarChar, 8)).Value = TEMP;
                myCmd.Parameters.Add(new OleDbParameter("@m_RH", OleDbType.VarChar, 8)).Value = RH;
                myCmd.Parameters.Add(new OleDbParameter("@m_PRESS", OleDbType.VarChar, 8)).Value = PRESS;
                myCmd.Parameters.Add(new OleDbParameter("@m_WD", OleDbType.VarChar, 8)).Value = WD;
                myCmd.Parameters.Add(new OleDbParameter("@m_WS", OleDbType.VarChar, 8)).Value = WS;
                myCmd.Parameters.Add(new OleDbParameter("@m_VISIBILITY", OleDbType.VarChar, 8)).Value = VISIBILITY;
            }

            try
            {
                myConn.Open();
                myCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                WriteTextLog("添加记录时产生错误，错误代码为：", e.Message, DateTime.Now);
                throw new Exception("添加记录时产生错误，错误代码为：" + e.ToString());
            }
            finally
            {
                myCmd.Dispose();
                myConn.Close();
                myConn.Dispose();
            }
        }
        #endregion
        #region EXCEL生成
        public bool ExportExcel(DateTime datetime, DataView myView, string DBFSavePath, string DBFTemplatePath_AQIHour)
        {
            try
            {
                WorkbookDesigner designer = new WorkbookDesigner();
                designer.Workbook = new Workbook();
                Cells cells;
                designer.Workbook.Open(DBFTemplatePath_AQIHour);

                cells = designer.Workbook.Worksheets[0].Cells;
                cells[1, 0].PutValue("日期：" + datetime.ToString("yyyy年MM月dd日"));
                int beginColumn = 1;
                int beginRow = 5;
                int columns = 20;
                int rows = 13;
                for (int i = beginRow; i <= rows; i++)
                {
                    beginColumn = 1;
                    for (int j = beginColumn; j <= columns; j++)
                    {
                        if ((i - 5) < myView.Count)
                        {
                            cells[i, j].PutValue(myView[i - 5][j - 1].ToString());
                        }
                    }
                }
                designer.Workbook.Save(DBFSavePath);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region << 数据审核 >>
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
            return g_AuditDataRepository.GenerateData(applicationType, portIds, dateStart, dateEnd, out errMsg);
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
            return g_AuditDataRepository.GenerateDataFromHourReport(applicationType, portIds, dateStart, dateEnd, out errMsg);
        }

        /// <summary>
        /// 生成区域审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="regionGuids">区域数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateDataRegion(ApplicationType applicationType, string[] regionGuids, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            return g_AuditDataRepository.GenerateDataRegion(applicationType, regionGuids, dateStart, dateEnd, out errMsg);
        }

        /// <summary>
        /// 生成城市审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="cityGuids">区域数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateDataCity(ApplicationType applicationType, string[] cityGuids, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            return g_AuditDataRepository.GenerateDataCity(applicationType, cityGuids, dateStart, dateEnd, out errMsg);
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
            return g_AuditDataRepository.GenerateDataFromDayReport(applicationType, portIds, dateStart, dateEnd, out errMsg);
        }

        /// <summary>
        /// 生成区域审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="regionGuids">区域数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateDataRegionFromDay(ApplicationType applicationType, string[] regionGuids, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            return g_AuditDataRepository.GenerateDataRegionFromDay(applicationType, regionGuids, dateStart, dateEnd, out errMsg);
        }

        /// <summary>
        /// 生成城市审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="cityGuids">区域数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateDataCityFromDay(ApplicationType applicationType, string[] cityGuids, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            return g_AuditDataRepository.GenerateDataCityFromDay(applicationType, cityGuids, dateStart, dateEnd, out errMsg);
        }
        #endregion

        #endregion
        /// <summary>
        /// 生成城市审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="cityGuids">区域数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public DataView getDataViewDayXJSNew(DateTime dtBegin, DateTime dtEnd, string[] portIds)
        {
            return g_AuditDataRepository.getDataViewDayXJSNew(dtBegin, dtEnd, portIds);
        }
        /// <summary>
        /// 生成城市审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="cityGuids">区域数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public DataView getDataViewSQDBF(DateTime dtBegin, string[] portIds)
        {
            AuditDataDAL auditDataDAL = new AuditDataDAL();
            return auditDataDAL.getDataViewSQDBF(dtBegin, portIds);
        }
        /// <summary>
        /// 审核提交
        /// </summary>
        /// <param name="applicationUID"></param>
        /// <param name="PointID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="CreatUser"></param>
        /// <param name="IsAuditSuggestion"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        public bool SubmitAudit(string[] PointID, DateTime beginTime, DateTime endTime)
        {
            AuditDataDAL auditDataDAL = new AuditDataDAL();
            bool isSuccess = true;
            #region 生成报表数据
            try
            {
                foreach (var point in PointID)
                {
                    auditDataDAL.SyncAuditAirInfectantByHourData(point, beginTime);
                }
                CreateSKAQIHourExportDBFFile(beginTime, endTime);
                return true;
            }
            catch
            {
                isSuccess = false;
            }
            #endregion
            return isSuccess;
        }
        /// <summary>
        /// 根据点位类型、时间段获取审核状态统计信息
        /// </summary>
        /// <param name="application">应用程序类型</param>
        /// <param name="point">点位因子</param>
        /// <param name="beginTime">时间</param>
        /// <returns></returns>
        public DataTable AuditFlagStatisticsByPointId(ApplicationValue application, string point, DateTime beginTime, DateTime endTime)
        {
            DataTable dt = new DataTable();
            dt = waterHourRep.AuditFlagStatisticsByPointId(EnumMapping.GetApplicationValue(application), point, beginTime, endTime);
            return dt;
        }

    }
}
