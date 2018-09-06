﻿using log4net;
using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Collections;

namespace NTDataProcessApplication
{
    /// <summary>
    /// 名称：Handle.aspx.cs
    /// 创建人：吕云
    /// 创建日期：2017-5-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：南通超级站数据解析
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class Handle
    {
        #region 变量
        ILog log = LogManager.GetLogger("App.Logging");//获取一个日志记录器
        //BaseDataModel BaseDataModel = new BaseDataModel();
        //AirAutoMonitorModel AirAutoMonitorModel = new AirAutoMonitorModel();
        //FrameworkModel FrameworkModel = new FrameworkModel();
        //MonitorBusinessModel MonitorBusinessModel = new MonitorBusinessModel();
        string ApplicationUid = "airaaira-aira-aira-aira-airaairaaira";
        string ApplicationName = "气";
        int DataDelayTime = Convert.ToInt32(ConfigurationManager.AppSettings["DataDelayTime"]);
        DAL d_DAL = new DAL();
        AQICalculateService d_AQICalculateService = new AQICalculateService();

        //获取数据库中所有因子
        string[] WSCodes = new BaseDataModel().SYS_Factors_Mappings.OrderBy(p => p.Id).Select(p => p.WSCode).ToArray();

        //int[] ConPointIds = new int[] {187,188,189,190 };

        string ConPointIds = "186,187,188,189,190";
        //获取所有站点
        int[] PointIds = new BaseDataModel().TB_MonitoringPoints.Select(p => p.PointId).ToArray();
        
        //常规站点
        int[] CommonPointIds = new BaseDataModel().V_Point_Airs.Where(p => p.EnableOrNot.Equals(true)).Where(p => p.SuperOrNot.Equals(false)).Select(p => p.PointId).ToArray();
        //超级站点
        int[] SuperPointIds = new BaseDataModel().V_Point_Airs.Where(p => p.EnableOrNot.Equals(true)).Where(p => p.SuperOrNot.Equals(true)).Select(p => p.PointId).ToArray();

        //获取数据类型
        V_CodeMainItem[] DataTypeCodeMainItems = new FrameworkModel().V_CodeMainItems.Where(p => p.MainGuid.Equals("0d24484d-d315-4f12-b2bc-f64552c4f6dd")).ToArray();

        //获取审核/报警字典表信息
        V_CodeMainItem[] FlagTypeCodeMainItems = new FrameworkModel().V_CodeMainItems.Where(p => p.MainGuid.Equals("3e8f63ea-64ea-4c23-8aba-c369129dde13")).ToArray();

        //获取报警类型字典表信息
        V_CodeMainItem[] AlarmTypes = new FrameworkModel().V_CodeMainItems.Where(p => p.MainGuid.Equals("7d5c609b-cc69-498f-a22e-183014c8f099")).ToArray();

        //获取空气点位区域类型Guid
        string[] m_regions = new FrameworkModel().V_CodeDictionaries.Where(p => p.CodeDictionaryName.Equals("空气点位区域类型") && p.CodeName.Equals("空气点位区域类型")).OrderByDescending(p => p.SortNumber).Select(p => p.ItemGuid).ToArray();

        string UserId = ConfigurationManager.AppSettings["UserId"];//调用接口用户名
        string Password = ConfigurationManager.AppSettings["Password"];//调用接口密码

        #endregion

        #region 常规站数据接入
        /// <summary>
        /// 接入所有站点当前时间因子小时数据
        /// </summary>
        public void AnalyzeCurrentData()
        {
            int n = 0;
            Analysis(DateTime.Now.AddHours(-1).ToString(), n);
            //Analysis("2017-01-01 16:59:59", n);
        }
        /// <summary>
        /// 填补数据
        /// </summary>
        public void AutoFillData()
        {
            try
            {
                DateTime startTime = Convert.ToDateTime(DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd 00:00:00"));//获取前上月0点
                DateTime endTime = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));//获取昨天23点
                for (DateTime time = startTime; time <= endTime; time = time.AddDays(1))
                {
                    int n = 0;
                    RunBy60(Convert.ToDateTime(time.ToString("yyyy-MM-dd 00:00:00")), Convert.ToDateTime(time.ToString("yyyy-MM-dd 23:59:59")));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        #region 同步数据
        /// <summary>
        /// 同步数据
        /// </summary>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        public void FillData(string sTime, string eTime)
        {
            try
            {
                int n = 0;
                DateTime startTime = Convert.ToDateTime(sTime);
                DateTime endTime = Convert.ToDateTime(eTime);
                for (DateTime time = startTime; time <= endTime; time = time.AddHours(1))
                {
                    Analysis(time.ToString(), n);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }

        #endregion

        /// <summary>
        /// 接入小时数据
        /// </summary>
        /// <param name="dateTime">时间</param>
        public void Analysis(string dateTime, int n)
        {
            try
            {
                //调用数据接口
                ServiceReference1.AirQualityWebServiceSoapClient client = new ServiceReference1.AirQualityWebServiceSoapClient();
                ServiceReference1.MySoapHeader header = new ServiceReference1.MySoapHeader();
                header.UserId = UserId;
                header.Password = Password;
                
                string Factors = string.Join(",", WSCodes);//字符串连接
                //获取所有站点所有因子小时数据
                DataSet ds = client.getHourlyDataSet(header, Factors, dateTime);
                DataTable dt = ds.Tables[0];
                #region 删除没有数据的行
                #region 测试
                //DataTable dt1 = GetNewDataTable(dt,"");
                //DataRow dr = dt.NewRow();
                //dr["StationId"] = "11";
                //dr["StationName"] = "站点";
                //dr["DateTime"] = DateTime.Now;
                //dt.Rows.Add(dr);

                //DataRow dr1 = dt.NewRow();
                //dr1["StationId"] = "22";
                //dr1["StationName"] = "站点";
                //dr1["DateTime"] = DateTime.Now;
                //dr1["SO2Value"] = 0.001;
                //dt.Rows.Add(dr1);
                #endregion

                ////删除没有数据的行
                //for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                //{
                //    bool flag = true;//标识此行数据是否都为空
                //    for (int iCol = 0; iCol < dt.Columns.Count; iCol++)
                //    {
                //        if (dt.Columns[iCol].ColumnName != "StationId" && dt.Columns[iCol].ColumnName != "StationName" && dt.Columns[iCol].ColumnName != "DateTime" && dt.Rows[iRow][iCol] != DBNull.Value && dt.Rows[iRow][iCol].ToString() != "")//遍历到因子列
                //        {
                //            flag = false;//该行存在非空数据
                //            break;//跳出列循环
                //        }
                //    }
                //    if (flag == true)
                //    {
                //        log.Info("接口该行没数据：" + dt.Rows[iRow]["StationName"] + dt.Rows[iRow]["DateTime"]);
                //        //删除该行
                //        dt.Rows[iRow].Delete();
                //    }
                //    else
                //    {
                //        continue;//继续循环下一行
                //    }
                //}
                //dt.AcceptChanges();//提交修改
                #endregion

                using (AirAutoMonitorModel AirAutoMonitorModel = new AirAutoMonitorModel())
                {
                    using (BaseDataModel BaseDataModel = new BaseDataModel())
                    {
                        for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                        {
                            //接口上传的站点id
                            string WSPointId = dt.Rows[iRow]["StationId"].ToString();
                            //到站点映射表里查找数据库对应的id
                            string LocalPointId = BaseDataModel.SYS_Point_Mappings.Where(p => p.WSPointId == WSPointId).Select(p => p.LocalPointId).FirstOrDefault();
                            if (string.IsNullOrWhiteSpace(LocalPointId))
                            {
                                continue;
                            }
                            //获取该站点因子对应配置
                            IQueryable<SYS_PointsFactors_Mapping> PointsFactors_Mappings = BaseDataModel.SYS_PointsFactors_Mappings.Where(p => p.WSPointId == WSPointId);
                            //获取站点监测项
                            string[] WSCodesByPoint = PointsFactors_Mappings.Select(p => p.WSCode).ToArray();

                            for (int iCol = 0; iCol < dt.Columns.Count; iCol++)
                            {
                                string WSCode = dt.Columns[iCol].ColumnName.Replace("Value", "");
                                if (WSCodesByPoint.Contains(WSCode))//遍历到因子列
                                {
                                    //到因子映射表里查找数据库对应的code
                                    string LocalCode = BaseDataModel.SYS_Factors_Mappings.Where(p => p.WSCode == WSCode).Select(p => p.LocalCode).ToArray()[0];
                                    if (!string.IsNullOrWhiteSpace(LocalPointId) && !string.IsNullOrWhiteSpace(LocalCode))
                                    {
                                        int localPointId = Convert.ToInt32(LocalPointId);
                                        DateTime tstamp = Convert.ToDateTime(dt.Rows[iRow]["DateTime"]);



                                        //查找数据库中是否已存在该时间点、该站点、该因子数据
                                        TB_InfectantBy60Buffer InfectantBy60BufferExist = AirAutoMonitorModel.TB_InfectantBy60Buffers.Where(p => p.PointId.Equals(localPointId) && p.PollutantCode.Equals(LocalCode) && p.Tstamp.Equals(tstamp)).FirstOrDefault();
                                        if (InfectantBy60BufferExist != null)
                                        {
                                            continue;
                                            #region 更新(注释)
                                            //InfectantBy60BufferExist.ReceiveTime = DateTime.Now;
                                            //if (dt.Rows[iRow][iCol] != DBNull.Value)
                                            //{
                                            //    InfectantBy60BufferExist.PollutantValue = Convert.ToDecimal(dt.Rows[iRow][iCol]);
                                            //}
                                            //if (dt.Rows[iRow][iCol + 1] != DBNull.Value && !string.IsNullOrWhiteSpace(dt.Rows[iRow][iCol + 1].ToString()))
                                            //{
                                            //    //标记列处理
                                            //    char[] StatusList = dt.Rows[iRow][iCol + 1].ToString().ToCharArray();
                                            //    StringBuilder sb = new StringBuilder();
                                            //    foreach (char status in StatusList)
                                            //    {
                                            //        sb.Append(status + ",");
                                            //    }
                                            //    sb.Remove(sb.Length - 1, 1);
                                            //    string StatusStr = sb.ToString();
                                            //    InfectantBy60BufferExist.Status = StatusStr;
                                            //}
                                            ////获取该站点该因子监测类型
                                            //string MonitoringDataType = PointsFactors_Mappings.Where(p => p.WSCode.Equals(WSCode)).Select(p => p.MonitoringDataTypeCode).FirstOrDefault();
                                            //InfectantBy60BufferExist.MonitoringDataTypeCode = MonitoringDataType;
                                            //InfectantBy60BufferExist.UpdateUser = "SystemSync";
                                            //InfectantBy60BufferExist.UpdateDateTime = DateTime.Now;
                                            #endregion
                                        }
                                        else if (dt.Rows[iRow][iCol] != DBNull.Value)//排除因子为空的数据
                                        {
                                            #region 新增
                                            TB_InfectantBy60Buffer InfectantBy60Buffer = new TB_InfectantBy60Buffer();

                                            InfectantBy60Buffer.PointId = localPointId;
                                            InfectantBy60Buffer.Tstamp = tstamp;
                                            InfectantBy60Buffer.ReceiveTime = DateTime.Now;
                                            InfectantBy60Buffer.PollutantCode = LocalCode;
                                            if (dt.Rows[iRow][iCol] != DBNull.Value)
                                            {
                                                InfectantBy60Buffer.PollutantValue = Convert.ToDecimal(dt.Rows[iRow][iCol]);
                                            }
                                            if (dt.Rows[iRow][iCol + 1] != DBNull.Value && !string.IsNullOrWhiteSpace(dt.Rows[iRow][iCol + 1].ToString()))
                                            {
                                                //标记列处理
                                                char[] StatusList = dt.Rows[iRow][iCol + 1].ToString().ToCharArray();
                                                StringBuilder sb = new StringBuilder();
                                                foreach (char status in StatusList)
                                                {
                                                    sb.Append(status + ",");
                                                }
                                                sb.Remove(sb.Length - 1, 1);
                                                string StatusStr = sb.ToString();
                                                InfectantBy60Buffer.Status = StatusStr;
                                            }
                                            //获取该站点该因子监测类型
                                            string MonitoringDataType = PointsFactors_Mappings.Where(p => p.WSCode.Equals(WSCode)).Select(p => p.MonitoringDataTypeCode).FirstOrDefault();
                                            InfectantBy60Buffer.MonitoringDataTypeCode = MonitoringDataType;
                                            InfectantBy60Buffer.CreatUser = "SystemSync";
                                            InfectantBy60Buffer.CreatDateTime = DateTime.Now;
                                            AirAutoMonitorModel.Add(InfectantBy60Buffer);
                                            #endregion
                                        }

                                    }
                                    iCol++;
                                }
                            }
                        }
                    }
                    AirAutoMonitorModel.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                n++;
                if (n <= 3)
                {
                    log.Info("第" + n + "次尝试修复");
                    Analysis(dateTime, n);

                }

            }

        }

        #endregion

        #region 线程方法
        /// <summary>
        /// 每分钟执行
        /// </summary>
        public void RunBy1()
        {
            try
            {
                //log.Info("------------------------------------------------------------RunBy1开始-------------------");
                using (FrameworkModel FrameworkModel = new FrameworkModel())
                {
                    V_CodeMainItem DataType = DataTypeCodeMainItems.Where(p => p.ItemValue.Equals("Min1")).FirstOrDefault();
                    string DataTypeUid = DataType.ItemGuid;
                    using (BaseDataModel BaseDataModel = new BaseDataModel())
                    {
                        DT_ConfigIDInfo IDLogFlagPre = BaseDataModel.DT_ConfigIDInfos.Where(p => p.Memo.Equals("TB_InfectantBy1BufferFlag")).FirstOrDefault();
                        long IDLogFlag = Convert.ToInt32(IDLogFlagPre.Maxid);
                        DT_ConfigIDInfo OriIDLogFlagPre = BaseDataModel.DT_ConfigIDInfos.Where(p => p.Memo.Equals("TB_InfectantBy1Flag")).FirstOrDefault();
                        long OriIDLogFlag = Convert.ToInt32(OriIDLogFlagPre.Maxid);
                        DT_ConfigIDInfo BufferIDLogBatchProcessPre = BaseDataModel.DT_ConfigIDInfos.Where(p => p.Memo.Equals("TB_InfectantBy1BufferBatchProcess")).FirstOrDefault();
                        long BufferIDLogBatchProcess = Convert.ToInt32(BufferIDLogBatchProcessPre.Maxid);
                        DT_ConfigIDInfo IDLogBatchProcessPre = BaseDataModel.DT_ConfigIDInfos.Where(p => p.Memo.Equals("TB_InfectantBy1BatchProcess")).FirstOrDefault();
                        long IDLogBatchProcess = Convert.ToInt32(IDLogBatchProcessPre.Maxid);

                        Flag(DataTypeUid, IDLogFlag, OriIDLogFlag);
                        InstrumentOnline(DataTypeUid);

                        using (AirAutoMonitorModel AirAutoMonitorModel = new AirAutoMonitorModel())
                        {
                            //获取表中最新ID
                            TB_InfectantBy1Buffer Last_InfectantBy1Buffer = AirAutoMonitorModel.TB_InfectantBy1Buffers.OrderByDescending(p => p.Id).FirstOrDefault();
                            if (Last_InfectantBy1Buffer != null)
                            {
                                long NewID = Last_InfectantBy1Buffer.Id;
                                IDLogFlagPre.Maxid = NewID;
                                IDLogFlagPre.Updatedatetime = DateTime.Now;
                                BaseDataModel.SaveChanges();

                                BatchProcess(DataTypeUid, BufferIDLogBatchProcess, IDLogBatchProcess, NewID);

                                BufferIDLogBatchProcessPre.Maxid = NewID;
                                BufferIDLogBatchProcessPre.Updatedatetime = DateTime.Now;
                                //获取原始表中最新ID
                                TB_InfectantBy1 Last_InfectantBy1 = AirAutoMonitorModel.TB_InfectantBy1.OrderByDescending(p => p.Id).FirstOrDefault();
                                if (Last_InfectantBy1 != null)
                                {
                                    long OriNewID = Last_InfectantBy1.Id;
                                    IDLogBatchProcessPre.Maxid = OriNewID;
                                    IDLogBatchProcessPre.Updatedatetime = DateTime.Now;

                                    OriIDLogFlagPre.Maxid = OriNewID;
                                    OriIDLogFlagPre.Updatedatetime = DateTime.Now;

                                }

                                BaseDataModel.SaveChanges();
                            }
                        }
                    }
                }
                //log.Info("-------------------------------------------------------------RunBy1结束-------------------");
            }
            catch (Exception ex)
            {
                log.Error("-------------------------RunBy1异常：" + ex.ToString());
            }
        }
        /// <summary>
        /// 每五分钟执行
        /// </summary>
        public void RunBy5()
        {
            try
            {

                //log.Info("-------------------------------------------------------------RunBy5开始-------------------");
                using (FrameworkModel FrameworkModel = new FrameworkModel())
                {
                    V_CodeMainItem DataType = DataTypeCodeMainItems.Where(p => p.ItemValue.Equals("Min5")).FirstOrDefault();
                    string DataTypeUid = DataType.ItemGuid;


                    using (BaseDataModel BaseDataModel = new BaseDataModel())
                    {
                        DT_ConfigIDInfo IDLogFlagPre = BaseDataModel.DT_ConfigIDInfos.Where(p => p.Memo.Equals("TB_InfectantBy5BufferFlag")).FirstOrDefault();
                        long IDLogFlag = Convert.ToInt32(IDLogFlagPre.Maxid);
                        DT_ConfigIDInfo OriIDLogFlagPre = BaseDataModel.DT_ConfigIDInfos.Where(p => p.Memo.Equals("TB_InfectantBy5Flag")).FirstOrDefault();
                        long OriIDLogFlag = Convert.ToInt32(OriIDLogFlagPre.Maxid);
                        DT_ConfigIDInfo BufferIDLogBatchProcessPre = BaseDataModel.DT_ConfigIDInfos.Where(p => p.Memo.Equals("TB_InfectantBy5BufferBatchProcess")).FirstOrDefault();
                        long BufferIDLogBatchProcess = Convert.ToInt32(BufferIDLogBatchProcessPre.Maxid);
                        DT_ConfigIDInfo IDLogBatchProcessPre = BaseDataModel.DT_ConfigIDInfos.Where(p => p.Memo.Equals("TB_InfectantBy5BatchProcess")).FirstOrDefault();
                        long IDLogBatchProcess = Convert.ToInt32(IDLogBatchProcessPre.Maxid);

                        Flag(DataTypeUid, IDLogFlag, OriIDLogFlag);
                        InstrumentOnline(DataTypeUid);//5分钟类型更新

                        V_CodeMainItem DataTypeMin60 = DataTypeCodeMainItems.Where(p => p.ItemValue.Equals("Min60")).FirstOrDefault();
                        string DataTypeUidMin60 = DataTypeMin60.ItemGuid;

                        //log.Info("60分钟仪器在线情况更新");
                        InstrumentOnline(DataTypeUidMin60);//60分钟类型更新
                        using (AirAutoMonitorModel AirAutoMonitorModel = new AirAutoMonitorModel())
                        {
                            //获取表中最新ID
                            TB_InfectantBy5Buffer Last_InfectantBy5Buffer = AirAutoMonitorModel.TB_InfectantBy5Buffers.OrderByDescending(p => p.Id).FirstOrDefault();
                            if (Last_InfectantBy5Buffer != null)
                            {
                                long NewID = Last_InfectantBy5Buffer.Id;
                                IDLogFlagPre.Maxid = NewID;
                                IDLogFlagPre.Updatedatetime = DateTime.Now;
                                BaseDataModel.SaveChanges();

                                BatchProcess(DataTypeUid, BufferIDLogBatchProcess, IDLogBatchProcess, NewID);

                                BufferIDLogBatchProcessPre.Maxid = NewID;
                                BufferIDLogBatchProcessPre.Updatedatetime = DateTime.Now;
                                //获取原始表中最新ID
                                TB_InfectantBy5 Last_InfectantBy5 = AirAutoMonitorModel.TB_InfectantBy5.OrderByDescending(p => p.Id).FirstOrDefault();
                                if (Last_InfectantBy5 != null)
                                {
                                    long OriNewID = Last_InfectantBy5.Id;
                                    IDLogBatchProcessPre.Maxid = OriNewID;
                                    IDLogBatchProcessPre.Updatedatetime = DateTime.Now;

                                    OriIDLogFlagPre.Maxid = OriNewID;
                                    OriIDLogFlagPre.Updatedatetime = DateTime.Now;

                                }

                                BaseDataModel.SaveChanges();
                            }
                        }
                    }
                }
                //log.Info("-------------------------------------------------------------RunBy5结束-------------------");
            }
            catch (Exception ex)
            {
                log.Error("-------------------------RunBy5异常：" + ex.ToString());
            }
        }
        public void RunBy60(DateTime sTime, DateTime eTime)
        {
            try
            {
                //log.Info("-------------------------------------------------------------RunBy60开始-------------------");
                log.Info("数据同步开始时间——" + DateTime.Now);
                using (FrameworkModel FrameworkModel = new FrameworkModel())
                {
                    V_CodeMainItem DataType = DataTypeCodeMainItems.Where(p => p.ItemValue.Equals("Min60")).FirstOrDefault();
                    string DataTypeUid = DataType.ItemGuid;
                    using (BaseDataModel BaseDataModel = new BaseDataModel())
                    {
                        DT_ConfigIDInfo IDLogFlagPre = BaseDataModel.DT_ConfigIDInfos.Where(p => p.Memo.Equals("TB_InfectantBy60BufferFlag")).FirstOrDefault();
                        long IDLogFlag = Convert.ToInt32(IDLogFlagPre.Maxid);
                        DT_ConfigIDInfo OriIDLogFlagPre = BaseDataModel.DT_ConfigIDInfos.Where(p => p.Memo.Equals("TB_InfectantBy60Flag")).FirstOrDefault();
                        long OriIDLogFlag = Convert.ToInt32(OriIDLogFlagPre.Maxid);
                        DT_ConfigIDInfo BufferIDLogBatchProcessPre = BaseDataModel.DT_ConfigIDInfos.Where(p => p.Memo.Equals("TB_InfectantBy60BufferBatchProcess")).FirstOrDefault();
                        long BufferIDLogBatchProcess = Convert.ToInt32(BufferIDLogBatchProcessPre.Maxid);
                        DT_ConfigIDInfo IDLogBatchProcessPre = BaseDataModel.DT_ConfigIDInfos.Where(p => p.Memo.Equals("TB_InfectantBy60BatchProcess")).FirstOrDefault();
                        long IDLogBatchProcess = Convert.ToInt32(IDLogBatchProcessPre.Maxid);
                        try
                        {
                            AnalyzeCurrentData();
                            //先计算国控点AQI
                            log.Info("国控点计算AQI开始时间——" + DateTime.Now);
                            CalculateConBy60(sTime, eTime);
                            log.Info("国控点计算AQI结束时间——" + DateTime.Now);
                            Flag(DataTypeUid, IDLogFlag, OriIDLogFlag);
                            V_CodeMainItem DataTypeMin60 = DataTypeCodeMainItems.Where(p => p.ItemValue.Equals("Min60")).FirstOrDefault();
                            string DataTypeUidMin60 = DataTypeMin60.ItemGuid;

                            //log.Info("60分钟仪器在线情况更新");
                            //InstrumentOnline(DataTypeUidMin60);//60分钟类型更新
                            using (AirAutoMonitorModel AirAutoMonitorModel = new AirAutoMonitorModel())
                            {
                                //获取缓存表中最新ID
                                TB_InfectantBy60Buffer Last_InfectantBy60Buffer = AirAutoMonitorModel.TB_InfectantBy60Buffers.OrderByDescending(p => p.Id).FirstOrDefault();
                                if (Last_InfectantBy60Buffer != null)
                                {
                                    long NewID = Last_InfectantBy60Buffer.Id;
                                    IDLogFlagPre.Maxid = NewID;
                                    IDLogFlagPre.Updatedatetime = DateTime.Now;
                                    BaseDataModel.SaveChanges();

                                    try
                                    {
                                        BatchProcess(DataTypeUid, BufferIDLogBatchProcess, IDLogBatchProcess, NewID);

                                        BufferIDLogBatchProcessPre.Maxid = NewID;
                                        BufferIDLogBatchProcessPre.Updatedatetime = DateTime.Now;
                                        //获取原始表中最新ID
                                        TB_InfectantBy60 Last_InfectantBy60 = AirAutoMonitorModel.TB_InfectantBy60.OrderByDescending(p => p.Id).FirstOrDefault();
                                        if (Last_InfectantBy60 != null)
                                        {
                                            long OriNewID = Last_InfectantBy60.Id;
                                            IDLogBatchProcessPre.Maxid = OriNewID;
                                            IDLogBatchProcessPre.Updatedatetime = DateTime.Now;

                                            OriIDLogFlagPre.Maxid = OriNewID;
                                            OriIDLogFlagPre.Updatedatetime = DateTime.Now;
                                        }
                                        BaseDataModel.SaveChanges();
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error("----------------------------BatchProcess异常：" + ex.ToString());
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("----------------------------Flag异常：" + ex.ToString());
                        }

                    }
                }
                //时间范围：计算当天和昨天数据

                log.Info("数据同步完成时间——" + DateTime.Now);
                log.Info("计算AQI开始时间——" + DateTime.Now);
                CalculateBy60(sTime, eTime);
                log.Info("计算AQI结束时间——" + DateTime.Now);
            }
            catch (Exception ex)
            {
                log.Error("----------------------------RunBy60异常：" + ex.ToString());
            }
        }
        /// <summary>
        /// 每小时执行
        /// </summary>
        public void RunBy60()
        {
            try
            {
                //log.Info("-------------------------------------------------------------RunBy60开始-------------------");
                log.Info("数据同步开始时间——" + DateTime.Now);
                DateTime sTime = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00"));
                DateTime eTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
                using (FrameworkModel FrameworkModel = new FrameworkModel())
                {
                    V_CodeMainItem DataType = DataTypeCodeMainItems.Where(p => p.ItemValue.Equals("Min60")).FirstOrDefault();
                    string DataTypeUid = DataType.ItemGuid;
                    using (BaseDataModel BaseDataModel = new BaseDataModel())
                    {
                        DT_ConfigIDInfo IDLogFlagPre = BaseDataModel.DT_ConfigIDInfos.Where(p => p.Memo.Equals("TB_InfectantBy60BufferFlag")).FirstOrDefault();
                        long IDLogFlag = Convert.ToInt32(IDLogFlagPre.Maxid);
                        DT_ConfigIDInfo OriIDLogFlagPre = BaseDataModel.DT_ConfigIDInfos.Where(p => p.Memo.Equals("TB_InfectantBy60Flag")).FirstOrDefault();
                        long OriIDLogFlag = Convert.ToInt32(OriIDLogFlagPre.Maxid);
                        DT_ConfigIDInfo BufferIDLogBatchProcessPre = BaseDataModel.DT_ConfigIDInfos.Where(p => p.Memo.Equals("TB_InfectantBy60BufferBatchProcess")).FirstOrDefault();
                        long BufferIDLogBatchProcess = Convert.ToInt32(BufferIDLogBatchProcessPre.Maxid);
                        DT_ConfigIDInfo IDLogBatchProcessPre = BaseDataModel.DT_ConfigIDInfos.Where(p => p.Memo.Equals("TB_InfectantBy60BatchProcess")).FirstOrDefault();
                        long IDLogBatchProcess = Convert.ToInt32(IDLogBatchProcessPre.Maxid);
                        try
                        {
                            AnalyzeCurrentData();
                            //先计算国控点AQI
                            log.Info("国控点计算AQI开始时间——" + DateTime.Now);
                            CalculateConBy60(sTime, eTime);
                            log.Info("国控点计算AQI结束时间——" + DateTime.Now);
                            Flag(DataTypeUid, IDLogFlag, OriIDLogFlag);
                            V_CodeMainItem DataTypeMin60 = DataTypeCodeMainItems.Where(p => p.ItemValue.Equals("Min60")).FirstOrDefault();
                            string DataTypeUidMin60 = DataTypeMin60.ItemGuid;

                            ////log.Info("60分钟仪器在线情况更新");
                            //InstrumentOnline(DataTypeUidMin60);//60分钟类型更新
                            using (AirAutoMonitorModel AirAutoMonitorModel = new AirAutoMonitorModel())
                            {
                                //获取缓存表中最新ID
                                TB_InfectantBy60Buffer Last_InfectantBy60Buffer = AirAutoMonitorModel.TB_InfectantBy60Buffers.OrderByDescending(p => p.Id).FirstOrDefault();
                                if (Last_InfectantBy60Buffer != null)
                                {
                                    long NewID = Last_InfectantBy60Buffer.Id;
                                    IDLogFlagPre.Maxid = NewID;
                                    IDLogFlagPre.Updatedatetime = DateTime.Now;
                                    BaseDataModel.SaveChanges();

                                    try
                                    {
                                        BatchProcess(DataTypeUid, BufferIDLogBatchProcess, IDLogBatchProcess, NewID);

                                        BufferIDLogBatchProcessPre.Maxid = NewID;
                                        BufferIDLogBatchProcessPre.Updatedatetime = DateTime.Now;
                                        //获取原始表中最新ID
                                        TB_InfectantBy60 Last_InfectantBy60 = AirAutoMonitorModel.TB_InfectantBy60.OrderByDescending(p => p.Id).FirstOrDefault();
                                        if (Last_InfectantBy60 != null)
                                        {
                                            long OriNewID = Last_InfectantBy60.Id;
                                            IDLogBatchProcessPre.Maxid = OriNewID;
                                            IDLogBatchProcessPre.Updatedatetime = DateTime.Now;

                                            OriIDLogFlagPre.Maxid = OriNewID;
                                            OriIDLogFlagPre.Updatedatetime = DateTime.Now;
                                        }
                                        BaseDataModel.SaveChanges();
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error("----------------------------BatchProcess异常：" + ex.ToString());
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("----------------------------Flag异常：" + ex.ToString());
                        }

                    }
                }
                //时间范围：计算当天和昨天数据
                
                log.Info("数据同步完成时间——"+DateTime.Now);
                log.Info("计算AQI开始时间——" + DateTime.Now);
                CalculateBy60(sTime, eTime);
                log.Info("计算AQI结束时间——" + DateTime.Now);
            }
            catch (Exception ex)
            {
                log.Error("----------------------------RunBy60异常：" + ex.ToString());
            }
        }
        /// <summary>
        /// 每天执行
        /// </summary>
        public void RunByDay()
        {
            try
            {
                //log.Info("-------------------------------------------------------RunByDay开始---------------------------");
                //月数据时间范围：上个月和这个月
                DateTime sMonthTime = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01 00:00:00"));
                DateTime eMonthTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));

                //周数据时间范围：上周和本周
                DateTime sWeekTime = Convert.ToDateTime(DateTime.Now.AddDays(-14).ToString("yyyy-MM-dd 00:00:00"));
                DateTime eWeekTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));

                CalculateByDay(sMonthTime, eMonthTime, sWeekTime, eWeekTime);
                //log.Info("-------------------------------------------------------RunByDay结束---------------------------");
            }
            catch (Exception ex)
            {
                log.Error("------------------------------------------------RunByDay异常：" + ex.ToString());
            }
        }
        /// <summary>
        /// 每月执行
        /// </summary>
        public void RunByMonth()
        {
            try
            {
                //季数据时间范围：上季度和本季度
                DateTime sSeasonTime = Convert.ToDateTime(DateTime.Now.AddMonths(-3 - ((DateTime.Now.Month - 1) % 3)).ToString("yyyy-MM-01"));
                DateTime eSeasonTime = DateTime.Now;

                //年数据时间范围：去年和今年
                DateTime sYearTime = Convert.ToDateTime(DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddYears(-1).ToShortDateString());
                DateTime eYearTime = DateTime.Now;
                CalculateByMonth(sSeasonTime, eSeasonTime, sYearTime, eYearTime);
            }
            catch (Exception ex)
            {
                log.Error("------------------------------------------------RunByMonth异常：" + ex.ToString());
            }
        }
        #endregion

        #region 自定义方法
        /// <summary>
        /// 标记数据
        /// </summary>
        /// <param name="DataTypeUid">数据类型</param>
        /// <param name="IDLog">ID记录</param>
        public void Flag(string DataTypeUid, long IDLog, long OriIDLog)
        {
            try
            {
                //获取本机ip
                string ip = d_DAL.GetIpAddress();
                string HSp = ConfigurationManager.AppSettings["HSp"];
                string LSp = ConfigurationManager.AppSettings["LSp"];
                string UpdateUser = "SystemSync";
                DateTime UpdateDateTime = DateTime.Now;
                using (BaseDataModel BaseDataModel = new BaseDataModel())
                {
                    using (MonitorBusinessModel MonitorBusinessModel = new MonitorBusinessModel())
                    {
                        //获取配置表中信息
                        DT_DataTypeConfig DataTypeConfig = BaseDataModel.DT_DataTypeConfigs.Where(p => p.EnableOrNot.Equals(true) && p.DataTypeUid.Equals(DataTypeUid)).FirstOrDefault();
                        string SY_BufferTable = DataTypeConfig.SYBufferTable;//缓存表同义词
                        string BufferTable = DataTypeConfig.BufferTable;//缓存表
                        string OriginTable = DataTypeConfig.OriginalTable;//原始表
                        string SY_OriTable = DataTypeConfig.SYOriginalTable;//缓存表同义词

                        //删除重复数据
                        d_DAL.DeleteRepeatData(IDLog, BufferTable);

                        #region 自动审核
                        //获取审核规则用途字典表信息
                        V_CodeMainItem AuditCodeMainItem = FlagTypeCodeMainItems.Where(p => p.ItemText.Equals("审核")).FirstOrDefault();
                        if (AuditCodeMainItem == null)
                        {
                            log.Error("审核字典表信息未找到！");
                            return;
                        }
                        string AuditFlag = "AuditFlag";//对应数据表中字段名称
                        string UseForAuditUid = AuditCodeMainItem.ItemGuid;
                        #region 超标
                        d_DAL.FlagUpper(DataTypeUid, IDLog, HSp, AuditFlag, UseForAuditUid, SY_BufferTable, ApplicationUid, UpdateUser, UpdateDateTime);
                        d_DAL.LogUpper(DataTypeUid, IDLog, HSp, UseForAuditUid, SY_BufferTable, ip, ApplicationUid);

                        d_DAL.FlagLower(DataTypeUid, IDLog, LSp, AuditFlag, UseForAuditUid, SY_BufferTable, ApplicationUid, UpdateUser, UpdateDateTime);
                        d_DAL.LogLower(DataTypeUid, IDLog, LSp, UseForAuditUid, SY_BufferTable, ip, ApplicationUid);
                        #endregion

                        #region 重复
                        //根据重复值限配置信息到数据表中获取重复数据
                        DataTable dtAuditRepeatLimitConfig = d_DAL.GetRepeatLimitConfig(UseForAuditUid, DataTypeUid, ApplicationUid);
                        foreach (DataRow dr in dtAuditRepeatLimitConfig.Rows)
                        {
                            int RepeatableNumber = Convert.ToInt32(dr["RepeatableNumber"]);
                            string PointId = dr["PointId"].ToString();
                            string PollutantCode = dr["PollutantCode"].ToString();
                            DataTable dtRepeatLimitData = d_DAL.GetRepeatLimitData(IDLog, BufferTable, PointId, PollutantCode);
                            int Temp_IDS = -1;//数据临时行号
                            DateTime DateStart = DateTime.Now;//重复开始时间
                            DateTime DateEnd = DateTime.Now;//重复截止时间
                            int Pre_IDS = -1;//重复判断时使用的前一条临时行号
                            decimal PreFactorValue = -9999;//重复判断时使用的上一条数据值
                            for (int i = 0; i < dtRepeatLimitData.Rows.Count; i++)
                            {
                                DateTime Date = Convert.ToDateTime(dtRepeatLimitData.Rows[i]["Tstamp"]);
                                DateTime Date1 = Convert.ToDateTime(dtRepeatLimitData.Rows[i]["Tstamp1"]);
                                decimal FactorValue = Convert.ToDecimal(dtRepeatLimitData.Rows[i]["PollutantValue"]);
                                int IDS = Convert.ToInt32(dtRepeatLimitData.Rows[i]["IDS"]);
                                if (Temp_IDS == -1)
                                {
                                    Temp_IDS = 1;
                                    DateStart = Date;
                                    DateEnd = Date1;
                                    Pre_IDS = IDS;
                                    PreFactorValue = FactorValue;
                                }
                                else if (Pre_IDS == IDS - 1 && PreFactorValue == FactorValue)//判断当前行和前一行数据相同
                                {
                                    Temp_IDS += 1;
                                    DateEnd = Date1;
                                    Pre_IDS = IDS;
                                    PreFactorValue = FactorValue;
                                }
                                else
                                {
                                    DateStart = Date;
                                    Pre_IDS = IDS;
                                    PreFactorValue = FactorValue;
                                }
                                if (Temp_IDS >= RepeatableNumber)
                                {
                                    d_DAL.RepeatLimitFlag(PointId, PollutantCode, DateStart, DateEnd, BufferTable, AuditFlag, UpdateUser, UpdateDateTime);
                                    d_DAL.LogRepeatLimit(PointId, PollutantCode, DateStart, DateEnd, BufferTable, AuditFlag, ip);

                                    Temp_IDS = -1;
                                }
                            }
                        }
                        #endregion

                        #endregion
                        #region 自动报警
                        //获取审核规则用途字典表信息
                        V_CodeMainItem AlarmCodeMainItem = FlagTypeCodeMainItems.Where(p => p.ItemText.Equals("报警")).FirstOrDefault();
                        if (AuditCodeMainItem == null)
                        {
                            log.Error("报警字典表信息未找到！");
                            return;
                        }


                        string DataFlag = "DataFlag";//对应数据表中字段名称
                        string UseForAlarmUid = AlarmCodeMainItem.ItemGuid;
                        #region 超标
                        //超级站
                        d_DAL.FlagUpperSuper(DataTypeUid, OriIDLog, HSp, DataFlag, UseForAlarmUid, SY_OriTable, ApplicationUid, UpdateUser, UpdateDateTime);
                        d_DAL.FlagLowerSuper(DataTypeUid, OriIDLog, LSp, DataFlag, UseForAlarmUid, SY_OriTable, ApplicationUid, UpdateUser, UpdateDateTime);

                        //常规站
                        d_DAL.FlagUpper(DataTypeUid, IDLog, HSp, DataFlag, UseForAlarmUid, SY_BufferTable, ApplicationUid, UpdateUser, UpdateDateTime);
                        d_DAL.FlagLower(DataTypeUid, IDLog, LSp, DataFlag, UseForAlarmUid, SY_BufferTable, ApplicationUid, UpdateUser, UpdateDateTime);
                        #region 生成报警信息
                        DataTable dtAlarmExcessiveConfig = d_DAL.GetExcessiveConfig(DataTypeUid, UseForAlarmUid, ApplicationUid);
                        foreach (DataRow dr in dtAlarmExcessiveConfig.Rows)
                        {
                            string PUid = dr["MonitoringPointUid"].ToString();
                            string PId = dr["PointId"].ToString();
                            string PName = dr["MonitoringPointName"].ToString();
                            string FactorCode = dr["PollutantCode"].ToString();
                            string FactorName = dr["PollutantName"].ToString();
                            string NotifyGradeUid = dr["NotifyGradeUid"].ToString();
                            decimal? ExcessiveUpper = null, ExcessiveLow = null;
                            if (dr["ExcessiveUpper"] != DBNull.Value)
                            {
                                ExcessiveUpper = Convert.ToDecimal(dr["ExcessiveUpper"]);
                            }
                            if (dr["ExcessiveLow"] != DBNull.Value)
                            {
                                ExcessiveLow = Convert.ToDecimal(dr["ExcessiveLow"]);
                            }
                            #region 生成超上限报警信息
                            //获取超上限字典表信息
                            V_CodeMainItem HspAlarmType = AlarmTypes.Where(p => p.ItemValue.Equals(HSp)).FirstOrDefault();
                            string m_AlarmTypeUid_Hsp = HspAlarmType.ItemGuid;
                            if (ExcessiveUpper != null & m_AlarmTypeUid_Hsp != null)
                            {
                                //超级站获取超上限数据
                                DataTable dtUpperDataSuper = d_DAL.GetUpperDataSuper(DataTypeUid, OriIDLog, HSp, DataFlag, UseForAlarmUid, SY_OriTable, ApplicationUid);

                                //常规站获取超上限数据
                                DataTable dtUpperData = d_DAL.GetUpperData(DataTypeUid, IDLog, HSp, DataFlag, UseForAlarmUid, SY_BufferTable, ApplicationUid);
                                //两表取并集
                                IEnumerable<DataRow> query2 = dtUpperData.AsEnumerable().Union(dtUpperDataSuper.AsEnumerable(), DataRowComparer.Default);
                                if (query2.Count() > 0)
                                {
                                    dtUpperData = query2.CopyToDataTable();
                                }
                                if (dtUpperData.Rows.Count > 0)
                                {
                                    DataView dvUpperData = dtUpperData.DefaultView;
                                    dvUpperData.Sort = "Tstamp";
                                    DateTime DTBegin = Convert.ToDateTime(dvUpperData[0]["Tstamp"].ToString());
                                    dvUpperData.Sort = "Tstamp desc";
                                    DateTime DTEnd = Convert.ToDateTime(dvUpperData[0]["Tstamp"].ToString());
                                    //生成超上限报警信息
                                    d_DAL.CreateAlarmUpper(ApplicationUid, PUid, m_AlarmTypeUid_Hsp, NotifyGradeUid, DataTypeUid, PName, Convert.ToDecimal(ExcessiveUpper), FactorName, FactorCode, PId, SY_BufferTable, DTBegin, DTEnd, HSp);
                                }
                            }
                            #endregion
                            #region 生成超下限报警信息
                            V_CodeMainItem LspAlarmType = AlarmTypes.Where(p => p.ItemValue.Equals(LSp)).FirstOrDefault();
                            string m_AlarmTypeUid_Lsp = LspAlarmType.ItemGuid;
                            if (ExcessiveLow != null & m_AlarmTypeUid_Lsp != null)
                            {
                                //超级站获取超下限数据
                                DataTable dtLowerDataSuper = d_DAL.GetLowerDataSuper(DataTypeUid, OriIDLog, LSp, DataFlag, UseForAlarmUid, SY_OriTable, ApplicationUid);

                                //常规站获取超下限数据
                                DataTable dtLowerData = d_DAL.GetLowerData(DataTypeUid, IDLog, LSp, DataFlag, UseForAlarmUid, SY_BufferTable, ApplicationUid);
                                //两表取并集
                                IEnumerable<DataRow> query2 = dtLowerData.AsEnumerable().Union(dtLowerDataSuper.AsEnumerable(), DataRowComparer.Default);
                                if (query2.Count() > 0)
                                {
                                    dtLowerData = query2.CopyToDataTable();
                                }

                                if (dtLowerData.Rows.Count > 0)
                                {
                                    DataView dvLowerData = dtLowerData.DefaultView;
                                    dvLowerData.Sort = "Tstamp";
                                    DateTime DTBegin = Convert.ToDateTime(dvLowerData[0]["Tstamp"].ToString());
                                    dvLowerData.Sort = "Tstamp desc";
                                    DateTime DTEnd = Convert.ToDateTime(dvLowerData[0]["Tstamp"].ToString());
                                    //生成超下限报警信息
                                    d_DAL.CreateAlarmLower(ApplicationUid, PUid, m_AlarmTypeUid_Lsp, NotifyGradeUid, DataTypeUid, PName, Convert.ToDecimal(ExcessiveLow), FactorName, FactorCode, PId, SY_BufferTable, DTBegin, DTEnd, LSp);
                                }
                            }
                            #endregion
                        }
                        #endregion
                        #endregion
                        #region 重复
                        //根据重复值限配置信息到数据表中获取重复数据
                        DataTable dtAlarmRepeatLimitConfig = d_DAL.GetRepeatLimitConfig(UseForAlarmUid, DataTypeUid, ApplicationUid);
                        foreach (DataRow dr in dtAlarmRepeatLimitConfig.Rows)
                        {
                            string PUid = dr["MonitoringPointUid"].ToString();
                            string PId = dr["PointId"].ToString();
                            string PName = dr["MonitoringPointName"].ToString();
                            string FactorCode = dr["PollutantCode"].ToString();
                            string FactorName = dr["PollutantName"].ToString();
                            string NotifyGradeUid = dr["NotifyGradeUid"].ToString();
                            int RepeatableNumber = Convert.ToInt32(dr["RepeatableNumber"]);
                            DataTable dtRepeatLimitData = new DataTable();
                            if (SuperPointIds.Contains(Convert.ToInt32(PId)))//该站点是超级站
                            {
                                dtRepeatLimitData = d_DAL.GetRepeatLimitData(OriIDLog, OriginTable, PId, FactorCode);
                            }
                            else//该站点是常规站
                            {
                                dtRepeatLimitData = d_DAL.GetRepeatLimitData(IDLog, BufferTable, PId, FactorCode);
                            }
                            int Temp_IDS = -1;//数据临时行号
                            DateTime DateStart = DateTime.Now;//重复开始时间
                            DateTime DateEnd = DateTime.Now;//重复截止时间
                            int Pre_IDS = -1;//重复判断时使用的前一条临时行号
                            decimal PreFactorValue = -9999;//重复判断时使用的上一条数据值
                            for (int i = 0; i < dtRepeatLimitData.Rows.Count; i++)
                            {
                                DateTime Date = Convert.ToDateTime(dtRepeatLimitData.Rows[i]["Tstamp"]);
                                DateTime Date1 = Convert.ToDateTime(dtRepeatLimitData.Rows[i]["Tstamp1"]);
                                decimal FactorValue = Convert.ToDecimal(dtRepeatLimitData.Rows[i]["PollutantValue"]);
                                int IDS = Convert.ToInt32(dtRepeatLimitData.Rows[i]["IDS"]);
                                if (Temp_IDS == -1)
                                {
                                    Temp_IDS = 1;
                                    DateStart = Date;
                                    DateEnd = Date1;
                                    Pre_IDS = IDS;
                                    PreFactorValue = FactorValue;
                                }
                                else if (Pre_IDS == IDS - 1 && PreFactorValue == FactorValue)//判断当前行和前一行数据相同
                                {
                                    Temp_IDS += 1;
                                    DateEnd = Date1;
                                    Pre_IDS = IDS;
                                    PreFactorValue = FactorValue;
                                }
                                else
                                {
                                    DateStart = Date;
                                    Pre_IDS = IDS;
                                    PreFactorValue = FactorValue;
                                }

                                if (Temp_IDS >= RepeatableNumber)
                                {
                                    d_DAL.RepeatLimitFlag(PId, FactorCode, DateStart, DateEnd, BufferTable, DataFlag, UpdateUser, UpdateDateTime);

                                    #region 生成重复报警信息
                                    V_CodeMainItem RepAlarmType = AlarmTypes.Where(p => p.ItemValue.Equals("Rep")).FirstOrDefault();
                                    string m_AlarmTypeUid_Rep = RepAlarmType.ItemGuid;
                                    string content = PName + "站" + DateStart.ToString("yy月dd日HH时") + FactorName + "[" + PreFactorValue + "]连续" + (Temp_IDS + 1) + "组数据重复";
                                    TB_CreatAlarm[] CreatAlarmExists = BaseDataModel.TB_CreatAlarms.Where(p => p.Content.Equals(content)).ToArray();
                                    if (CreatAlarmExists.Count() == 0)
                                    {
                                        TB_CreatAlarm NewCreatAlarm = new TB_CreatAlarm();
                                        NewCreatAlarm.AlarmUid = Guid.NewGuid().ToString();
                                        NewCreatAlarm.ApplicationUid = ApplicationUid;
                                        NewCreatAlarm.MonitoringPointUid = PUid;
                                        NewCreatAlarm.RecordDateTime = DateStart;
                                        NewCreatAlarm.AlarmEventUid = m_AlarmTypeUid_Rep;
                                        NewCreatAlarm.AlarmGradeUid = NotifyGradeUid;
                                        NewCreatAlarm.DataTypeUid = DataTypeUid;
                                        NewCreatAlarm.Content = content;
                                        NewCreatAlarm.SendContent = "@1@=" + ApplicationName + ",@2@=" + PName + ",@3@=" + DateStart.Month + ",@4@=" + DateStart.Day + ",@5@=" + DateStart.Hour + ",@6@=" + FactorName + ",@7@=" + PreFactorValue + ",@8@=" + (Temp_IDS + 1);
                                        NewCreatAlarm.ItemCode = FactorCode;
                                        NewCreatAlarm.ItemName = FactorName;
                                        NewCreatAlarm.ItemValue = PreFactorValue.ToString();
                                        NewCreatAlarm.MonitoringPoint = PName;
                                        NewCreatAlarm.CreatUser = "SYSTEM";
                                        NewCreatAlarm.CreatDateTime = DateTime.Now;

                                        BaseDataModel.Add(NewCreatAlarm);
                                        BaseDataModel.SaveChanges();
                                    }
                                    #endregion
                                    Temp_IDS = -1;
                                }


                            }
                        }
                        #endregion
                        #region 缺失
                        DataTable dtLostConfig = d_DAL.GetLostConfig(ApplicationUid, DataTypeUid);

                        foreach (DataRow dr in dtLostConfig.Rows)
                        {

                            string PUid = dr["MonitoringPointUid"].ToString();
                            int PId = Convert.ToInt32(dr["PointId"]);
                            string PName = dr["MonitoringPointName"].ToString();
                            string NotifyGradeUid = dr["NotifyGradeUid"].ToString();
                            string FactorCode = dr["PollutantCode"].ToString();
                            string FactorName = dr["PollutantName"].ToString();
                            int LostNum = Convert.ToInt32(dr["LostNum"].ToString());
                            DataTable dtLastestByPointData = new DataTable();
                            DataTable dtLastestByPointFactorData = new DataTable();
                            if (SuperPointIds.Contains(PId))//该站点是超级站
                            {
                                //获取当前站点最新数据时间
                                dtLastestByPointData = d_DAL.GetLatestData(OriIDLog, OriginTable, PId);
                                dtLastestByPointFactorData = d_DAL.GetLatestData(OriIDLog, OriginTable, PId, FactorCode);
                            }
                            else//该站点是常规站
                            {
                                //获取当前站点最新数据时间
                                dtLastestByPointData = d_DAL.GetLatestData(IDLog, BufferTable, PId);
                                dtLastestByPointFactorData = d_DAL.GetLatestData(IDLog, BufferTable, PId, FactorCode);
                            }
                            if (dtLastestByPointData.Rows.Count > 0 && dtLastestByPointFactorData.Rows.Count > 0)
                            {
                                //int DataDelayTime = Convert.ToInt32(ConfigurationManager.AppSettings["DataDelayTime"]);
                                DateTime DTFinishByPoint = Convert.ToDateTime(dtLastestByPointData.Rows[0]["Tstamp"]);
                                DateTime DataDT = Convert.ToDateTime(dtLastestByPointFactorData.Rows[0]["Tstamp"]);
                                if (SuperPointIds.Contains(PId))//该站点是超级站
                                {
                                    DataDT = DataDT.AddMinutes(-DataDelayTime);
                                }
                                TimeSpan ts = DTFinishByPoint - DataDT;

                                if (ts.TotalMinutes > LostNum)
                                {
                                    V_CodeMainItem HAlarmType = AlarmTypes.Where(p => p.ItemValue.Equals("H")).FirstOrDefault();
                                    string m_AlarmTypeUid_H = HAlarmType.ItemGuid;

                                    string content = DataDT.ToString("MM月dd日HH时") + "-" + DTFinishByPoint.ToString("MM月dd日HH时") + "，" + PName + "点位" + FactorName + "数据缺失";
                                    string SendContent = PName + ";" + DataDT.ToString("yyyy-MM-dd HH:mm:dd") + ";" + DTFinishByPoint.ToString("yyyy-MM-dd HH:mm:dd") + ";" + FactorName;
                                    TB_CreatAlarm[] CreatAlarmExists = BaseDataModel.TB_CreatAlarms.Where(p => p.Content.Equals(content)).ToArray();
                                    if (CreatAlarmExists.Count() == 0)
                                    {
                                        TB_CreatAlarm NewCreatAlarm = new TB_CreatAlarm();
                                        NewCreatAlarm.AlarmUid = Guid.NewGuid().ToString();
                                        NewCreatAlarm.ApplicationUid = ApplicationUid;
                                        NewCreatAlarm.MonitoringPointUid = PUid;
                                        NewCreatAlarm.RecordDateTime = DataDT;
                                        NewCreatAlarm.AlarmEventUid = m_AlarmTypeUid_H;
                                        NewCreatAlarm.AlarmGradeUid = NotifyGradeUid;
                                        NewCreatAlarm.DataTypeUid = DataTypeUid;
                                        NewCreatAlarm.Content = content;
                                        NewCreatAlarm.SendContent = SendContent;
                                        NewCreatAlarm.ItemCode = FactorCode;
                                        NewCreatAlarm.ItemName = FactorName;
                                        NewCreatAlarm.MonitoringPoint = PName;
                                        NewCreatAlarm.CreatUser = "SYSTEM";
                                        NewCreatAlarm.CreatDateTime = DateTime.Now;

                                        BaseDataModel.Add(NewCreatAlarm);
                                        BaseDataModel.SaveChanges();
                                    }
                                }
                            }

                        }
                        #endregion
                        #region 离线
                        DataTable dtOfflineConfig = d_DAL.GetOfflineConfig(ApplicationUid, DataTypeUid);
                        //获取所有站点最新数据时间
                        DataTable dtLastestData = d_DAL.GetLatestData(IDLog, BufferTable);

                        foreach (DataRow dr in dtOfflineConfig.Rows)
                        {
                            string PUid = dr["MonitoringPointUid"].ToString();
                            int PId = Convert.ToInt32(dr["PointId"]);
                            string PName = dr["MonitoringPointName"].ToString();
                            string NotifyGradeUid = dr["NotifyGradeUid"].ToString();
                            int OffLineTimeSpan = Convert.ToInt32(dr["OffLineTimeSpan"].ToString());
                            DateTime DataDT = DateTime.Now;
                            DataTable dtLatestOnline = new DataTable();
                            int Recent24Records = 0;
                            if (SuperPointIds.Contains(PId))//该站点是超级站
                            {
                                //根据下位报文数据获取在线信息（适用于超级站）
                                dtLatestOnline = d_DAL.GetLatestOnline(PId);
                                if (dtLatestOnline.Rows.Count > 0)
                                {
                                    DataDT = Convert.ToDateTime(dtLatestOnline.Rows[0]["receiveTime"]).AddMinutes(-DataDelayTime);
                                }

                                #region 报文时间格式化
                                if (DataTypeUid == "1b6367f1-5287-4c14-b120-7a35bd176db1")//小时数据
                                {
                                    DataDT = Convert.ToDateTime(DataDT.ToString("yyyy-MM-dd HH:00:00"));
                                }
                                else if (DataTypeUid == "7a894b1f-e990-4cc3-87bb-be1e431c46bf")//5分钟数据
                                {
                                    int minuteTimes = DataDT.Minute / 5;
                                    string minute = (minuteTimes * 5).ToString();
                                    DataDT = Convert.ToDateTime(DataDT.ToString("yyyy-MM-dd HH:") + minute + ":00");
                                }
                                else if (DataTypeUid == "c36398ef-2bec-49be-8fca-b491fecaa359")//1分钟数据
                                {
                                    DataDT = Convert.ToDateTime(DataDT.ToString("yyyy-MM-dd HH:mm:00"));
                                }
                                #endregion
                                Recent24Records = d_DAL.GetRecentHourRecords(PId, DateTime.Now.AddHours(-24), DateTime.Now, "dbo.SY_Air_InfectantBy60");
                            }
                            else//该站点是常规站
                            {
                                //根据缓存表数据获取在线信息（适用于常规站）
                                dtLatestOnline = d_DAL.GetLatestData(PId, BufferTable);
                                if (dtLatestOnline.Rows.Count > 0)
                                    DataDT = Convert.ToDateTime(dtLatestOnline.Rows[0]["Tstamp"]);

                                //获取最近24小时记录数
                                Recent24Records = d_DAL.GetRecentHourRecords(PId, DateTime.Now.AddHours(-24), DateTime.Now, "dbo.SY_Air_InfectantBy60Buffer");

                            }
                            DataOnline DataOnlineExists = MonitorBusinessModel.DataOnlines.Where(p => p.PointId.Equals(PId) && p.DataTypeUid.Equals(DataTypeUid)).FirstOrDefault();
                            if (dtLatestOnline.Rows.Count > 0 && dtLastestData.Rows.Count > 0)
                            {
                                DateTime DTFinish = Convert.ToDateTime(dtLastestData.Rows[0]["Tstamp"]);


                                TimeSpan ts = DTFinish - DataDT;
                                if (ts.TotalMinutes > OffLineTimeSpan)
                                {
                                    //站点离线
                                    V_CodeMainItem OffAlarmType = AlarmTypes.Where(p => p.ItemValue.Equals("Off")).FirstOrDefault();
                                    string m_AlarmTypeUid_Off = OffAlarmType.ItemGuid;

                                    string content = DataDT.ToString("MM月dd日HH时") + "-" + DTFinish.ToString("MM月dd日HH时") + "，" + PName + "点位断线";
                                    string SendContent = PName + ";" + DataDT.ToString("yyyy-MM-dd HH:mm:dd") + ";" + DTFinish.ToString("yyyy-MM-dd HH:mm:dd");

                                    TB_CreatAlarm[] CreatAlarmExists = BaseDataModel.TB_CreatAlarms.Where(p => p.Content.Equals(content)).ToArray();
                                    if (CreatAlarmExists.Count() == 0)
                                    {
                                        TB_CreatAlarm NewCreatAlarm = new TB_CreatAlarm();
                                        NewCreatAlarm.AlarmUid = Guid.NewGuid().ToString();
                                        NewCreatAlarm.ApplicationUid = ApplicationUid;
                                        NewCreatAlarm.MonitoringPointUid = PUid;
                                        NewCreatAlarm.RecordDateTime = DTFinish;
                                        NewCreatAlarm.AlarmEventUid = m_AlarmTypeUid_Off;
                                        NewCreatAlarm.AlarmGradeUid = NotifyGradeUid;
                                        NewCreatAlarm.DataTypeUid = DataTypeUid;
                                        NewCreatAlarm.Content = content;
                                        NewCreatAlarm.SendContent = SendContent;
                                        NewCreatAlarm.ItemCode = "Offline";
                                        NewCreatAlarm.ItemName = "断线";
                                        NewCreatAlarm.MonitoringPoint = PName;
                                        NewCreatAlarm.CreatUser = "SYSTEM";
                                        NewCreatAlarm.CreatDateTime = DateTime.Now;

                                        BaseDataModel.Add(NewCreatAlarm);
                                        BaseDataModel.SaveChanges();
                                    }
                                    if (DataOnlineExists == null)//新增
                                    {
                                        DataOnline NewDataOnline = new DataOnline();
                                        NewDataOnline.ApplicationUid = ApplicationUid;
                                        NewDataOnline.MonitoringPointUid = PUid;
                                        NewDataOnline.PointId = PId;
                                        NewDataOnline.DataTypeUid = DataTypeUid;
                                        NewDataOnline.IsOnline = 0;
                                        NewDataOnline.NewDataTime = DataDT;
                                        NewDataOnline.OffLineTime = Convert.ToInt32(ts.TotalMinutes);
                                        NewDataOnline.Recent24HourRecords = Recent24Records;
                                        MonitorBusinessModel.Add(NewDataOnline);
                                    }
                                    else//更新
                                    {
                                        DataOnlineExists.IsOnline = 0;
                                        DataOnlineExists.NewDataTime = DataDT;
                                        DataOnlineExists.OffLineTime = Convert.ToInt32(ts.TotalMinutes);
                                        DataOnlineExists.Recent24HourRecords = Recent24Records;

                                    }
                                }
                                else
                                {
                                    //站点在线
                                    if (DataOnlineExists == null)//新增
                                    {
                                        DataOnline NewDataOnline = new DataOnline();
                                        NewDataOnline.ApplicationUid = ApplicationUid;
                                        NewDataOnline.MonitoringPointUid = PUid;
                                        NewDataOnline.PointId = PId;
                                        NewDataOnline.DataTypeUid = DataTypeUid;
                                        NewDataOnline.IsOnline = 1;
                                        NewDataOnline.NewDataTime = DataDT;
                                        NewDataOnline.OffLineTime = null;
                                        NewDataOnline.Recent24HourRecords = Recent24Records;
                                        MonitorBusinessModel.Add(NewDataOnline);
                                    }
                                    else//更新
                                    {
                                        DataOnlineExists.IsOnline = 1;
                                        DataOnlineExists.NewDataTime = DataDT;
                                        DataOnlineExists.OffLineTime = null;
                                        DataOnlineExists.Recent24HourRecords = Recent24Records;
                                    }
                                }

                            }

                        }
                        #endregion
                        #endregion
                        MonitorBusinessModel.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("------------------------------------------Flag异常:" + ex.ToString());
            }

        }
        /// <summary>
        /// 批量处理数据
        /// </summary>
        /// <param name="DataTypeUid">数据类型</param>
        /// <param name="IDLog">ID记录</param>
        public void BatchProcess(string DataTypeUid, long BufferIDLog, long OriIDLog, long FlagIDLog)
        {
            try
            {

                string CommonIfOriginal = ConfigurationManager.AppSettings["CommonIfOriginal"];//常规站生成原始数据开关
                string CommonIfPreAudit = ConfigurationManager.AppSettings["CommonIfPreAudit"];//常规站生成预审核数据开关
                string CommonIfCalculate = ConfigurationManager.AppSettings["CommonIfCalculate"];//常规站生成数据计算表开关
                string SuperIfOriginal = ConfigurationManager.AppSettings["SuperIfOriginal"];//超级站生成原始数据开关
                string SuperIfPreAudit = ConfigurationManager.AppSettings["SuperIfPreAudit"];//超级站生成预审核数据开关
                string SuperIfCalculate = ConfigurationManager.AppSettings["SuperIfCalculate"];//超级站生成数据计算表开关
                using (BaseDataModel BaseDataModel = new BaseDataModel())
                {
                    //获取配置表中信息
                    DT_DataTypeConfig DataTypeConfig = BaseDataModel.DT_DataTypeConfigs.Where(p => p.EnableOrNot.Equals(true) && p.DataTypeUid.Equals(DataTypeUid)).FirstOrDefault();
                    string BufferTable = DataTypeConfig.BufferTable;//缓存表
                    string OriginalTable = DataTypeConfig.OriginalTable;//原始表

                    #region 生成原始查询表数据
                    if (CommonIfOriginal.ToLower().Equals("true"))
                    {
                        //批量处理标记后的数据
                        d_DAL.AddOriginalData(BufferTable, OriginalTable, BufferIDLog, FlagIDLog, CommonPointIds);

                    }
                    if (SuperIfOriginal.ToLower().Equals("true"))
                    {
                        //批量处理标记后的数据
                        d_DAL.AddOriginalData(BufferTable, OriginalTable, BufferIDLog, FlagIDLog, SuperPointIds);

                    }
                    #endregion

                    #region 小时数据需生成预审核数据和小时计算数据和报表数据
                    if (DataTypeUid.Equals("1b6367f1-5287-4c14-b120-7a35bd176db1"))
                    {
                        //log.Info("--------------小时数据批量生成预审核表数据");
                        #region 预审核
                        //生成预审核表数据
                        if (CommonIfPreAudit.ToLower().Equals("true"))
                        {
                            //log.Info("BufferIDLog:" + BufferIDLog + "FlagIDLog:" + FlagIDLog);
                            //批量处理标记后的数据
                            d_DAL.Load_AuditPreData_NotAudit(CommonPointIds, ApplicationUid, BufferIDLog, FlagIDLog);

                        }
                        if (SuperIfPreAudit.ToLower().Equals("true"))
                        {
                            //批量处理标记后的数据
                            d_DAL.Load_AuditPreData_NotAudit(SuperPointIds, ApplicationUid, OriIDLog);
                        }
                        #endregion

                        #region 数据计算和报表
                        //生成数据计算表数据和小时报表数据
                        if (CommonIfCalculate.ToLower().Equals("true"))
                        {
                            string CalculateTableName = "AirReport.TB_HourReport_Calculate";
                            //小时计算数据
                            d_DAL.AddCommonAirReport_Hour_Mul(BufferIDLog, FlagIDLog, CommonPointIds, CalculateTableName);
                            string AuditTableName = "AirReport.TB_HourReport";
                            //审核小时数据
                            d_DAL.AddCommonAirReport_Hour_Mul(BufferIDLog, FlagIDLog, CommonPointIds, AuditTableName);

                        }
                        if (SuperIfCalculate.ToLower().Equals("true"))
                        {
                            string CalculateTableName = "AirReport.TB_HourReport_Calculate";
                            //小时计算数据
                            d_DAL.AddAirReport_Hour_Mul(BufferIDLog, FlagIDLog, SuperPointIds, CalculateTableName);
                            string AuditTableName = "AirReport.TB_HourReport";
                            //审核小时数据
                            d_DAL.AddAirReport_Hour_Mul(BufferIDLog, FlagIDLog, SuperPointIds, AuditTableName);
                        }
                        #endregion
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                log.Error("------------------------------------------------BatchProcess异常:" + ex.ToString());
            }

        }
        /// <summary>
        /// 历史数据批量处理
        /// </summary>
        public void FillProcessData()
        {
            try
            {
                using (FrameworkModel FrameworkModel = new FrameworkModel())
                {
                    V_CodeMainItem DataType = DataTypeCodeMainItems.Where(p => p.ItemValue.Equals("Min60")).FirstOrDefault();
                    string DataTypeUid = DataType.ItemGuid;
                    using (BaseDataModel BaseDataModel = new BaseDataModel())
                    {
                        DT_ConfigIDInfo IDLogFlagPre = BaseDataModel.DT_ConfigIDInfos.Where(p => p.Memo.Equals("TB_InfectantBy60BufferFlag")).FirstOrDefault();
                        long IDLogFlag = Convert.ToInt32(IDLogFlagPre.Maxid);
                        DT_ConfigIDInfo OriIDLogFlagPre = BaseDataModel.DT_ConfigIDInfos.Where(p => p.Memo.Equals("TB_InfectantBy60Flag")).FirstOrDefault();
                        long OriIDLogFlag = Convert.ToInt32(OriIDLogFlagPre.Maxid);
                        DT_ConfigIDInfo BufferIDLogBatchProcessPre = BaseDataModel.DT_ConfigIDInfos.Where(p => p.Memo.Equals("TB_InfectantBy60BufferBatchProcess")).FirstOrDefault();
                        long BufferIDLogBatchProcess = Convert.ToInt32(BufferIDLogBatchProcessPre.Maxid);
                        DT_ConfigIDInfo IDLogBatchProcessPre = BaseDataModel.DT_ConfigIDInfos.Where(p => p.Memo.Equals("TB_InfectantBy60BatchProcess")).FirstOrDefault();
                        long IDLogBatchProcess = Convert.ToInt32(IDLogBatchProcessPre.Maxid);
                        try
                        {
                            Flag(DataTypeUid, IDLogFlag, OriIDLogFlag);
                            InstrumentOnline(DataTypeUid);
                            using (AirAutoMonitorModel AirAutoMonitorModel = new AirAutoMonitorModel())
                            {
                                //获取缓存表中最新ID
                                TB_InfectantBy60Buffer Last_InfectantBy60Buffer = AirAutoMonitorModel.TB_InfectantBy60Buffers.OrderByDescending(p => p.Id).FirstOrDefault();
                                if (Last_InfectantBy60Buffer != null)
                                {
                                    long NewID = Last_InfectantBy60Buffer.Id;
                                    IDLogFlagPre.Maxid = NewID;
                                    IDLogFlagPre.Updatedatetime = DateTime.Now;
                                    BaseDataModel.SaveChanges();

                                    try
                                    {
                                        BatchProcess(DataTypeUid, BufferIDLogBatchProcess, IDLogBatchProcess, NewID);

                                        BufferIDLogBatchProcessPre.Maxid = NewID;
                                        BufferIDLogBatchProcessPre.Updatedatetime = DateTime.Now;
                                        //获取原始表中最新ID
                                        TB_InfectantBy60 Last_InfectantBy60 = AirAutoMonitorModel.TB_InfectantBy60.OrderByDescending(p => p.Id).FirstOrDefault();
                                        if (Last_InfectantBy60 != null)
                                        {
                                            long OriNewID = Last_InfectantBy60.Id;
                                            IDLogBatchProcessPre.Maxid = OriNewID;
                                            IDLogBatchProcessPre.Updatedatetime = DateTime.Now;
                                        }
                                        BaseDataModel.SaveChanges();
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error("----------------------------BatchProcess异常：" + ex.ToString());
                                    }

                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("----------------------------Flag异常：" + ex.ToString());
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("----------------------------FillProcessData异常：" + ex.ToString());
            }
        }
        /// <summary>
        /// 小时数据计算小时AQI,日数据日AQI(该方法执行时间较长)
        /// </summary>
        public void CalculateBy60(DateTime startTime, DateTime endTime)
        {
            try
            {
                //log.Info("-------------------------------------------------------------CalculateBy60开始-------------------");
                DateTime sTime = Convert.ToDateTime(startTime.ToString("yyyy-MM-dd 00:00:00"));
                DateTime eTime = Convert.ToDateTime(endTime.ToString("yyyy-MM-dd 23:59:59"));
                string PointIdstr = string.Empty;
                for (int m = 0; m < PointIds.Length;m++ )
                {
                    PointIdstr += PointIds[m]+",";
                }
                using (AirAutoMonitorModel AirAutoMonitorModel = new AirAutoMonitorModel())
                {
                    #region 原始测点AQI
                    foreach (int PointId in PointIds)
                    {
                        #region 计算原始小时AQI
                        DataTable dtPivotOriHour = d_DAL.GetPivotOriHourData(PointId, sTime, eTime);
                        foreach (DataRow dr in dtPivotOriHour.Rows)
                        {
                            #region 数据
                            DateTime Tstamp = Convert.ToDateTime(dr["Tstamp"]);
                            decimal? SO2_Value = null;
                            if (dr["SO2"] != DBNull.Value)
                                SO2_Value = Convert.ToDecimal(dr["SO2"]);
                            int? SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 1);

                            decimal? NO2_Value = null;
                            if (dr["NO2"] != DBNull.Value)
                                NO2_Value = Convert.ToDecimal(dr["NO2"]);
                            int? AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 1);

                            decimal? PM10_Value = null;
                            if (dr["PM10"] != DBNull.Value)
                                PM10_Value = Convert.ToDecimal(dr["PM10"]);
                            int? AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);

                            decimal? Recent24HoursPM10_Value = null;
                            if (dr["Recent24HoursPM10"] != DBNull.Value)
                                Recent24HoursPM10_Value = Convert.ToDecimal(dr["Recent24HoursPM10"]);
                            int? AQI_Recent24HoursPM10 = d_AQICalculateService.GetIAQI("a34002", Recent24HoursPM10_Value, 24);

                            decimal? CO_Value = null;
                            //先修约再计算AQI
                            if (dr["CO"] != DBNull.Value)
                            {
                                DataTable dtFactor = d_AQICalculateService.GetPollutantUnit("a21005");
                                int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);
                                if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m3"))
                                {
                                    CO_Value = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO"]), decimalNum) * 1000);

                                }
                                else
                                {
                                    CO_Value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO"]), decimalNum);
                                }
                                //CO_Value = Convert.ToDecimal(dr["CO"]);
                            }
                            int? AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 1);

                            decimal? O3_Value = null;
                            if (dr["O3"] != DBNull.Value)
                                O3_Value = Convert.ToDecimal(dr["O3"]);
                            int? AQI_O3 = d_AQICalculateService.GetIAQI("a05024", O3_Value, 1);

                            decimal? Recent8HoursO3_Value = null;
                            if (dr["Recent8HoursO3"] != DBNull.Value)
                                Recent8HoursO3_Value = Convert.ToDecimal(dr["Recent8HoursO3"]);
                            int? AQI_Recent8HoursO3 = d_AQICalculateService.GetIAQI("a05024", Recent8HoursO3_Value, 8);

                            decimal? Recent8HoursO3NT_Value = null;
                            if (dr["Recent8HoursO3NT"] != DBNull.Value)
                                Recent8HoursO3NT_Value = Convert.ToDecimal(dr["Recent8HoursO3NT"]);
                            int? AQI_Recent8HoursO3NT = d_AQICalculateService.GetIAQI("a05024", Recent8HoursO3NT_Value, 8);

                            decimal? PM25_Value = null;
                            if (dr["PM25"] != DBNull.Value)
                                PM25_Value = Convert.ToDecimal(dr["PM25"]);
                            int? AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);

                            decimal? Recent24HoursPM25_Value = null;
                            if (dr["Recent24HoursPM25"] != DBNull.Value)
                                Recent24HoursPM25_Value = Convert.ToDecimal(dr["Recent24HoursPM25"]);
                            int? AQI_Recent24HoursPM25 = d_AQICalculateService.GetIAQI("a34004", Recent24HoursPM25_Value, 24);
                            #endregion
                            TB_OriHourAQI OriHourAQIExists = AirAutoMonitorModel.TB_OriHourAQIs.Where(p => p.PointId.Equals(PointId) && p.DateTime.Equals(Tstamp)).FirstOrDefault();
                            if (OriHourAQIExists == null)
                            {
                                #region 新增小时AQI
                                TB_OriHourAQI NewOriHourAQI = new TB_OriHourAQI();
                                NewOriHourAQI.PointId = PointId;
                                NewOriHourAQI.DateTime = Tstamp;

                                if (SO2_Value != null)
                                    NewOriHourAQI.SO2 = SO2_Value.ToString();
                                if (SO2_IAQI != null)
                                    NewOriHourAQI.SO2_IAQI = SO2_IAQI.ToString();

                                if (NO2_Value != null)
                                    NewOriHourAQI.NO2 = NO2_Value.ToString();
                                if (NO2_Value != null)
                                    NewOriHourAQI.NO2_IAQI = AQI_NO2.ToString();

                                if (PM10_Value != null)
                                    NewOriHourAQI.PM10 = PM10_Value.ToString();
                                if (AQI_PM10 != null)
                                    NewOriHourAQI.PM10_IAQI = AQI_PM10.ToString();

                                if (Recent24HoursPM10_Value != null)
                                    NewOriHourAQI.Recent24HoursPM10 = Recent24HoursPM10_Value.ToString();
                                if (AQI_Recent24HoursPM10 != null)
                                    NewOriHourAQI.Recent24HoursPM10_IAQI = AQI_Recent24HoursPM10.ToString();

                                if (CO_Value != null)
                                    NewOriHourAQI.CO = CO_Value.ToString();
                                if (AQI_CO != null)
                                    NewOriHourAQI.CO_IAQI = AQI_CO.ToString();

                                if (O3_Value != null)
                                    NewOriHourAQI.O3 = O3_Value.ToString();
                                if (AQI_O3 != null)
                                    NewOriHourAQI.O3_IAQI = AQI_O3.ToString();

                                if (Recent8HoursO3_Value != null)
                                    NewOriHourAQI.Recent8HoursO3 = Recent8HoursO3_Value.ToString();
                                if (AQI_Recent8HoursO3 != null)
                                    NewOriHourAQI.Recent8HoursO3_IAQI = AQI_Recent8HoursO3.ToString();

                                if (Recent8HoursO3NT_Value != null)
                                    NewOriHourAQI.Recent8HoursO3NT = Recent8HoursO3NT_Value.ToString();
                                if (AQI_Recent8HoursO3NT != null)
                                    NewOriHourAQI.Recent8HoursO3NT_IAQI = AQI_Recent8HoursO3NT.ToString();

                                if (PM25_Value != null)
                                    NewOriHourAQI.PM25 = PM25_Value.ToString();
                                if (AQI_PM25 != null)
                                    NewOriHourAQI.PM25_IAQI = AQI_PM25.ToString();

                                if (Recent24HoursPM25_Value != null)
                                    NewOriHourAQI.Recent24HoursPM25 = Recent24HoursPM25_Value.ToString();
                                if (AQI_Recent24HoursPM25 != null)
                                    NewOriHourAQI.Recent24HoursPM25_IAQI = AQI_Recent24HoursPM25.ToString();

                                string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
                                NewOriHourAQI.AQIValue = AQIValue;
                                NewOriHourAQI.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");
                                if (!string.IsNullOrWhiteSpace(AQIValue))
                                {
                                    NewOriHourAQI.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                    NewOriHourAQI.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                    NewOriHourAQI.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                    NewOriHourAQI.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                    NewOriHourAQI.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
                                    NewOriHourAQI.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                                }
                                NewOriHourAQI.CreatUser = "SystemSync";
                                NewOriHourAQI.CreatDateTime = DateTime.Now;
                                AirAutoMonitorModel.Add(NewOriHourAQI);
                                //AirAutoMonitorModel.SaveChanges();
                                #endregion
                            }
                            else
                            {
                                #region 更新小时AQI
                                if (SO2_Value != null)
                                    OriHourAQIExists.SO2 = SO2_Value.ToString();
                                else
                                    OriHourAQIExists.SO2 = null;
                                if (SO2_IAQI != null)
                                    OriHourAQIExists.SO2_IAQI = SO2_IAQI.ToString();
                                else
                                    OriHourAQIExists.SO2_IAQI = null;
                                if (NO2_Value != null)
                                    OriHourAQIExists.NO2 = NO2_Value.ToString();
                                else
                                    OriHourAQIExists.NO2 = null;
                                if (AQI_NO2 != null)
                                    OriHourAQIExists.NO2_IAQI = AQI_NO2.ToString();
                                else
                                    OriHourAQIExists.NO2_IAQI = null;

                                if (PM10_Value != null)
                                    OriHourAQIExists.PM10 = PM10_Value.ToString();
                                else
                                    OriHourAQIExists.PM10 = null;
                                if (AQI_PM10 != null)
                                    OriHourAQIExists.PM10_IAQI = AQI_PM10.ToString();
                                else
                                    OriHourAQIExists.PM10_IAQI = null;

                                if (Recent24HoursPM10_Value != null)
                                    OriHourAQIExists.Recent24HoursPM10 = Recent24HoursPM10_Value.ToString();
                                else
                                    OriHourAQIExists.Recent24HoursPM10 = null;
                                if (AQI_Recent24HoursPM10 != null)
                                    OriHourAQIExists.Recent24HoursPM10_IAQI = AQI_Recent24HoursPM10.ToString();
                                else
                                    OriHourAQIExists.Recent24HoursPM10_IAQI = null;

                                if (CO_Value != null)
                                    OriHourAQIExists.CO = CO_Value.ToString();
                                else
                                    OriHourAQIExists.CO = null;
                                if (AQI_CO != null)
                                    OriHourAQIExists.CO_IAQI = AQI_CO.ToString();
                                else
                                    OriHourAQIExists.CO_IAQI = null;

                                if (O3_Value != null)
                                    OriHourAQIExists.O3 = O3_Value.ToString();
                                else
                                    OriHourAQIExists.O3 = null;
                                if (AQI_O3 != null)
                                    OriHourAQIExists.O3_IAQI = AQI_O3.ToString();
                                else
                                    OriHourAQIExists.O3_IAQI = null;

                                if (Recent8HoursO3_Value != null)
                                    OriHourAQIExists.Recent8HoursO3 = Recent8HoursO3_Value.ToString();
                                else
                                    OriHourAQIExists.Recent8HoursO3 = null;
                                if (AQI_Recent8HoursO3 != null)
                                    OriHourAQIExists.Recent8HoursO3_IAQI = AQI_Recent8HoursO3.ToString();
                                else
                                    OriHourAQIExists.Recent8HoursO3_IAQI = null;

                                if (Recent8HoursO3NT_Value != null)
                                    OriHourAQIExists.Recent8HoursO3NT = Recent8HoursO3NT_Value.ToString();
                                else
                                    OriHourAQIExists.Recent8HoursO3NT = null;
                                if (AQI_Recent8HoursO3NT != null)
                                    OriHourAQIExists.Recent8HoursO3NT_IAQI = AQI_Recent8HoursO3NT.ToString();
                                else
                                    OriHourAQIExists.Recent8HoursO3NT_IAQI = null;

                                if (PM25_Value != null)
                                    OriHourAQIExists.PM25 = PM25_Value.ToString();
                                else
                                    OriHourAQIExists.PM25 = null;
                                if (AQI_PM25 != null)
                                    OriHourAQIExists.PM25_IAQI = AQI_PM25.ToString();
                                else
                                    OriHourAQIExists.PM25_IAQI = null;

                                if (Recent24HoursPM25_Value != null)
                                    OriHourAQIExists.Recent24HoursPM25 = Recent24HoursPM25_Value.ToString();
                                else
                                    OriHourAQIExists.Recent24HoursPM25 = null;
                                if (AQI_Recent24HoursPM25 != null)
                                    OriHourAQIExists.Recent24HoursPM25_IAQI = AQI_Recent24HoursPM25.ToString();
                                else
                                    OriHourAQIExists.Recent24HoursPM25_IAQI = null;

                                string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
                                OriHourAQIExists.AQIValue = AQIValue;
                                OriHourAQIExists.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");
                                if (!string.IsNullOrWhiteSpace(AQIValue))
                                {
                                    OriHourAQIExists.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                    OriHourAQIExists.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                    OriHourAQIExists.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                    OriHourAQIExists.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                    OriHourAQIExists.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
                                    OriHourAQIExists.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                                }
                                else
                                {
                                    OriHourAQIExists.Range = null;
                                    OriHourAQIExists.RGBValue = null;
                                    OriHourAQIExists.Class = null;
                                    OriHourAQIExists.Grade = null;
                                    OriHourAQIExists.HealthEffect = null;
                                    OriHourAQIExists.TakeStep = null;
                                }
                                OriHourAQIExists.UpdateUser = "SystemSync";
                                OriHourAQIExists.UpdateDateTime = DateTime.Now;
                                //AirAutoMonitorModel.SaveChanges();
                                #endregion
                            }
                        }
                        #endregion
                        AirAutoMonitorModel.SaveChanges();

                        //计算生成原始日数据
                        d_DAL.AddOriginalDayData(sTime, eTime, PointId);

                        #region 计算生成原始日AQI
                        DataTable dtPivotDay = d_DAL.GetPivotOriDayData(PointId, sTime, eTime);
                        foreach (DataRow dr in dtPivotDay.Rows)
                        {
                            #region 数据
                            DateTime Tstamp = Convert.ToDateTime(dr["DateTime"]);
                            decimal? SO2_Value = null;
                            if (dr["SO2"] != DBNull.Value)
                                SO2_Value = Convert.ToDecimal(dr["SO2"]);
                            int? SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 24);

                            decimal? NO2_Value = null;
                            if (dr["NO2"] != DBNull.Value)
                                NO2_Value = Convert.ToDecimal(dr["NO2"]);
                            int? AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 24);

                            decimal? PM10_Value = null;
                            if (dr["PM10"] != DBNull.Value)
                                PM10_Value = Convert.ToDecimal(dr["PM10"]);
                            int? AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);


                            decimal? CO_Value = null;
                            if (dr["CO"] != DBNull.Value)
                                CO_Value = Convert.ToDecimal(dr["CO"]);
                            int? AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 24);

                            decimal? MaxOneHourO3_Value = null;
                            if (dr["MaxOneHourO3"] != DBNull.Value)
                                MaxOneHourO3_Value = Convert.ToDecimal(dr["MaxOneHourO3"]);
                            int? AQI_MaxOneHourO3 = d_AQICalculateService.GetIAQI("a05024", MaxOneHourO3_Value, 1);

                            decimal? Max8HourO3_Value = null;
                            if (dr["Max8HourO3"] != DBNull.Value)
                                Max8HourO3_Value = Convert.ToDecimal(dr["Max8HourO3"]);
                            int? AQI_Max8HourO3 = d_AQICalculateService.GetIAQI("a05024", Max8HourO3_Value, 8);

                            decimal? PM25_Value = null;
                            if (dr["PM25"] != DBNull.Value)
                                PM25_Value = Convert.ToDecimal(dr["PM25"]);
                            int? AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);

                            #endregion
                            TB_OriDayAQI OriDayAQIExists = AirAutoMonitorModel.TB_OriDayAQIs.Where(p => p.PointId.Equals(PointId) && p.DateTime.Equals(Tstamp)).FirstOrDefault();
                            if (OriDayAQIExists == null)
                            {
                                #region 新增日AQI
                                TB_OriDayAQI NewOriDayAQI = new TB_OriDayAQI();
                                NewOriDayAQI.PointId = PointId;
                                NewOriDayAQI.DateTime = Tstamp;

                                if (SO2_Value != null)
                                    NewOriDayAQI.SO2 = SO2_Value.ToString();
                                if (SO2_IAQI != null)
                                    NewOriDayAQI.SO2_IAQI = SO2_IAQI.ToString();

                                if (NO2_Value != null)
                                    NewOriDayAQI.NO2 = NO2_Value.ToString();
                                if (AQI_NO2 != null)
                                    NewOriDayAQI.NO2_IAQI = AQI_NO2.ToString();

                                if (PM10_Value != null)
                                    NewOriDayAQI.PM10 = PM10_Value.ToString();
                                if (AQI_PM10 != null)
                                    NewOriDayAQI.PM10_IAQI = AQI_PM10.ToString();

                                if (CO_Value != null)
                                    NewOriDayAQI.CO = CO_Value.ToString();
                                if (AQI_CO != null)
                                    NewOriDayAQI.CO_IAQI = AQI_CO.ToString();

                                if (MaxOneHourO3_Value != null)
                                    NewOriDayAQI.MaxOneHourO3 = MaxOneHourO3_Value.ToString();
                                if (AQI_MaxOneHourO3 != null)
                                    NewOriDayAQI.MaxOneHourO3_IAQI = AQI_MaxOneHourO3.ToString();

                                if (Max8HourO3_Value != null)
                                    NewOriDayAQI.Max8HourO3 = Max8HourO3_Value.ToString();
                                if (AQI_Max8HourO3 != null)
                                    NewOriDayAQI.Max8HourO3_IAQI = AQI_Max8HourO3.ToString();

                                if (PM25_Value != null)
                                    NewOriDayAQI.PM25 = PM25_Value.ToString();
                                if (AQI_PM25 != null)
                                    NewOriDayAQI.PM25_IAQI = AQI_PM25.ToString();

                                string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "V");
                                NewOriDayAQI.AQIValue = AQIValue;
                                NewOriDayAQI.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "N");
                                if (!string.IsNullOrWhiteSpace(AQIValue))
                                {
                                    NewOriDayAQI.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                    NewOriDayAQI.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                    NewOriDayAQI.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                    NewOriDayAQI.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                    NewOriDayAQI.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
                                    NewOriDayAQI.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                                }
                                NewOriDayAQI.CreatUser = "SystemSync";
                                NewOriDayAQI.CreatDateTime = DateTime.Now;
                                AirAutoMonitorModel.Add(NewOriDayAQI);
                                //AirAutoMonitorModel.SaveChanges();
                                #endregion
                            }
                            else
                            {
                                #region 更新日AQI
                                if (SO2_Value != null)
                                    OriDayAQIExists.SO2 = SO2_Value.ToString();
                                else
                                    OriDayAQIExists.SO2 = null;

                                if (SO2_IAQI != null)
                                    OriDayAQIExists.SO2_IAQI = SO2_IAQI.ToString();
                                else
                                    OriDayAQIExists.SO2_IAQI = null;

                                if (NO2_Value != null)
                                    OriDayAQIExists.NO2 = NO2_Value.ToString();
                                else
                                    OriDayAQIExists.NO2 = null;
                                if (AQI_NO2 != null)
                                    OriDayAQIExists.NO2_IAQI = AQI_NO2.ToString();
                                else
                                    OriDayAQIExists.NO2_IAQI = null;

                                if (PM10_Value != null)
                                    OriDayAQIExists.PM10 = PM10_Value.ToString();
                                else
                                    OriDayAQIExists.PM10 = null;
                                if (AQI_PM10 != null)
                                    OriDayAQIExists.PM10_IAQI = AQI_PM10.ToString();
                                else
                                    OriDayAQIExists.PM10_IAQI = null;

                                if (CO_Value != null)
                                    OriDayAQIExists.CO = CO_Value.ToString();
                                else
                                    OriDayAQIExists.CO = null;
                                if (AQI_CO != null)
                                    OriDayAQIExists.CO_IAQI = AQI_CO.ToString();
                                else
                                    OriDayAQIExists.CO_IAQI = null;

                                if (MaxOneHourO3_Value != null)
                                    OriDayAQIExists.MaxOneHourO3 = MaxOneHourO3_Value.ToString();
                                else
                                    OriDayAQIExists.MaxOneHourO3 = null;
                                if (AQI_MaxOneHourO3 != null)
                                    OriDayAQIExists.MaxOneHourO3_IAQI = AQI_MaxOneHourO3.ToString();
                                else
                                    OriDayAQIExists.MaxOneHourO3_IAQI = null;

                                if (Max8HourO3_Value != null)
                                    OriDayAQIExists.Max8HourO3 = Max8HourO3_Value.ToString();
                                else
                                    OriDayAQIExists.Max8HourO3 = null;
                                if (AQI_Max8HourO3 != null)
                                    OriDayAQIExists.Max8HourO3_IAQI = AQI_Max8HourO3.ToString();
                                else
                                    OriDayAQIExists.Max8HourO3_IAQI = null;

                                if (PM25_Value != null)
                                    OriDayAQIExists.PM25 = PM25_Value.ToString();
                                else
                                    OriDayAQIExists.PM25 = null;
                                if (AQI_PM25 != null)
                                    OriDayAQIExists.PM25_IAQI = AQI_PM25.ToString();
                                else
                                    OriDayAQIExists.PM25_IAQI = null;

                                string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "V");
                                OriDayAQIExists.AQIValue = AQIValue;
                                OriDayAQIExists.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "N");
                                if (!string.IsNullOrWhiteSpace(AQIValue))
                                {
                                    OriDayAQIExists.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                    OriDayAQIExists.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                    OriDayAQIExists.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                    OriDayAQIExists.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                    OriDayAQIExists.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEAlTHEFFECT");
                                    OriDayAQIExists.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                                }
                                else
                                {
                                    OriDayAQIExists.Range = null;
                                    OriDayAQIExists.RGBValue = null;
                                    OriDayAQIExists.Class = null;
                                    OriDayAQIExists.Grade = null;
                                    OriDayAQIExists.HealthEffect = null;
                                    OriDayAQIExists.TakeStep = null;
                                }
                                OriDayAQIExists.UpdateUser = "SystemSync";
                                OriDayAQIExists.UpdateDateTime = DateTime.Now;
                                //AirAutoMonitorModel.SaveChanges();
                                #endregion
                            }
                        }
                        #endregion
                        AirAutoMonitorModel.SaveChanges();

                    }
                    #endregion
                    #region 原始区域AQI
                    foreach (string m_region in m_regions)
                    {
                        #region 原始小时区域AQI
                        DataTable dtPivotOriRegionHour = d_DAL.GetPivotOriRegionHourData(m_region, sTime, eTime);
                        foreach (DataRow dr in dtPivotOriRegionHour.Rows)
                        {
                            #region 数据
                            DateTime Tstamp = Convert.ToDateTime(dr["DateTime"]);
                            decimal? SO2_Value = null;
                            if (dr["SO2"] != DBNull.Value)
                                SO2_Value = Convert.ToDecimal(dr["SO2"]);
                            int? SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 1);

                            decimal? NO2_Value = null;
                            if (dr["NO2"] != DBNull.Value)
                                NO2_Value = Convert.ToDecimal(dr["NO2"]);
                            int? AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 1);

                            decimal? PM10_Value = null;
                            if (dr["PM10"] != DBNull.Value)
                                PM10_Value = Convert.ToDecimal(dr["PM10"]);
                            int? AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);

                            decimal? Recent24HoursPM10_Value = null;
                            if (dr["Recent24HoursPM10"] != DBNull.Value)
                                Recent24HoursPM10_Value = Convert.ToDecimal(dr["Recent24HoursPM10"]);
                            int? AQI_Recent24HoursPM10 = d_AQICalculateService.GetIAQI("a34002", Recent24HoursPM10_Value, 24);

                            decimal? CO_Value = null;
                            //先修约再计算AQI
                            if (dr["CO"] != DBNull.Value)
                            {
                                DataTable dtFactor = d_AQICalculateService.GetPollutantUnit("a21005");
                                int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);
                                if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m3"))
                                {
                                    CO_Value = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO"]), decimalNum) * 1000);

                                }
                                else
                                {
                                    CO_Value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO"]), decimalNum);
                                }
                            }
                            int? AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 1);

                            decimal? O3_Value = null;
                            if (dr["O3"] != DBNull.Value)
                                O3_Value = Convert.ToDecimal(dr["O3"]);
                            int? AQI_O3 = d_AQICalculateService.GetIAQI("a05024", O3_Value, 1);

                            decimal? Recent8HoursO3_Value = null;
                            if (dr["Recent8HoursO3"] != DBNull.Value)
                                Recent8HoursO3_Value = Convert.ToDecimal(dr["Recent8HoursO3"]);
                            int? AQI_Recent8HoursO3 = d_AQICalculateService.GetIAQI("a05024", Recent8HoursO3_Value, 8);

                            decimal? Recent8HoursO3NT_Value = null;
                            if (dr["Recent8HoursO3NT"] != DBNull.Value)
                                Recent8HoursO3NT_Value = Convert.ToDecimal(dr["Recent8HoursO3NT"]);
                            int? AQI_Recent8HoursO3NT = d_AQICalculateService.GetIAQI("a05024", Recent8HoursO3NT_Value, 8);

                            decimal? PM25_Value = null;
                            if (dr["PM25"] != DBNull.Value)
                                PM25_Value = Convert.ToDecimal(dr["PM25"]);
                            int? AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);

                            decimal? Recent24HoursPM25_Value = null;
                            if (dr["Recent24HoursPM25"] != DBNull.Value)
                                Recent24HoursPM25_Value = Convert.ToDecimal(dr["Recent24HoursPM25"]);
                            int? AQI_Recent24HoursPM25 = d_AQICalculateService.GetIAQI("a34004", Recent24HoursPM25_Value, 24);
                            #endregion
                            TB_OriRegionHourAQI OriRegionHourAQIExists = AirAutoMonitorModel.TB_OriRegionHourAQIs.Where(p => p.MonitoringRegionUid.Equals(m_region) && p.DateTime.Equals(Tstamp)).FirstOrDefault();
                            if (OriRegionHourAQIExists == null)
                            {
                                #region 新增小时AQI
                                TB_OriRegionHourAQI NewOriHourAQI = new TB_OriRegionHourAQI();
                                NewOriHourAQI.MonitoringRegionUid = m_region;
                                NewOriHourAQI.DateTime = Tstamp;
                                NewOriHourAQI.StatisticalType = "CG";

                                if (SO2_Value != null)
                                    NewOriHourAQI.SO2 = SO2_Value.ToString();
                                if (SO2_IAQI != null)
                                    NewOriHourAQI.SO2_IAQI = SO2_IAQI.ToString();

                                if (NO2_Value != null)
                                    NewOriHourAQI.NO2 = NO2_Value.ToString();
                                if (AQI_NO2 != null)
                                    NewOriHourAQI.NO2_IAQI = AQI_NO2.ToString();

                                if (PM10_Value != null)
                                    NewOriHourAQI.PM10 = PM10_Value.ToString();
                                if (AQI_PM10 != null)
                                    NewOriHourAQI.PM10_IAQI = AQI_PM10.ToString();

                                if (Recent24HoursPM10_Value != null)
                                    NewOriHourAQI.Recent24HoursPM10 = Recent24HoursPM10_Value.ToString();
                                if (AQI_Recent24HoursPM10 != null)
                                    NewOriHourAQI.Recent24HoursPM10_IAQI = AQI_Recent24HoursPM10.ToString();

                                if (CO_Value != null)
                                    NewOriHourAQI.CO = CO_Value.ToString();
                                if (AQI_CO != null)
                                    NewOriHourAQI.CO_IAQI = AQI_CO.ToString();

                                if (O3_Value != null)
                                    NewOriHourAQI.O3 = O3_Value.ToString();
                                if (AQI_O3 != null)
                                    NewOriHourAQI.O3_IAQI = AQI_O3.ToString();

                                if (Recent8HoursO3_Value != null)
                                    NewOriHourAQI.Recent8HoursO3 = Recent8HoursO3_Value.ToString();
                                if (AQI_Recent8HoursO3 != null)
                                    NewOriHourAQI.Recent8HoursO3_IAQI = AQI_Recent8HoursO3.ToString();

                                if (Recent8HoursO3NT_Value != null)
                                    NewOriHourAQI.Recent8HoursO3NT = Recent8HoursO3NT_Value.ToString();
                                if (AQI_Recent8HoursO3NT != null)
                                    NewOriHourAQI.Recent8HoursO3NT_IAQI = AQI_Recent8HoursO3NT.ToString();

                                if (PM25_Value != null)
                                    NewOriHourAQI.PM25 = PM25_Value.ToString();
                                if (AQI_PM25 != null)
                                    NewOriHourAQI.PM25_IAQI = AQI_PM25.ToString();

                                if (Recent24HoursPM25_Value != null)
                                    NewOriHourAQI.Recent24HoursPM25 = Recent24HoursPM25_Value.ToString();
                                if (AQI_Recent24HoursPM25 != null)
                                    NewOriHourAQI.Recent24HoursPM25_IAQI = AQI_Recent24HoursPM25.ToString();

                                string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
                                NewOriHourAQI.AQIValue = AQIValue;
                                NewOriHourAQI.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");
                                if (!string.IsNullOrWhiteSpace(AQIValue))
                                {
                                    NewOriHourAQI.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                    NewOriHourAQI.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                    NewOriHourAQI.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                    NewOriHourAQI.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                    NewOriHourAQI.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
                                    NewOriHourAQI.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                                }
                                NewOriHourAQI.CreatUser = "SystemSync";
                                NewOriHourAQI.CreatDateTime = DateTime.Now;
                                AirAutoMonitorModel.Add(NewOriHourAQI);
                                //AirAutoMonitorModel.SaveChanges();
                                #endregion
                            }
                            else
                            {
                                #region 更新小时AQI
                                if (SO2_Value != null)
                                    OriRegionHourAQIExists.SO2 = SO2_Value.ToString();
                                else
                                    OriRegionHourAQIExists.SO2 = null;
                                if (SO2_IAQI != null)
                                    OriRegionHourAQIExists.SO2_IAQI = SO2_IAQI.ToString();
                                else
                                    OriRegionHourAQIExists.SO2_IAQI = null;

                                if (NO2_Value != null)
                                    OriRegionHourAQIExists.NO2 = NO2_Value.ToString();
                                else
                                    OriRegionHourAQIExists.NO2 = null;
                                if (AQI_NO2 != null)
                                    OriRegionHourAQIExists.NO2_IAQI = AQI_NO2.ToString();
                                else
                                    OriRegionHourAQIExists.NO2_IAQI = null;

                                if (PM10_Value != null)
                                    OriRegionHourAQIExists.PM10 = PM10_Value.ToString();
                                else
                                    OriRegionHourAQIExists.PM10 = null;
                                if (AQI_PM10 != null)
                                    OriRegionHourAQIExists.PM10_IAQI = AQI_PM10.ToString();
                                else
                                    OriRegionHourAQIExists.PM10_IAQI = null;

                                if (Recent24HoursPM10_Value != null)
                                    OriRegionHourAQIExists.Recent24HoursPM10 = Recent24HoursPM10_Value.ToString();
                                else
                                    OriRegionHourAQIExists.Recent24HoursPM10 = null;
                                if (AQI_Recent24HoursPM10 != null)
                                    OriRegionHourAQIExists.Recent24HoursPM10_IAQI = AQI_Recent24HoursPM10.ToString();
                                else
                                    OriRegionHourAQIExists.Recent24HoursPM10_IAQI = null;

                                if (CO_Value != null)
                                    OriRegionHourAQIExists.CO = CO_Value.ToString();
                                else
                                    OriRegionHourAQIExists.CO = null;
                                if (AQI_CO != null)
                                    OriRegionHourAQIExists.CO_IAQI = AQI_CO.ToString();
                                else
                                    OriRegionHourAQIExists.CO_IAQI = null;

                                if (O3_Value != null)
                                    OriRegionHourAQIExists.O3 = O3_Value.ToString();
                                else
                                    OriRegionHourAQIExists.O3 = null;
                                if (AQI_O3 != null)
                                    OriRegionHourAQIExists.O3_IAQI = AQI_O3.ToString();
                                else
                                    OriRegionHourAQIExists.O3_IAQI = null;

                                if (Recent8HoursO3_Value != null)
                                    OriRegionHourAQIExists.Recent8HoursO3 = Recent8HoursO3_Value.ToString();
                                else
                                    OriRegionHourAQIExists.Recent8HoursO3 = null;
                                if (AQI_Recent8HoursO3 != null)
                                    OriRegionHourAQIExists.Recent8HoursO3_IAQI = AQI_Recent8HoursO3.ToString();
                                else
                                    OriRegionHourAQIExists.Recent8HoursO3_IAQI = null;

                                if (Recent8HoursO3NT_Value != null)
                                    OriRegionHourAQIExists.Recent8HoursO3NT = Recent8HoursO3NT_Value.ToString();
                                else
                                    OriRegionHourAQIExists.Recent8HoursO3NT = null;
                                if (AQI_Recent8HoursO3NT != null)
                                    OriRegionHourAQIExists.Recent8HoursO3NT_IAQI = AQI_Recent8HoursO3NT.ToString();
                                else
                                    OriRegionHourAQIExists.Recent8HoursO3NT_IAQI = null;

                                if (PM25_Value != null)
                                    OriRegionHourAQIExists.PM25 = PM25_Value.ToString();
                                else
                                    OriRegionHourAQIExists.PM25 = null;
                                if (AQI_PM25 != null)
                                    OriRegionHourAQIExists.PM25_IAQI = AQI_PM25.ToString();
                                else
                                    OriRegionHourAQIExists.PM25_IAQI = null;

                                if (Recent24HoursPM25_Value != null)
                                    OriRegionHourAQIExists.Recent24HoursPM25 = Recent24HoursPM25_Value.ToString();
                                else
                                    OriRegionHourAQIExists.Recent24HoursPM25 = null;
                                if (AQI_Recent24HoursPM25 != null)
                                    OriRegionHourAQIExists.Recent24HoursPM25_IAQI = AQI_Recent24HoursPM25.ToString();
                                else
                                    OriRegionHourAQIExists.Recent24HoursPM25_IAQI = null;

                                string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
                                OriRegionHourAQIExists.AQIValue = AQIValue;
                                OriRegionHourAQIExists.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");
                                if (!string.IsNullOrWhiteSpace(AQIValue))
                                {
                                    OriRegionHourAQIExists.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                    OriRegionHourAQIExists.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                    OriRegionHourAQIExists.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                    OriRegionHourAQIExists.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                    OriRegionHourAQIExists.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
                                    OriRegionHourAQIExists.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                                }
                                else
                                {
                                    OriRegionHourAQIExists.Range = null;
                                    OriRegionHourAQIExists.RGBValue = null;
                                    OriRegionHourAQIExists.Class = null;
                                    OriRegionHourAQIExists.Grade = null;
                                    OriRegionHourAQIExists.HealthEffect = null;
                                    OriRegionHourAQIExists.TakeStep = null;
                                }
                                OriRegionHourAQIExists.UpdateUser = "SystemSync";
                                OriRegionHourAQIExists.UpdateDateTime = DateTime.Now;
                                //AirAutoMonitorModel.SaveChanges();
                                #endregion
                            }
                        }
                        #endregion
                        AirAutoMonitorModel.SaveChanges();

                        #region 原始日区域AQI
                        DataTable dtPivotOriRegionDay = d_DAL.GetPivotOriRegionDayData(m_region, sTime, eTime);
                        foreach (DataRow dr in dtPivotOriRegionDay.Rows)
                        {
                            #region 数据
                            DateTime Tstamp = Convert.ToDateTime(dr["DateTime"]);

                            //decimal? SO2_Value = null;
                            //if (dr["SO2"] != DBNull.Value)
                            //    SO2_Value = Convert.ToDecimal(dr["SO2"]);
                            //int? SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 24);

                            //decimal? NO2_Value = null;
                            //if (dr["NO2"] != DBNull.Value)
                            //    NO2_Value = Convert.ToDecimal(dr["NO2"]);
                            //int? AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 24);

                            //decimal? PM10_Value = null;
                            //if (dr["PM10"] != DBNull.Value)
                            //    PM10_Value = Convert.ToDecimal(dr["PM10"]);
                            //int? AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);


                            //decimal? CO_Value = null;
                            //if (dr["CO"] != DBNull.Value)
                            //    CO_Value = Convert.ToDecimal(dr["CO"]);
                            //int? AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 24);

                            //decimal? MaxOneHourO3_Value = null;
                            //if (dr["MaxOneHourO3"] != DBNull.Value)
                            //    MaxOneHourO3_Value = Convert.ToDecimal(dr["MaxOneHourO3"]);
                            //int? AQI_MaxOneHourO3 = d_AQICalculateService.GetIAQI("a05024", MaxOneHourO3_Value, 1);

                            //decimal? Max8HourO3_Value = null;
                            //if (dr["Max8HourO3"] != DBNull.Value)
                            //    Max8HourO3_Value = Convert.ToDecimal(dr["Max8HourO3"]);
                            //int? AQI_Max8HourO3 = d_AQICalculateService.GetIAQI("a05024", Max8HourO3_Value, 8);

                            //decimal? PM25_Value = null;
                            //if (dr["PM25"] != DBNull.Value)
                            //    PM25_Value = Convert.ToDecimal(dr["PM25"]);
                            //int? AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);

                            decimal? SO2_Value = null;
                            int? SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_MaxOneHourO3, AQI_Max8HourO3, AQI_PM25;
                            if (dr["SO2"] != DBNull.Value)
                            {
                                if (Convert.ToDecimal(dr["SO2"]) >= 10000)
                                {
                                    SO2_Value = Convert.ToDecimal(dr["SO2"]) - 10000;
                                    SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", null, 24);
                                }
                                else
                                {
                                    SO2_Value = Convert.ToDecimal(dr["SO2"]);
                                    SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 24);
                                }
                            }
                            else
                            {
                                SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 24);
                            }


                            decimal? NO2_Value = null;
                            if (dr["NO2"] != DBNull.Value)
                            {
                                if (Convert.ToDecimal(dr["NO2"]) >= 10000)
                                {
                                    NO2_Value = Convert.ToDecimal(dr["NO2"]) - 10000;
                                    AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", null, 24);
                                }
                                else
                                {
                                    NO2_Value = Convert.ToDecimal(dr["NO2"]);
                                    AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 24);
                                }
                            }
                            else
                            {
                                AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 24);
                            }


                            decimal? PM10_Value = null;
                            if (dr["PM10"] != DBNull.Value)
                            {
                                if (Convert.ToDecimal(dr["PM10"]) >= 10000)
                                {
                                    PM10_Value = Convert.ToDecimal(dr["PM10"]) - 10000;
                                    AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", null, 24);
                                }
                                else
                                {
                                    PM10_Value = Convert.ToDecimal(dr["PM10"]);
                                    AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);
                                }
                            }
                            else
                            {
                                AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);
                            }


                            decimal? CO_Value = null;
                            if (dr["CO"] != DBNull.Value)
                            {
                                if (Convert.ToDecimal(dr["CO"]) >= 10000)
                                {
                                    CO_Value = Convert.ToDecimal(dr["CO"]) - 10000;
                                    AQI_CO = d_AQICalculateService.GetIAQI("a21005", null, 24);
                                }
                                else
                                {
                                    CO_Value = Convert.ToDecimal(dr["CO"]);
                                    AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 24);
                                }
                            }
                            else
                            {
                                AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 24);
                            }


                            decimal? MaxOneHourO3_Value = null;
                            if (dr["MaxOneHourO3"] != DBNull.Value)
                            {

                                if (Convert.ToDecimal(dr["MaxOneHourO3"]) >= 10000)
                                {
                                    MaxOneHourO3_Value = Convert.ToDecimal(dr["MaxOneHourO3"]) - 10000;
                                    AQI_MaxOneHourO3 = d_AQICalculateService.GetIAQI("a05024", null, 1);
                                }
                                else
                                {
                                    MaxOneHourO3_Value = Convert.ToDecimal(dr["MaxOneHourO3"]);
                                    AQI_MaxOneHourO3 = d_AQICalculateService.GetIAQI("a05024", MaxOneHourO3_Value, 1);
                                }
                            }
                            else
                            {
                                AQI_MaxOneHourO3 = d_AQICalculateService.GetIAQI("a05024", MaxOneHourO3_Value, 1);
                            }


                            decimal? Max8HourO3_Value = null;
                            if (dr["Max8HourO3"] != DBNull.Value)
                            {
                                if (Convert.ToDecimal(dr["Max8HourO3"]) >= 10000)
                                {
                                    Max8HourO3_Value = Convert.ToDecimal(dr["Max8HourO3"]) - 10000;
                                    AQI_Max8HourO3 = d_AQICalculateService.GetIAQI("a05024", null, 8);
                                }
                                else
                                {
                                    Max8HourO3_Value = Convert.ToDecimal(dr["Max8HourO3"]);
                                    AQI_Max8HourO3 = d_AQICalculateService.GetIAQI("a05024", Max8HourO3_Value, 8);
                                }
                            }
                            else
                            {
                                AQI_Max8HourO3 = d_AQICalculateService.GetIAQI("a05024", Max8HourO3_Value, 8);
                            }


                            decimal? PM25_Value = null;
                            if (dr["PM25"] != DBNull.Value)
                            {
                                if (Convert.ToDecimal(dr["PM25"]) >= 10000)
                                {
                                    PM25_Value = Convert.ToDecimal(dr["PM25"]) - 10000;
                                    AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", null, 24);
                                }
                                else
                                {
                                    PM25_Value = Convert.ToDecimal(dr["PM25"]);
                                    AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);
                                }
                            }
                            else
                            {
                                AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);
                            }

                            #endregion
                            TB_OriRegionDayAQIReport OriRegionDayAQIExists = AirAutoMonitorModel.TB_OriRegionDayAQIReports.Where(p => p.MonitoringRegionUid.Equals(m_region) && p.ReportDateTime.Equals(Tstamp)).FirstOrDefault();
                            if (OriRegionDayAQIExists == null)
                            {
                                #region 新增日AQI
                                TB_OriRegionDayAQIReport NewOriDayAQI = new TB_OriRegionDayAQIReport();
                                NewOriDayAQI.MonitoringRegionUid = m_region;
                                NewOriDayAQI.ReportDateTime = Tstamp;
                                NewOriDayAQI.StatisticalType = "CG";

                                if (SO2_Value != null)
                                    NewOriDayAQI.SO2 = SO2_Value.ToString();
                                if (SO2_IAQI != null)
                                    NewOriDayAQI.SO2_IAQI = SO2_IAQI.ToString();

                                if (NO2_Value != null)
                                    NewOriDayAQI.NO2 = NO2_Value.ToString();
                                if (AQI_NO2 != null)
                                    NewOriDayAQI.NO2_IAQI = AQI_NO2.ToString();

                                if (PM10_Value != null)
                                    NewOriDayAQI.PM10 = PM10_Value.ToString();
                                if (AQI_PM10 != null)
                                    NewOriDayAQI.PM10_IAQI = AQI_PM10.ToString();

                                if (CO_Value != null)
                                    NewOriDayAQI.CO = CO_Value.ToString();
                                if (AQI_CO != null)
                                    NewOriDayAQI.CO_IAQI = AQI_CO.ToString();

                                if (MaxOneHourO3_Value != null)
                                    NewOriDayAQI.MaxOneHourO3 = MaxOneHourO3_Value.ToString();
                                if (AQI_MaxOneHourO3 != null)
                                    NewOriDayAQI.MaxOneHourO3_IAQI = AQI_MaxOneHourO3.ToString();

                                if (Max8HourO3_Value != null)
                                    NewOriDayAQI.Max8HourO3 = Max8HourO3_Value.ToString();
                                if (AQI_Max8HourO3 != null)
                                    NewOriDayAQI.Max8HourO3_IAQI = AQI_Max8HourO3.ToString();

                                if (PM25_Value != null)
                                    NewOriDayAQI.PM25 = PM25_Value.ToString();
                                if (AQI_PM25 != null)
                                    NewOriDayAQI.PM25_IAQI = AQI_PM25.ToString();

                                string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "V");
                                NewOriDayAQI.AQIValue = AQIValue;
                                NewOriDayAQI.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "N");
                                if (!string.IsNullOrWhiteSpace(AQIValue))
                                {
                                    NewOriDayAQI.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                    NewOriDayAQI.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                    NewOriDayAQI.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                    NewOriDayAQI.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                    NewOriDayAQI.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
                                    NewOriDayAQI.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                                }
                                NewOriDayAQI.CreatUser = "SystemSync";
                                NewOriDayAQI.CreatDateTime = DateTime.Now;
                                AirAutoMonitorModel.Add(NewOriDayAQI);
                                //AirAutoMonitorModel.SaveChanges();
                                #endregion
                            }
                            else
                            {
                                #region 更新日AQI
                                if (SO2_Value != null)
                                    OriRegionDayAQIExists.SO2 = SO2_Value.ToString();
                                else
                                    OriRegionDayAQIExists.SO2 = null;
                                if (SO2_IAQI != null)
                                    OriRegionDayAQIExists.SO2_IAQI = SO2_IAQI.ToString();
                                else
                                    OriRegionDayAQIExists.SO2_IAQI = null;

                                if (NO2_Value != null)
                                    OriRegionDayAQIExists.NO2 = NO2_Value.ToString();
                                else
                                    OriRegionDayAQIExists.NO2 = null;
                                if (AQI_NO2 != null)
                                    OriRegionDayAQIExists.NO2_IAQI = AQI_NO2.ToString();
                                else
                                    OriRegionDayAQIExists.NO2_IAQI = null;

                                if (PM10_Value != null)
                                    OriRegionDayAQIExists.PM10 = PM10_Value.ToString();
                                else
                                    OriRegionDayAQIExists.PM10 = null;
                                if (AQI_PM10 != null)
                                    OriRegionDayAQIExists.PM10_IAQI = AQI_PM10.ToString();
                                else
                                    OriRegionDayAQIExists.PM10_IAQI = null;

                                if (CO_Value != null)
                                    OriRegionDayAQIExists.CO = CO_Value.ToString();
                                else
                                    OriRegionDayAQIExists.CO = null;
                                if (AQI_CO != null)
                                    OriRegionDayAQIExists.CO_IAQI = AQI_CO.ToString();
                                else
                                    OriRegionDayAQIExists.CO_IAQI = null;

                                if (MaxOneHourO3_Value != null)
                                    OriRegionDayAQIExists.MaxOneHourO3 = MaxOneHourO3_Value.ToString();
                                else
                                    OriRegionDayAQIExists.MaxOneHourO3 = null;
                                if (AQI_MaxOneHourO3 != null)
                                    OriRegionDayAQIExists.MaxOneHourO3_IAQI = AQI_MaxOneHourO3.ToString();
                                else
                                    OriRegionDayAQIExists.MaxOneHourO3_IAQI = null;

                                if (Max8HourO3_Value != null)
                                    OriRegionDayAQIExists.Max8HourO3 = Max8HourO3_Value.ToString();
                                else
                                    OriRegionDayAQIExists.Max8HourO3 = null;
                                if (AQI_Max8HourO3 != null)
                                    OriRegionDayAQIExists.Max8HourO3_IAQI = AQI_Max8HourO3.ToString();
                                else
                                    OriRegionDayAQIExists.Max8HourO3_IAQI = null;

                                if (PM25_Value != null)
                                    OriRegionDayAQIExists.PM25 = PM25_Value.ToString();
                                else
                                    OriRegionDayAQIExists.PM25 = null;
                                if (AQI_PM25 != null)
                                    OriRegionDayAQIExists.PM25_IAQI = AQI_PM25.ToString();
                                else
                                    OriRegionDayAQIExists.PM25_IAQI = null;

                                string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "V");
                                OriRegionDayAQIExists.AQIValue = AQIValue;
                                OriRegionDayAQIExists.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "N");
                                if (!string.IsNullOrWhiteSpace(AQIValue))
                                {
                                    OriRegionDayAQIExists.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                    OriRegionDayAQIExists.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                    OriRegionDayAQIExists.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                    OriRegionDayAQIExists.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                    OriRegionDayAQIExists.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEAlTHEFFECT");
                                    OriRegionDayAQIExists.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                                }
                                else
                                {
                                    OriRegionDayAQIExists.Range = null;
                                    OriRegionDayAQIExists.RGBValue = null;
                                    OriRegionDayAQIExists.Class = null;
                                    OriRegionDayAQIExists.Grade = null;
                                    OriRegionDayAQIExists.HealthEffect = null;
                                    OriRegionDayAQIExists.TakeStep = null;
                                }
                                OriRegionDayAQIExists.UpdateUser = "SystemSync";
                                OriRegionDayAQIExists.UpdateDateTime = DateTime.Now;
                                //AirAutoMonitorModel.SaveChanges();
                                #endregion
                            }
                        }
                        #endregion
                        AirAutoMonitorModel.SaveChanges();
                    }
                    #endregion
                }
                #region
                using (MonitorBusinessModel MonitorBusinessModel = new MonitorBusinessModel())
                {
                    #region 审核测点AQI
                    foreach (int PointId in PointIds)
                    {
                        #region 计算审核小时AQI
                        DataTable dtPivotAudHour = d_DAL.GetPivotAudHourData(PointId, sTime, eTime);
                        foreach (DataRow dr in dtPivotAudHour.Rows)
                        {
                            #region 数据
                            DateTime Tstamp = Convert.ToDateTime(dr["Tstamp"]);
                            decimal? SO2_Value = null;
                            if (dr["SO2"] != DBNull.Value)
                                SO2_Value = Convert.ToDecimal(dr["SO2"]);
                            int? SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 1);

                            decimal? NO2_Value = null;
                            if (dr["NO2"] != DBNull.Value)
                                NO2_Value = Convert.ToDecimal(dr["NO2"]);
                            int? AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 1);

                            decimal? PM10_Value = null;
                            if (dr["PM10"] != DBNull.Value)
                                PM10_Value = Convert.ToDecimal(dr["PM10"]);
                            int? AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);

                            decimal? Recent24HoursPM10_Value = null;
                            if (dr["Recent24HoursPM10"] != DBNull.Value)
                                Recent24HoursPM10_Value = Convert.ToDecimal(dr["Recent24HoursPM10"]);
                            int? AQI_Recent24HoursPM10 = d_AQICalculateService.GetIAQI("a34002", Recent24HoursPM10_Value, 24);

                            decimal? CO_Value = null;
                            //先修约再计算AQI
                            if (dr["CO"] != DBNull.Value)
                            {
                                DataTable dtFactor = d_AQICalculateService.GetPollutantUnit("a21005");
                                int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);
                                if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m3"))
                                {
                                    CO_Value = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO"]), decimalNum) * 1000);

                                }
                                else
                                {
                                    CO_Value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO"]), decimalNum);
                                }
                            }
                            int? AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 1);

                            decimal? O3_Value = null;
                            if (dr["O3"] != DBNull.Value)
                                O3_Value = Convert.ToDecimal(dr["O3"]);
                            int? AQI_O3 = d_AQICalculateService.GetIAQI("a05024", O3_Value, 1);

                            decimal? Recent8HoursO3_Value = null;
                            if (dr["Recent8HoursO3"] != DBNull.Value)
                                Recent8HoursO3_Value = Convert.ToDecimal(dr["Recent8HoursO3"]);
                            int? AQI_Recent8HoursO3 = d_AQICalculateService.GetIAQI("a05024", Recent8HoursO3_Value, 8);

                            decimal? Recent8HoursO3NT_Value = null;
                            if (dr["Recent8HoursO3NT"] != DBNull.Value)
                                Recent8HoursO3NT_Value = Convert.ToDecimal(dr["Recent8HoursO3NT"]);
                            int? AQI_Recent8HoursO3NT = d_AQICalculateService.GetIAQI("a05024", Recent8HoursO3NT_Value, 8);

                            decimal? PM25_Value = null;
                            if (dr["PM25"] != DBNull.Value)
                                PM25_Value = Convert.ToDecimal(dr["PM25"]);
                            int? AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);

                            decimal? Recent24HoursPM25_Value = null;
                            if (dr["Recent24HoursPM25"] != DBNull.Value)
                                Recent24HoursPM25_Value = Convert.ToDecimal(dr["Recent24HoursPM25"]);
                            int? AQI_Recent24HoursPM25 = d_AQICalculateService.GetIAQI("a34004", Recent24HoursPM25_Value, 24);
                            #endregion

                            TB_HourAQI HourAQIExists = MonitorBusinessModel.TB_HourAQIs.Where(p => p.PointId.Equals(PointId) && p.DateTime.Equals(Tstamp)).FirstOrDefault();
                            if (HourAQIExists == null)
                            {
                                #region 新增小时AQI
                                TB_HourAQI NewAudHourAQI = new TB_HourAQI();
                                NewAudHourAQI.PointId = PointId;
                                NewAudHourAQI.DateTime = Tstamp;

                                if (SO2_Value != null)
                                    NewAudHourAQI.SO2 = SO2_Value.ToString();
                                if (SO2_IAQI != null)
                                    NewAudHourAQI.SO2_IAQI = SO2_IAQI.ToString();

                                if (NO2_Value != null)
                                    NewAudHourAQI.NO2 = NO2_Value.ToString();
                                if (AQI_NO2 != null)
                                    NewAudHourAQI.NO2_IAQI = AQI_NO2.ToString();

                                if (PM10_Value != null)
                                    NewAudHourAQI.PM10 = PM10_Value.ToString();
                                if (AQI_PM10 != null)
                                    NewAudHourAQI.PM10_IAQI = AQI_PM10.ToString();

                                if (Recent24HoursPM10_Value != null)
                                    NewAudHourAQI.Recent24HoursPM10 = Recent24HoursPM10_Value.ToString();
                                if (AQI_Recent24HoursPM10 != null)
                                    NewAudHourAQI.Recent24HoursPM10_IAQI = AQI_Recent24HoursPM10.ToString();

                                if (CO_Value != null)
                                    NewAudHourAQI.CO = CO_Value.ToString();
                                if (AQI_CO != null)
                                    NewAudHourAQI.CO_IAQI = AQI_CO.ToString();

                                if (O3_Value != null)
                                    NewAudHourAQI.O3 = O3_Value.ToString();
                                if (AQI_O3 != null)
                                    NewAudHourAQI.O3_IAQI = AQI_O3.ToString();

                                if (Recent8HoursO3_Value != null)
                                    NewAudHourAQI.Recent8HoursO3 = Recent8HoursO3_Value.ToString();
                                if (AQI_Recent8HoursO3 != null)
                                    NewAudHourAQI.Recent8HoursO3_IAQI = AQI_Recent8HoursO3.ToString();

                                if (Recent8HoursO3NT_Value != null)
                                    NewAudHourAQI.Recent8HoursO3NT = Recent8HoursO3NT_Value.ToString();
                                if (AQI_Recent8HoursO3NT != null)
                                    NewAudHourAQI.Recent8HoursO3NT_IAQI = AQI_Recent8HoursO3NT.ToString();

                                if (PM25_Value != null)
                                    NewAudHourAQI.PM25 = PM25_Value.ToString();
                                if (AQI_PM25 != null)
                                    NewAudHourAQI.PM25_IAQI = AQI_PM25.ToString();

                                if (Recent24HoursPM25_Value != null)
                                    NewAudHourAQI.Recent24HoursPM25 = Recent24HoursPM25_Value.ToString();
                                if (AQI_Recent24HoursPM25 != null)
                                    NewAudHourAQI.Recent24HoursPM25_IAQI = AQI_Recent24HoursPM25.ToString();

                                string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
                                NewAudHourAQI.AQIValue = AQIValue;
                                NewAudHourAQI.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");
                                if (!string.IsNullOrWhiteSpace(AQIValue))
                                {
                                    NewAudHourAQI.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                    NewAudHourAQI.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                    NewAudHourAQI.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                    NewAudHourAQI.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                    NewAudHourAQI.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
                                    NewAudHourAQI.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                                }
                                NewAudHourAQI.CreatUser = "SystemSync";
                                NewAudHourAQI.CreatDateTime = DateTime.Now;
                                MonitorBusinessModel.Add(NewAudHourAQI);
                                //MonitorBusinessModel.SaveChanges();
                                #endregion
                            }
                            else
                            {
                                #region 更新小时AQI
                                if (SO2_Value != null)
                                    HourAQIExists.SO2 = SO2_Value.ToString();
                                else
                                    HourAQIExists.SO2 = null;
                                if (SO2_IAQI != null)
                                    HourAQIExists.SO2_IAQI = SO2_IAQI.ToString();
                                else
                                    HourAQIExists.SO2_IAQI = null;
                                if (NO2_Value != null)
                                    HourAQIExists.NO2 = NO2_Value.ToString();
                                else
                                    HourAQIExists.NO2 = null;
                                if (AQI_NO2 != null)
                                    HourAQIExists.NO2_IAQI = AQI_NO2.ToString();
                                else
                                    HourAQIExists.NO2_IAQI = null;
                                if (PM10_Value != null)
                                    HourAQIExists.PM10 = PM10_Value.ToString();
                                else
                                    HourAQIExists.PM10 = null;
                                if (AQI_PM10 != null)
                                    HourAQIExists.PM10_IAQI = AQI_PM10.ToString();
                                else
                                    HourAQIExists.PM10_IAQI = null;
                                if (Recent24HoursPM10_Value != null)
                                    HourAQIExists.Recent24HoursPM10 = Recent24HoursPM10_Value.ToString();
                                else
                                    HourAQIExists.Recent24HoursPM10 = null;
                                if (AQI_Recent24HoursPM10 != null)
                                    HourAQIExists.Recent24HoursPM10_IAQI = AQI_Recent24HoursPM10.ToString();
                                else
                                    HourAQIExists.Recent24HoursPM10_IAQI = null;
                                if (CO_Value != null)
                                    HourAQIExists.CO = CO_Value.ToString();
                                else
                                    HourAQIExists.CO = null;
                                if (AQI_CO != null)
                                    HourAQIExists.CO_IAQI = AQI_CO.ToString();
                                else
                                    HourAQIExists.CO_IAQI = null;
                                if (O3_Value != null)
                                    HourAQIExists.O3 = O3_Value.ToString();
                                else
                                    HourAQIExists.O3 = null;
                                if (AQI_O3 != null)
                                    HourAQIExists.O3_IAQI = AQI_O3.ToString();
                                else
                                    HourAQIExists.O3_IAQI = null;
                                if (Recent8HoursO3_Value != null)
                                    HourAQIExists.Recent8HoursO3 = Recent8HoursO3_Value.ToString();
                                else
                                    HourAQIExists.Recent8HoursO3 = null;
                                if (AQI_Recent8HoursO3 != null)
                                    HourAQIExists.Recent8HoursO3_IAQI = AQI_Recent8HoursO3.ToString();
                                else
                                    HourAQIExists.Recent8HoursO3_IAQI = null;
                                if (Recent8HoursO3NT_Value != null)
                                    HourAQIExists.Recent8HoursO3NT = Recent8HoursO3NT_Value.ToString();
                                else
                                    HourAQIExists.Recent8HoursO3NT = null;
                                if (AQI_Recent8HoursO3NT != null)
                                    HourAQIExists.Recent8HoursO3NT_IAQI = AQI_Recent8HoursO3NT.ToString();
                                else
                                    HourAQIExists.Recent8HoursO3NT_IAQI = null;
                                if (PM25_Value != null)
                                    HourAQIExists.PM25 = PM25_Value.ToString();
                                else
                                    HourAQIExists.PM25 = null;
                                if (AQI_PM25 != null)
                                    HourAQIExists.PM25_IAQI = AQI_PM25.ToString();
                                else
                                    HourAQIExists.PM25_IAQI = null;
                                if (Recent24HoursPM25_Value != null)
                                    HourAQIExists.Recent24HoursPM25 = Recent24HoursPM25_Value.ToString();
                                else
                                    HourAQIExists.Recent24HoursPM25 = null;
                                if (AQI_Recent24HoursPM25 != null)
                                    HourAQIExists.Recent24HoursPM25_IAQI = AQI_Recent24HoursPM25.ToString();
                                else
                                    HourAQIExists.Recent24HoursPM25_IAQI = null;
                                string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
                                HourAQIExists.AQIValue = AQIValue;
                                HourAQIExists.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");
                                if (!string.IsNullOrWhiteSpace(AQIValue))
                                {
                                    HourAQIExists.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                    HourAQIExists.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                    HourAQIExists.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                    HourAQIExists.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                    HourAQIExists.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
                                    HourAQIExists.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                                }
                                else
                                {
                                    HourAQIExists.Range = null;
                                    HourAQIExists.RGBValue = null;
                                    HourAQIExists.Class = null;
                                    HourAQIExists.Grade = null;
                                    HourAQIExists.HealthEffect = null;
                                    HourAQIExists.TakeStep = null;
                                }
                                HourAQIExists.UpdateUser = "SystemSync";
                                HourAQIExists.UpdateDateTime = DateTime.Now;
                                //MonitorBusinessModel.SaveChanges();
                                #endregion
                            }
                        }
                        #endregion
                        MonitorBusinessModel.SaveChanges();

                        string CalculateTableName = "AirReport.TB_DayReport_Calculate";
                        // 生成日计算数据
                        d_DAL.AddAirReport_Day_Mul(sTime, eTime, PointId, CalculateTableName);

                        string AuditTableName = "AirReport.TB_DayReport";
                        //日审核数据
                        d_DAL.AddAirReport_Day_Mul(sTime, eTime, PointId, AuditTableName);

                        #region 计算生成审核日AQI
                        DataTable dtPivotDay = d_DAL.GetPivotAudDayData(PointId, sTime, eTime);
                        foreach (DataRow dr in dtPivotDay.Rows)
                        {
                            #region 数据
                            DateTime Tstamp = Convert.ToDateTime(dr["DateTime"]);
                            decimal? SO2_Value = null;
                            if (dr["SO2"] != DBNull.Value)
                                SO2_Value = Convert.ToDecimal(dr["SO2"]);
                            int? SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 24);

                            decimal? NO2_Value = null;
                            if (dr["NO2"] != DBNull.Value)
                                NO2_Value = Convert.ToDecimal(dr["NO2"]);
                            int? AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 24);

                            decimal? PM10_Value = null;
                            if (dr["PM10"] != DBNull.Value)
                                PM10_Value = Convert.ToDecimal(dr["PM10"]);
                            int? AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);


                            decimal? CO_Value = null;
                            if (dr["CO"] != DBNull.Value)
                                CO_Value = Convert.ToDecimal(dr["CO"]);
                            int? AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 24);

                            decimal? MaxOneHourO3_Value = null;
                            if (dr["MaxOneHourO3"] != DBNull.Value)
                                MaxOneHourO3_Value = Convert.ToDecimal(dr["MaxOneHourO3"]);
                            int? AQI_MaxOneHourO3 = d_AQICalculateService.GetIAQI("a05024", MaxOneHourO3_Value, 1);

                            decimal? Max8HourO3_Value = null;
                            if (dr["Max8HourO3"] != DBNull.Value)
                                Max8HourO3_Value = Convert.ToDecimal(dr["Max8HourO3"]);
                            int? AQI_Max8HourO3 = d_AQICalculateService.GetIAQI("a05024", Max8HourO3_Value, 8);

                            decimal? PM25_Value = null;
                            if (dr["PM25"] != DBNull.Value)
                                PM25_Value = Convert.ToDecimal(dr["PM25"]);
                            int? AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);

                            #endregion
                            TB_DayAQI AudDayAQIExists = MonitorBusinessModel.TB_DayAQIs.Where(p => p.PointId.Equals(PointId) && p.DateTime.Equals(Tstamp)).FirstOrDefault();
                            if (AudDayAQIExists == null)
                            {
                                #region 新增日AQI
                                TB_DayAQI NewAudDayAQI = new TB_DayAQI();
                                NewAudDayAQI.PointId = PointId;
                                NewAudDayAQI.DateTime = Tstamp;

                                if (SO2_Value != null)
                                    NewAudDayAQI.SO2 = SO2_Value.ToString();
                                if (SO2_IAQI != null)
                                    NewAudDayAQI.SO2_IAQI = SO2_IAQI.ToString();

                                if (NO2_Value != null)
                                    NewAudDayAQI.NO2 = NO2_Value.ToString();
                                if (AQI_NO2 != null)
                                    NewAudDayAQI.NO2_IAQI = AQI_NO2.ToString();

                                if (PM10_Value != null)
                                    NewAudDayAQI.PM10 = PM10_Value.ToString();
                                if (AQI_PM10 != null)
                                    NewAudDayAQI.PM10_IAQI = AQI_PM10.ToString();

                                if (CO_Value != null)
                                    NewAudDayAQI.CO = CO_Value.ToString();
                                if (AQI_CO != null)
                                    NewAudDayAQI.CO_IAQI = AQI_CO.ToString();

                                if (MaxOneHourO3_Value != null)
                                    NewAudDayAQI.MaxOneHourO3 = MaxOneHourO3_Value.ToString();
                                if (AQI_MaxOneHourO3 != null)
                                    NewAudDayAQI.MaxOneHourO3_IAQI = AQI_MaxOneHourO3.ToString();

                                if (Max8HourO3_Value != null)
                                    NewAudDayAQI.Max8HourO3 = Max8HourO3_Value.ToString();
                                if (AQI_Max8HourO3 != null)
                                    NewAudDayAQI.Max8HourO3_IAQI = AQI_Max8HourO3.ToString();

                                if (PM25_Value != null)
                                    NewAudDayAQI.PM25 = PM25_Value.ToString();
                                if (AQI_PM25 != null)
                                    NewAudDayAQI.PM25_IAQI = AQI_PM25.ToString();

                                string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "V");
                                NewAudDayAQI.AQIValue = AQIValue;
                                NewAudDayAQI.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "N");
                                if (!string.IsNullOrWhiteSpace(AQIValue))
                                {
                                    NewAudDayAQI.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                    NewAudDayAQI.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                    NewAudDayAQI.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                    NewAudDayAQI.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                    NewAudDayAQI.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
                                    NewAudDayAQI.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                                }

                                NewAudDayAQI.CreatUser = "SystemSync";
                                NewAudDayAQI.CreatDateTime = DateTime.Now;
                                MonitorBusinessModel.Add(NewAudDayAQI);
                                //MonitorBusinessModel.SaveChanges();
                                #endregion
                            }
                            else
                            {
                                #region 更新日AQI
                                if (SO2_Value != null)
                                    AudDayAQIExists.SO2 = SO2_Value.ToString();
                                else
                                    AudDayAQIExists.SO2 = null;
                                if (SO2_IAQI != null)
                                    AudDayAQIExists.SO2_IAQI = SO2_IAQI.ToString();
                                else
                                    AudDayAQIExists.SO2_IAQI = null;

                                if (NO2_Value != null)
                                    AudDayAQIExists.NO2 = NO2_Value.ToString();
                                else
                                    AudDayAQIExists.NO2 = null;
                                if (AQI_NO2 != null)
                                    AudDayAQIExists.NO2_IAQI = AQI_NO2.ToString();
                                else
                                    AudDayAQIExists.NO2_IAQI = null;

                                if (PM10_Value != null)
                                    AudDayAQIExists.PM10 = PM10_Value.ToString();
                                else
                                    AudDayAQIExists.PM10 = null;
                                if (AQI_PM10 != null)
                                    AudDayAQIExists.PM10_IAQI = AQI_PM10.ToString();
                                else
                                    AudDayAQIExists.PM10_IAQI = null;

                                if (CO_Value != null)
                                    AudDayAQIExists.CO = CO_Value.ToString();
                                else
                                    AudDayAQIExists.CO = null;
                                if (AQI_CO != null)
                                    AudDayAQIExists.CO_IAQI = AQI_CO.ToString();
                                else
                                    AudDayAQIExists.CO_IAQI = null;

                                if (MaxOneHourO3_Value != null)
                                    AudDayAQIExists.MaxOneHourO3 = MaxOneHourO3_Value.ToString();
                                else
                                    AudDayAQIExists.MaxOneHourO3 = null;
                                if (AQI_MaxOneHourO3 != null)
                                    AudDayAQIExists.MaxOneHourO3_IAQI = AQI_MaxOneHourO3.ToString();
                                else
                                    AudDayAQIExists.MaxOneHourO3_IAQI = null;

                                if (Max8HourO3_Value != null)
                                    AudDayAQIExists.Max8HourO3 = Max8HourO3_Value.ToString();
                                else
                                    AudDayAQIExists.Max8HourO3 = null;
                                if (AQI_Max8HourO3 != null)
                                    AudDayAQIExists.Max8HourO3_IAQI = AQI_Max8HourO3.ToString();
                                else
                                    AudDayAQIExists.Max8HourO3_IAQI = null;

                                if (PM25_Value != null)
                                    AudDayAQIExists.PM25 = PM25_Value.ToString();
                                else
                                    AudDayAQIExists.PM25 = null;
                                if (AQI_PM25 != null)
                                    AudDayAQIExists.PM25_IAQI = AQI_PM25.ToString();
                                else
                                    AudDayAQIExists.PM25_IAQI = null;

                                string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "V");
                                AudDayAQIExists.AQIValue = AQIValue;
                                AudDayAQIExists.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "N");
                                if (!string.IsNullOrWhiteSpace(AQIValue))
                                {
                                    AudDayAQIExists.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                    AudDayAQIExists.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                    AudDayAQIExists.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                    AudDayAQIExists.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                    AudDayAQIExists.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEAlTHEFFECT");
                                    AudDayAQIExists.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                                }
                                else
                                {
                                    AudDayAQIExists.Range = null;
                                    AudDayAQIExists.RGBValue = null;
                                    AudDayAQIExists.Class = null;
                                    AudDayAQIExists.Grade = null;
                                    AudDayAQIExists.HealthEffect = null;
                                    AudDayAQIExists.TakeStep = null;
                                }
                                AudDayAQIExists.UpdateUser = "SystemSync";
                                AudDayAQIExists.UpdateDateTime = DateTime.Now;
                                //MonitorBusinessModel.SaveChanges();
                                #endregion
                            }
                        }
                        #endregion
                        MonitorBusinessModel.SaveChanges();
                    }
                    #endregion
                    #region 审核区域AQI
                    foreach (string m_region in m_regions)
                    {
                        #region 审核小时区域AQI
                        DataTable dtPivotAudRegionHour = d_DAL.GetPivotAudRegionHourData(m_region, sTime, eTime);
                        foreach (DataRow dr in dtPivotAudRegionHour.Rows)
                        {
                            #region 数据
                            DateTime Tstamp = Convert.ToDateTime(dr["DateTime"]);
                            decimal? SO2_Value = null;
                            if (dr["SO2"] != DBNull.Value)
                                SO2_Value = Convert.ToDecimal(dr["SO2"]);
                            int? SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 1);

                            decimal? NO2_Value = null;
                            if (dr["NO2"] != DBNull.Value)
                                NO2_Value = Convert.ToDecimal(dr["NO2"]);
                            int? AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 1);

                            decimal? PM10_Value = null;
                            if (dr["PM10"] != DBNull.Value)
                                PM10_Value = Convert.ToDecimal(dr["PM10"]);
                            int? AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);

                            decimal? Recent24HoursPM10_Value = null;
                            if (dr["Recent24HoursPM10"] != DBNull.Value)
                                Recent24HoursPM10_Value = Convert.ToDecimal(dr["Recent24HoursPM10"]);
                            int? AQI_Recent24HoursPM10 = d_AQICalculateService.GetIAQI("a34002", Recent24HoursPM10_Value, 24);

                            decimal? CO_Value = null;
                            //先修约再计算AQI
                            if (dr["CO"] != DBNull.Value)
                            {
                                DataTable dtFactor = d_AQICalculateService.GetPollutantUnit("a21005");
                                int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);
                                if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m3"))
                                {
                                    CO_Value = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO"]), decimalNum) * 1000);

                                }
                                else
                                {
                                    CO_Value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO"]), decimalNum);
                                }
                            }
                            int? AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 1);

                            decimal? O3_Value = null;
                            if (dr["O3"] != DBNull.Value)
                                O3_Value = Convert.ToDecimal(dr["O3"]);
                            int? AQI_O3 = d_AQICalculateService.GetIAQI("a05024", O3_Value, 1);

                            decimal? Recent8HoursO3_Value = null;
                            if (dr["Recent8HoursO3"] != DBNull.Value)
                                Recent8HoursO3_Value = Convert.ToDecimal(dr["Recent8HoursO3"]);
                            int? AQI_Recent8HoursO3 = d_AQICalculateService.GetIAQI("a05024", Recent8HoursO3_Value, 8);

                            decimal? Recent8HoursO3NT_Value = null;
                            if (dr["Recent8HoursO3NT"] != DBNull.Value)
                                Recent8HoursO3NT_Value = Convert.ToDecimal(dr["Recent8HoursO3NT"]);
                            int? AQI_Recent8HoursO3NT = d_AQICalculateService.GetIAQI("a05024", Recent8HoursO3NT_Value, 8);

                            decimal? PM25_Value = null;
                            if (dr["PM25"] != DBNull.Value)
                                PM25_Value = Convert.ToDecimal(dr["PM25"]);
                            int? AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);

                            decimal? Recent24HoursPM25_Value = null;
                            if (dr["Recent24HoursPM25"] != DBNull.Value)
                                Recent24HoursPM25_Value = Convert.ToDecimal(dr["Recent24HoursPM25"]);
                            int? AQI_Recent24HoursPM25 = d_AQICalculateService.GetIAQI("a34004", Recent24HoursPM25_Value, 24);
                            #endregion
                            TB_RegionHourAQI AudRegionHourAQIExists = MonitorBusinessModel.TB_RegionHourAQIs.Where(p => p.MonitoringRegionUid.Equals(m_region) && p.DateTime.Equals(Tstamp)).FirstOrDefault();
                            if (AudRegionHourAQIExists == null)
                            {
                                #region 新增小时AQI
                                TB_RegionHourAQI NewAudHourAQI = new TB_RegionHourAQI();
                                NewAudHourAQI.MonitoringRegionUid = m_region;
                                NewAudHourAQI.DateTime = Tstamp;
                                NewAudHourAQI.StatisticalType = "CG";

                                if (SO2_Value != null)
                                    NewAudHourAQI.SO2 = SO2_Value.ToString();
                                if (SO2_IAQI != null)
                                    NewAudHourAQI.SO2_IAQI = SO2_IAQI.ToString();

                                if (NO2_Value != null)
                                    NewAudHourAQI.NO2 = NO2_Value.ToString();
                                if (AQI_NO2 != null)
                                    NewAudHourAQI.NO2_IAQI = AQI_NO2.ToString();

                                if (PM10_Value != null)
                                    NewAudHourAQI.PM10 = PM10_Value.ToString();
                                if (AQI_PM10 != null)
                                    NewAudHourAQI.PM10_IAQI = AQI_PM10.ToString();

                                if (Recent24HoursPM10_Value != null)
                                    NewAudHourAQI.Recent24HoursPM10 = Recent24HoursPM10_Value.ToString();
                                if (AQI_Recent24HoursPM10 != null)
                                    NewAudHourAQI.Recent24HoursPM10_IAQI = AQI_Recent24HoursPM10.ToString();

                                if (CO_Value != null)
                                    NewAudHourAQI.CO = CO_Value.ToString();
                                if (AQI_CO != null)
                                    NewAudHourAQI.CO_IAQI = AQI_CO.ToString();

                                if (O3_Value != null)
                                    NewAudHourAQI.O3 = O3_Value.ToString();
                                if (AQI_O3 != null)
                                    NewAudHourAQI.O3_IAQI = AQI_O3.ToString();

                                if (Recent8HoursO3_Value != null)
                                    NewAudHourAQI.Recent8HoursO3 = Recent8HoursO3_Value.ToString();
                                if (AQI_Recent8HoursO3 != null)
                                    NewAudHourAQI.Recent8HoursO3_IAQI = AQI_Recent8HoursO3.ToString();

                                if (Recent8HoursO3NT_Value != null)
                                    NewAudHourAQI.Recent8HoursO3NT = Recent8HoursO3NT_Value.ToString();
                                if (AQI_Recent8HoursO3NT != null)
                                    NewAudHourAQI.Recent8HoursO3NT_IAQI = AQI_Recent8HoursO3NT.ToString();

                                if (PM25_Value != null)
                                    NewAudHourAQI.PM25 = PM25_Value.ToString();
                                if (AQI_PM25 != null)
                                    NewAudHourAQI.PM25_IAQI = AQI_PM25.ToString();

                                if (Recent24HoursPM25_Value != null)
                                    NewAudHourAQI.Recent24HoursPM25 = Recent24HoursPM25_Value.ToString();
                                if (AQI_Recent24HoursPM25 != null)
                                    NewAudHourAQI.Recent24HoursPM25_IAQI = AQI_Recent24HoursPM25.ToString();

                                string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
                                NewAudHourAQI.AQIValue = AQIValue;
                                NewAudHourAQI.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");
                                if (!string.IsNullOrWhiteSpace(AQIValue))
                                {
                                    NewAudHourAQI.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                    NewAudHourAQI.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                    NewAudHourAQI.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                    NewAudHourAQI.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                    NewAudHourAQI.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
                                    NewAudHourAQI.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                                }
                                NewAudHourAQI.CreatUser = "SystemSync";
                                NewAudHourAQI.CreatDateTime = DateTime.Now;
                                MonitorBusinessModel.Add(NewAudHourAQI);
                                #endregion
                            }
                            else
                            {
                                #region 更新小时AQI
                                if (SO2_Value != null)
                                    AudRegionHourAQIExists.SO2 = SO2_Value.ToString();
                                else
                                    AudRegionHourAQIExists.SO2 = null;
                                if (SO2_IAQI != null)
                                    AudRegionHourAQIExists.SO2_IAQI = SO2_IAQI.ToString();
                                else
                                    AudRegionHourAQIExists.SO2_IAQI = null;

                                if (NO2_Value != null)
                                    AudRegionHourAQIExists.NO2 = NO2_Value.ToString();
                                else
                                    AudRegionHourAQIExists.NO2 = null;
                                if (AQI_NO2 != null)
                                    AudRegionHourAQIExists.NO2_IAQI = AQI_NO2.ToString();
                                else
                                    AudRegionHourAQIExists.NO2_IAQI = null;

                                if (PM10_Value != null)
                                    AudRegionHourAQIExists.PM10 = PM10_Value.ToString();
                                else
                                    AudRegionHourAQIExists.PM10 = null;
                                if (AQI_PM10 != null)
                                    AudRegionHourAQIExists.PM10_IAQI = AQI_PM10.ToString();
                                else
                                    AudRegionHourAQIExists.PM10_IAQI = null;

                                if (Recent24HoursPM10_Value != null)
                                    AudRegionHourAQIExists.Recent24HoursPM10 = Recent24HoursPM10_Value.ToString();
                                else
                                    AudRegionHourAQIExists.Recent24HoursPM10 = null;
                                if (AQI_Recent24HoursPM10 != null)
                                    AudRegionHourAQIExists.Recent24HoursPM10_IAQI = AQI_Recent24HoursPM10.ToString();
                                else
                                    AudRegionHourAQIExists.Recent24HoursPM10_IAQI = null;

                                if (CO_Value != null)
                                    AudRegionHourAQIExists.CO = CO_Value.ToString();
                                else
                                    AudRegionHourAQIExists.CO = null;
                                if (AQI_CO != null)
                                    AudRegionHourAQIExists.CO_IAQI = AQI_CO.ToString();
                                else
                                    AudRegionHourAQIExists.CO_IAQI = null;

                                if (O3_Value != null)
                                    AudRegionHourAQIExists.O3 = O3_Value.ToString();
                                else
                                    AudRegionHourAQIExists.O3 = null;
                                if (AQI_O3 != null)
                                    AudRegionHourAQIExists.O3_IAQI = AQI_O3.ToString();
                                else
                                    AudRegionHourAQIExists.O3_IAQI = null;

                                if (Recent8HoursO3_Value != null)
                                    AudRegionHourAQIExists.Recent8HoursO3 = Recent8HoursO3_Value.ToString();
                                else
                                    AudRegionHourAQIExists.Recent8HoursO3 = null;
                                if (AQI_Recent8HoursO3 != null)
                                    AudRegionHourAQIExists.Recent8HoursO3_IAQI = AQI_Recent8HoursO3.ToString();
                                else
                                    AudRegionHourAQIExists.Recent8HoursO3_IAQI = null;

                                if (Recent8HoursO3NT_Value != null)
                                    AudRegionHourAQIExists.Recent8HoursO3NT = Recent8HoursO3NT_Value.ToString();
                                else
                                    AudRegionHourAQIExists.Recent8HoursO3NT = null;
                                if (AQI_Recent8HoursO3NT != null)
                                    AudRegionHourAQIExists.Recent8HoursO3NT_IAQI = AQI_Recent8HoursO3NT.ToString();
                                else
                                    AudRegionHourAQIExists.Recent8HoursO3NT_IAQI = null;

                                if (PM25_Value != null)
                                    AudRegionHourAQIExists.PM25 = PM25_Value.ToString();
                                else
                                    AudRegionHourAQIExists.PM25 = null;
                                if (AQI_PM25 != null)
                                    AudRegionHourAQIExists.PM25_IAQI = AQI_PM25.ToString();
                                else
                                    AudRegionHourAQIExists.PM25_IAQI = null;

                                if (Recent24HoursPM25_Value != null)
                                    AudRegionHourAQIExists.Recent24HoursPM25 = Recent24HoursPM25_Value.ToString();
                                else
                                    AudRegionHourAQIExists.Recent24HoursPM25 = null;
                                if (AQI_Recent24HoursPM25 != null)
                                    AudRegionHourAQIExists.Recent24HoursPM25_IAQI = AQI_Recent24HoursPM25.ToString();
                                else
                                    AudRegionHourAQIExists.Recent24HoursPM25_IAQI = null;

                                string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
                                AudRegionHourAQIExists.AQIValue = AQIValue;
                                AudRegionHourAQIExists.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");
                                if (!string.IsNullOrWhiteSpace(AQIValue))
                                {
                                    AudRegionHourAQIExists.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                    AudRegionHourAQIExists.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                    AudRegionHourAQIExists.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                    AudRegionHourAQIExists.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                    AudRegionHourAQIExists.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
                                    AudRegionHourAQIExists.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                                }
                                else
                                {
                                    AudRegionHourAQIExists.Range = null;
                                    AudRegionHourAQIExists.RGBValue = null;
                                    AudRegionHourAQIExists.Class = null;
                                    AudRegionHourAQIExists.Grade = null;
                                    AudRegionHourAQIExists.HealthEffect = null;
                                    AudRegionHourAQIExists.TakeStep = null;
                                }
                                AudRegionHourAQIExists.UpdateUser = "SystemSync";
                                AudRegionHourAQIExists.UpdateDateTime = DateTime.Now;
                                #endregion
                            }
                        }
                        #endregion
                        MonitorBusinessModel.SaveChanges();

                        #region 审核日区域AQI
                        DataTable dtPivotAudRegionDay = d_DAL.GetPivotAudRegionDayData(m_region, sTime, eTime);
                        foreach (DataRow dr in dtPivotAudRegionDay.Rows)
                        {
                            #region 数据
                            DateTime Tstamp = Convert.ToDateTime(dr["DateTime"]);
                            //decimal? SO2_Value = null;
                            //if (dr["SO2"] != DBNull.Value)
                            //    SO2_Value = Convert.ToDecimal(dr["SO2"]);
                            //int? SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 24);

                            //decimal? NO2_Value = null;
                            //if (dr["NO2"] != DBNull.Value)
                            //    NO2_Value = Convert.ToDecimal(dr["NO2"]);
                            //int? AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 24);

                            //decimal? PM10_Value = null;
                            //if (dr["PM10"] != DBNull.Value)
                            //    PM10_Value = Convert.ToDecimal(dr["PM10"]);
                            //int? AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);


                            //decimal? CO_Value = null;
                            //if (dr["CO"] != DBNull.Value)
                            //    CO_Value = Convert.ToDecimal(dr["CO"]);
                            //int? AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 24);

                            //decimal? MaxOneHourO3_Value = null;
                            //if (dr["MaxOneHourO3"] != DBNull.Value)
                            //    MaxOneHourO3_Value = Convert.ToDecimal(dr["MaxOneHourO3"]);
                            //int? AQI_MaxOneHourO3 = d_AQICalculateService.GetIAQI("a05024", MaxOneHourO3_Value, 1);

                            //decimal? Max8HourO3_Value = null;
                            //if (dr["Max8HourO3"] != DBNull.Value)
                            //    Max8HourO3_Value = Convert.ToDecimal(dr["Max8HourO3"]);
                            //int? AQI_Max8HourO3 = d_AQICalculateService.GetIAQI("a05024", Max8HourO3_Value, 8);

                            //decimal? PM25_Value = null;
                            //if (dr["PM25"] != DBNull.Value)
                            //    PM25_Value = Convert.ToDecimal(dr["PM25"]);
                            //int? AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);
                            decimal? SO2_Value = null;
                            int? SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_MaxOneHourO3, AQI_Max8HourO3, AQI_PM25;
                            if (dr["SO2"] != DBNull.Value)
                            {
                                if (Convert.ToDecimal(dr["SO2"]) >= 10000)
                                {
                                    SO2_Value = Convert.ToDecimal(dr["SO2"]) - 10000;
                                    SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", null, 24);
                                }
                                else
                                {
                                    SO2_Value = Convert.ToDecimal(dr["SO2"]);
                                    SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 24);
                                }
                            }
                            else
                            {
                                SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 24);
                            }


                            decimal? NO2_Value = null;
                            if (dr["NO2"] != DBNull.Value)
                            {
                                if (Convert.ToDecimal(dr["NO2"]) >= 10000)
                                {
                                    NO2_Value = Convert.ToDecimal(dr["NO2"]) - 10000;
                                    AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", null, 24);
                                }
                                else
                                {
                                    NO2_Value = Convert.ToDecimal(dr["NO2"]);
                                    AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 24);
                                }
                            }
                            else
                            {
                                AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 24);
                            }


                            decimal? PM10_Value = null;
                            if (dr["PM10"] != DBNull.Value)
                            {
                                if (Convert.ToDecimal(dr["PM10"]) >= 10000)
                                {
                                    PM10_Value = Convert.ToDecimal(dr["PM10"]) - 10000;
                                    AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", null, 24);
                                }
                                else
                                {
                                    PM10_Value = Convert.ToDecimal(dr["PM10"]);
                                    AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);
                                }
                            }
                            else
                            {
                                AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);
                            }


                            decimal? CO_Value = null;
                            if (dr["CO"] != DBNull.Value)
                            {
                                if (Convert.ToDecimal(dr["CO"]) >= 10000)
                                {
                                    CO_Value = Convert.ToDecimal(dr["CO"]) - 10000;
                                    AQI_CO = d_AQICalculateService.GetIAQI("a21005", null, 24);
                                }
                                else
                                {
                                    CO_Value = Convert.ToDecimal(dr["CO"]);
                                    AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 24);
                                }
                            }
                            else
                            {
                                AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 24);
                            }


                            decimal? MaxOneHourO3_Value = null;
                            if (dr["MaxOneHourO3"] != DBNull.Value)
                            {

                                if (Convert.ToDecimal(dr["MaxOneHourO3"]) >= 10000)
                                {
                                    MaxOneHourO3_Value = Convert.ToDecimal(dr["MaxOneHourO3"]) - 10000;
                                    AQI_MaxOneHourO3 = d_AQICalculateService.GetIAQI("a05024", null, 1);
                                }
                                else
                                {
                                    MaxOneHourO3_Value = Convert.ToDecimal(dr["MaxOneHourO3"]);
                                    AQI_MaxOneHourO3 = d_AQICalculateService.GetIAQI("a05024", MaxOneHourO3_Value, 1);
                                }
                            }
                            else
                            {
                                AQI_MaxOneHourO3 = d_AQICalculateService.GetIAQI("a05024", MaxOneHourO3_Value, 1);
                            }


                            decimal? Max8HourO3_Value = null;
                            if (dr["Max8HourO3"] != DBNull.Value)
                            {
                                if (Convert.ToDecimal(dr["Max8HourO3"]) >= 10000)
                                {
                                    Max8HourO3_Value = Convert.ToDecimal(dr["Max8HourO3"]) - 10000;
                                    AQI_Max8HourO3 = d_AQICalculateService.GetIAQI("a05024", null, 8);
                                }
                                else
                                {
                                    Max8HourO3_Value = Convert.ToDecimal(dr["Max8HourO3"]);
                                    AQI_Max8HourO3 = d_AQICalculateService.GetIAQI("a05024", Max8HourO3_Value, 8);
                                }
                            }
                            else
                            {
                                AQI_Max8HourO3 = d_AQICalculateService.GetIAQI("a05024", Max8HourO3_Value, 8);
                            }


                            decimal? PM25_Value = null;
                            if (dr["PM25"] != DBNull.Value)
                            {
                                if (Convert.ToDecimal(dr["PM25"]) >= 10000)
                                {
                                    PM25_Value = Convert.ToDecimal(dr["PM25"]) - 10000;
                                    AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", null, 24);
                                }
                                else
                                {
                                    PM25_Value = Convert.ToDecimal(dr["PM25"]);
                                    AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);
                                }
                            }
                            else
                            {
                                AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);
                            }

                            #endregion
                            TB_RegionDayAQIReport AudRegionDayAQIExists = MonitorBusinessModel.TB_RegionDayAQIReports.Where(p => p.MonitoringRegionUid.Equals(m_region) && p.ReportDateTime.Equals(Tstamp)).FirstOrDefault();
                            if (AudRegionDayAQIExists == null)
                            {
                                #region 新增日AQI
                                TB_RegionDayAQIReport NewAudDayAQI = new TB_RegionDayAQIReport();
                                NewAudDayAQI.MonitoringRegionUid = m_region;
                                NewAudDayAQI.ReportDateTime = Tstamp;
                                NewAudDayAQI.StatisticalType = "CG";

                                if (SO2_Value != null)
                                    NewAudDayAQI.SO2 = SO2_Value.ToString();
                                if (SO2_IAQI != null)
                                    NewAudDayAQI.SO2_IAQI = SO2_IAQI.ToString();

                                if (NO2_Value != null)
                                    NewAudDayAQI.NO2 = NO2_Value.ToString();
                                if (AQI_NO2 != null)
                                    NewAudDayAQI.NO2_IAQI = AQI_NO2.ToString();

                                if (PM10_Value != null)
                                    NewAudDayAQI.PM10 = PM10_Value.ToString();
                                if (AQI_PM10 != null)
                                    NewAudDayAQI.PM10_IAQI = AQI_PM10.ToString();

                                if (CO_Value != null)
                                    NewAudDayAQI.CO = CO_Value.ToString();
                                if (AQI_CO != null)
                                    NewAudDayAQI.CO_IAQI = AQI_CO.ToString();

                                if (MaxOneHourO3_Value != null)
                                    NewAudDayAQI.MaxOneHourO3 = MaxOneHourO3_Value.ToString();
                                if (AQI_MaxOneHourO3 != null)
                                    NewAudDayAQI.MaxOneHourO3_IAQI = AQI_MaxOneHourO3.ToString();

                                if (Max8HourO3_Value != null)
                                    NewAudDayAQI.Max8HourO3 = Max8HourO3_Value.ToString();
                                if (AQI_Max8HourO3 != null)
                                    NewAudDayAQI.Max8HourO3_IAQI = AQI_Max8HourO3.ToString();

                                if (PM25_Value != null)
                                    NewAudDayAQI.PM25 = PM25_Value.ToString();
                                if (AQI_PM25 != null)
                                    NewAudDayAQI.PM25_IAQI = AQI_PM25.ToString();

                                string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "V");
                                NewAudDayAQI.AQIValue = AQIValue;
                                NewAudDayAQI.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "N");
                                if (!string.IsNullOrWhiteSpace(AQIValue))
                                {
                                    NewAudDayAQI.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                    NewAudDayAQI.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                    NewAudDayAQI.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                    NewAudDayAQI.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                    NewAudDayAQI.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
                                    NewAudDayAQI.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                                }

                                NewAudDayAQI.CreatUser = "SystemSync";
                                NewAudDayAQI.CreatDateTime = DateTime.Now;
                                MonitorBusinessModel.Add(NewAudDayAQI);
                                #endregion
                            }
                            else
                            {
                                #region 更新日AQI
                                if (SO2_Value != null)
                                    AudRegionDayAQIExists.SO2 = SO2_Value.ToString();
                                else
                                    AudRegionDayAQIExists.SO2 = null;
                                if (SO2_IAQI != null)
                                    AudRegionDayAQIExists.SO2_IAQI = SO2_IAQI.ToString();
                                else
                                    AudRegionDayAQIExists.SO2_IAQI = null;

                                if (NO2_Value != null)
                                    AudRegionDayAQIExists.NO2 = NO2_Value.ToString();
                                else
                                    AudRegionDayAQIExists.NO2 = null;
                                if (AQI_NO2 != null)
                                    AudRegionDayAQIExists.NO2_IAQI = AQI_NO2.ToString();
                                else
                                    AudRegionDayAQIExists.NO2_IAQI = null;

                                if (PM10_Value != null)
                                    AudRegionDayAQIExists.PM10 = PM10_Value.ToString();
                                else
                                    AudRegionDayAQIExists.PM10 = null;
                                if (AQI_PM10 != null)
                                    AudRegionDayAQIExists.PM10_IAQI = AQI_PM10.ToString();
                                else
                                    AudRegionDayAQIExists.PM10_IAQI = null;

                                if (CO_Value != null)
                                    AudRegionDayAQIExists.CO = CO_Value.ToString();
                                else
                                    AudRegionDayAQIExists.CO = null;
                                if (AQI_CO != null)
                                    AudRegionDayAQIExists.CO_IAQI = AQI_CO.ToString();
                                else
                                    AudRegionDayAQIExists.SO2 = null;

                                if (MaxOneHourO3_Value != null)
                                    AudRegionDayAQIExists.MaxOneHourO3 = MaxOneHourO3_Value.ToString();
                                else
                                    AudRegionDayAQIExists.MaxOneHourO3 = null;
                                if (AQI_MaxOneHourO3 != null)
                                    AudRegionDayAQIExists.MaxOneHourO3_IAQI = AQI_MaxOneHourO3.ToString();
                                else
                                    AudRegionDayAQIExists.MaxOneHourO3_IAQI = null;

                                if (Max8HourO3_Value != null)
                                    AudRegionDayAQIExists.Max8HourO3 = Max8HourO3_Value.ToString();
                                else
                                    AudRegionDayAQIExists.Max8HourO3 = null;
                                if (AQI_Max8HourO3 != null)
                                    AudRegionDayAQIExists.Max8HourO3_IAQI = AQI_Max8HourO3.ToString();
                                else
                                    AudRegionDayAQIExists.Max8HourO3_IAQI = null;

                                if (PM25_Value != null)
                                    AudRegionDayAQIExists.PM25 = PM25_Value.ToString();
                                else
                                    AudRegionDayAQIExists.PM25 = null;
                                if (AQI_PM25 != null)
                                    AudRegionDayAQIExists.PM25_IAQI = AQI_PM25.ToString();
                                else
                                    AudRegionDayAQIExists.PM25_IAQI = null;

                                string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "V");
                                AudRegionDayAQIExists.AQIValue = AQIValue;
                                AudRegionDayAQIExists.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "N");
                                if (!string.IsNullOrWhiteSpace(AQIValue))
                                {
                                    AudRegionDayAQIExists.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                    AudRegionDayAQIExists.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                    AudRegionDayAQIExists.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                    AudRegionDayAQIExists.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                    AudRegionDayAQIExists.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEAlTHEFFECT");
                                    AudRegionDayAQIExists.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                                }
                                else
                                {
                                    AudRegionDayAQIExists.Range = null;
                                    AudRegionDayAQIExists.RGBValue = null;
                                    AudRegionDayAQIExists.Class = null;
                                    AudRegionDayAQIExists.Grade = null;
                                    AudRegionDayAQIExists.HealthEffect = null;
                                    AudRegionDayAQIExists.TakeStep = null;
                                }
                                AudRegionDayAQIExists.UpdateUser = "SystemSync";
                                AudRegionDayAQIExists.UpdateDateTime = DateTime.Now;

                                #endregion
                            }
                        }
                        #endregion
                        MonitorBusinessModel.SaveChanges();
                    }
                    #endregion

                }
                log.Info("-------------------------------------------------------CalculateBy60结束----------------------------");
                #endregion
            }
            catch (Exception ex)
            {
                log.Error("--------------------------------------------------------CalculateBy60异常:" + ex.ToString());
            }

        }
        #region 新版本
        ///// <summary>
        ///// 国控四站点小时数据计算小时AQI,日数据日AQI
        ///// </summary>
        //public void CalculateConBy60(DateTime startTime, DateTime endTime)
        //{
        //    try
        //    {
        //        log.Info("-------------------------------------------------------------CalculateConBy60原始数据开始-------------------");
        //        DateTime sTime = Convert.ToDateTime(startTime.ToString("yyyy-MM-dd 00:00:00"));
        //        DateTime eTime = Convert.ToDateTime(endTime.ToString("yyyy-MM-dd 23:59:59"));

        //        using (AirAutoMonitorModel AirAutoMonitorModel = new AirAutoMonitorModel())
        //        {
        //            #region 原始测点AQI
        //            //foreach (int PointId in ConPointIds)
        //            //{
        //                #region 计算原始小时AQI
        //            DataTable dtPivotOriHour = d_DAL.GetConPivotOriHourData(ConPointIds, sTime, eTime);
        //                foreach (DataRow dr in dtPivotOriHour.Rows)
        //                {
        //                    #region 数据
        //                    int? PointId = Convert.ToInt32(dr["PointId"]);
        //                    DateTime Tstamp = Convert.ToDateTime(dr["Tstamp"]);
        //                    decimal? SO2_Value = null;
        //                    if (dr["SO2"] != DBNull.Value)
        //                        SO2_Value = Convert.ToDecimal(dr["SO2"]);
        //                    int? SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 1);

        //                    decimal? NO2_Value = null;
        //                    if (dr["NO2"] != DBNull.Value)
        //                        NO2_Value = Convert.ToDecimal(dr["NO2"]);
        //                    int? AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 1);

        //                    decimal? PM10_Value = null;
        //                    if (dr["PM10"] != DBNull.Value)
        //                        PM10_Value = Convert.ToDecimal(dr["PM10"]);
        //                    int? AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);

        //                    decimal? Recent24HoursPM10_Value = null;
        //                    if (dr["Recent24HoursPM10"] != DBNull.Value)
        //                        Recent24HoursPM10_Value = Convert.ToDecimal(dr["Recent24HoursPM10"]);
        //                    int? AQI_Recent24HoursPM10 = d_AQICalculateService.GetIAQI("a34002", Recent24HoursPM10_Value, 24);

        //                    decimal? CO_Value = null;
        //                    //先修约再计算AQI
        //                    if (dr["CO"] != DBNull.Value)
        //                    {
        //                        DataTable dtFactor = d_AQICalculateService.GetPollutantUnit("a21005");
        //                        int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);
        //                        if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m3"))
        //                        {
        //                            CO_Value = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO"]), decimalNum) * 1000);

        //                        }
        //                        else
        //                        {
        //                            CO_Value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO"]), decimalNum);
        //                        }
        //                        //CO_Value = Convert.ToDecimal(dr["CO"]);
        //                    }
        //                    int? AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 1);

        //                    decimal? O3_Value = null;
        //                    if (dr["O3"] != DBNull.Value)
        //                        O3_Value = Convert.ToDecimal(dr["O3"]);
        //                    int? AQI_O3 = d_AQICalculateService.GetIAQI("a05024", O3_Value, 1);

        //                    decimal? Recent8HoursO3_Value = null;
        //                    if (dr["Recent8HoursO3"] != DBNull.Value)
        //                        Recent8HoursO3_Value = Convert.ToDecimal(dr["Recent8HoursO3"]);
        //                    int? AQI_Recent8HoursO3 = d_AQICalculateService.GetIAQI("a05024", Recent8HoursO3_Value, 8);

        //                    decimal? Recent8HoursO3NT_Value = null;
        //                    if (dr["Recent8HoursO3NT"] != DBNull.Value)
        //                        Recent8HoursO3NT_Value = Convert.ToDecimal(dr["Recent8HoursO3NT"]);
        //                    int? AQI_Recent8HoursO3NT = d_AQICalculateService.GetIAQI("a05024", Recent8HoursO3NT_Value, 8);

        //                    decimal? PM25_Value = null;
        //                    if (dr["PM25"] != DBNull.Value)
        //                        PM25_Value = Convert.ToDecimal(dr["PM25"]);
        //                    int? AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);

        //                    decimal? Recent24HoursPM25_Value = null;
        //                    if (dr["Recent24HoursPM25"] != DBNull.Value)
        //                        Recent24HoursPM25_Value = Convert.ToDecimal(dr["Recent24HoursPM25"]);
        //                    int? AQI_Recent24HoursPM25 = d_AQICalculateService.GetIAQI("a34004", Recent24HoursPM25_Value, 24);
        //                    #endregion
        //                    TB_OriHourAQI OriHourAQIExists = AirAutoMonitorModel.TB_OriHourAQIs.Where(p => p.PointId.Equals(PointId) && p.DateTime.Equals(Tstamp)).FirstOrDefault();
        //                    //TB_OriHourAQI OriHourAQIExists = AirAutoMonitorModel.TB_OriHourAQIs.Where(p => ConPointIds.Contains(p.PointId.ToString()) && p.DateTime.Equals(Tstamp)).FirstOrDefault();
        //                    if (OriHourAQIExists == null)
        //                    {
        //                        #region 新增小时AQI
        //                        TB_OriHourAQI NewOriHourAQI = new TB_OriHourAQI();
        //                        NewOriHourAQI.PointId = PointId;
        //                        NewOriHourAQI.DateTime = Tstamp;

        //                        if (SO2_Value != null)
        //                            NewOriHourAQI.SO2 = SO2_Value.ToString();
        //                        if (SO2_IAQI != null)
        //                            NewOriHourAQI.SO2_IAQI = SO2_IAQI.ToString();

        //                        if (NO2_Value != null)
        //                            NewOriHourAQI.NO2 = NO2_Value.ToString();
        //                        if (NO2_Value != null)
        //                            NewOriHourAQI.NO2_IAQI = AQI_NO2.ToString();

        //                        if (PM10_Value != null)
        //                            NewOriHourAQI.PM10 = PM10_Value.ToString();
        //                        if (AQI_PM10 != null)
        //                            NewOriHourAQI.PM10_IAQI = AQI_PM10.ToString();

        //                        if (Recent24HoursPM10_Value != null)
        //                            NewOriHourAQI.Recent24HoursPM10 = Recent24HoursPM10_Value.ToString();
        //                        if (AQI_Recent24HoursPM10 != null)
        //                            NewOriHourAQI.Recent24HoursPM10_IAQI = AQI_Recent24HoursPM10.ToString();

        //                        if (CO_Value != null)
        //                            NewOriHourAQI.CO = CO_Value.ToString();
        //                        if (AQI_CO != null)
        //                            NewOriHourAQI.CO_IAQI = AQI_CO.ToString();

        //                        if (O3_Value != null)
        //                            NewOriHourAQI.O3 = O3_Value.ToString();
        //                        if (AQI_O3 != null)
        //                            NewOriHourAQI.O3_IAQI = AQI_O3.ToString();

        //                        if (Recent8HoursO3_Value != null)
        //                            NewOriHourAQI.Recent8HoursO3 = Recent8HoursO3_Value.ToString();
        //                        if (AQI_Recent8HoursO3 != null)
        //                            NewOriHourAQI.Recent8HoursO3_IAQI = AQI_Recent8HoursO3.ToString();

        //                        if (Recent8HoursO3NT_Value != null)
        //                            NewOriHourAQI.Recent8HoursO3NT = Recent8HoursO3NT_Value.ToString();
        //                        if (AQI_Recent8HoursO3NT != null)
        //                            NewOriHourAQI.Recent8HoursO3NT_IAQI = AQI_Recent8HoursO3NT.ToString();

        //                        if (PM25_Value != null)
        //                            NewOriHourAQI.PM25 = PM25_Value.ToString();
        //                        if (AQI_PM25 != null)
        //                            NewOriHourAQI.PM25_IAQI = AQI_PM25.ToString();

        //                        if (Recent24HoursPM25_Value != null)
        //                            NewOriHourAQI.Recent24HoursPM25 = Recent24HoursPM25_Value.ToString();
        //                        if (AQI_Recent24HoursPM25 != null)
        //                            NewOriHourAQI.Recent24HoursPM25_IAQI = AQI_Recent24HoursPM25.ToString();

        //                        string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
        //                        NewOriHourAQI.AQIValue = AQIValue;
        //                        NewOriHourAQI.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");
        //                        if (!string.IsNullOrWhiteSpace(AQIValue))
        //                        {
        //                            NewOriHourAQI.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
        //                            NewOriHourAQI.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
        //                            NewOriHourAQI.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
        //                            NewOriHourAQI.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
        //                            NewOriHourAQI.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
        //                            NewOriHourAQI.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
        //                        }
        //                        NewOriHourAQI.CreatUser = "SystemSync";
        //                        NewOriHourAQI.CreatDateTime = DateTime.Now;
        //                        AirAutoMonitorModel.Add(NewOriHourAQI);
        //                        //AirAutoMonitorModel.SaveChanges();
        //                        #endregion
        //                    }
        //                    else
        //                    {
        //                        #region 更新小时AQI
        //                        if (SO2_Value != null)
        //                            OriHourAQIExists.SO2 = SO2_Value.ToString();
        //                        else
        //                            OriHourAQIExists.SO2 = null;
        //                        if (SO2_IAQI != null)
        //                            OriHourAQIExists.SO2_IAQI = SO2_IAQI.ToString();
        //                        else
        //                            OriHourAQIExists.SO2_IAQI = null;
        //                        if (NO2_Value != null)
        //                            OriHourAQIExists.NO2 = NO2_Value.ToString();
        //                        else
        //                            OriHourAQIExists.NO2 = null;
        //                        if (AQI_NO2 != null)
        //                            OriHourAQIExists.NO2_IAQI = AQI_NO2.ToString();
        //                        else
        //                            OriHourAQIExists.NO2_IAQI = null;

        //                        if (PM10_Value != null)
        //                            OriHourAQIExists.PM10 = PM10_Value.ToString();
        //                        else
        //                            OriHourAQIExists.PM10 = null;
        //                        if (AQI_PM10 != null)
        //                            OriHourAQIExists.PM10_IAQI = AQI_PM10.ToString();
        //                        else
        //                            OriHourAQIExists.PM10_IAQI = null;

        //                        if (Recent24HoursPM10_Value != null)
        //                            OriHourAQIExists.Recent24HoursPM10 = Recent24HoursPM10_Value.ToString();
        //                        else
        //                            OriHourAQIExists.Recent24HoursPM10 = null;
        //                        if (AQI_Recent24HoursPM10 != null)
        //                            OriHourAQIExists.Recent24HoursPM10_IAQI = AQI_Recent24HoursPM10.ToString();
        //                        else
        //                            OriHourAQIExists.Recent24HoursPM10_IAQI = null;

        //                        if (CO_Value != null)
        //                            OriHourAQIExists.CO = CO_Value.ToString();
        //                        else
        //                            OriHourAQIExists.CO = null;
        //                        if (AQI_CO != null)
        //                            OriHourAQIExists.CO_IAQI = AQI_CO.ToString();
        //                        else
        //                            OriHourAQIExists.CO_IAQI = null;

        //                        if (O3_Value != null)
        //                            OriHourAQIExists.O3 = O3_Value.ToString();
        //                        else
        //                            OriHourAQIExists.O3 = null;
        //                        if (AQI_O3 != null)
        //                            OriHourAQIExists.O3_IAQI = AQI_O3.ToString();
        //                        else
        //                            OriHourAQIExists.O3_IAQI = null;

        //                        if (Recent8HoursO3_Value != null)
        //                            OriHourAQIExists.Recent8HoursO3 = Recent8HoursO3_Value.ToString();
        //                        else
        //                            OriHourAQIExists.Recent8HoursO3 = null;
        //                        if (AQI_Recent8HoursO3 != null)
        //                            OriHourAQIExists.Recent8HoursO3_IAQI = AQI_Recent8HoursO3.ToString();
        //                        else
        //                            OriHourAQIExists.Recent8HoursO3_IAQI = null;

        //                        if (Recent8HoursO3NT_Value != null)
        //                            OriHourAQIExists.Recent8HoursO3NT = Recent8HoursO3NT_Value.ToString();
        //                        else
        //                            OriHourAQIExists.Recent8HoursO3NT = null;
        //                        if (AQI_Recent8HoursO3NT != null)
        //                            OriHourAQIExists.Recent8HoursO3NT_IAQI = AQI_Recent8HoursO3NT.ToString();
        //                        else
        //                            OriHourAQIExists.Recent8HoursO3NT_IAQI = null;

        //                        if (PM25_Value != null)
        //                            OriHourAQIExists.PM25 = PM25_Value.ToString();
        //                        else
        //                            OriHourAQIExists.PM25 = null;
        //                        if (AQI_PM25 != null)
        //                            OriHourAQIExists.PM25_IAQI = AQI_PM25.ToString();
        //                        else
        //                            OriHourAQIExists.PM25_IAQI = null;

        //                        if (Recent24HoursPM25_Value != null)
        //                            OriHourAQIExists.Recent24HoursPM25 = Recent24HoursPM25_Value.ToString();
        //                        else
        //                            OriHourAQIExists.Recent24HoursPM25 = null;
        //                        if (AQI_Recent24HoursPM25 != null)
        //                            OriHourAQIExists.Recent24HoursPM25_IAQI = AQI_Recent24HoursPM25.ToString();
        //                        else
        //                            OriHourAQIExists.Recent24HoursPM25_IAQI = null;

        //                        string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
        //                        OriHourAQIExists.AQIValue = AQIValue;
        //                        OriHourAQIExists.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");
        //                        if (!string.IsNullOrWhiteSpace(AQIValue))
        //                        {
        //                            OriHourAQIExists.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
        //                            OriHourAQIExists.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
        //                            OriHourAQIExists.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
        //                            OriHourAQIExists.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
        //                            OriHourAQIExists.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
        //                            OriHourAQIExists.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
        //                        }
        //                        else
        //                        {
        //                            OriHourAQIExists.Range = null;
        //                            OriHourAQIExists.RGBValue = null;
        //                            OriHourAQIExists.Class = null;
        //                            OriHourAQIExists.Grade = null;
        //                            OriHourAQIExists.HealthEffect = null;
        //                            OriHourAQIExists.TakeStep = null;
        //                        }
        //                        OriHourAQIExists.UpdateUser = "SystemSync";
        //                        OriHourAQIExists.UpdateDateTime = DateTime.Now;
        //                        //AirAutoMonitorModel.SaveChanges();
        //                        #endregion
        //                    }
        //                }
        //                #endregion
        //                AirAutoMonitorModel.SaveChanges();

        //                //计算生成原始日数据
        //                d_DAL.AddConOriginalDayData(sTime, eTime, ConPointIds);

                        
        //            #endregion

        //            #region 原始区域AQI
        //            string m_regionCon = string.Empty;
        //            foreach (string str in m_regions)
        //            {
        //                m_regionCon += "'" + str + "',";
        //            }
        //            //foreach (string m_region in m_regions)
        //            //{
        //                #region 原始小时区域AQI
        //            DataTable dtPivotOriRegionHour = d_DAL.GetConPivotOriRegionHourData(m_regionCon.TrimEnd(','), sTime, eTime);
        //                foreach (DataRow dr in dtPivotOriRegionHour.Rows)
        //                {
        //                    #region 数据
        //                    string m_region = dr["CityTypeUid"].ToString();
        //                    DateTime Tstamp = Convert.ToDateTime(dr["DateTime"]);
        //                    decimal? SO2_Value = null;
        //                    if (dr["SO2"] != DBNull.Value)
        //                        SO2_Value = Convert.ToDecimal(dr["SO2"]);
        //                    int? SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 1);

        //                    decimal? NO2_Value = null;
        //                    if (dr["NO2"] != DBNull.Value)
        //                        NO2_Value = Convert.ToDecimal(dr["NO2"]);
        //                    int? AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 1);

        //                    decimal? PM10_Value = null;
        //                    if (dr["PM10"] != DBNull.Value)
        //                        PM10_Value = Convert.ToDecimal(dr["PM10"]);
        //                    int? AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);

        //                    decimal? Recent24HoursPM10_Value = null;
        //                    if (dr["Recent24HoursPM10"] != DBNull.Value)
        //                        Recent24HoursPM10_Value = Convert.ToDecimal(dr["Recent24HoursPM10"]);
        //                    int? AQI_Recent24HoursPM10 = d_AQICalculateService.GetIAQI("a34002", Recent24HoursPM10_Value, 24);

        //                    decimal? CO_Value = null;
        //                    //先修约再计算AQI
        //                    if (dr["CO"] != DBNull.Value)
        //                    {
        //                        DataTable dtFactor = d_AQICalculateService.GetPollutantUnit("a21005");
        //                        int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);
        //                        if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m3"))
        //                        {
        //                            CO_Value = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO"]), decimalNum) * 1000);

        //                        }
        //                        else
        //                        {
        //                            CO_Value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO"]), decimalNum);
        //                        }
        //                    }
        //                    int? AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 1);

        //                    decimal? O3_Value = null;
        //                    if (dr["O3"] != DBNull.Value)
        //                        O3_Value = Convert.ToDecimal(dr["O3"]);
        //                    int? AQI_O3 = d_AQICalculateService.GetIAQI("a05024", O3_Value, 1);

        //                    decimal? Recent8HoursO3_Value = null;
        //                    if (dr["Recent8HoursO3"] != DBNull.Value)
        //                        Recent8HoursO3_Value = Convert.ToDecimal(dr["Recent8HoursO3"]);
        //                    int? AQI_Recent8HoursO3 = d_AQICalculateService.GetIAQI("a05024", Recent8HoursO3_Value, 8);

        //                    decimal? Recent8HoursO3NT_Value = null;
        //                    if (dr["Recent8HoursO3NT"] != DBNull.Value)
        //                        Recent8HoursO3NT_Value = Convert.ToDecimal(dr["Recent8HoursO3NT"]);
        //                    int? AQI_Recent8HoursO3NT = d_AQICalculateService.GetIAQI("a05024", Recent8HoursO3NT_Value, 8);

        //                    decimal? PM25_Value = null;
        //                    if (dr["PM25"] != DBNull.Value)
        //                        PM25_Value = Convert.ToDecimal(dr["PM25"]);
        //                    int? AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);

        //                    decimal? Recent24HoursPM25_Value = null;
        //                    if (dr["Recent24HoursPM25"] != DBNull.Value)
        //                        Recent24HoursPM25_Value = Convert.ToDecimal(dr["Recent24HoursPM25"]);
        //                    int? AQI_Recent24HoursPM25 = d_AQICalculateService.GetIAQI("a34004", Recent24HoursPM25_Value, 24);
        //                    #endregion
        //                    TB_OriRegionHourAQI OriRegionHourAQIExists = AirAutoMonitorModel.TB_OriRegionHourAQIs.Where(p => p.MonitoringRegionUid.Equals(m_region) && p.DateTime.Equals(Tstamp)).FirstOrDefault();
        //                    if (OriRegionHourAQIExists == null)
        //                    {
        //                        #region 新增小时AQI
        //                        TB_OriRegionHourAQI NewOriHourAQI = new TB_OriRegionHourAQI();
        //                        NewOriHourAQI.MonitoringRegionUid = m_region;
        //                        NewOriHourAQI.DateTime = Tstamp;
        //                        NewOriHourAQI.StatisticalType = "CG";

        //                        if (SO2_Value != null)
        //                            NewOriHourAQI.SO2 = SO2_Value.ToString();
        //                        if (SO2_IAQI != null)
        //                            NewOriHourAQI.SO2_IAQI = SO2_IAQI.ToString();

        //                        if (NO2_Value != null)
        //                            NewOriHourAQI.NO2 = NO2_Value.ToString();
        //                        if (AQI_NO2 != null)
        //                            NewOriHourAQI.NO2_IAQI = AQI_NO2.ToString();

        //                        if (PM10_Value != null)
        //                            NewOriHourAQI.PM10 = PM10_Value.ToString();
        //                        if (AQI_PM10 != null)
        //                            NewOriHourAQI.PM10_IAQI = AQI_PM10.ToString();

        //                        if (Recent24HoursPM10_Value != null)
        //                            NewOriHourAQI.Recent24HoursPM10 = Recent24HoursPM10_Value.ToString();
        //                        if (AQI_Recent24HoursPM10 != null)
        //                            NewOriHourAQI.Recent24HoursPM10_IAQI = AQI_Recent24HoursPM10.ToString();

        //                        if (CO_Value != null)
        //                            NewOriHourAQI.CO = CO_Value.ToString();
        //                        if (AQI_CO != null)
        //                            NewOriHourAQI.CO_IAQI = AQI_CO.ToString();

        //                        if (O3_Value != null)
        //                            NewOriHourAQI.O3 = O3_Value.ToString();
        //                        if (AQI_O3 != null)
        //                            NewOriHourAQI.O3_IAQI = AQI_O3.ToString();

        //                        if (Recent8HoursO3_Value != null)
        //                            NewOriHourAQI.Recent8HoursO3 = Recent8HoursO3_Value.ToString();
        //                        if (AQI_Recent8HoursO3 != null)
        //                            NewOriHourAQI.Recent8HoursO3_IAQI = AQI_Recent8HoursO3.ToString();

        //                        if (Recent8HoursO3NT_Value != null)
        //                            NewOriHourAQI.Recent8HoursO3NT = Recent8HoursO3NT_Value.ToString();
        //                        if (AQI_Recent8HoursO3NT != null)
        //                            NewOriHourAQI.Recent8HoursO3NT_IAQI = AQI_Recent8HoursO3NT.ToString();

        //                        if (PM25_Value != null)
        //                            NewOriHourAQI.PM25 = PM25_Value.ToString();
        //                        if (AQI_PM25 != null)
        //                            NewOriHourAQI.PM25_IAQI = AQI_PM25.ToString();

        //                        if (Recent24HoursPM25_Value != null)
        //                            NewOriHourAQI.Recent24HoursPM25 = Recent24HoursPM25_Value.ToString();
        //                        if (AQI_Recent24HoursPM25 != null)
        //                            NewOriHourAQI.Recent24HoursPM25_IAQI = AQI_Recent24HoursPM25.ToString();

        //                        string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
        //                        NewOriHourAQI.AQIValue = AQIValue;
        //                        NewOriHourAQI.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");
        //                        if (!string.IsNullOrWhiteSpace(AQIValue))
        //                        {
        //                            NewOriHourAQI.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
        //                            NewOriHourAQI.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
        //                            NewOriHourAQI.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
        //                            NewOriHourAQI.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
        //                            NewOriHourAQI.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
        //                            NewOriHourAQI.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
        //                        }
        //                        NewOriHourAQI.CreatUser = "SystemSync";
        //                        NewOriHourAQI.CreatDateTime = DateTime.Now;
        //                        AirAutoMonitorModel.Add(NewOriHourAQI);
        //                        //AirAutoMonitorModel.SaveChanges();
        //                        #endregion
        //                    }
        //                    else
        //                    {
        //                        #region 更新小时AQI
        //                        if (SO2_Value != null)
        //                            OriRegionHourAQIExists.SO2 = SO2_Value.ToString();
        //                        else
        //                            OriRegionHourAQIExists.SO2 = null;
        //                        if (SO2_IAQI != null)
        //                            OriRegionHourAQIExists.SO2_IAQI = SO2_IAQI.ToString();
        //                        else
        //                            OriRegionHourAQIExists.SO2_IAQI = null;

        //                        if (NO2_Value != null)
        //                            OriRegionHourAQIExists.NO2 = NO2_Value.ToString();
        //                        else
        //                            OriRegionHourAQIExists.NO2 = null;
        //                        if (AQI_NO2 != null)
        //                            OriRegionHourAQIExists.NO2_IAQI = AQI_NO2.ToString();
        //                        else
        //                            OriRegionHourAQIExists.NO2_IAQI = null;

        //                        if (PM10_Value != null)
        //                            OriRegionHourAQIExists.PM10 = PM10_Value.ToString();
        //                        else
        //                            OriRegionHourAQIExists.PM10 = null;
        //                        if (AQI_PM10 != null)
        //                            OriRegionHourAQIExists.PM10_IAQI = AQI_PM10.ToString();
        //                        else
        //                            OriRegionHourAQIExists.PM10_IAQI = null;

        //                        if (Recent24HoursPM10_Value != null)
        //                            OriRegionHourAQIExists.Recent24HoursPM10 = Recent24HoursPM10_Value.ToString();
        //                        else
        //                            OriRegionHourAQIExists.Recent24HoursPM10 = null;
        //                        if (AQI_Recent24HoursPM10 != null)
        //                            OriRegionHourAQIExists.Recent24HoursPM10_IAQI = AQI_Recent24HoursPM10.ToString();
        //                        else
        //                            OriRegionHourAQIExists.Recent24HoursPM10_IAQI = null;

        //                        if (CO_Value != null)
        //                            OriRegionHourAQIExists.CO = CO_Value.ToString();
        //                        else
        //                            OriRegionHourAQIExists.CO = null;
        //                        if (AQI_CO != null)
        //                            OriRegionHourAQIExists.CO_IAQI = AQI_CO.ToString();
        //                        else
        //                            OriRegionHourAQIExists.CO_IAQI = null;

        //                        if (O3_Value != null)
        //                            OriRegionHourAQIExists.O3 = O3_Value.ToString();
        //                        else
        //                            OriRegionHourAQIExists.O3 = null;
        //                        if (AQI_O3 != null)
        //                            OriRegionHourAQIExists.O3_IAQI = AQI_O3.ToString();
        //                        else
        //                            OriRegionHourAQIExists.O3_IAQI = null;

        //                        if (Recent8HoursO3_Value != null)
        //                            OriRegionHourAQIExists.Recent8HoursO3 = Recent8HoursO3_Value.ToString();
        //                        else
        //                            OriRegionHourAQIExists.Recent8HoursO3 = null;
        //                        if (AQI_Recent8HoursO3 != null)
        //                            OriRegionHourAQIExists.Recent8HoursO3_IAQI = AQI_Recent8HoursO3.ToString();
        //                        else
        //                            OriRegionHourAQIExists.Recent8HoursO3_IAQI = null;

        //                        if (Recent8HoursO3NT_Value != null)
        //                            OriRegionHourAQIExists.Recent8HoursO3NT = Recent8HoursO3NT_Value.ToString();
        //                        else
        //                            OriRegionHourAQIExists.Recent8HoursO3NT = null;
        //                        if (AQI_Recent8HoursO3NT != null)
        //                            OriRegionHourAQIExists.Recent8HoursO3NT_IAQI = AQI_Recent8HoursO3NT.ToString();
        //                        else
        //                            OriRegionHourAQIExists.Recent8HoursO3NT_IAQI = null;

        //                        if (PM25_Value != null)
        //                            OriRegionHourAQIExists.PM25 = PM25_Value.ToString();
        //                        else
        //                            OriRegionHourAQIExists.PM25 = null;
        //                        if (AQI_PM25 != null)
        //                            OriRegionHourAQIExists.PM25_IAQI = AQI_PM25.ToString();
        //                        else
        //                            OriRegionHourAQIExists.PM25_IAQI = null;

        //                        if (Recent24HoursPM25_Value != null)
        //                            OriRegionHourAQIExists.Recent24HoursPM25 = Recent24HoursPM25_Value.ToString();
        //                        else
        //                            OriRegionHourAQIExists.Recent24HoursPM25 = null;
        //                        if (AQI_Recent24HoursPM25 != null)
        //                            OriRegionHourAQIExists.Recent24HoursPM25_IAQI = AQI_Recent24HoursPM25.ToString();
        //                        else
        //                            OriRegionHourAQIExists.Recent24HoursPM25_IAQI = null;

        //                        string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
        //                        OriRegionHourAQIExists.AQIValue = AQIValue;
        //                        OriRegionHourAQIExists.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");
        //                        if (!string.IsNullOrWhiteSpace(AQIValue))
        //                        {
        //                            OriRegionHourAQIExists.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
        //                            OriRegionHourAQIExists.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
        //                            OriRegionHourAQIExists.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
        //                            OriRegionHourAQIExists.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
        //                            OriRegionHourAQIExists.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
        //                            OriRegionHourAQIExists.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
        //                        }
        //                        else
        //                        {
        //                            OriRegionHourAQIExists.Range = null;
        //                            OriRegionHourAQIExists.RGBValue = null;
        //                            OriRegionHourAQIExists.Class = null;
        //                            OriRegionHourAQIExists.Grade = null;
        //                            OriRegionHourAQIExists.HealthEffect = null;
        //                            OriRegionHourAQIExists.TakeStep = null;
        //                        }
        //                        OriRegionHourAQIExists.UpdateUser = "SystemSync";
        //                        OriRegionHourAQIExists.UpdateDateTime = DateTime.Now;
        //                        //AirAutoMonitorModel.SaveChanges();
        //                        #endregion
        //                    }
        //                }
        //                #endregion
        //                AirAutoMonitorModel.SaveChanges();

        //                #region 原始日区域AQI
        //                DataTable dtPivotOriRegionDay = d_DAL.GetConPivotOriRegionDayData(m_regionCon.TrimEnd(','), sTime, eTime);
        //                foreach (DataRow dr in dtPivotOriRegionDay.Rows)
        //                {
        //                    #region 数据
        //                    string m_region = dr["CityTypeUid"].ToString();
        //                    DateTime Tstamp = Convert.ToDateTime(dr["DateTime"]);

        //                    decimal? SO2_Value = null;
        //                    int? SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_MaxOneHourO3, AQI_Max8HourO3, AQI_PM25;
        //                    if (dr["SO2"] != DBNull.Value)
        //                    {
        //                        if (Convert.ToDecimal(dr["SO2"]) >= 10000)
        //                        {
        //                            SO2_Value = Convert.ToDecimal(dr["SO2"]) - 10000;
        //                            SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", null, 24);
        //                        }
        //                        else
        //                        {
        //                            SO2_Value = Convert.ToDecimal(dr["SO2"]);
        //                            SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 24);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 24);
        //                    }


        //                    decimal? NO2_Value = null;
        //                    if (dr["NO2"] != DBNull.Value)
        //                    {
        //                        if (Convert.ToDecimal(dr["NO2"]) >= 10000)
        //                        {
        //                            NO2_Value = Convert.ToDecimal(dr["NO2"]) - 10000;
        //                            AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", null, 24);
        //                        }
        //                        else
        //                        {
        //                            NO2_Value = Convert.ToDecimal(dr["NO2"]);
        //                            AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 24);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 24);
        //                    }
                            

        //                    decimal? PM10_Value = null;
        //                    if (dr["PM10"] != DBNull.Value)
        //                    {
        //                        if (Convert.ToDecimal(dr["PM10"]) >= 10000)
        //                        {
        //                            PM10_Value = Convert.ToDecimal(dr["PM10"]) - 10000;
        //                            AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", null, 24);
        //                        }
        //                        else
        //                        {
        //                            PM10_Value = Convert.ToDecimal(dr["PM10"]);
        //                            AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);
        //                    }


        //                    decimal? CO_Value = null;
        //                    if (dr["CO"] != DBNull.Value)
        //                    {
        //                        if (Convert.ToDecimal(dr["CO"]) >= 10000)
        //                        {
        //                            CO_Value = Convert.ToDecimal(dr["CO"]) - 10000;
        //                            AQI_CO = d_AQICalculateService.GetIAQI("a21005", null, 24);
        //                        }
        //                        else
        //                        {
        //                            CO_Value = Convert.ToDecimal(dr["CO"]);
        //                            AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 24);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 24);
        //                    }
                            

        //                    decimal? MaxOneHourO3_Value = null;
        //                    if (dr["MaxOneHourO3"] != DBNull.Value)
        //                    {

        //                        if (Convert.ToDecimal(dr["MaxOneHourO3"]) >= 10000)
        //                        {
        //                            MaxOneHourO3_Value = Convert.ToDecimal(dr["MaxOneHourO3"]) - 10000;
        //                            AQI_MaxOneHourO3 = d_AQICalculateService.GetIAQI("a05024", null, 1);
        //                        }
        //                        else
        //                        {
        //                            MaxOneHourO3_Value = Convert.ToDecimal(dr["MaxOneHourO3"]);
        //                            AQI_MaxOneHourO3 = d_AQICalculateService.GetIAQI("a05024", MaxOneHourO3_Value, 1);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        AQI_MaxOneHourO3 = d_AQICalculateService.GetIAQI("a05024", MaxOneHourO3_Value, 1);
        //                    }
                            

        //                    decimal? Max8HourO3_Value = null;
        //                    if (dr["Max8HourO3"] != DBNull.Value)
        //                    {
        //                        if (Convert.ToDecimal(dr["Max8HourO3"]) >= 10000)
        //                        {
        //                            Max8HourO3_Value = Convert.ToDecimal(dr["Max8HourO3"]) - 10000;
        //                            AQI_Max8HourO3 = d_AQICalculateService.GetIAQI("a05024", null, 8);
        //                        }
        //                        else
        //                        {
        //                            Max8HourO3_Value = Convert.ToDecimal(dr["Max8HourO3"]);
        //                            AQI_Max8HourO3 = d_AQICalculateService.GetIAQI("a05024", Max8HourO3_Value, 8);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        AQI_Max8HourO3 = d_AQICalculateService.GetIAQI("a05024", Max8HourO3_Value, 8);
        //                    }


        //                    decimal? PM25_Value = null;
        //                    if (dr["PM25"] != DBNull.Value)
        //                    {
        //                        if (Convert.ToDecimal(dr["PM25"]) >= 10000)
        //                        {
        //                            PM25_Value = Convert.ToDecimal(dr["PM25"]) - 10000;
        //                            AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", null, 24);
        //                        }
        //                        else
        //                        {
        //                            PM25_Value = Convert.ToDecimal(dr["PM25"]);
        //                            AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);
        //                    }


        //                    #endregion
        //                    TB_OriRegionDayAQIReport OriRegionDayAQIExists = AirAutoMonitorModel.TB_OriRegionDayAQIReports.Where(p => p.MonitoringRegionUid.Equals(m_region) && p.ReportDateTime.Equals(Tstamp)).FirstOrDefault();
        //                    if (OriRegionDayAQIExists == null)
        //                    {
        //                        #region 新增日AQI
        //                        TB_OriRegionDayAQIReport NewOriDayAQI = new TB_OriRegionDayAQIReport();
        //                        NewOriDayAQI.MonitoringRegionUid = m_region;
        //                        NewOriDayAQI.ReportDateTime = Tstamp;
        //                        NewOriDayAQI.StatisticalType = "CG";

        //                        if (SO2_Value != null)
        //                            NewOriDayAQI.SO2 = SO2_Value.ToString();
        //                        if (SO2_IAQI != null)
        //                            NewOriDayAQI.SO2_IAQI = SO2_IAQI.ToString();

        //                        if (NO2_Value != null)
        //                            NewOriDayAQI.NO2 = NO2_Value.ToString();
        //                        if (AQI_NO2 != null)
        //                            NewOriDayAQI.NO2_IAQI = AQI_NO2.ToString();

        //                        if (PM10_Value != null)
        //                            NewOriDayAQI.PM10 = PM10_Value.ToString();
        //                        if (AQI_PM10 != null)
        //                            NewOriDayAQI.PM10_IAQI = AQI_PM10.ToString();

        //                        if (CO_Value != null)
        //                            NewOriDayAQI.CO = CO_Value.ToString();
        //                        if (AQI_CO != null)
        //                            NewOriDayAQI.CO_IAQI = AQI_CO.ToString();

        //                        if (MaxOneHourO3_Value != null)
        //                            NewOriDayAQI.MaxOneHourO3 = MaxOneHourO3_Value.ToString();
        //                        if (AQI_MaxOneHourO3 != null)
        //                            NewOriDayAQI.MaxOneHourO3_IAQI = AQI_MaxOneHourO3.ToString();

        //                        if (Max8HourO3_Value != null)
        //                            NewOriDayAQI.Max8HourO3 = Max8HourO3_Value.ToString();
        //                        if (AQI_Max8HourO3 != null)
        //                            NewOriDayAQI.Max8HourO3_IAQI = AQI_Max8HourO3.ToString();

        //                        if (PM25_Value != null)
        //                            NewOriDayAQI.PM25 = PM25_Value.ToString();
        //                        if (AQI_PM25 != null)
        //                            NewOriDayAQI.PM25_IAQI = AQI_PM25.ToString();

        //                        string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "V");
        //                        NewOriDayAQI.AQIValue = AQIValue;
        //                        NewOriDayAQI.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "N");
        //                        if (!string.IsNullOrWhiteSpace(AQIValue))
        //                        {
        //                            NewOriDayAQI.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
        //                            NewOriDayAQI.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
        //                            NewOriDayAQI.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
        //                            NewOriDayAQI.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
        //                            NewOriDayAQI.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
        //                            NewOriDayAQI.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
        //                        }
        //                        NewOriDayAQI.CreatUser = "SystemSync";
        //                        NewOriDayAQI.CreatDateTime = DateTime.Now;
        //                        AirAutoMonitorModel.Add(NewOriDayAQI);
        //                        //AirAutoMonitorModel.SaveChanges();
        //                        #endregion
        //                    }
        //                    else
        //                    {
        //                        #region 更新日AQI
        //                        if (SO2_Value != null)
        //                            OriRegionDayAQIExists.SO2 = SO2_Value.ToString();
        //                        else
        //                            OriRegionDayAQIExists.SO2 = null;
        //                        if (SO2_IAQI != null)
        //                            OriRegionDayAQIExists.SO2_IAQI = SO2_IAQI.ToString();
        //                        else
        //                            OriRegionDayAQIExists.SO2_IAQI = null;

        //                        if (NO2_Value != null)
        //                            OriRegionDayAQIExists.NO2 = NO2_Value.ToString();
        //                        else
        //                            OriRegionDayAQIExists.NO2 = null;
        //                        if (AQI_NO2 != null)
        //                            OriRegionDayAQIExists.NO2_IAQI = AQI_NO2.ToString();
        //                        else
        //                            OriRegionDayAQIExists.NO2_IAQI = null;

        //                        if (PM10_Value != null)
        //                            OriRegionDayAQIExists.PM10 = PM10_Value.ToString();
        //                        else
        //                            OriRegionDayAQIExists.PM10 = null;
        //                        if (AQI_PM10 != null)
        //                            OriRegionDayAQIExists.PM10_IAQI = AQI_PM10.ToString();
        //                        else
        //                            OriRegionDayAQIExists.PM10_IAQI = null;

        //                        if (CO_Value != null)
        //                            OriRegionDayAQIExists.CO = CO_Value.ToString();
        //                        else
        //                            OriRegionDayAQIExists.CO = null;
        //                        if (AQI_CO != null)
        //                            OriRegionDayAQIExists.CO_IAQI = AQI_CO.ToString();
        //                        else
        //                            OriRegionDayAQIExists.CO_IAQI = null;

        //                        if (MaxOneHourO3_Value != null)
        //                            OriRegionDayAQIExists.MaxOneHourO3 = MaxOneHourO3_Value.ToString();
        //                        else
        //                            OriRegionDayAQIExists.MaxOneHourO3 = null;
        //                        if (AQI_MaxOneHourO3 != null)
        //                            OriRegionDayAQIExists.MaxOneHourO3_IAQI = AQI_MaxOneHourO3.ToString();
        //                        else
        //                            OriRegionDayAQIExists.MaxOneHourO3_IAQI = null;

        //                        if (Max8HourO3_Value != null)
        //                            OriRegionDayAQIExists.Max8HourO3 = Max8HourO3_Value.ToString();
        //                        else
        //                            OriRegionDayAQIExists.Max8HourO3 = null;
        //                        if (AQI_Max8HourO3 != null)
        //                            OriRegionDayAQIExists.Max8HourO3_IAQI = AQI_Max8HourO3.ToString();
        //                        else
        //                            OriRegionDayAQIExists.Max8HourO3_IAQI = null;

        //                        if (PM25_Value != null)
        //                            OriRegionDayAQIExists.PM25 = PM25_Value.ToString();
        //                        else
        //                            OriRegionDayAQIExists.PM25 = null;
        //                        if (AQI_PM25 != null)
        //                            OriRegionDayAQIExists.PM25_IAQI = AQI_PM25.ToString();
        //                        else
        //                            OriRegionDayAQIExists.PM25_IAQI = null;

        //                        string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "V");
        //                        OriRegionDayAQIExists.AQIValue = AQIValue;
        //                        OriRegionDayAQIExists.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "N");
        //                        if (!string.IsNullOrWhiteSpace(AQIValue))
        //                        {
        //                            OriRegionDayAQIExists.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
        //                            OriRegionDayAQIExists.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
        //                            OriRegionDayAQIExists.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
        //                            OriRegionDayAQIExists.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
        //                            OriRegionDayAQIExists.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEAlTHEFFECT");
        //                            OriRegionDayAQIExists.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
        //                        }
        //                        else
        //                        {
        //                            OriRegionDayAQIExists.Range = null;
        //                            OriRegionDayAQIExists.RGBValue = null;
        //                            OriRegionDayAQIExists.Class = null;
        //                            OriRegionDayAQIExists.Grade = null;
        //                            OriRegionDayAQIExists.HealthEffect = null;
        //                            OriRegionDayAQIExists.TakeStep = null;
        //                        }
        //                        OriRegionDayAQIExists.UpdateUser = "SystemSync";
        //                        OriRegionDayAQIExists.UpdateDateTime = DateTime.Now;
        //                        //AirAutoMonitorModel.SaveChanges();
        //                        #endregion
        //                    }
        //                }
        //                #endregion
        //                AirAutoMonitorModel.SaveChanges();
        //            //}
        //            #endregion
        //        }
        //        log.Info("-------------------------------------------------------CalculateConBy60原始数据结束----------------------------");
        //        #region
        //        log.Info("-------------------------------------------------------CalculateConBy60审核数据开始----------------------------");
        //        using (MonitorBusinessModel MonitorBusinessModel = new MonitorBusinessModel())
        //        {
        //            #region 审核测点AQI
        //            //foreach (int PointId in PointIds)
        //            //{
        //                #region 计算审核小时AQI
        //            DataTable dtPivotAudHour = d_DAL.GetConPivotAudHourData(ConPointIds, sTime, eTime);
        //                foreach (DataRow dr in dtPivotAudHour.Rows)
        //                {
        //                    #region 数据
        //                    int? PointId = Convert.ToInt32(dr["PointId"]);
        //                    DateTime Tstamp = Convert.ToDateTime(dr["Tstamp"]);
        //                    decimal? SO2_Value = null;
        //                    if (dr["SO2"] != DBNull.Value)
        //                        SO2_Value = Convert.ToDecimal(dr["SO2"]);
        //                    int? SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 1);

        //                    decimal? NO2_Value = null;
        //                    if (dr["NO2"] != DBNull.Value)
        //                        NO2_Value = Convert.ToDecimal(dr["NO2"]);
        //                    int? AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 1);

        //                    decimal? PM10_Value = null;
        //                    if (dr["PM10"] != DBNull.Value)
        //                        PM10_Value = Convert.ToDecimal(dr["PM10"]);
        //                    int? AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);

        //                    decimal? Recent24HoursPM10_Value = null;
        //                    if (dr["Recent24HoursPM10"] != DBNull.Value)
        //                        Recent24HoursPM10_Value = Convert.ToDecimal(dr["Recent24HoursPM10"]);
        //                    int? AQI_Recent24HoursPM10 = d_AQICalculateService.GetIAQI("a34002", Recent24HoursPM10_Value, 24);

        //                    decimal? CO_Value = null;
        //                    //先修约再计算AQI
        //                    if (dr["CO"] != DBNull.Value)
        //                    {
        //                        DataTable dtFactor = d_AQICalculateService.GetPollutantUnit("a21005");
        //                        int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);
        //                        if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m3"))
        //                        {
        //                            CO_Value = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO"]), decimalNum) * 1000);

        //                        }
        //                        else
        //                        {
        //                            CO_Value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO"]), decimalNum);
        //                        }
        //                    }
        //                    int? AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 1);

        //                    decimal? O3_Value = null;
        //                    if (dr["O3"] != DBNull.Value)
        //                        O3_Value = Convert.ToDecimal(dr["O3"]);
        //                    int? AQI_O3 = d_AQICalculateService.GetIAQI("a05024", O3_Value, 1);

        //                    decimal? Recent8HoursO3_Value = null;
        //                    if (dr["Recent8HoursO3"] != DBNull.Value)
        //                        Recent8HoursO3_Value = Convert.ToDecimal(dr["Recent8HoursO3"]);
        //                    int? AQI_Recent8HoursO3 = d_AQICalculateService.GetIAQI("a05024", Recent8HoursO3_Value, 8);

        //                    decimal? Recent8HoursO3NT_Value = null;
        //                    if (dr["Recent8HoursO3NT"] != DBNull.Value)
        //                        Recent8HoursO3NT_Value = Convert.ToDecimal(dr["Recent8HoursO3NT"]);
        //                    int? AQI_Recent8HoursO3NT = d_AQICalculateService.GetIAQI("a05024", Recent8HoursO3NT_Value, 8);

        //                    decimal? PM25_Value = null;
        //                    if (dr["PM25"] != DBNull.Value)
        //                        PM25_Value = Convert.ToDecimal(dr["PM25"]);
        //                    int? AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);

        //                    decimal? Recent24HoursPM25_Value = null;
        //                    if (dr["Recent24HoursPM25"] != DBNull.Value)
        //                        Recent24HoursPM25_Value = Convert.ToDecimal(dr["Recent24HoursPM25"]);
        //                    int? AQI_Recent24HoursPM25 = d_AQICalculateService.GetIAQI("a34004", Recent24HoursPM25_Value, 24);
        //                    #endregion

        //                    TB_HourAQI HourAQIExists = MonitorBusinessModel.TB_HourAQIs.Where(p => p.PointId.Equals(PointId) && p.DateTime.Equals(Tstamp)).FirstOrDefault();
        //                    if (HourAQIExists == null)
        //                    {
        //                        #region 新增小时AQI
        //                        TB_HourAQI NewAudHourAQI = new TB_HourAQI();
        //                        NewAudHourAQI.PointId = PointId;
        //                        NewAudHourAQI.DateTime = Tstamp;

        //                        if (SO2_Value != null)
        //                            NewAudHourAQI.SO2 = SO2_Value.ToString();
        //                        if (SO2_IAQI != null)
        //                            NewAudHourAQI.SO2_IAQI = SO2_IAQI.ToString();

        //                        if (NO2_Value != null)
        //                            NewAudHourAQI.NO2 = NO2_Value.ToString();
        //                        if (AQI_NO2 != null)
        //                            NewAudHourAQI.NO2_IAQI = AQI_NO2.ToString();

        //                        if (PM10_Value != null)
        //                            NewAudHourAQI.PM10 = PM10_Value.ToString();
        //                        if (AQI_PM10 != null)
        //                            NewAudHourAQI.PM10_IAQI = AQI_PM10.ToString();

        //                        if (Recent24HoursPM10_Value != null)
        //                            NewAudHourAQI.Recent24HoursPM10 = Recent24HoursPM10_Value.ToString();
        //                        if (AQI_Recent24HoursPM10 != null)
        //                            NewAudHourAQI.Recent24HoursPM10_IAQI = AQI_Recent24HoursPM10.ToString();

        //                        if (CO_Value != null)
        //                            NewAudHourAQI.CO = CO_Value.ToString();
        //                        if (AQI_CO != null)
        //                            NewAudHourAQI.CO_IAQI = AQI_CO.ToString();

        //                        if (O3_Value != null)
        //                            NewAudHourAQI.O3 = O3_Value.ToString();
        //                        if (AQI_O3 != null)
        //                            NewAudHourAQI.O3_IAQI = AQI_O3.ToString();

        //                        if (Recent8HoursO3_Value != null)
        //                            NewAudHourAQI.Recent8HoursO3 = Recent8HoursO3_Value.ToString();
        //                        if (AQI_Recent8HoursO3 != null)
        //                            NewAudHourAQI.Recent8HoursO3_IAQI = AQI_Recent8HoursO3.ToString();

        //                        if (Recent8HoursO3NT_Value != null)
        //                            NewAudHourAQI.Recent8HoursO3NT = Recent8HoursO3NT_Value.ToString();
        //                        if (AQI_Recent8HoursO3NT != null)
        //                            NewAudHourAQI.Recent8HoursO3NT_IAQI = AQI_Recent8HoursO3NT.ToString();

        //                        if (PM25_Value != null)
        //                            NewAudHourAQI.PM25 = PM25_Value.ToString();
        //                        if (AQI_PM25 != null)
        //                            NewAudHourAQI.PM25_IAQI = AQI_PM25.ToString();

        //                        if (Recent24HoursPM25_Value != null)
        //                            NewAudHourAQI.Recent24HoursPM25 = Recent24HoursPM25_Value.ToString();
        //                        if (AQI_Recent24HoursPM25 != null)
        //                            NewAudHourAQI.Recent24HoursPM25_IAQI = AQI_Recent24HoursPM25.ToString();

        //                        string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
        //                        NewAudHourAQI.AQIValue = AQIValue;
        //                        NewAudHourAQI.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");
        //                        if (!string.IsNullOrWhiteSpace(AQIValue))
        //                        {
        //                            NewAudHourAQI.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
        //                            NewAudHourAQI.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
        //                            NewAudHourAQI.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
        //                            NewAudHourAQI.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
        //                            NewAudHourAQI.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
        //                            NewAudHourAQI.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
        //                        }
        //                        NewAudHourAQI.CreatUser = "SystemSync";
        //                        NewAudHourAQI.CreatDateTime = DateTime.Now;
        //                        MonitorBusinessModel.Add(NewAudHourAQI);
        //                        //MonitorBusinessModel.SaveChanges();
        //                        #endregion
        //                    }
        //                    else
        //                    {
        //                        #region 更新小时AQI
        //                        if (SO2_Value != null)
        //                            HourAQIExists.SO2 = SO2_Value.ToString();
        //                        else
        //                            HourAQIExists.SO2 = null;
        //                        if (SO2_IAQI != null)
        //                            HourAQIExists.SO2_IAQI = SO2_IAQI.ToString();
        //                        else
        //                            HourAQIExists.SO2_IAQI = null;
        //                        if (NO2_Value != null)
        //                            HourAQIExists.NO2 = NO2_Value.ToString();
        //                        else
        //                            HourAQIExists.NO2 = null;
        //                        if (AQI_NO2 != null)
        //                            HourAQIExists.NO2_IAQI = AQI_NO2.ToString();
        //                        else
        //                            HourAQIExists.NO2_IAQI = null;
        //                        if (PM10_Value != null)
        //                            HourAQIExists.PM10 = PM10_Value.ToString();
        //                        else
        //                            HourAQIExists.PM10 = null;
        //                        if (AQI_PM10 != null)
        //                            HourAQIExists.PM10_IAQI = AQI_PM10.ToString();
        //                        else
        //                            HourAQIExists.PM10_IAQI = null;
        //                        if (Recent24HoursPM10_Value != null)
        //                            HourAQIExists.Recent24HoursPM10 = Recent24HoursPM10_Value.ToString();
        //                        else
        //                            HourAQIExists.Recent24HoursPM10 = null;
        //                        if (AQI_Recent24HoursPM10 != null)
        //                            HourAQIExists.Recent24HoursPM10_IAQI = AQI_Recent24HoursPM10.ToString();
        //                        else
        //                            HourAQIExists.Recent24HoursPM10_IAQI = null;
        //                        if (CO_Value != null)
        //                            HourAQIExists.CO = CO_Value.ToString();
        //                        else
        //                            HourAQIExists.CO = null;
        //                        if (AQI_CO != null)
        //                            HourAQIExists.CO_IAQI = AQI_CO.ToString();
        //                        else
        //                            HourAQIExists.CO_IAQI = null;
        //                        if (O3_Value != null)
        //                            HourAQIExists.O3 = O3_Value.ToString();
        //                        else
        //                            HourAQIExists.O3 = null;
        //                        if (AQI_O3 != null)
        //                            HourAQIExists.O3_IAQI = AQI_O3.ToString();
        //                        else
        //                            HourAQIExists.O3_IAQI = null;
        //                        if (Recent8HoursO3_Value != null)
        //                            HourAQIExists.Recent8HoursO3 = Recent8HoursO3_Value.ToString();
        //                        else
        //                            HourAQIExists.Recent8HoursO3 = null;
        //                        if (AQI_Recent8HoursO3 != null)
        //                            HourAQIExists.Recent8HoursO3_IAQI = AQI_Recent8HoursO3.ToString();
        //                        else
        //                            HourAQIExists.Recent8HoursO3_IAQI = null;
        //                        if (Recent8HoursO3NT_Value != null)
        //                            HourAQIExists.Recent8HoursO3NT = Recent8HoursO3NT_Value.ToString();
        //                        else
        //                            HourAQIExists.Recent8HoursO3NT = null;
        //                        if (AQI_Recent8HoursO3NT != null)
        //                            HourAQIExists.Recent8HoursO3NT_IAQI = AQI_Recent8HoursO3NT.ToString();
        //                        else
        //                            HourAQIExists.Recent8HoursO3NT_IAQI = null;
        //                        if (PM25_Value != null)
        //                            HourAQIExists.PM25 = PM25_Value.ToString();
        //                        else
        //                            HourAQIExists.PM25 = null;
        //                        if (AQI_PM25 != null)
        //                            HourAQIExists.PM25_IAQI = AQI_PM25.ToString();
        //                        else
        //                            HourAQIExists.PM25_IAQI = null;
        //                        if (Recent24HoursPM25_Value != null)
        //                            HourAQIExists.Recent24HoursPM25 = Recent24HoursPM25_Value.ToString();
        //                        else
        //                            HourAQIExists.Recent24HoursPM25 = null;
        //                        if (AQI_Recent24HoursPM25 != null)
        //                            HourAQIExists.Recent24HoursPM25_IAQI = AQI_Recent24HoursPM25.ToString();
        //                        else
        //                            HourAQIExists.Recent24HoursPM25_IAQI = null;
        //                        string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
        //                        HourAQIExists.AQIValue = AQIValue;
        //                        HourAQIExists.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");
        //                        if (!string.IsNullOrWhiteSpace(AQIValue))
        //                        {
        //                            HourAQIExists.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
        //                            HourAQIExists.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
        //                            HourAQIExists.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
        //                            HourAQIExists.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
        //                            HourAQIExists.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
        //                            HourAQIExists.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
        //                        }
        //                        else
        //                        {
        //                            HourAQIExists.Range = null;
        //                            HourAQIExists.RGBValue = null;
        //                            HourAQIExists.Class = null;
        //                            HourAQIExists.Grade = null;
        //                            HourAQIExists.HealthEffect = null;
        //                            HourAQIExists.TakeStep = null;
        //                        }
        //                        HourAQIExists.UpdateUser = "SystemSync";
        //                        HourAQIExists.UpdateDateTime = DateTime.Now;
        //                        //MonitorBusinessModel.SaveChanges();
        //                        #endregion
        //                    }
        //                }
        //                #endregion
        //                MonitorBusinessModel.SaveChanges();

        //                string CalculateTableName = "AirReport.TB_DayReport_Calculate";
        //                // 生成日计算数据
        //                d_DAL.AddConAirReport_Day_Mul(sTime, eTime, ConPointIds, CalculateTableName);

        //                string AuditTableName = "AirReport.TB_DayReport";
        //                //日审核数据
        //                d_DAL.AddConAirReport_Day_Mul(sTime, eTime, ConPointIds, AuditTableName);

        //                #region 计算生成审核日AQI
        //                DataTable dtPivotDay = d_DAL.GetConPivotAudDayData(ConPointIds, sTime, eTime);
        //                foreach (DataRow dr in dtPivotDay.Rows)
        //                {
        //                    #region 数据
        //                    int? PointId = Convert.ToInt32(dr["PointId"]);
        //                    DateTime Tstamp = Convert.ToDateTime(dr["DateTime"]);
        //                    decimal? SO2_Value = null;
        //                    if (dr["SO2"] != DBNull.Value)
        //                        SO2_Value = Convert.ToDecimal(dr["SO2"]);
        //                    int? SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 24);

        //                    decimal? NO2_Value = null;
        //                    if (dr["NO2"] != DBNull.Value)
        //                        NO2_Value = Convert.ToDecimal(dr["NO2"]);
        //                    int? AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 24);

        //                    decimal? PM10_Value = null;
        //                    if (dr["PM10"] != DBNull.Value)
        //                        PM10_Value = Convert.ToDecimal(dr["PM10"]);
        //                    int? AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);


        //                    decimal? CO_Value = null;
        //                    if (dr["CO"] != DBNull.Value)
        //                        CO_Value = Convert.ToDecimal(dr["CO"]);
        //                    int? AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 24);

        //                    decimal? MaxOneHourO3_Value = null;
        //                    if (dr["MaxOneHourO3"] != DBNull.Value)
        //                        MaxOneHourO3_Value = Convert.ToDecimal(dr["MaxOneHourO3"]);
        //                    int? AQI_MaxOneHourO3 = d_AQICalculateService.GetIAQI("a05024", MaxOneHourO3_Value, 1);

        //                    decimal? Max8HourO3_Value = null;
        //                    if (dr["Max8HourO3"] != DBNull.Value)
        //                        Max8HourO3_Value = Convert.ToDecimal(dr["Max8HourO3"]);
        //                    int? AQI_Max8HourO3 = d_AQICalculateService.GetIAQI("a05024", Max8HourO3_Value, 8);

        //                    decimal? PM25_Value = null;
        //                    if (dr["PM25"] != DBNull.Value)
        //                        PM25_Value = Convert.ToDecimal(dr["PM25"]);
        //                    int? AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);

        //                    #endregion
        //                    TB_DayAQI AudDayAQIExists = MonitorBusinessModel.TB_DayAQIs.Where(p => p.PointId.Equals(PointId) && p.DateTime.Equals(Tstamp)).FirstOrDefault();
        //                    if (AudDayAQIExists == null)
        //                    {
        //                        #region 新增日AQI
        //                        TB_DayAQI NewAudDayAQI = new TB_DayAQI();
        //                        NewAudDayAQI.PointId = PointId;
        //                        NewAudDayAQI.DateTime = Tstamp;

        //                        if (SO2_Value != null)
        //                            NewAudDayAQI.SO2 = SO2_Value.ToString();
        //                        if (SO2_IAQI != null)
        //                            NewAudDayAQI.SO2_IAQI = SO2_IAQI.ToString();

        //                        if (NO2_Value != null)
        //                            NewAudDayAQI.NO2 = NO2_Value.ToString();
        //                        if (AQI_NO2 != null)
        //                            NewAudDayAQI.NO2_IAQI = AQI_NO2.ToString();

        //                        if (PM10_Value != null)
        //                            NewAudDayAQI.PM10 = PM10_Value.ToString();
        //                        if (AQI_PM10 != null)
        //                            NewAudDayAQI.PM10_IAQI = AQI_PM10.ToString();

        //                        if (CO_Value != null)
        //                            NewAudDayAQI.CO = CO_Value.ToString();
        //                        if (AQI_CO != null)
        //                            NewAudDayAQI.CO_IAQI = AQI_CO.ToString();

        //                        if (MaxOneHourO3_Value != null)
        //                            NewAudDayAQI.MaxOneHourO3 = MaxOneHourO3_Value.ToString();
        //                        if (AQI_MaxOneHourO3 != null)
        //                            NewAudDayAQI.MaxOneHourO3_IAQI = AQI_MaxOneHourO3.ToString();

        //                        if (Max8HourO3_Value != null)
        //                            NewAudDayAQI.Max8HourO3 = Max8HourO3_Value.ToString();
        //                        if (AQI_Max8HourO3 != null)
        //                            NewAudDayAQI.Max8HourO3_IAQI = AQI_Max8HourO3.ToString();

        //                        if (PM25_Value != null)
        //                            NewAudDayAQI.PM25 = PM25_Value.ToString();
        //                        if (AQI_PM25 != null)
        //                            NewAudDayAQI.PM25_IAQI = AQI_PM25.ToString();

        //                        string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "V");
        //                        NewAudDayAQI.AQIValue = AQIValue;
        //                        NewAudDayAQI.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "N");
        //                        if (!string.IsNullOrWhiteSpace(AQIValue))
        //                        {
        //                            NewAudDayAQI.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
        //                            NewAudDayAQI.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
        //                            NewAudDayAQI.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
        //                            NewAudDayAQI.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
        //                            NewAudDayAQI.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
        //                            NewAudDayAQI.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
        //                        }

        //                        NewAudDayAQI.CreatUser = "SystemSync";
        //                        NewAudDayAQI.CreatDateTime = DateTime.Now;
        //                        MonitorBusinessModel.Add(NewAudDayAQI);
        //                        //MonitorBusinessModel.SaveChanges();
        //                        #endregion
        //                    }
        //                    else
        //                    {
        //                        #region 更新日AQI
        //                        if (SO2_Value != null)
        //                            AudDayAQIExists.SO2 = SO2_Value.ToString();
        //                        else
        //                            AudDayAQIExists.SO2 = null;
        //                        if (SO2_IAQI != null)
        //                            AudDayAQIExists.SO2_IAQI = SO2_IAQI.ToString();
        //                        else
        //                            AudDayAQIExists.SO2_IAQI = null;

        //                        if (NO2_Value != null)
        //                            AudDayAQIExists.NO2 = NO2_Value.ToString();
        //                        else
        //                            AudDayAQIExists.NO2 = null;
        //                        if (AQI_NO2 != null)
        //                            AudDayAQIExists.NO2_IAQI = AQI_NO2.ToString();
        //                        else
        //                            AudDayAQIExists.NO2_IAQI = null;

        //                        if (PM10_Value != null)
        //                            AudDayAQIExists.PM10 = PM10_Value.ToString();
        //                        else
        //                            AudDayAQIExists.PM10 = null;
        //                        if (AQI_PM10 != null)
        //                            AudDayAQIExists.PM10_IAQI = AQI_PM10.ToString();
        //                        else
        //                            AudDayAQIExists.PM10_IAQI = null;

        //                        if (CO_Value != null)
        //                            AudDayAQIExists.CO = CO_Value.ToString();
        //                        else
        //                            AudDayAQIExists.CO = null;
        //                        if (AQI_CO != null)
        //                            AudDayAQIExists.CO_IAQI = AQI_CO.ToString();
        //                        else
        //                            AudDayAQIExists.CO_IAQI = null;

        //                        if (MaxOneHourO3_Value != null)
        //                            AudDayAQIExists.MaxOneHourO3 = MaxOneHourO3_Value.ToString();
        //                        else
        //                            AudDayAQIExists.MaxOneHourO3 = null;
        //                        if (AQI_MaxOneHourO3 != null)
        //                            AudDayAQIExists.MaxOneHourO3_IAQI = AQI_MaxOneHourO3.ToString();
        //                        else
        //                            AudDayAQIExists.MaxOneHourO3_IAQI = null;

        //                        if (Max8HourO3_Value != null)
        //                            AudDayAQIExists.Max8HourO3 = Max8HourO3_Value.ToString();
        //                        else
        //                            AudDayAQIExists.Max8HourO3 = null;
        //                        if (AQI_Max8HourO3 != null)
        //                            AudDayAQIExists.Max8HourO3_IAQI = AQI_Max8HourO3.ToString();
        //                        else
        //                            AudDayAQIExists.Max8HourO3_IAQI = null;

        //                        if (PM25_Value != null)
        //                            AudDayAQIExists.PM25 = PM25_Value.ToString();
        //                        else
        //                            AudDayAQIExists.PM25 = null;
        //                        if (AQI_PM25 != null)
        //                            AudDayAQIExists.PM25_IAQI = AQI_PM25.ToString();
        //                        else
        //                            AudDayAQIExists.PM25_IAQI = null;

        //                        string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "V");
        //                        AudDayAQIExists.AQIValue = AQIValue;
        //                        AudDayAQIExists.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "N");
        //                        if (!string.IsNullOrWhiteSpace(AQIValue))
        //                        {
        //                            AudDayAQIExists.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
        //                            AudDayAQIExists.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
        //                            AudDayAQIExists.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
        //                            AudDayAQIExists.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
        //                            AudDayAQIExists.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEAlTHEFFECT");
        //                            AudDayAQIExists.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
        //                        }
        //                        else
        //                        {
        //                            AudDayAQIExists.Range = null;
        //                            AudDayAQIExists.RGBValue = null;
        //                            AudDayAQIExists.Class = null;
        //                            AudDayAQIExists.Grade = null;
        //                            AudDayAQIExists.HealthEffect = null;
        //                            AudDayAQIExists.TakeStep = null;
        //                        }
        //                        AudDayAQIExists.UpdateUser = "SystemSync";
        //                        AudDayAQIExists.UpdateDateTime = DateTime.Now;
        //                        //MonitorBusinessModel.SaveChanges();
        //                        #endregion
        //                    }
        //                }
        //                #endregion
        //                MonitorBusinessModel.SaveChanges();
        //            //}
        //            #endregion
        //            #region 审核区域AQI
        //                string m_regionCon = string.Empty;
        //                foreach (string str in m_regions)
        //                {
        //                    m_regionCon += "'" + str + "',";
        //                }
        //            //foreach (string m_region in m_regions)
        //            //{
        //                #region 审核小时区域AQI
        //                DataTable dtPivotAudRegionHour = d_DAL.GetConPivotAudRegionHourData(m_regionCon.TrimEnd(','), sTime, eTime);
        //                foreach (DataRow dr in dtPivotAudRegionHour.Rows)
        //                {
        //                    #region 数据
        //                    string m_region = dr["CityTypeUid"].ToString();
        //                    DateTime Tstamp = Convert.ToDateTime(dr["DateTime"]);
        //                    decimal? SO2_Value = null;
        //                    if (dr["SO2"] != DBNull.Value)
        //                        SO2_Value = Convert.ToDecimal(dr["SO2"]);
        //                    int? SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 1);

        //                    decimal? NO2_Value = null;
        //                    if (dr["NO2"] != DBNull.Value)
        //                        NO2_Value = Convert.ToDecimal(dr["NO2"]);
        //                    int? AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 1);

        //                    decimal? PM10_Value = null;
        //                    if (dr["PM10"] != DBNull.Value)
        //                        PM10_Value = Convert.ToDecimal(dr["PM10"]);
        //                    int? AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);

        //                    decimal? Recent24HoursPM10_Value = null;
        //                    if (dr["Recent24HoursPM10"] != DBNull.Value)
        //                        Recent24HoursPM10_Value = Convert.ToDecimal(dr["Recent24HoursPM10"]);
        //                    int? AQI_Recent24HoursPM10 = d_AQICalculateService.GetIAQI("a34002", Recent24HoursPM10_Value, 24);

        //                    decimal? CO_Value = null;
        //                    //先修约再计算AQI
        //                    if (dr["CO"] != DBNull.Value)
        //                    {
        //                        DataTable dtFactor = d_AQICalculateService.GetPollutantUnit("a21005");
        //                        int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);
        //                        if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m3"))
        //                        {
        //                            CO_Value = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO"]), decimalNum) * 1000);

        //                        }
        //                        else
        //                        {
        //                            CO_Value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO"]), decimalNum);
        //                        }
        //                    }
        //                    int? AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 1);

        //                    decimal? O3_Value = null;
        //                    if (dr["O3"] != DBNull.Value)
        //                        O3_Value = Convert.ToDecimal(dr["O3"]);
        //                    int? AQI_O3 = d_AQICalculateService.GetIAQI("a05024", O3_Value, 1);

        //                    decimal? Recent8HoursO3_Value = null;
        //                    if (dr["Recent8HoursO3"] != DBNull.Value)
        //                        Recent8HoursO3_Value = Convert.ToDecimal(dr["Recent8HoursO3"]);
        //                    int? AQI_Recent8HoursO3 = d_AQICalculateService.GetIAQI("a05024", Recent8HoursO3_Value, 8);

        //                    decimal? Recent8HoursO3NT_Value = null;
        //                    if (dr["Recent8HoursO3NT"] != DBNull.Value)
        //                        Recent8HoursO3NT_Value = Convert.ToDecimal(dr["Recent8HoursO3NT"]);
        //                    int? AQI_Recent8HoursO3NT = d_AQICalculateService.GetIAQI("a05024", Recent8HoursO3NT_Value, 8);

        //                    decimal? PM25_Value = null;
        //                    if (dr["PM25"] != DBNull.Value)
        //                        PM25_Value = Convert.ToDecimal(dr["PM25"]);
        //                    int? AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);

        //                    decimal? Recent24HoursPM25_Value = null;
        //                    if (dr["Recent24HoursPM25"] != DBNull.Value)
        //                        Recent24HoursPM25_Value = Convert.ToDecimal(dr["Recent24HoursPM25"]);
        //                    int? AQI_Recent24HoursPM25 = d_AQICalculateService.GetIAQI("a34004", Recent24HoursPM25_Value, 24);
        //                    #endregion
        //                    TB_RegionHourAQI AudRegionHourAQIExists = MonitorBusinessModel.TB_RegionHourAQIs.Where(p => p.MonitoringRegionUid.Equals(m_region) && p.DateTime.Equals(Tstamp)).FirstOrDefault();
        //                    if (AudRegionHourAQIExists == null)
        //                    {
        //                        #region 新增小时AQI
        //                        TB_RegionHourAQI NewAudHourAQI = new TB_RegionHourAQI();
        //                        NewAudHourAQI.MonitoringRegionUid = m_region;
        //                        NewAudHourAQI.DateTime = Tstamp;
        //                        NewAudHourAQI.StatisticalType = "CG";

        //                        if (SO2_Value != null)
        //                            NewAudHourAQI.SO2 = SO2_Value.ToString();
        //                        if (SO2_IAQI != null)
        //                            NewAudHourAQI.SO2_IAQI = SO2_IAQI.ToString();

        //                        if (NO2_Value != null)
        //                            NewAudHourAQI.NO2 = NO2_Value.ToString();
        //                        if (AQI_NO2 != null)
        //                            NewAudHourAQI.NO2_IAQI = AQI_NO2.ToString();

        //                        if (PM10_Value != null)
        //                            NewAudHourAQI.PM10 = PM10_Value.ToString();
        //                        if (AQI_PM10 != null)
        //                            NewAudHourAQI.PM10_IAQI = AQI_PM10.ToString();

        //                        if (Recent24HoursPM10_Value != null)
        //                            NewAudHourAQI.Recent24HoursPM10 = Recent24HoursPM10_Value.ToString();
        //                        if (AQI_Recent24HoursPM10 != null)
        //                            NewAudHourAQI.Recent24HoursPM10_IAQI = AQI_Recent24HoursPM10.ToString();

        //                        if (CO_Value != null)
        //                            NewAudHourAQI.CO = CO_Value.ToString();
        //                        if (AQI_CO != null)
        //                            NewAudHourAQI.CO_IAQI = AQI_CO.ToString();

        //                        if (O3_Value != null)
        //                            NewAudHourAQI.O3 = O3_Value.ToString();
        //                        if (AQI_O3 != null)
        //                            NewAudHourAQI.O3_IAQI = AQI_O3.ToString();

        //                        if (Recent8HoursO3_Value != null)
        //                            NewAudHourAQI.Recent8HoursO3 = Recent8HoursO3_Value.ToString();
        //                        if (AQI_Recent8HoursO3 != null)
        //                            NewAudHourAQI.Recent8HoursO3_IAQI = AQI_Recent8HoursO3.ToString();

        //                        if (Recent8HoursO3NT_Value != null)
        //                            NewAudHourAQI.Recent8HoursO3NT = Recent8HoursO3NT_Value.ToString();
        //                        if (AQI_Recent8HoursO3NT != null)
        //                            NewAudHourAQI.Recent8HoursO3NT_IAQI = AQI_Recent8HoursO3NT.ToString();

        //                        if (PM25_Value != null)
        //                            NewAudHourAQI.PM25 = PM25_Value.ToString();
        //                        if (AQI_PM25 != null)
        //                            NewAudHourAQI.PM25_IAQI = AQI_PM25.ToString();

        //                        if (Recent24HoursPM25_Value != null)
        //                            NewAudHourAQI.Recent24HoursPM25 = Recent24HoursPM25_Value.ToString();
        //                        if (AQI_Recent24HoursPM25 != null)
        //                            NewAudHourAQI.Recent24HoursPM25_IAQI = AQI_Recent24HoursPM25.ToString();

        //                        string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
        //                        NewAudHourAQI.AQIValue = AQIValue;
        //                        NewAudHourAQI.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");
        //                        if (!string.IsNullOrWhiteSpace(AQIValue))
        //                        {
        //                            NewAudHourAQI.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
        //                            NewAudHourAQI.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
        //                            NewAudHourAQI.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
        //                            NewAudHourAQI.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
        //                            NewAudHourAQI.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
        //                            NewAudHourAQI.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
        //                        }
        //                        NewAudHourAQI.CreatUser = "SystemSync";
        //                        NewAudHourAQI.CreatDateTime = DateTime.Now;
        //                        MonitorBusinessModel.Add(NewAudHourAQI);
        //                        #endregion
        //                    }
        //                    else
        //                    {
        //                        #region 更新小时AQI
        //                        if (SO2_Value != null)
        //                            AudRegionHourAQIExists.SO2 = SO2_Value.ToString();
        //                        else
        //                            AudRegionHourAQIExists.SO2 = null;
        //                        if (SO2_IAQI != null)
        //                            AudRegionHourAQIExists.SO2_IAQI = SO2_IAQI.ToString();
        //                        else
        //                            AudRegionHourAQIExists.SO2_IAQI = null;

        //                        if (NO2_Value != null)
        //                            AudRegionHourAQIExists.NO2 = NO2_Value.ToString();
        //                        else
        //                            AudRegionHourAQIExists.NO2 = null;
        //                        if (AQI_NO2 != null)
        //                            AudRegionHourAQIExists.NO2_IAQI = AQI_NO2.ToString();
        //                        else
        //                            AudRegionHourAQIExists.NO2_IAQI = null;

        //                        if (PM10_Value != null)
        //                            AudRegionHourAQIExists.PM10 = PM10_Value.ToString();
        //                        else
        //                            AudRegionHourAQIExists.PM10 = null;
        //                        if (AQI_PM10 != null)
        //                            AudRegionHourAQIExists.PM10_IAQI = AQI_PM10.ToString();
        //                        else
        //                            AudRegionHourAQIExists.PM10_IAQI = null;

        //                        if (Recent24HoursPM10_Value != null)
        //                            AudRegionHourAQIExists.Recent24HoursPM10 = Recent24HoursPM10_Value.ToString();
        //                        else
        //                            AudRegionHourAQIExists.Recent24HoursPM10 = null;
        //                        if (AQI_Recent24HoursPM10 != null)
        //                            AudRegionHourAQIExists.Recent24HoursPM10_IAQI = AQI_Recent24HoursPM10.ToString();
        //                        else
        //                            AudRegionHourAQIExists.Recent24HoursPM10_IAQI = null;

        //                        if (CO_Value != null)
        //                            AudRegionHourAQIExists.CO = CO_Value.ToString();
        //                        else
        //                            AudRegionHourAQIExists.CO = null;
        //                        if (AQI_CO != null)
        //                            AudRegionHourAQIExists.CO_IAQI = AQI_CO.ToString();
        //                        else
        //                            AudRegionHourAQIExists.CO_IAQI = null;

        //                        if (O3_Value != null)
        //                            AudRegionHourAQIExists.O3 = O3_Value.ToString();
        //                        else
        //                            AudRegionHourAQIExists.O3 = null;
        //                        if (AQI_O3 != null)
        //                            AudRegionHourAQIExists.O3_IAQI = AQI_O3.ToString();
        //                        else
        //                            AudRegionHourAQIExists.O3_IAQI = null;

        //                        if (Recent8HoursO3_Value != null)
        //                            AudRegionHourAQIExists.Recent8HoursO3 = Recent8HoursO3_Value.ToString();
        //                        else
        //                            AudRegionHourAQIExists.Recent8HoursO3 = null;
        //                        if (AQI_Recent8HoursO3 != null)
        //                            AudRegionHourAQIExists.Recent8HoursO3_IAQI = AQI_Recent8HoursO3.ToString();
        //                        else
        //                            AudRegionHourAQIExists.Recent8HoursO3_IAQI = null;

        //                        if (Recent8HoursO3NT_Value != null)
        //                            AudRegionHourAQIExists.Recent8HoursO3NT = Recent8HoursO3NT_Value.ToString();
        //                        else
        //                            AudRegionHourAQIExists.Recent8HoursO3NT = null;
        //                        if (AQI_Recent8HoursO3NT != null)
        //                            AudRegionHourAQIExists.Recent8HoursO3NT_IAQI = AQI_Recent8HoursO3NT.ToString();
        //                        else
        //                            AudRegionHourAQIExists.Recent8HoursO3NT_IAQI = null;

        //                        if (PM25_Value != null)
        //                            AudRegionHourAQIExists.PM25 = PM25_Value.ToString();
        //                        else
        //                            AudRegionHourAQIExists.PM25 = null;
        //                        if (AQI_PM25 != null)
        //                            AudRegionHourAQIExists.PM25_IAQI = AQI_PM25.ToString();
        //                        else
        //                            AudRegionHourAQIExists.PM25_IAQI = null;

        //                        if (Recent24HoursPM25_Value != null)
        //                            AudRegionHourAQIExists.Recent24HoursPM25 = Recent24HoursPM25_Value.ToString();
        //                        else
        //                            AudRegionHourAQIExists.Recent24HoursPM25 = null;
        //                        if (AQI_Recent24HoursPM25 != null)
        //                            AudRegionHourAQIExists.Recent24HoursPM25_IAQI = AQI_Recent24HoursPM25.ToString();
        //                        else
        //                            AudRegionHourAQIExists.Recent24HoursPM25_IAQI = null;

        //                        string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
        //                        AudRegionHourAQIExists.AQIValue = AQIValue;
        //                        AudRegionHourAQIExists.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");
        //                        if (!string.IsNullOrWhiteSpace(AQIValue))
        //                        {
        //                            AudRegionHourAQIExists.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
        //                            AudRegionHourAQIExists.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
        //                            AudRegionHourAQIExists.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
        //                            AudRegionHourAQIExists.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
        //                            AudRegionHourAQIExists.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
        //                            AudRegionHourAQIExists.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
        //                        }
        //                        else
        //                        {
        //                            AudRegionHourAQIExists.Range = null;
        //                            AudRegionHourAQIExists.RGBValue = null;
        //                            AudRegionHourAQIExists.Class = null;
        //                            AudRegionHourAQIExists.Grade = null;
        //                            AudRegionHourAQIExists.HealthEffect = null;
        //                            AudRegionHourAQIExists.TakeStep = null;
        //                        }
        //                        AudRegionHourAQIExists.UpdateUser = "SystemSync";
        //                        AudRegionHourAQIExists.UpdateDateTime = DateTime.Now;
        //                        #endregion
        //                    }
        //                }
        //                #endregion
        //                MonitorBusinessModel.SaveChanges();

        //                #region 审核日区域AQI
        //                DataTable dtPivotAudRegionDay = d_DAL.GetConPivotAudRegionDayData(m_regionCon.TrimEnd(','), sTime, eTime);
        //                foreach (DataRow dr in dtPivotAudRegionDay.Rows)
        //                {
        //                    #region 数据
        //                    string m_region = dr["CityTypeUid"].ToString();
        //                    DateTime Tstamp = Convert.ToDateTime(dr["DateTime"]);

        //                    //decimal? SO2_Value = null;
        //                    //if (dr["SO2"] != DBNull.Value)
        //                    //    SO2_Value = Convert.ToDecimal(dr["SO2"]);
        //                    //int? SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 24);

        //                    //decimal? NO2_Value = null;
        //                    //if (dr["NO2"] != DBNull.Value)
        //                    //    NO2_Value = Convert.ToDecimal(dr["NO2"]);
        //                    //int? AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 24);

        //                    //decimal? PM10_Value = null;
        //                    //if (dr["PM10"] != DBNull.Value)
        //                    //    PM10_Value = Convert.ToDecimal(dr["PM10"]);
        //                    //int? AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);


        //                    //decimal? CO_Value = null;
        //                    //if (dr["CO"] != DBNull.Value)
        //                    //    CO_Value = Convert.ToDecimal(dr["CO"]);
        //                    //int? AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 24);

        //                    //decimal? MaxOneHourO3_Value = null;
        //                    //if (dr["MaxOneHourO3"] != DBNull.Value)
        //                    //    MaxOneHourO3_Value = Convert.ToDecimal(dr["MaxOneHourO3"]);
        //                    //int? AQI_MaxOneHourO3 = d_AQICalculateService.GetIAQI("a05024", MaxOneHourO3_Value, 1);

        //                    //decimal? Max8HourO3_Value = null;
        //                    //if (dr["Max8HourO3"] != DBNull.Value)
        //                    //    Max8HourO3_Value = Convert.ToDecimal(dr["Max8HourO3"]);
        //                    //int? AQI_Max8HourO3 = d_AQICalculateService.GetIAQI("a05024", Max8HourO3_Value, 8);

        //                    //decimal? PM25_Value = null;
        //                    //if (dr["PM25"] != DBNull.Value)
        //                    //    PM25_Value = Convert.ToDecimal(dr["PM25"]);
        //                    //int? AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);

        //                    decimal? SO2_Value = null;
        //                    int? SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_MaxOneHourO3, AQI_Max8HourO3, AQI_PM25;
        //                    if (dr["SO2"] != DBNull.Value)
        //                    {
        //                        if (Convert.ToDecimal(dr["SO2"]) >= 10000)
        //                        {
        //                            SO2_Value = Convert.ToDecimal(dr["SO2"]) - 10000;
        //                            SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", null, 24);
        //                        }
        //                        else
        //                        {
        //                            SO2_Value = Convert.ToDecimal(dr["SO2"]);
        //                            SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 24);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 24);
        //                    }


        //                    decimal? NO2_Value = null;
        //                    if (dr["NO2"] != DBNull.Value)
        //                    {
        //                        if (Convert.ToDecimal(dr["NO2"]) >= 10000)
        //                        {
        //                            NO2_Value = Convert.ToDecimal(dr["NO2"]) - 10000;
        //                            AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", null, 24);
        //                        }
        //                        else
        //                        {
        //                            NO2_Value = Convert.ToDecimal(dr["NO2"]);
        //                            AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 24);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 24);
        //                    }


        //                    decimal? PM10_Value = null;
        //                    if (dr["PM10"] != DBNull.Value)
        //                    {
        //                        if (Convert.ToDecimal(dr["PM10"]) >= 10000)
        //                        {
        //                            PM10_Value = Convert.ToDecimal(dr["PM10"]) - 10000;
        //                            AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", null, 24);
        //                        }
        //                        else
        //                        {
        //                            PM10_Value = Convert.ToDecimal(dr["PM10"]);
        //                            AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);
        //                    }


        //                    decimal? CO_Value = null;
        //                    if (dr["CO"] != DBNull.Value)
        //                    {
        //                        if (Convert.ToDecimal(dr["CO"]) >= 10000)
        //                        {
        //                            CO_Value = Convert.ToDecimal(dr["CO"]) - 10000;
        //                            AQI_CO = d_AQICalculateService.GetIAQI("a21005", null, 24);
        //                        }
        //                        else
        //                        {
        //                            CO_Value = Convert.ToDecimal(dr["CO"]);
        //                            AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 24);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 24);
        //                    }


        //                    decimal? MaxOneHourO3_Value = null;
        //                    if (dr["MaxOneHourO3"] != DBNull.Value)
        //                    {

        //                        if (Convert.ToDecimal(dr["MaxOneHourO3"]) >= 10000)
        //                        {
        //                            MaxOneHourO3_Value = Convert.ToDecimal(dr["MaxOneHourO3"]) - 10000;
        //                            AQI_MaxOneHourO3 = d_AQICalculateService.GetIAQI("a05024", null, 1);
        //                        }
        //                        else
        //                        {
        //                            MaxOneHourO3_Value = Convert.ToDecimal(dr["MaxOneHourO3"]);
        //                            AQI_MaxOneHourO3 = d_AQICalculateService.GetIAQI("a05024", MaxOneHourO3_Value, 1);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        AQI_MaxOneHourO3 = d_AQICalculateService.GetIAQI("a05024", MaxOneHourO3_Value, 1);
        //                    }


        //                    decimal? Max8HourO3_Value = null;
        //                    if (dr["Max8HourO3"] != DBNull.Value)
        //                    {
        //                        if (Convert.ToDecimal(dr["Max8HourO3"]) >= 10000)
        //                        {
        //                            Max8HourO3_Value = Convert.ToDecimal(dr["Max8HourO3"]) - 10000;
        //                            AQI_Max8HourO3 = d_AQICalculateService.GetIAQI("a05024", null, 8);
        //                        }
        //                        else
        //                        {
        //                            Max8HourO3_Value = Convert.ToDecimal(dr["Max8HourO3"]);
        //                            AQI_Max8HourO3 = d_AQICalculateService.GetIAQI("a05024", Max8HourO3_Value, 8);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        AQI_Max8HourO3 = d_AQICalculateService.GetIAQI("a05024", Max8HourO3_Value, 8);
        //                    }


        //                    decimal? PM25_Value = null;
        //                    if (dr["PM25"] != DBNull.Value)
        //                    {
        //                        if (Convert.ToDecimal(dr["PM25"]) >= 10000)
        //                        {
        //                            PM25_Value = Convert.ToDecimal(dr["PM25"]) - 10000;
        //                            AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", null, 24);
        //                        }
        //                        else
        //                        {
        //                            PM25_Value = Convert.ToDecimal(dr["PM25"]);
        //                            AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);
        //                    }

        //                    #endregion
        //                    TB_RegionDayAQIReport AudRegionDayAQIExists = MonitorBusinessModel.TB_RegionDayAQIReports.Where(p => p.MonitoringRegionUid.Equals(m_region) && p.ReportDateTime.Equals(Tstamp)).FirstOrDefault();
        //                    if (AudRegionDayAQIExists == null)
        //                    {
        //                        #region 新增日AQI
        //                        TB_RegionDayAQIReport NewAudDayAQI = new TB_RegionDayAQIReport();
        //                        NewAudDayAQI.MonitoringRegionUid = m_region;
        //                        NewAudDayAQI.ReportDateTime = Tstamp;
        //                        NewAudDayAQI.StatisticalType = "CG";

        //                        if (SO2_Value != null)
        //                            NewAudDayAQI.SO2 = SO2_Value.ToString();
        //                        if (SO2_IAQI != null)
        //                            NewAudDayAQI.SO2_IAQI = SO2_IAQI.ToString();

        //                        if (NO2_Value != null)
        //                            NewAudDayAQI.NO2 = NO2_Value.ToString();
        //                        if (AQI_NO2 != null)
        //                            NewAudDayAQI.NO2_IAQI = AQI_NO2.ToString();

        //                        if (PM10_Value != null)
        //                            NewAudDayAQI.PM10 = PM10_Value.ToString();
        //                        if (AQI_PM10 != null)
        //                            NewAudDayAQI.PM10_IAQI = AQI_PM10.ToString();

        //                        if (CO_Value != null)
        //                            NewAudDayAQI.CO = CO_Value.ToString();
        //                        if (AQI_CO != null)
        //                            NewAudDayAQI.CO_IAQI = AQI_CO.ToString();

        //                        if (MaxOneHourO3_Value != null)
        //                            NewAudDayAQI.MaxOneHourO3 = MaxOneHourO3_Value.ToString();
        //                        if (AQI_MaxOneHourO3 != null)
        //                            NewAudDayAQI.MaxOneHourO3_IAQI = AQI_MaxOneHourO3.ToString();

        //                        if (Max8HourO3_Value != null)
        //                            NewAudDayAQI.Max8HourO3 = Max8HourO3_Value.ToString();
        //                        if (AQI_Max8HourO3 != null)
        //                            NewAudDayAQI.Max8HourO3_IAQI = AQI_Max8HourO3.ToString();

        //                        if (PM25_Value != null)
        //                            NewAudDayAQI.PM25 = PM25_Value.ToString();
        //                        if (AQI_PM25 != null)
        //                            NewAudDayAQI.PM25_IAQI = AQI_PM25.ToString();

        //                        string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "V");
        //                        NewAudDayAQI.AQIValue = AQIValue;
        //                        NewAudDayAQI.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "N");
        //                        if (!string.IsNullOrWhiteSpace(AQIValue))
        //                        {
        //                            NewAudDayAQI.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
        //                            NewAudDayAQI.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
        //                            NewAudDayAQI.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
        //                            NewAudDayAQI.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
        //                            NewAudDayAQI.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
        //                            NewAudDayAQI.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
        //                        }

        //                        NewAudDayAQI.CreatUser = "SystemSync";
        //                        NewAudDayAQI.CreatDateTime = DateTime.Now;
        //                        MonitorBusinessModel.Add(NewAudDayAQI);
        //                        #endregion
        //                    }
        //                    else
        //                    {
        //                        #region 更新日AQI
        //                        if (SO2_Value != null)
        //                            AudRegionDayAQIExists.SO2 = SO2_Value.ToString();
        //                        else
        //                            AudRegionDayAQIExists.SO2 = null;
        //                        if (SO2_IAQI != null)
        //                            AudRegionDayAQIExists.SO2_IAQI = SO2_IAQI.ToString();
        //                        else
        //                            AudRegionDayAQIExists.SO2_IAQI = null;

        //                        if (NO2_Value != null)
        //                            AudRegionDayAQIExists.NO2 = NO2_Value.ToString();
        //                        else
        //                            AudRegionDayAQIExists.NO2 = null;
        //                        if (AQI_NO2 != null)
        //                            AudRegionDayAQIExists.NO2_IAQI = AQI_NO2.ToString();
        //                        else
        //                            AudRegionDayAQIExists.NO2_IAQI = null;

        //                        if (PM10_Value != null)
        //                            AudRegionDayAQIExists.PM10 = PM10_Value.ToString();
        //                        else
        //                            AudRegionDayAQIExists.PM10 = null;
        //                        if (AQI_PM10 != null)
        //                            AudRegionDayAQIExists.PM10_IAQI = AQI_PM10.ToString();
        //                        else
        //                            AudRegionDayAQIExists.PM10_IAQI = null;

        //                        if (CO_Value != null)
        //                            AudRegionDayAQIExists.CO = CO_Value.ToString();
        //                        else
        //                            AudRegionDayAQIExists.CO = null;
        //                        if (AQI_CO != null)
        //                            AudRegionDayAQIExists.CO_IAQI = AQI_CO.ToString();
        //                        else
        //                            AudRegionDayAQIExists.SO2 = null;

        //                        if (MaxOneHourO3_Value != null)
        //                            AudRegionDayAQIExists.MaxOneHourO3 = MaxOneHourO3_Value.ToString();
        //                        else
        //                            AudRegionDayAQIExists.MaxOneHourO3 = null;
        //                        if (AQI_MaxOneHourO3 != null)
        //                            AudRegionDayAQIExists.MaxOneHourO3_IAQI = AQI_MaxOneHourO3.ToString();
        //                        else
        //                            AudRegionDayAQIExists.MaxOneHourO3_IAQI = null;

        //                        if (Max8HourO3_Value != null)
        //                            AudRegionDayAQIExists.Max8HourO3 = Max8HourO3_Value.ToString();
        //                        else
        //                            AudRegionDayAQIExists.Max8HourO3 = null;
        //                        if (AQI_Max8HourO3 != null)
        //                            AudRegionDayAQIExists.Max8HourO3_IAQI = AQI_Max8HourO3.ToString();
        //                        else
        //                            AudRegionDayAQIExists.Max8HourO3_IAQI = null;

        //                        if (PM25_Value != null)
        //                            AudRegionDayAQIExists.PM25 = PM25_Value.ToString();
        //                        else
        //                            AudRegionDayAQIExists.PM25 = null;
        //                        if (AQI_PM25 != null)
        //                            AudRegionDayAQIExists.PM25_IAQI = AQI_PM25.ToString();
        //                        else
        //                            AudRegionDayAQIExists.PM25_IAQI = null;

        //                        string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "V");
        //                        AudRegionDayAQIExists.AQIValue = AQIValue;
        //                        AudRegionDayAQIExists.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "N");
        //                        if (!string.IsNullOrWhiteSpace(AQIValue))
        //                        {
        //                            AudRegionDayAQIExists.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
        //                            AudRegionDayAQIExists.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
        //                            AudRegionDayAQIExists.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
        //                            AudRegionDayAQIExists.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
        //                            AudRegionDayAQIExists.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEAlTHEFFECT");
        //                            AudRegionDayAQIExists.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
        //                        }
        //                        else
        //                        {
        //                            AudRegionDayAQIExists.Range = null;
        //                            AudRegionDayAQIExists.RGBValue = null;
        //                            AudRegionDayAQIExists.Class = null;
        //                            AudRegionDayAQIExists.Grade = null;
        //                            AudRegionDayAQIExists.HealthEffect = null;
        //                            AudRegionDayAQIExists.TakeStep = null;
        //                        }
        //                        AudRegionDayAQIExists.UpdateUser = "SystemSync";
        //                        AudRegionDayAQIExists.UpdateDateTime = DateTime.Now;

        //                        #endregion
        //                    }
        //                }
        //                #endregion
        //                MonitorBusinessModel.SaveChanges();
        //            //}
        //            #endregion

        //        }
        //        log.Info("-------------------------------------------------------CalculateConBy60审核数据结束----------------------------");
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("--------------------------------------------------------CalculateConBy60异常:" + ex.ToString());
        //    }

        //}
        #endregion
        /// <summary>
        /// 国控四站点小时数据计算小时AQI,日数据日AQI
        /// </summary>
        public void CalculateConBy60(DateTime startTime, DateTime endTime)
        {
            try
            {
                log.Info("-------------------------------------------------------------CalculateConBy60原始数据开始-------------------");
                DateTime sTime = Convert.ToDateTime(startTime.ToString("yyyy-MM-dd 00:00:00"));
                DateTime eTime = Convert.ToDateTime(endTime.ToString("yyyy-MM-dd 23:59:59"));

                using (AirAutoMonitorModel AirAutoMonitorModel = new AirAutoMonitorModel())
                {
                    #region 原始测点AQI
                    //foreach (int PointId in ConPointIds)
                    //{
                    #region 计算原始小时AQI
                    DataTable dtPivotOriHour = d_DAL.GetConPivotOriHourData(ConPointIds, sTime, eTime);
                    foreach (DataRow dr in dtPivotOriHour.Rows)
                    {
                        #region 数据
                        int? PointId = Convert.ToInt32(dr["PointId"]);
                        DateTime Tstamp = Convert.ToDateTime(dr["Tstamp"]);
                        decimal? SO2_Value = null;
                        if (dr["SO2"] != DBNull.Value)
                            SO2_Value = Convert.ToDecimal(dr["SO2"]);
                        int? SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 1);

                        decimal? NO2_Value = null;
                        if (dr["NO2"] != DBNull.Value)
                            NO2_Value = Convert.ToDecimal(dr["NO2"]);
                        int? AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 1);

                        decimal? PM10_Value = null;
                        if (dr["PM10"] != DBNull.Value)
                            PM10_Value = Convert.ToDecimal(dr["PM10"]);
                        int? AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);

                        decimal? Recent24HoursPM10_Value = null;
                        if (dr["Recent24HoursPM10"] != DBNull.Value)
                            Recent24HoursPM10_Value = Convert.ToDecimal(dr["Recent24HoursPM10"]);
                        int? AQI_Recent24HoursPM10 = d_AQICalculateService.GetIAQI("a34002", Recent24HoursPM10_Value, 24);

                        decimal? CO_Value = null;
                        //先修约再计算AQI
                        if (dr["CO"] != DBNull.Value)
                        {
                            DataTable dtFactor = d_AQICalculateService.GetPollutantUnit("a21005");
                            int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);
                            if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m3"))
                            {
                                CO_Value = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO"]), decimalNum) * 1000);

                            }
                            else
                            {
                                CO_Value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO"]), decimalNum);
                            }
                            //CO_Value = Convert.ToDecimal(dr["CO"]);
                        }
                        int? AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 1);

                        decimal? O3_Value = null;
                        if (dr["O3"] != DBNull.Value)
                            O3_Value = Convert.ToDecimal(dr["O3"]);
                        int? AQI_O3 = d_AQICalculateService.GetIAQI("a05024", O3_Value, 1);

                        decimal? Recent8HoursO3_Value = null;
                        if (dr["Recent8HoursO3"] != DBNull.Value)
                            Recent8HoursO3_Value = Convert.ToDecimal(dr["Recent8HoursO3"]);
                        int? AQI_Recent8HoursO3 = d_AQICalculateService.GetIAQI("a05024", Recent8HoursO3_Value, 8);

                        decimal? Recent8HoursO3NT_Value = null;
                        if (dr["Recent8HoursO3NT"] != DBNull.Value)
                            Recent8HoursO3NT_Value = Convert.ToDecimal(dr["Recent8HoursO3NT"]);
                        int? AQI_Recent8HoursO3NT = d_AQICalculateService.GetIAQI("a05024", Recent8HoursO3NT_Value, 8);

                        decimal? PM25_Value = null;
                        if (dr["PM25"] != DBNull.Value)
                            PM25_Value = Convert.ToDecimal(dr["PM25"]);
                        int? AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);

                        decimal? Recent24HoursPM25_Value = null;
                        if (dr["Recent24HoursPM25"] != DBNull.Value)
                            Recent24HoursPM25_Value = Convert.ToDecimal(dr["Recent24HoursPM25"]);
                        int? AQI_Recent24HoursPM25 = d_AQICalculateService.GetIAQI("a34004", Recent24HoursPM25_Value, 24);
                        #endregion
                        TB_OriHourAQI OriHourAQIExists = AirAutoMonitorModel.TB_OriHourAQIs.Where(p => p.PointId.Equals(PointId) && p.DateTime.Equals(Tstamp)).FirstOrDefault();
                        //TB_OriHourAQI OriHourAQIExists = AirAutoMonitorModel.TB_OriHourAQIs.Where(p => ConPointIds.Contains(p.PointId.ToString()) && p.DateTime.Equals(Tstamp)).FirstOrDefault();
                        if (OriHourAQIExists == null)
                        {
                            #region 新增小时AQI
                            TB_OriHourAQI NewOriHourAQI = new TB_OriHourAQI();
                            NewOriHourAQI.PointId = PointId;
                            NewOriHourAQI.DateTime = Tstamp;

                            if (SO2_Value != null)
                                NewOriHourAQI.SO2 = SO2_Value.ToString();
                            if (SO2_IAQI != null)
                                NewOriHourAQI.SO2_IAQI = SO2_IAQI.ToString();

                            if (NO2_Value != null)
                                NewOriHourAQI.NO2 = NO2_Value.ToString();
                            if (NO2_Value != null)
                                NewOriHourAQI.NO2_IAQI = AQI_NO2.ToString();

                            if (PM10_Value != null)
                                NewOriHourAQI.PM10 = PM10_Value.ToString();
                            if (AQI_PM10 != null)
                                NewOriHourAQI.PM10_IAQI = AQI_PM10.ToString();

                            if (Recent24HoursPM10_Value != null)
                                NewOriHourAQI.Recent24HoursPM10 = Recent24HoursPM10_Value.ToString();
                            if (AQI_Recent24HoursPM10 != null)
                                NewOriHourAQI.Recent24HoursPM10_IAQI = AQI_Recent24HoursPM10.ToString();

                            if (CO_Value != null)
                                NewOriHourAQI.CO = CO_Value.ToString();
                            if (AQI_CO != null)
                                NewOriHourAQI.CO_IAQI = AQI_CO.ToString();

                            if (O3_Value != null)
                                NewOriHourAQI.O3 = O3_Value.ToString();
                            if (AQI_O3 != null)
                                NewOriHourAQI.O3_IAQI = AQI_O3.ToString();

                            if (Recent8HoursO3_Value != null)
                                NewOriHourAQI.Recent8HoursO3 = Recent8HoursO3_Value.ToString();
                            if (AQI_Recent8HoursO3 != null)
                                NewOriHourAQI.Recent8HoursO3_IAQI = AQI_Recent8HoursO3.ToString();

                            if (Recent8HoursO3NT_Value != null)
                                NewOriHourAQI.Recent8HoursO3NT = Recent8HoursO3NT_Value.ToString();
                            if (AQI_Recent8HoursO3NT != null)
                                NewOriHourAQI.Recent8HoursO3NT_IAQI = AQI_Recent8HoursO3NT.ToString();

                            if (PM25_Value != null)
                                NewOriHourAQI.PM25 = PM25_Value.ToString();
                            if (AQI_PM25 != null)
                                NewOriHourAQI.PM25_IAQI = AQI_PM25.ToString();

                            if (Recent24HoursPM25_Value != null)
                                NewOriHourAQI.Recent24HoursPM25 = Recent24HoursPM25_Value.ToString();
                            if (AQI_Recent24HoursPM25 != null)
                                NewOriHourAQI.Recent24HoursPM25_IAQI = AQI_Recent24HoursPM25.ToString();

                            string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
                            NewOriHourAQI.AQIValue = AQIValue;
                            NewOriHourAQI.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");
                            if (!string.IsNullOrWhiteSpace(AQIValue))
                            {
                                NewOriHourAQI.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                NewOriHourAQI.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                NewOriHourAQI.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                NewOriHourAQI.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                NewOriHourAQI.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
                                NewOriHourAQI.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                            }
                            NewOriHourAQI.CreatUser = "SystemSync";
                            NewOriHourAQI.CreatDateTime = DateTime.Now;
                            AirAutoMonitorModel.Add(NewOriHourAQI);
                            //AirAutoMonitorModel.SaveChanges();
                            #endregion
                        }
                        else
                        {
                            #region 更新小时AQI
                            if (SO2_Value != null)
                                OriHourAQIExists.SO2 = SO2_Value.ToString();
                            else
                                OriHourAQIExists.SO2 = null;
                            if (SO2_IAQI != null)
                                OriHourAQIExists.SO2_IAQI = SO2_IAQI.ToString();
                            else
                                OriHourAQIExists.SO2_IAQI = null;
                            if (NO2_Value != null)
                                OriHourAQIExists.NO2 = NO2_Value.ToString();
                            else
                                OriHourAQIExists.NO2 = null;
                            if (AQI_NO2 != null)
                                OriHourAQIExists.NO2_IAQI = AQI_NO2.ToString();
                            else
                                OriHourAQIExists.NO2_IAQI = null;

                            if (PM10_Value != null)
                                OriHourAQIExists.PM10 = PM10_Value.ToString();
                            else
                                OriHourAQIExists.PM10 = null;
                            if (AQI_PM10 != null)
                                OriHourAQIExists.PM10_IAQI = AQI_PM10.ToString();
                            else
                                OriHourAQIExists.PM10_IAQI = null;

                            if (Recent24HoursPM10_Value != null)
                                OriHourAQIExists.Recent24HoursPM10 = Recent24HoursPM10_Value.ToString();
                            else
                                OriHourAQIExists.Recent24HoursPM10 = null;
                            if (AQI_Recent24HoursPM10 != null)
                                OriHourAQIExists.Recent24HoursPM10_IAQI = AQI_Recent24HoursPM10.ToString();
                            else
                                OriHourAQIExists.Recent24HoursPM10_IAQI = null;

                            if (CO_Value != null)
                                OriHourAQIExists.CO = CO_Value.ToString();
                            else
                                OriHourAQIExists.CO = null;
                            if (AQI_CO != null)
                                OriHourAQIExists.CO_IAQI = AQI_CO.ToString();
                            else
                                OriHourAQIExists.CO_IAQI = null;

                            if (O3_Value != null)
                                OriHourAQIExists.O3 = O3_Value.ToString();
                            else
                                OriHourAQIExists.O3 = null;
                            if (AQI_O3 != null)
                                OriHourAQIExists.O3_IAQI = AQI_O3.ToString();
                            else
                                OriHourAQIExists.O3_IAQI = null;

                            if (Recent8HoursO3_Value != null)
                                OriHourAQIExists.Recent8HoursO3 = Recent8HoursO3_Value.ToString();
                            else
                                OriHourAQIExists.Recent8HoursO3 = null;
                            if (AQI_Recent8HoursO3 != null)
                                OriHourAQIExists.Recent8HoursO3_IAQI = AQI_Recent8HoursO3.ToString();
                            else
                                OriHourAQIExists.Recent8HoursO3_IAQI = null;

                            if (Recent8HoursO3NT_Value != null)
                                OriHourAQIExists.Recent8HoursO3NT = Recent8HoursO3NT_Value.ToString();
                            else
                                OriHourAQIExists.Recent8HoursO3NT = null;
                            if (AQI_Recent8HoursO3NT != null)
                                OriHourAQIExists.Recent8HoursO3NT_IAQI = AQI_Recent8HoursO3NT.ToString();
                            else
                                OriHourAQIExists.Recent8HoursO3NT_IAQI = null;

                            if (PM25_Value != null)
                                OriHourAQIExists.PM25 = PM25_Value.ToString();
                            else
                                OriHourAQIExists.PM25 = null;
                            if (AQI_PM25 != null)
                                OriHourAQIExists.PM25_IAQI = AQI_PM25.ToString();
                            else
                                OriHourAQIExists.PM25_IAQI = null;

                            if (Recent24HoursPM25_Value != null)
                                OriHourAQIExists.Recent24HoursPM25 = Recent24HoursPM25_Value.ToString();
                            else
                                OriHourAQIExists.Recent24HoursPM25 = null;
                            if (AQI_Recent24HoursPM25 != null)
                                OriHourAQIExists.Recent24HoursPM25_IAQI = AQI_Recent24HoursPM25.ToString();
                            else
                                OriHourAQIExists.Recent24HoursPM25_IAQI = null;

                            string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
                            OriHourAQIExists.AQIValue = AQIValue;
                            OriHourAQIExists.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");
                            if (!string.IsNullOrWhiteSpace(AQIValue))
                            {
                                OriHourAQIExists.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                OriHourAQIExists.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                OriHourAQIExists.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                OriHourAQIExists.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                OriHourAQIExists.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
                                OriHourAQIExists.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                            }
                            else
                            {
                                OriHourAQIExists.Range = null;
                                OriHourAQIExists.RGBValue = null;
                                OriHourAQIExists.Class = null;
                                OriHourAQIExists.Grade = null;
                                OriHourAQIExists.HealthEffect = null;
                                OriHourAQIExists.TakeStep = null;
                            }
                            OriHourAQIExists.UpdateUser = "SystemSync";
                            OriHourAQIExists.UpdateDateTime = DateTime.Now;
                            //AirAutoMonitorModel.SaveChanges();
                            #endregion
                        }
                    }
                    #endregion
                    AirAutoMonitorModel.SaveChanges();

                    //计算生成原始日数据
                    d_DAL.AddConOriginalDayData(sTime, eTime, ConPointIds);


                    #endregion

                    #region 原始区域AQI
                    string m_regionCon = string.Empty;
                    foreach (string str in m_regions)
                    {
                        m_regionCon += "'" + str + "',";
                    }
                    //foreach (string m_region in m_regions)
                    //{
                    #region 原始小时区域AQI
                    DataTable dtPivotOriRegionHour = d_DAL.GetConPivotOriRegionHourData(m_regionCon.TrimEnd(','), sTime, eTime);
                    foreach (DataRow dr in dtPivotOriRegionHour.Rows)
                    {
                        #region 数据
                        string m_region = dr["CityTypeUid"].ToString();
                        DateTime Tstamp = Convert.ToDateTime(dr["DateTime"]);
                        decimal? SO2_Value = null;
                        if (dr["SO2"] != DBNull.Value)
                            SO2_Value = Convert.ToDecimal(dr["SO2"]);
                        int? SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 1);

                        decimal? NO2_Value = null;
                        if (dr["NO2"] != DBNull.Value)
                            NO2_Value = Convert.ToDecimal(dr["NO2"]);
                        int? AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 1);

                        decimal? PM10_Value = null;
                        if (dr["PM10"] != DBNull.Value)
                            PM10_Value = Convert.ToDecimal(dr["PM10"]);
                        int? AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);

                        decimal? Recent24HoursPM10_Value = null;
                        if (dr["Recent24HoursPM10"] != DBNull.Value)
                            Recent24HoursPM10_Value = Convert.ToDecimal(dr["Recent24HoursPM10"]);
                        int? AQI_Recent24HoursPM10 = d_AQICalculateService.GetIAQI("a34002", Recent24HoursPM10_Value, 24);

                        decimal? CO_Value = null;
                        //先修约再计算AQI
                        if (dr["CO"] != DBNull.Value)
                        {
                            DataTable dtFactor = d_AQICalculateService.GetPollutantUnit("a21005");
                            int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);
                            if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m3"))
                            {
                                CO_Value = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO"]), decimalNum) * 1000);

                            }
                            else
                            {
                                CO_Value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO"]), decimalNum);
                            }
                        }
                        int? AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 1);

                        decimal? O3_Value = null;
                        if (dr["O3"] != DBNull.Value)
                            O3_Value = Convert.ToDecimal(dr["O3"]);
                        int? AQI_O3 = d_AQICalculateService.GetIAQI("a05024", O3_Value, 1);

                        decimal? Recent8HoursO3_Value = null;
                        if (dr["Recent8HoursO3"] != DBNull.Value)
                            Recent8HoursO3_Value = Convert.ToDecimal(dr["Recent8HoursO3"]);
                        int? AQI_Recent8HoursO3 = d_AQICalculateService.GetIAQI("a05024", Recent8HoursO3_Value, 8);

                        decimal? Recent8HoursO3NT_Value = null;
                        if (dr["Recent8HoursO3NT"] != DBNull.Value)
                            Recent8HoursO3NT_Value = Convert.ToDecimal(dr["Recent8HoursO3NT"]);
                        int? AQI_Recent8HoursO3NT = d_AQICalculateService.GetIAQI("a05024", Recent8HoursO3NT_Value, 8);

                        decimal? PM25_Value = null;
                        if (dr["PM25"] != DBNull.Value)
                            PM25_Value = Convert.ToDecimal(dr["PM25"]);
                        int? AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);

                        decimal? Recent24HoursPM25_Value = null;
                        if (dr["Recent24HoursPM25"] != DBNull.Value)
                            Recent24HoursPM25_Value = Convert.ToDecimal(dr["Recent24HoursPM25"]);
                        int? AQI_Recent24HoursPM25 = d_AQICalculateService.GetIAQI("a34004", Recent24HoursPM25_Value, 24);
                        #endregion
                        TB_OriRegionHourAQI OriRegionHourAQIExists = AirAutoMonitorModel.TB_OriRegionHourAQIs.Where(p => p.MonitoringRegionUid.Equals(m_region) && p.DateTime.Equals(Tstamp)).FirstOrDefault();
                        if (OriRegionHourAQIExists == null)
                        {
                            #region 新增小时AQI
                            TB_OriRegionHourAQI NewOriHourAQI = new TB_OriRegionHourAQI();
                            NewOriHourAQI.MonitoringRegionUid = m_region;
                            NewOriHourAQI.DateTime = Tstamp;
                            NewOriHourAQI.StatisticalType = "CG";

                            if (SO2_Value != null)
                                NewOriHourAQI.SO2 = SO2_Value.ToString();
                            if (SO2_IAQI != null)
                                NewOriHourAQI.SO2_IAQI = SO2_IAQI.ToString();

                            if (NO2_Value != null)
                                NewOriHourAQI.NO2 = NO2_Value.ToString();
                            if (AQI_NO2 != null)
                                NewOriHourAQI.NO2_IAQI = AQI_NO2.ToString();

                            if (PM10_Value != null)
                                NewOriHourAQI.PM10 = PM10_Value.ToString();
                            if (AQI_PM10 != null)
                                NewOriHourAQI.PM10_IAQI = AQI_PM10.ToString();

                            if (Recent24HoursPM10_Value != null)
                                NewOriHourAQI.Recent24HoursPM10 = Recent24HoursPM10_Value.ToString();
                            if (AQI_Recent24HoursPM10 != null)
                                NewOriHourAQI.Recent24HoursPM10_IAQI = AQI_Recent24HoursPM10.ToString();

                            if (CO_Value != null)
                                NewOriHourAQI.CO = CO_Value.ToString();
                            if (AQI_CO != null)
                                NewOriHourAQI.CO_IAQI = AQI_CO.ToString();

                            if (O3_Value != null)
                                NewOriHourAQI.O3 = O3_Value.ToString();
                            if (AQI_O3 != null)
                                NewOriHourAQI.O3_IAQI = AQI_O3.ToString();

                            if (Recent8HoursO3_Value != null)
                                NewOriHourAQI.Recent8HoursO3 = Recent8HoursO3_Value.ToString();
                            if (AQI_Recent8HoursO3 != null)
                                NewOriHourAQI.Recent8HoursO3_IAQI = AQI_Recent8HoursO3.ToString();

                            if (Recent8HoursO3NT_Value != null)
                                NewOriHourAQI.Recent8HoursO3NT = Recent8HoursO3NT_Value.ToString();
                            if (AQI_Recent8HoursO3NT != null)
                                NewOriHourAQI.Recent8HoursO3NT_IAQI = AQI_Recent8HoursO3NT.ToString();

                            if (PM25_Value != null)
                                NewOriHourAQI.PM25 = PM25_Value.ToString();
                            if (AQI_PM25 != null)
                                NewOriHourAQI.PM25_IAQI = AQI_PM25.ToString();

                            if (Recent24HoursPM25_Value != null)
                                NewOriHourAQI.Recent24HoursPM25 = Recent24HoursPM25_Value.ToString();
                            if (AQI_Recent24HoursPM25 != null)
                                NewOriHourAQI.Recent24HoursPM25_IAQI = AQI_Recent24HoursPM25.ToString();

                            string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
                            NewOriHourAQI.AQIValue = AQIValue;
                            NewOriHourAQI.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");
                            if (!string.IsNullOrWhiteSpace(AQIValue))
                            {
                                NewOriHourAQI.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                NewOriHourAQI.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                NewOriHourAQI.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                NewOriHourAQI.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                NewOriHourAQI.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
                                NewOriHourAQI.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                            }
                            NewOriHourAQI.CreatUser = "SystemSync";
                            NewOriHourAQI.CreatDateTime = DateTime.Now;
                            AirAutoMonitorModel.Add(NewOriHourAQI);
                            //AirAutoMonitorModel.SaveChanges();
                            #endregion
                        }
                        else
                        {
                            #region 更新小时AQI
                            if (SO2_Value != null)
                                OriRegionHourAQIExists.SO2 = SO2_Value.ToString();
                            else
                                OriRegionHourAQIExists.SO2 = null;
                            if (SO2_IAQI != null)
                                OriRegionHourAQIExists.SO2_IAQI = SO2_IAQI.ToString();
                            else
                                OriRegionHourAQIExists.SO2_IAQI = null;

                            if (NO2_Value != null)
                                OriRegionHourAQIExists.NO2 = NO2_Value.ToString();
                            else
                                OriRegionHourAQIExists.NO2 = null;
                            if (AQI_NO2 != null)
                                OriRegionHourAQIExists.NO2_IAQI = AQI_NO2.ToString();
                            else
                                OriRegionHourAQIExists.NO2_IAQI = null;

                            if (PM10_Value != null)
                                OriRegionHourAQIExists.PM10 = PM10_Value.ToString();
                            else
                                OriRegionHourAQIExists.PM10 = null;
                            if (AQI_PM10 != null)
                                OriRegionHourAQIExists.PM10_IAQI = AQI_PM10.ToString();
                            else
                                OriRegionHourAQIExists.PM10_IAQI = null;

                            if (Recent24HoursPM10_Value != null)
                                OriRegionHourAQIExists.Recent24HoursPM10 = Recent24HoursPM10_Value.ToString();
                            else
                                OriRegionHourAQIExists.Recent24HoursPM10 = null;
                            if (AQI_Recent24HoursPM10 != null)
                                OriRegionHourAQIExists.Recent24HoursPM10_IAQI = AQI_Recent24HoursPM10.ToString();
                            else
                                OriRegionHourAQIExists.Recent24HoursPM10_IAQI = null;

                            if (CO_Value != null)
                                OriRegionHourAQIExists.CO = CO_Value.ToString();
                            else
                                OriRegionHourAQIExists.CO = null;
                            if (AQI_CO != null)
                                OriRegionHourAQIExists.CO_IAQI = AQI_CO.ToString();
                            else
                                OriRegionHourAQIExists.CO_IAQI = null;

                            if (O3_Value != null)
                                OriRegionHourAQIExists.O3 = O3_Value.ToString();
                            else
                                OriRegionHourAQIExists.O3 = null;
                            if (AQI_O3 != null)
                                OriRegionHourAQIExists.O3_IAQI = AQI_O3.ToString();
                            else
                                OriRegionHourAQIExists.O3_IAQI = null;

                            if (Recent8HoursO3_Value != null)
                                OriRegionHourAQIExists.Recent8HoursO3 = Recent8HoursO3_Value.ToString();
                            else
                                OriRegionHourAQIExists.Recent8HoursO3 = null;
                            if (AQI_Recent8HoursO3 != null)
                                OriRegionHourAQIExists.Recent8HoursO3_IAQI = AQI_Recent8HoursO3.ToString();
                            else
                                OriRegionHourAQIExists.Recent8HoursO3_IAQI = null;

                            if (Recent8HoursO3NT_Value != null)
                                OriRegionHourAQIExists.Recent8HoursO3NT = Recent8HoursO3NT_Value.ToString();
                            else
                                OriRegionHourAQIExists.Recent8HoursO3NT = null;
                            if (AQI_Recent8HoursO3NT != null)
                                OriRegionHourAQIExists.Recent8HoursO3NT_IAQI = AQI_Recent8HoursO3NT.ToString();
                            else
                                OriRegionHourAQIExists.Recent8HoursO3NT_IAQI = null;

                            if (PM25_Value != null)
                                OriRegionHourAQIExists.PM25 = PM25_Value.ToString();
                            else
                                OriRegionHourAQIExists.PM25 = null;
                            if (AQI_PM25 != null)
                                OriRegionHourAQIExists.PM25_IAQI = AQI_PM25.ToString();
                            else
                                OriRegionHourAQIExists.PM25_IAQI = null;

                            if (Recent24HoursPM25_Value != null)
                                OriRegionHourAQIExists.Recent24HoursPM25 = Recent24HoursPM25_Value.ToString();
                            else
                                OriRegionHourAQIExists.Recent24HoursPM25 = null;
                            if (AQI_Recent24HoursPM25 != null)
                                OriRegionHourAQIExists.Recent24HoursPM25_IAQI = AQI_Recent24HoursPM25.ToString();
                            else
                                OriRegionHourAQIExists.Recent24HoursPM25_IAQI = null;

                            string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
                            OriRegionHourAQIExists.AQIValue = AQIValue;
                            OriRegionHourAQIExists.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");
                            if (!string.IsNullOrWhiteSpace(AQIValue))
                            {
                                OriRegionHourAQIExists.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                OriRegionHourAQIExists.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                OriRegionHourAQIExists.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                OriRegionHourAQIExists.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                OriRegionHourAQIExists.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
                                OriRegionHourAQIExists.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                            }
                            else
                            {
                                OriRegionHourAQIExists.Range = null;
                                OriRegionHourAQIExists.RGBValue = null;
                                OriRegionHourAQIExists.Class = null;
                                OriRegionHourAQIExists.Grade = null;
                                OriRegionHourAQIExists.HealthEffect = null;
                                OriRegionHourAQIExists.TakeStep = null;
                            }
                            OriRegionHourAQIExists.UpdateUser = "SystemSync";
                            OriRegionHourAQIExists.UpdateDateTime = DateTime.Now;
                            //AirAutoMonitorModel.SaveChanges();
                            #endregion
                        }
                    }
                    #endregion
                    AirAutoMonitorModel.SaveChanges();

                    #region 原始日区域AQI
                    DataTable dtPivotOriRegionDay = d_DAL.GetConPivotOriRegionDayData(m_regionCon.TrimEnd(','), sTime, eTime);
                    foreach (DataRow dr in dtPivotOriRegionDay.Rows)
                    {
                        #region 数据
                        string m_region = dr["CityTypeUid"].ToString();
                        DateTime Tstamp = Convert.ToDateTime(dr["DateTime"]);
                        decimal? SO2_Value = null;
                        if (dr["SO2"] != DBNull.Value)
                            SO2_Value = Convert.ToDecimal(dr["SO2"]);
                        int? SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 24);

                        decimal? NO2_Value = null;
                        if (dr["NO2"] != DBNull.Value)
                            NO2_Value = Convert.ToDecimal(dr["NO2"]);
                        int? AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 24);

                        decimal? PM10_Value = null;
                        if (dr["PM10"] != DBNull.Value)
                            PM10_Value = Convert.ToDecimal(dr["PM10"]);
                        int? AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);


                        decimal? CO_Value = null;
                        if (dr["CO"] != DBNull.Value)
                            CO_Value = Convert.ToDecimal(dr["CO"]);
                        int? AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 24);

                        decimal? MaxOneHourO3_Value = null;
                        if (dr["MaxOneHourO3"] != DBNull.Value)
                            MaxOneHourO3_Value = Convert.ToDecimal(dr["MaxOneHourO3"]);
                        int? AQI_MaxOneHourO3 = d_AQICalculateService.GetIAQI("a05024", MaxOneHourO3_Value, 1);

                        decimal? Max8HourO3_Value = null;
                        if (dr["Max8HourO3"] != DBNull.Value)
                            Max8HourO3_Value = Convert.ToDecimal(dr["Max8HourO3"]);
                        int? AQI_Max8HourO3 = d_AQICalculateService.GetIAQI("a05024", Max8HourO3_Value, 8);

                        decimal? PM25_Value = null;
                        if (dr["PM25"] != DBNull.Value)
                            PM25_Value = Convert.ToDecimal(dr["PM25"]);
                        int? AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);

                        #endregion
                        TB_OriRegionDayAQIReport OriRegionDayAQIExists = AirAutoMonitorModel.TB_OriRegionDayAQIReports.Where(p => p.MonitoringRegionUid.Equals(m_region) && p.ReportDateTime.Equals(Tstamp)).FirstOrDefault();
                        if (OriRegionDayAQIExists == null)
                        {
                            #region 新增日AQI
                            TB_OriRegionDayAQIReport NewOriDayAQI = new TB_OriRegionDayAQIReport();
                            NewOriDayAQI.MonitoringRegionUid = m_region;
                            NewOriDayAQI.ReportDateTime = Tstamp;
                            NewOriDayAQI.StatisticalType = "CG";

                            if (SO2_Value != null)
                                NewOriDayAQI.SO2 = SO2_Value.ToString();
                            if (SO2_IAQI != null)
                                NewOriDayAQI.SO2_IAQI = SO2_IAQI.ToString();

                            if (NO2_Value != null)
                                NewOriDayAQI.NO2 = NO2_Value.ToString();
                            if (AQI_NO2 != null)
                                NewOriDayAQI.NO2_IAQI = AQI_NO2.ToString();

                            if (PM10_Value != null)
                                NewOriDayAQI.PM10 = PM10_Value.ToString();
                            if (AQI_PM10 != null)
                                NewOriDayAQI.PM10_IAQI = AQI_PM10.ToString();

                            if (CO_Value != null)
                                NewOriDayAQI.CO = CO_Value.ToString();
                            if (AQI_CO != null)
                                NewOriDayAQI.CO_IAQI = AQI_CO.ToString();

                            if (MaxOneHourO3_Value != null)
                                NewOriDayAQI.MaxOneHourO3 = MaxOneHourO3_Value.ToString();
                            if (AQI_MaxOneHourO3 != null)
                                NewOriDayAQI.MaxOneHourO3_IAQI = AQI_MaxOneHourO3.ToString();

                            if (Max8HourO3_Value != null)
                                NewOriDayAQI.Max8HourO3 = Max8HourO3_Value.ToString();
                            if (AQI_Max8HourO3 != null)
                                NewOriDayAQI.Max8HourO3_IAQI = AQI_Max8HourO3.ToString();

                            if (PM25_Value != null)
                                NewOriDayAQI.PM25 = PM25_Value.ToString();
                            if (AQI_PM25 != null)
                                NewOriDayAQI.PM25_IAQI = AQI_PM25.ToString();

                            string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "V");
                            NewOriDayAQI.AQIValue = AQIValue;
                            NewOriDayAQI.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "N");
                            if (!string.IsNullOrWhiteSpace(AQIValue))
                            {
                                NewOriDayAQI.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                NewOriDayAQI.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                NewOriDayAQI.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                NewOriDayAQI.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                NewOriDayAQI.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
                                NewOriDayAQI.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                            }
                            NewOriDayAQI.CreatUser = "SystemSync";
                            NewOriDayAQI.CreatDateTime = DateTime.Now;
                            AirAutoMonitorModel.Add(NewOriDayAQI);
                            //AirAutoMonitorModel.SaveChanges();
                            #endregion
                        }
                        else
                        {
                            #region 更新日AQI
                            if (SO2_Value != null)
                                OriRegionDayAQIExists.SO2 = SO2_Value.ToString();
                            else
                                OriRegionDayAQIExists.SO2 = null;
                            if (SO2_IAQI != null)
                                OriRegionDayAQIExists.SO2_IAQI = SO2_IAQI.ToString();
                            else
                                OriRegionDayAQIExists.SO2_IAQI = null;

                            if (NO2_Value != null)
                                OriRegionDayAQIExists.NO2 = NO2_Value.ToString();
                            else
                                OriRegionDayAQIExists.NO2 = null;
                            if (AQI_NO2 != null)
                                OriRegionDayAQIExists.NO2_IAQI = AQI_NO2.ToString();
                            else
                                OriRegionDayAQIExists.NO2_IAQI = null;

                            if (PM10_Value != null)
                                OriRegionDayAQIExists.PM10 = PM10_Value.ToString();
                            else
                                OriRegionDayAQIExists.PM10 = null;
                            if (AQI_PM10 != null)
                                OriRegionDayAQIExists.PM10_IAQI = AQI_PM10.ToString();
                            else
                                OriRegionDayAQIExists.PM10_IAQI = null;

                            if (CO_Value != null)
                                OriRegionDayAQIExists.CO = CO_Value.ToString();
                            else
                                OriRegionDayAQIExists.CO = null;
                            if (AQI_CO != null)
                                OriRegionDayAQIExists.CO_IAQI = AQI_CO.ToString();
                            else
                                OriRegionDayAQIExists.CO_IAQI = null;

                            if (MaxOneHourO3_Value != null)
                                OriRegionDayAQIExists.MaxOneHourO3 = MaxOneHourO3_Value.ToString();
                            else
                                OriRegionDayAQIExists.MaxOneHourO3 = null;
                            if (AQI_MaxOneHourO3 != null)
                                OriRegionDayAQIExists.MaxOneHourO3_IAQI = AQI_MaxOneHourO3.ToString();
                            else
                                OriRegionDayAQIExists.MaxOneHourO3_IAQI = null;

                            if (Max8HourO3_Value != null)
                                OriRegionDayAQIExists.Max8HourO3 = Max8HourO3_Value.ToString();
                            else
                                OriRegionDayAQIExists.Max8HourO3 = null;
                            if (AQI_Max8HourO3 != null)
                                OriRegionDayAQIExists.Max8HourO3_IAQI = AQI_Max8HourO3.ToString();
                            else
                                OriRegionDayAQIExists.Max8HourO3_IAQI = null;

                            if (PM25_Value != null)
                                OriRegionDayAQIExists.PM25 = PM25_Value.ToString();
                            else
                                OriRegionDayAQIExists.PM25 = null;
                            if (AQI_PM25 != null)
                                OriRegionDayAQIExists.PM25_IAQI = AQI_PM25.ToString();
                            else
                                OriRegionDayAQIExists.PM25_IAQI = null;

                            string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "V");
                            OriRegionDayAQIExists.AQIValue = AQIValue;
                            OriRegionDayAQIExists.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "N");
                            if (!string.IsNullOrWhiteSpace(AQIValue))
                            {
                                OriRegionDayAQIExists.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                OriRegionDayAQIExists.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                OriRegionDayAQIExists.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                OriRegionDayAQIExists.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                OriRegionDayAQIExists.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEAlTHEFFECT");
                                OriRegionDayAQIExists.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                            }
                            else
                            {
                                OriRegionDayAQIExists.Range = null;
                                OriRegionDayAQIExists.RGBValue = null;
                                OriRegionDayAQIExists.Class = null;
                                OriRegionDayAQIExists.Grade = null;
                                OriRegionDayAQIExists.HealthEffect = null;
                                OriRegionDayAQIExists.TakeStep = null;
                            }
                            OriRegionDayAQIExists.UpdateUser = "SystemSync";
                            OriRegionDayAQIExists.UpdateDateTime = DateTime.Now;
                            //AirAutoMonitorModel.SaveChanges();
                            #endregion
                        }
                    }
                    #endregion
                    AirAutoMonitorModel.SaveChanges();
                    //}
                    #endregion
                }
                log.Info("-------------------------------------------------------CalculateConBy60原始数据结束----------------------------");
                #region
                log.Info("-------------------------------------------------------CalculateConBy60审核数据开始----------------------------");
                using (MonitorBusinessModel MonitorBusinessModel = new MonitorBusinessModel())
                {
                    #region 审核测点AQI
                    //foreach (int PointId in PointIds)
                    //{
                    #region 计算审核小时AQI
                    DataTable dtPivotAudHour = d_DAL.GetConPivotAudHourData(ConPointIds, sTime, eTime);
                    foreach (DataRow dr in dtPivotAudHour.Rows)
                    {
                        #region 数据
                        int? PointId = Convert.ToInt32(dr["PointId"]);
                        DateTime Tstamp = Convert.ToDateTime(dr["Tstamp"]);
                        decimal? SO2_Value = null;
                        if (dr["SO2"] != DBNull.Value)
                            SO2_Value = Convert.ToDecimal(dr["SO2"]);
                        int? SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 1);

                        decimal? NO2_Value = null;
                        if (dr["NO2"] != DBNull.Value)
                            NO2_Value = Convert.ToDecimal(dr["NO2"]);
                        int? AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 1);

                        decimal? PM10_Value = null;
                        if (dr["PM10"] != DBNull.Value)
                            PM10_Value = Convert.ToDecimal(dr["PM10"]);
                        int? AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);

                        decimal? Recent24HoursPM10_Value = null;
                        if (dr["Recent24HoursPM10"] != DBNull.Value)
                            Recent24HoursPM10_Value = Convert.ToDecimal(dr["Recent24HoursPM10"]);
                        int? AQI_Recent24HoursPM10 = d_AQICalculateService.GetIAQI("a34002", Recent24HoursPM10_Value, 24);

                        decimal? CO_Value = null;
                        //先修约再计算AQI
                        if (dr["CO"] != DBNull.Value)
                        {
                            DataTable dtFactor = d_AQICalculateService.GetPollutantUnit("a21005");
                            int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);
                            if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m3"))
                            {
                                CO_Value = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO"]), decimalNum) * 1000);

                            }
                            else
                            {
                                CO_Value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO"]), decimalNum);
                            }
                        }
                        int? AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 1);

                        decimal? O3_Value = null;
                        if (dr["O3"] != DBNull.Value)
                            O3_Value = Convert.ToDecimal(dr["O3"]);
                        int? AQI_O3 = d_AQICalculateService.GetIAQI("a05024", O3_Value, 1);

                        decimal? Recent8HoursO3_Value = null;
                        if (dr["Recent8HoursO3"] != DBNull.Value)
                            Recent8HoursO3_Value = Convert.ToDecimal(dr["Recent8HoursO3"]);
                        int? AQI_Recent8HoursO3 = d_AQICalculateService.GetIAQI("a05024", Recent8HoursO3_Value, 8);

                        decimal? Recent8HoursO3NT_Value = null;
                        if (dr["Recent8HoursO3NT"] != DBNull.Value)
                            Recent8HoursO3NT_Value = Convert.ToDecimal(dr["Recent8HoursO3NT"]);
                        int? AQI_Recent8HoursO3NT = d_AQICalculateService.GetIAQI("a05024", Recent8HoursO3NT_Value, 8);

                        decimal? PM25_Value = null;
                        if (dr["PM25"] != DBNull.Value)
                            PM25_Value = Convert.ToDecimal(dr["PM25"]);
                        int? AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);

                        decimal? Recent24HoursPM25_Value = null;
                        if (dr["Recent24HoursPM25"] != DBNull.Value)
                            Recent24HoursPM25_Value = Convert.ToDecimal(dr["Recent24HoursPM25"]);
                        int? AQI_Recent24HoursPM25 = d_AQICalculateService.GetIAQI("a34004", Recent24HoursPM25_Value, 24);
                        #endregion

                        TB_HourAQI HourAQIExists = MonitorBusinessModel.TB_HourAQIs.Where(p => p.PointId.Equals(PointId) && p.DateTime.Equals(Tstamp)).FirstOrDefault();
                        if (HourAQIExists == null)
                        {
                            #region 新增小时AQI
                            TB_HourAQI NewAudHourAQI = new TB_HourAQI();
                            NewAudHourAQI.PointId = PointId;
                            NewAudHourAQI.DateTime = Tstamp;

                            if (SO2_Value != null)
                                NewAudHourAQI.SO2 = SO2_Value.ToString();
                            if (SO2_IAQI != null)
                                NewAudHourAQI.SO2_IAQI = SO2_IAQI.ToString();

                            if (NO2_Value != null)
                                NewAudHourAQI.NO2 = NO2_Value.ToString();
                            if (AQI_NO2 != null)
                                NewAudHourAQI.NO2_IAQI = AQI_NO2.ToString();

                            if (PM10_Value != null)
                                NewAudHourAQI.PM10 = PM10_Value.ToString();
                            if (AQI_PM10 != null)
                                NewAudHourAQI.PM10_IAQI = AQI_PM10.ToString();

                            if (Recent24HoursPM10_Value != null)
                                NewAudHourAQI.Recent24HoursPM10 = Recent24HoursPM10_Value.ToString();
                            if (AQI_Recent24HoursPM10 != null)
                                NewAudHourAQI.Recent24HoursPM10_IAQI = AQI_Recent24HoursPM10.ToString();

                            if (CO_Value != null)
                                NewAudHourAQI.CO = CO_Value.ToString();
                            if (AQI_CO != null)
                                NewAudHourAQI.CO_IAQI = AQI_CO.ToString();

                            if (O3_Value != null)
                                NewAudHourAQI.O3 = O3_Value.ToString();
                            if (AQI_O3 != null)
                                NewAudHourAQI.O3_IAQI = AQI_O3.ToString();

                            if (Recent8HoursO3_Value != null)
                                NewAudHourAQI.Recent8HoursO3 = Recent8HoursO3_Value.ToString();
                            if (AQI_Recent8HoursO3 != null)
                                NewAudHourAQI.Recent8HoursO3_IAQI = AQI_Recent8HoursO3.ToString();

                            if (Recent8HoursO3NT_Value != null)
                                NewAudHourAQI.Recent8HoursO3NT = Recent8HoursO3NT_Value.ToString();
                            if (AQI_Recent8HoursO3NT != null)
                                NewAudHourAQI.Recent8HoursO3NT_IAQI = AQI_Recent8HoursO3NT.ToString();

                            if (PM25_Value != null)
                                NewAudHourAQI.PM25 = PM25_Value.ToString();
                            if (AQI_PM25 != null)
                                NewAudHourAQI.PM25_IAQI = AQI_PM25.ToString();

                            if (Recent24HoursPM25_Value != null)
                                NewAudHourAQI.Recent24HoursPM25 = Recent24HoursPM25_Value.ToString();
                            if (AQI_Recent24HoursPM25 != null)
                                NewAudHourAQI.Recent24HoursPM25_IAQI = AQI_Recent24HoursPM25.ToString();

                            string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
                            NewAudHourAQI.AQIValue = AQIValue;
                            NewAudHourAQI.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");
                            if (!string.IsNullOrWhiteSpace(AQIValue))
                            {
                                NewAudHourAQI.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                NewAudHourAQI.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                NewAudHourAQI.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                NewAudHourAQI.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                NewAudHourAQI.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
                                NewAudHourAQI.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                            }
                            NewAudHourAQI.CreatUser = "SystemSync";
                            NewAudHourAQI.CreatDateTime = DateTime.Now;
                            MonitorBusinessModel.Add(NewAudHourAQI);
                            //MonitorBusinessModel.SaveChanges();
                            #endregion
                        }
                        else
                        {
                            #region 更新小时AQI
                            if (SO2_Value != null)
                                HourAQIExists.SO2 = SO2_Value.ToString();
                            else
                                HourAQIExists.SO2 = null;
                            if (SO2_IAQI != null)
                                HourAQIExists.SO2_IAQI = SO2_IAQI.ToString();
                            else
                                HourAQIExists.SO2_IAQI = null;
                            if (NO2_Value != null)
                                HourAQIExists.NO2 = NO2_Value.ToString();
                            else
                                HourAQIExists.NO2 = null;
                            if (AQI_NO2 != null)
                                HourAQIExists.NO2_IAQI = AQI_NO2.ToString();
                            else
                                HourAQIExists.NO2_IAQI = null;
                            if (PM10_Value != null)
                                HourAQIExists.PM10 = PM10_Value.ToString();
                            else
                                HourAQIExists.PM10 = null;
                            if (AQI_PM10 != null)
                                HourAQIExists.PM10_IAQI = AQI_PM10.ToString();
                            else
                                HourAQIExists.PM10_IAQI = null;
                            if (Recent24HoursPM10_Value != null)
                                HourAQIExists.Recent24HoursPM10 = Recent24HoursPM10_Value.ToString();
                            else
                                HourAQIExists.Recent24HoursPM10 = null;
                            if (AQI_Recent24HoursPM10 != null)
                                HourAQIExists.Recent24HoursPM10_IAQI = AQI_Recent24HoursPM10.ToString();
                            else
                                HourAQIExists.Recent24HoursPM10_IAQI = null;
                            if (CO_Value != null)
                                HourAQIExists.CO = CO_Value.ToString();
                            else
                                HourAQIExists.CO = null;
                            if (AQI_CO != null)
                                HourAQIExists.CO_IAQI = AQI_CO.ToString();
                            else
                                HourAQIExists.CO_IAQI = null;
                            if (O3_Value != null)
                                HourAQIExists.O3 = O3_Value.ToString();
                            else
                                HourAQIExists.O3 = null;
                            if (AQI_O3 != null)
                                HourAQIExists.O3_IAQI = AQI_O3.ToString();
                            else
                                HourAQIExists.O3_IAQI = null;
                            if (Recent8HoursO3_Value != null)
                                HourAQIExists.Recent8HoursO3 = Recent8HoursO3_Value.ToString();
                            else
                                HourAQIExists.Recent8HoursO3 = null;
                            if (AQI_Recent8HoursO3 != null)
                                HourAQIExists.Recent8HoursO3_IAQI = AQI_Recent8HoursO3.ToString();
                            else
                                HourAQIExists.Recent8HoursO3_IAQI = null;
                            if (Recent8HoursO3NT_Value != null)
                                HourAQIExists.Recent8HoursO3NT = Recent8HoursO3NT_Value.ToString();
                            else
                                HourAQIExists.Recent8HoursO3NT = null;
                            if (AQI_Recent8HoursO3NT != null)
                                HourAQIExists.Recent8HoursO3NT_IAQI = AQI_Recent8HoursO3NT.ToString();
                            else
                                HourAQIExists.Recent8HoursO3NT_IAQI = null;
                            if (PM25_Value != null)
                                HourAQIExists.PM25 = PM25_Value.ToString();
                            else
                                HourAQIExists.PM25 = null;
                            if (AQI_PM25 != null)
                                HourAQIExists.PM25_IAQI = AQI_PM25.ToString();
                            else
                                HourAQIExists.PM25_IAQI = null;
                            if (Recent24HoursPM25_Value != null)
                                HourAQIExists.Recent24HoursPM25 = Recent24HoursPM25_Value.ToString();
                            else
                                HourAQIExists.Recent24HoursPM25 = null;
                            if (AQI_Recent24HoursPM25 != null)
                                HourAQIExists.Recent24HoursPM25_IAQI = AQI_Recent24HoursPM25.ToString();
                            else
                                HourAQIExists.Recent24HoursPM25_IAQI = null;
                            string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
                            HourAQIExists.AQIValue = AQIValue;
                            HourAQIExists.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");
                            if (!string.IsNullOrWhiteSpace(AQIValue))
                            {
                                HourAQIExists.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                HourAQIExists.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                HourAQIExists.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                HourAQIExists.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                HourAQIExists.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
                                HourAQIExists.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                            }
                            else
                            {
                                HourAQIExists.Range = null;
                                HourAQIExists.RGBValue = null;
                                HourAQIExists.Class = null;
                                HourAQIExists.Grade = null;
                                HourAQIExists.HealthEffect = null;
                                HourAQIExists.TakeStep = null;
                            }
                            HourAQIExists.UpdateUser = "SystemSync";
                            HourAQIExists.UpdateDateTime = DateTime.Now;
                            //MonitorBusinessModel.SaveChanges();
                            #endregion
                        }
                    }
                    #endregion
                    MonitorBusinessModel.SaveChanges();

                    string CalculateTableName = "AirReport.TB_DayReport_Calculate";
                    // 生成日计算数据
                    d_DAL.AddConAirReport_Day_Mul(sTime, eTime, ConPointIds, CalculateTableName);

                    string AuditTableName = "AirReport.TB_DayReport";
                    //日审核数据
                    d_DAL.AddConAirReport_Day_Mul(sTime, eTime, ConPointIds, AuditTableName);

                    #region 计算生成审核日AQI
                    DataTable dtPivotDay = d_DAL.GetConPivotAudDayData(ConPointIds, sTime, eTime);
                    foreach (DataRow dr in dtPivotDay.Rows)
                    {
                        #region 数据
                        int? PointId = Convert.ToInt32(dr["PointId"]);
                        DateTime Tstamp = Convert.ToDateTime(dr["DateTime"]);
                        decimal? SO2_Value = null;
                        if (dr["SO2"] != DBNull.Value)
                            SO2_Value = Convert.ToDecimal(dr["SO2"]);
                        int? SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 24);

                        decimal? NO2_Value = null;
                        if (dr["NO2"] != DBNull.Value)
                            NO2_Value = Convert.ToDecimal(dr["NO2"]);
                        int? AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 24);

                        decimal? PM10_Value = null;
                        if (dr["PM10"] != DBNull.Value)
                            PM10_Value = Convert.ToDecimal(dr["PM10"]);
                        int? AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);


                        decimal? CO_Value = null;
                        if (dr["CO"] != DBNull.Value)
                            CO_Value = Convert.ToDecimal(dr["CO"]);
                        int? AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 24);

                        decimal? MaxOneHourO3_Value = null;
                        if (dr["MaxOneHourO3"] != DBNull.Value)
                            MaxOneHourO3_Value = Convert.ToDecimal(dr["MaxOneHourO3"]);
                        int? AQI_MaxOneHourO3 = d_AQICalculateService.GetIAQI("a05024", MaxOneHourO3_Value, 1);

                        decimal? Max8HourO3_Value = null;
                        if (dr["Max8HourO3"] != DBNull.Value)
                            Max8HourO3_Value = Convert.ToDecimal(dr["Max8HourO3"]);
                        int? AQI_Max8HourO3 = d_AQICalculateService.GetIAQI("a05024", Max8HourO3_Value, 8);

                        decimal? PM25_Value = null;
                        if (dr["PM25"] != DBNull.Value)
                            PM25_Value = Convert.ToDecimal(dr["PM25"]);
                        int? AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);

                        #endregion
                        TB_DayAQI AudDayAQIExists = MonitorBusinessModel.TB_DayAQIs.Where(p => p.PointId.Equals(PointId) && p.DateTime.Equals(Tstamp)).FirstOrDefault();
                        if (AudDayAQIExists == null)
                        {
                            #region 新增日AQI
                            TB_DayAQI NewAudDayAQI = new TB_DayAQI();
                            NewAudDayAQI.PointId = PointId;
                            NewAudDayAQI.DateTime = Tstamp;

                            if (SO2_Value != null)
                                NewAudDayAQI.SO2 = SO2_Value.ToString();
                            if (SO2_IAQI != null)
                                NewAudDayAQI.SO2_IAQI = SO2_IAQI.ToString();

                            if (NO2_Value != null)
                                NewAudDayAQI.NO2 = NO2_Value.ToString();
                            if (AQI_NO2 != null)
                                NewAudDayAQI.NO2_IAQI = AQI_NO2.ToString();

                            if (PM10_Value != null)
                                NewAudDayAQI.PM10 = PM10_Value.ToString();
                            if (AQI_PM10 != null)
                                NewAudDayAQI.PM10_IAQI = AQI_PM10.ToString();

                            if (CO_Value != null)
                                NewAudDayAQI.CO = CO_Value.ToString();
                            if (AQI_CO != null)
                                NewAudDayAQI.CO_IAQI = AQI_CO.ToString();

                            if (MaxOneHourO3_Value != null)
                                NewAudDayAQI.MaxOneHourO3 = MaxOneHourO3_Value.ToString();
                            if (AQI_MaxOneHourO3 != null)
                                NewAudDayAQI.MaxOneHourO3_IAQI = AQI_MaxOneHourO3.ToString();

                            if (Max8HourO3_Value != null)
                                NewAudDayAQI.Max8HourO3 = Max8HourO3_Value.ToString();
                            if (AQI_Max8HourO3 != null)
                                NewAudDayAQI.Max8HourO3_IAQI = AQI_Max8HourO3.ToString();

                            if (PM25_Value != null)
                                NewAudDayAQI.PM25 = PM25_Value.ToString();
                            if (AQI_PM25 != null)
                                NewAudDayAQI.PM25_IAQI = AQI_PM25.ToString();

                            string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "V");
                            NewAudDayAQI.AQIValue = AQIValue;
                            NewAudDayAQI.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "N");
                            if (!string.IsNullOrWhiteSpace(AQIValue))
                            {
                                NewAudDayAQI.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                NewAudDayAQI.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                NewAudDayAQI.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                NewAudDayAQI.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                NewAudDayAQI.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
                                NewAudDayAQI.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                            }

                            NewAudDayAQI.CreatUser = "SystemSync";
                            NewAudDayAQI.CreatDateTime = DateTime.Now;
                            MonitorBusinessModel.Add(NewAudDayAQI);
                            //MonitorBusinessModel.SaveChanges();
                            #endregion
                        }
                        else
                        {
                            #region 更新日AQI
                            if (SO2_Value != null)
                                AudDayAQIExists.SO2 = SO2_Value.ToString();
                            else
                                AudDayAQIExists.SO2 = null;
                            if (SO2_IAQI != null)
                                AudDayAQIExists.SO2_IAQI = SO2_IAQI.ToString();
                            else
                                AudDayAQIExists.SO2_IAQI = null;

                            if (NO2_Value != null)
                                AudDayAQIExists.NO2 = NO2_Value.ToString();
                            else
                                AudDayAQIExists.NO2 = null;
                            if (AQI_NO2 != null)
                                AudDayAQIExists.NO2_IAQI = AQI_NO2.ToString();
                            else
                                AudDayAQIExists.NO2_IAQI = null;

                            if (PM10_Value != null)
                                AudDayAQIExists.PM10 = PM10_Value.ToString();
                            else
                                AudDayAQIExists.PM10 = null;
                            if (AQI_PM10 != null)
                                AudDayAQIExists.PM10_IAQI = AQI_PM10.ToString();
                            else
                                AudDayAQIExists.PM10_IAQI = null;

                            if (CO_Value != null)
                                AudDayAQIExists.CO = CO_Value.ToString();
                            else
                                AudDayAQIExists.CO = null;
                            if (AQI_CO != null)
                                AudDayAQIExists.CO_IAQI = AQI_CO.ToString();
                            else
                                AudDayAQIExists.CO_IAQI = null;

                            if (MaxOneHourO3_Value != null)
                                AudDayAQIExists.MaxOneHourO3 = MaxOneHourO3_Value.ToString();
                            else
                                AudDayAQIExists.MaxOneHourO3 = null;
                            if (AQI_MaxOneHourO3 != null)
                                AudDayAQIExists.MaxOneHourO3_IAQI = AQI_MaxOneHourO3.ToString();
                            else
                                AudDayAQIExists.MaxOneHourO3_IAQI = null;

                            if (Max8HourO3_Value != null)
                                AudDayAQIExists.Max8HourO3 = Max8HourO3_Value.ToString();
                            else
                                AudDayAQIExists.Max8HourO3 = null;
                            if (AQI_Max8HourO3 != null)
                                AudDayAQIExists.Max8HourO3_IAQI = AQI_Max8HourO3.ToString();
                            else
                                AudDayAQIExists.Max8HourO3_IAQI = null;

                            if (PM25_Value != null)
                                AudDayAQIExists.PM25 = PM25_Value.ToString();
                            else
                                AudDayAQIExists.PM25 = null;
                            if (AQI_PM25 != null)
                                AudDayAQIExists.PM25_IAQI = AQI_PM25.ToString();
                            else
                                AudDayAQIExists.PM25_IAQI = null;

                            string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "V");
                            AudDayAQIExists.AQIValue = AQIValue;
                            AudDayAQIExists.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "N");
                            if (!string.IsNullOrWhiteSpace(AQIValue))
                            {
                                AudDayAQIExists.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                AudDayAQIExists.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                AudDayAQIExists.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                AudDayAQIExists.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                AudDayAQIExists.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEAlTHEFFECT");
                                AudDayAQIExists.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                            }
                            else
                            {
                                AudDayAQIExists.Range = null;
                                AudDayAQIExists.RGBValue = null;
                                AudDayAQIExists.Class = null;
                                AudDayAQIExists.Grade = null;
                                AudDayAQIExists.HealthEffect = null;
                                AudDayAQIExists.TakeStep = null;
                            }
                            AudDayAQIExists.UpdateUser = "SystemSync";
                            AudDayAQIExists.UpdateDateTime = DateTime.Now;
                            //MonitorBusinessModel.SaveChanges();
                            #endregion
                        }
                    }
                    #endregion
                    MonitorBusinessModel.SaveChanges();
                    //}
                    #endregion
                    #region 审核区域AQI
                    string m_regionCon = string.Empty;
                    foreach (string str in m_regions)
                    {
                        m_regionCon += "'" + str + "',";
                    }
                    //foreach (string m_region in m_regions)
                    //{
                    #region 审核小时区域AQI
                    DataTable dtPivotAudRegionHour = d_DAL.GetConPivotAudRegionHourData(m_regionCon.TrimEnd(','), sTime, eTime);
                    foreach (DataRow dr in dtPivotAudRegionHour.Rows)
                    {
                        #region 数据
                        string m_region = dr["CityTypeUid"].ToString();
                        DateTime Tstamp = Convert.ToDateTime(dr["DateTime"]);
                        decimal? SO2_Value = null;
                        if (dr["SO2"] != DBNull.Value)
                            SO2_Value = Convert.ToDecimal(dr["SO2"]);
                        int? SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 1);

                        decimal? NO2_Value = null;
                        if (dr["NO2"] != DBNull.Value)
                            NO2_Value = Convert.ToDecimal(dr["NO2"]);
                        int? AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 1);

                        decimal? PM10_Value = null;
                        if (dr["PM10"] != DBNull.Value)
                            PM10_Value = Convert.ToDecimal(dr["PM10"]);
                        int? AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);

                        decimal? Recent24HoursPM10_Value = null;
                        if (dr["Recent24HoursPM10"] != DBNull.Value)
                            Recent24HoursPM10_Value = Convert.ToDecimal(dr["Recent24HoursPM10"]);
                        int? AQI_Recent24HoursPM10 = d_AQICalculateService.GetIAQI("a34002", Recent24HoursPM10_Value, 24);

                        decimal? CO_Value = null;
                        //先修约再计算AQI
                        if (dr["CO"] != DBNull.Value)
                        {
                            DataTable dtFactor = d_AQICalculateService.GetPollutantUnit("a21005");
                            int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);
                            if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m3"))
                            {
                                CO_Value = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO"]), decimalNum) * 1000);

                            }
                            else
                            {
                                CO_Value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO"]), decimalNum);
                            }
                        }
                        int? AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 1);

                        decimal? O3_Value = null;
                        if (dr["O3"] != DBNull.Value)
                            O3_Value = Convert.ToDecimal(dr["O3"]);
                        int? AQI_O3 = d_AQICalculateService.GetIAQI("a05024", O3_Value, 1);

                        decimal? Recent8HoursO3_Value = null;
                        if (dr["Recent8HoursO3"] != DBNull.Value)
                            Recent8HoursO3_Value = Convert.ToDecimal(dr["Recent8HoursO3"]);
                        int? AQI_Recent8HoursO3 = d_AQICalculateService.GetIAQI("a05024", Recent8HoursO3_Value, 8);

                        decimal? Recent8HoursO3NT_Value = null;
                        if (dr["Recent8HoursO3NT"] != DBNull.Value)
                            Recent8HoursO3NT_Value = Convert.ToDecimal(dr["Recent8HoursO3NT"]);
                        int? AQI_Recent8HoursO3NT = d_AQICalculateService.GetIAQI("a05024", Recent8HoursO3NT_Value, 8);

                        decimal? PM25_Value = null;
                        if (dr["PM25"] != DBNull.Value)
                            PM25_Value = Convert.ToDecimal(dr["PM25"]);
                        int? AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);

                        decimal? Recent24HoursPM25_Value = null;
                        if (dr["Recent24HoursPM25"] != DBNull.Value)
                            Recent24HoursPM25_Value = Convert.ToDecimal(dr["Recent24HoursPM25"]);
                        int? AQI_Recent24HoursPM25 = d_AQICalculateService.GetIAQI("a34004", Recent24HoursPM25_Value, 24);
                        #endregion
                        TB_RegionHourAQI AudRegionHourAQIExists = MonitorBusinessModel.TB_RegionHourAQIs.Where(p => p.MonitoringRegionUid.Equals(m_region) && p.DateTime.Equals(Tstamp)).FirstOrDefault();
                        if (AudRegionHourAQIExists == null)
                        {
                            #region 新增小时AQI
                            TB_RegionHourAQI NewAudHourAQI = new TB_RegionHourAQI();
                            NewAudHourAQI.MonitoringRegionUid = m_region;
                            NewAudHourAQI.DateTime = Tstamp;
                            NewAudHourAQI.StatisticalType = "CG";

                            if (SO2_Value != null)
                                NewAudHourAQI.SO2 = SO2_Value.ToString();
                            if (SO2_IAQI != null)
                                NewAudHourAQI.SO2_IAQI = SO2_IAQI.ToString();

                            if (NO2_Value != null)
                                NewAudHourAQI.NO2 = NO2_Value.ToString();
                            if (AQI_NO2 != null)
                                NewAudHourAQI.NO2_IAQI = AQI_NO2.ToString();

                            if (PM10_Value != null)
                                NewAudHourAQI.PM10 = PM10_Value.ToString();
                            if (AQI_PM10 != null)
                                NewAudHourAQI.PM10_IAQI = AQI_PM10.ToString();

                            if (Recent24HoursPM10_Value != null)
                                NewAudHourAQI.Recent24HoursPM10 = Recent24HoursPM10_Value.ToString();
                            if (AQI_Recent24HoursPM10 != null)
                                NewAudHourAQI.Recent24HoursPM10_IAQI = AQI_Recent24HoursPM10.ToString();

                            if (CO_Value != null)
                                NewAudHourAQI.CO = CO_Value.ToString();
                            if (AQI_CO != null)
                                NewAudHourAQI.CO_IAQI = AQI_CO.ToString();

                            if (O3_Value != null)
                                NewAudHourAQI.O3 = O3_Value.ToString();
                            if (AQI_O3 != null)
                                NewAudHourAQI.O3_IAQI = AQI_O3.ToString();

                            if (Recent8HoursO3_Value != null)
                                NewAudHourAQI.Recent8HoursO3 = Recent8HoursO3_Value.ToString();
                            if (AQI_Recent8HoursO3 != null)
                                NewAudHourAQI.Recent8HoursO3_IAQI = AQI_Recent8HoursO3.ToString();

                            if (Recent8HoursO3NT_Value != null)
                                NewAudHourAQI.Recent8HoursO3NT = Recent8HoursO3NT_Value.ToString();
                            if (AQI_Recent8HoursO3NT != null)
                                NewAudHourAQI.Recent8HoursO3NT_IAQI = AQI_Recent8HoursO3NT.ToString();

                            if (PM25_Value != null)
                                NewAudHourAQI.PM25 = PM25_Value.ToString();
                            if (AQI_PM25 != null)
                                NewAudHourAQI.PM25_IAQI = AQI_PM25.ToString();

                            if (Recent24HoursPM25_Value != null)
                                NewAudHourAQI.Recent24HoursPM25 = Recent24HoursPM25_Value.ToString();
                            if (AQI_Recent24HoursPM25 != null)
                                NewAudHourAQI.Recent24HoursPM25_IAQI = AQI_Recent24HoursPM25.ToString();

                            string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
                            NewAudHourAQI.AQIValue = AQIValue;
                            NewAudHourAQI.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");
                            if (!string.IsNullOrWhiteSpace(AQIValue))
                            {
                                NewAudHourAQI.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                NewAudHourAQI.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                NewAudHourAQI.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                NewAudHourAQI.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                NewAudHourAQI.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
                                NewAudHourAQI.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                            }
                            NewAudHourAQI.CreatUser = "SystemSync";
                            NewAudHourAQI.CreatDateTime = DateTime.Now;
                            MonitorBusinessModel.Add(NewAudHourAQI);
                            #endregion
                        }
                        else
                        {
                            #region 更新小时AQI
                            if (SO2_Value != null)
                                AudRegionHourAQIExists.SO2 = SO2_Value.ToString();
                            else
                                AudRegionHourAQIExists.SO2 = null;
                            if (SO2_IAQI != null)
                                AudRegionHourAQIExists.SO2_IAQI = SO2_IAQI.ToString();
                            else
                                AudRegionHourAQIExists.SO2_IAQI = null;

                            if (NO2_Value != null)
                                AudRegionHourAQIExists.NO2 = NO2_Value.ToString();
                            else
                                AudRegionHourAQIExists.NO2 = null;
                            if (AQI_NO2 != null)
                                AudRegionHourAQIExists.NO2_IAQI = AQI_NO2.ToString();
                            else
                                AudRegionHourAQIExists.NO2_IAQI = null;

                            if (PM10_Value != null)
                                AudRegionHourAQIExists.PM10 = PM10_Value.ToString();
                            else
                                AudRegionHourAQIExists.PM10 = null;
                            if (AQI_PM10 != null)
                                AudRegionHourAQIExists.PM10_IAQI = AQI_PM10.ToString();
                            else
                                AudRegionHourAQIExists.PM10_IAQI = null;

                            if (Recent24HoursPM10_Value != null)
                                AudRegionHourAQIExists.Recent24HoursPM10 = Recent24HoursPM10_Value.ToString();
                            else
                                AudRegionHourAQIExists.Recent24HoursPM10 = null;
                            if (AQI_Recent24HoursPM10 != null)
                                AudRegionHourAQIExists.Recent24HoursPM10_IAQI = AQI_Recent24HoursPM10.ToString();
                            else
                                AudRegionHourAQIExists.Recent24HoursPM10_IAQI = null;

                            if (CO_Value != null)
                                AudRegionHourAQIExists.CO = CO_Value.ToString();
                            else
                                AudRegionHourAQIExists.CO = null;
                            if (AQI_CO != null)
                                AudRegionHourAQIExists.CO_IAQI = AQI_CO.ToString();
                            else
                                AudRegionHourAQIExists.CO_IAQI = null;

                            if (O3_Value != null)
                                AudRegionHourAQIExists.O3 = O3_Value.ToString();
                            else
                                AudRegionHourAQIExists.O3 = null;
                            if (AQI_O3 != null)
                                AudRegionHourAQIExists.O3_IAQI = AQI_O3.ToString();
                            else
                                AudRegionHourAQIExists.O3_IAQI = null;

                            if (Recent8HoursO3_Value != null)
                                AudRegionHourAQIExists.Recent8HoursO3 = Recent8HoursO3_Value.ToString();
                            else
                                AudRegionHourAQIExists.Recent8HoursO3 = null;
                            if (AQI_Recent8HoursO3 != null)
                                AudRegionHourAQIExists.Recent8HoursO3_IAQI = AQI_Recent8HoursO3.ToString();
                            else
                                AudRegionHourAQIExists.Recent8HoursO3_IAQI = null;

                            if (Recent8HoursO3NT_Value != null)
                                AudRegionHourAQIExists.Recent8HoursO3NT = Recent8HoursO3NT_Value.ToString();
                            else
                                AudRegionHourAQIExists.Recent8HoursO3NT = null;
                            if (AQI_Recent8HoursO3NT != null)
                                AudRegionHourAQIExists.Recent8HoursO3NT_IAQI = AQI_Recent8HoursO3NT.ToString();
                            else
                                AudRegionHourAQIExists.Recent8HoursO3NT_IAQI = null;

                            if (PM25_Value != null)
                                AudRegionHourAQIExists.PM25 = PM25_Value.ToString();
                            else
                                AudRegionHourAQIExists.PM25 = null;
                            if (AQI_PM25 != null)
                                AudRegionHourAQIExists.PM25_IAQI = AQI_PM25.ToString();
                            else
                                AudRegionHourAQIExists.PM25_IAQI = null;

                            if (Recent24HoursPM25_Value != null)
                                AudRegionHourAQIExists.Recent24HoursPM25 = Recent24HoursPM25_Value.ToString();
                            else
                                AudRegionHourAQIExists.Recent24HoursPM25 = null;
                            if (AQI_Recent24HoursPM25 != null)
                                AudRegionHourAQIExists.Recent24HoursPM25_IAQI = AQI_Recent24HoursPM25.ToString();
                            else
                                AudRegionHourAQIExists.Recent24HoursPM25_IAQI = null;

                            string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
                            AudRegionHourAQIExists.AQIValue = AQIValue;
                            AudRegionHourAQIExists.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");
                            if (!string.IsNullOrWhiteSpace(AQIValue))
                            {
                                AudRegionHourAQIExists.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                AudRegionHourAQIExists.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                AudRegionHourAQIExists.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                AudRegionHourAQIExists.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                AudRegionHourAQIExists.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
                                AudRegionHourAQIExists.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                            }
                            else
                            {
                                AudRegionHourAQIExists.Range = null;
                                AudRegionHourAQIExists.RGBValue = null;
                                AudRegionHourAQIExists.Class = null;
                                AudRegionHourAQIExists.Grade = null;
                                AudRegionHourAQIExists.HealthEffect = null;
                                AudRegionHourAQIExists.TakeStep = null;
                            }
                            AudRegionHourAQIExists.UpdateUser = "SystemSync";
                            AudRegionHourAQIExists.UpdateDateTime = DateTime.Now;
                            #endregion
                        }
                    }
                    #endregion
                    MonitorBusinessModel.SaveChanges();

                    #region 审核日区域AQI
                    DataTable dtPivotAudRegionDay = d_DAL.GetConPivotAudRegionDayData(m_regionCon.TrimEnd(','), sTime, eTime);
                    foreach (DataRow dr in dtPivotAudRegionDay.Rows)
                    {
                        #region 数据
                        string m_region = dr["CityTypeUid"].ToString();
                        DateTime Tstamp = Convert.ToDateTime(dr["DateTime"]);
                        decimal? SO2_Value = null;
                        if (dr["SO2"] != DBNull.Value)
                            SO2_Value = Convert.ToDecimal(dr["SO2"]);
                        int? SO2_IAQI = d_AQICalculateService.GetIAQI("a21026", SO2_Value, 24);

                        decimal? NO2_Value = null;
                        if (dr["NO2"] != DBNull.Value)
                            NO2_Value = Convert.ToDecimal(dr["NO2"]);
                        int? AQI_NO2 = d_AQICalculateService.GetIAQI("a21004", NO2_Value, 24);

                        decimal? PM10_Value = null;
                        if (dr["PM10"] != DBNull.Value)
                            PM10_Value = Convert.ToDecimal(dr["PM10"]);
                        int? AQI_PM10 = d_AQICalculateService.GetIAQI("a34002", PM10_Value, 24);


                        decimal? CO_Value = null;
                        if (dr["CO"] != DBNull.Value)
                            CO_Value = Convert.ToDecimal(dr["CO"]);
                        int? AQI_CO = d_AQICalculateService.GetIAQI("a21005", CO_Value, 24);

                        decimal? MaxOneHourO3_Value = null;
                        if (dr["MaxOneHourO3"] != DBNull.Value)
                            MaxOneHourO3_Value = Convert.ToDecimal(dr["MaxOneHourO3"]);
                        int? AQI_MaxOneHourO3 = d_AQICalculateService.GetIAQI("a05024", MaxOneHourO3_Value, 1);

                        decimal? Max8HourO3_Value = null;
                        if (dr["Max8HourO3"] != DBNull.Value)
                            Max8HourO3_Value = Convert.ToDecimal(dr["Max8HourO3"]);
                        int? AQI_Max8HourO3 = d_AQICalculateService.GetIAQI("a05024", Max8HourO3_Value, 8);

                        decimal? PM25_Value = null;
                        if (dr["PM25"] != DBNull.Value)
                            PM25_Value = Convert.ToDecimal(dr["PM25"]);
                        int? AQI_PM25 = d_AQICalculateService.GetIAQI("a34004", PM25_Value, 24);

                        #endregion
                        TB_RegionDayAQIReport AudRegionDayAQIExists = MonitorBusinessModel.TB_RegionDayAQIReports.Where(p => p.MonitoringRegionUid.Equals(m_region) && p.ReportDateTime.Equals(Tstamp)).FirstOrDefault();
                        if (AudRegionDayAQIExists == null)
                        {
                            #region 新增日AQI
                            TB_RegionDayAQIReport NewAudDayAQI = new TB_RegionDayAQIReport();
                            NewAudDayAQI.MonitoringRegionUid = m_region;
                            NewAudDayAQI.ReportDateTime = Tstamp;
                            NewAudDayAQI.StatisticalType = "CG";

                            if (SO2_Value != null)
                                NewAudDayAQI.SO2 = SO2_Value.ToString();
                            if (SO2_IAQI != null)
                                NewAudDayAQI.SO2_IAQI = SO2_IAQI.ToString();

                            if (NO2_Value != null)
                                NewAudDayAQI.NO2 = NO2_Value.ToString();
                            if (AQI_NO2 != null)
                                NewAudDayAQI.NO2_IAQI = AQI_NO2.ToString();

                            if (PM10_Value != null)
                                NewAudDayAQI.PM10 = PM10_Value.ToString();
                            if (AQI_PM10 != null)
                                NewAudDayAQI.PM10_IAQI = AQI_PM10.ToString();

                            if (CO_Value != null)
                                NewAudDayAQI.CO = CO_Value.ToString();
                            if (AQI_CO != null)
                                NewAudDayAQI.CO_IAQI = AQI_CO.ToString();

                            if (MaxOneHourO3_Value != null)
                                NewAudDayAQI.MaxOneHourO3 = MaxOneHourO3_Value.ToString();
                            if (AQI_MaxOneHourO3 != null)
                                NewAudDayAQI.MaxOneHourO3_IAQI = AQI_MaxOneHourO3.ToString();

                            if (Max8HourO3_Value != null)
                                NewAudDayAQI.Max8HourO3 = Max8HourO3_Value.ToString();
                            if (AQI_Max8HourO3 != null)
                                NewAudDayAQI.Max8HourO3_IAQI = AQI_Max8HourO3.ToString();

                            if (PM25_Value != null)
                                NewAudDayAQI.PM25 = PM25_Value.ToString();
                            if (AQI_PM25 != null)
                                NewAudDayAQI.PM25_IAQI = AQI_PM25.ToString();

                            string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "V");
                            NewAudDayAQI.AQIValue = AQIValue;
                            NewAudDayAQI.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "N");
                            if (!string.IsNullOrWhiteSpace(AQIValue))
                            {
                                NewAudDayAQI.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                NewAudDayAQI.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                NewAudDayAQI.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                NewAudDayAQI.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                NewAudDayAQI.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEALTHEFFECT");
                                NewAudDayAQI.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                            }

                            NewAudDayAQI.CreatUser = "SystemSync";
                            NewAudDayAQI.CreatDateTime = DateTime.Now;
                            MonitorBusinessModel.Add(NewAudDayAQI);
                            #endregion
                        }
                        else
                        {
                            #region 更新日AQI
                            if (SO2_Value != null)
                                AudRegionDayAQIExists.SO2 = SO2_Value.ToString();
                            else
                                AudRegionDayAQIExists.SO2 = null;
                            if (SO2_IAQI != null)
                                AudRegionDayAQIExists.SO2_IAQI = SO2_IAQI.ToString();
                            else
                                AudRegionDayAQIExists.SO2_IAQI = null;

                            if (NO2_Value != null)
                                AudRegionDayAQIExists.NO2 = NO2_Value.ToString();
                            else
                                AudRegionDayAQIExists.NO2 = null;
                            if (AQI_NO2 != null)
                                AudRegionDayAQIExists.NO2_IAQI = AQI_NO2.ToString();
                            else
                                AudRegionDayAQIExists.NO2_IAQI = null;

                            if (PM10_Value != null)
                                AudRegionDayAQIExists.PM10 = PM10_Value.ToString();
                            else
                                AudRegionDayAQIExists.PM10 = null;
                            if (AQI_PM10 != null)
                                AudRegionDayAQIExists.PM10_IAQI = AQI_PM10.ToString();
                            else
                                AudRegionDayAQIExists.PM10_IAQI = null;

                            if (CO_Value != null)
                                AudRegionDayAQIExists.CO = CO_Value.ToString();
                            else
                                AudRegionDayAQIExists.CO = null;
                            if (AQI_CO != null)
                                AudRegionDayAQIExists.CO_IAQI = AQI_CO.ToString();
                            else
                                AudRegionDayAQIExists.SO2 = null;

                            if (MaxOneHourO3_Value != null)
                                AudRegionDayAQIExists.MaxOneHourO3 = MaxOneHourO3_Value.ToString();
                            else
                                AudRegionDayAQIExists.MaxOneHourO3 = null;
                            if (AQI_MaxOneHourO3 != null)
                                AudRegionDayAQIExists.MaxOneHourO3_IAQI = AQI_MaxOneHourO3.ToString();
                            else
                                AudRegionDayAQIExists.MaxOneHourO3_IAQI = null;

                            if (Max8HourO3_Value != null)
                                AudRegionDayAQIExists.Max8HourO3 = Max8HourO3_Value.ToString();
                            else
                                AudRegionDayAQIExists.Max8HourO3 = null;
                            if (AQI_Max8HourO3 != null)
                                AudRegionDayAQIExists.Max8HourO3_IAQI = AQI_Max8HourO3.ToString();
                            else
                                AudRegionDayAQIExists.Max8HourO3_IAQI = null;

                            if (PM25_Value != null)
                                AudRegionDayAQIExists.PM25 = PM25_Value.ToString();
                            else
                                AudRegionDayAQIExists.PM25 = null;
                            if (AQI_PM25 != null)
                                AudRegionDayAQIExists.PM25_IAQI = AQI_PM25.ToString();
                            else
                                AudRegionDayAQIExists.PM25_IAQI = null;

                            string AQIValue = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "V");
                            AudRegionDayAQIExists.AQIValue = AQIValue;
                            AudRegionDayAQIExists.PrimaryPollutant = d_AQICalculateService.GetAQI_Max_CNV(SO2_IAQI, AQI_NO2, AQI_PM10, AQI_CO, AQI_Max8HourO3, AQI_PM25, "N");
                            if (!string.IsNullOrWhiteSpace(AQIValue))
                            {
                                AudRegionDayAQIExists.Range = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "RANGE");
                                AudRegionDayAQIExists.RGBValue = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                AudRegionDayAQIExists.Class = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "CLASS");
                                AudRegionDayAQIExists.Grade = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "GRADE");
                                AudRegionDayAQIExists.HealthEffect = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "HEAlTHEFFECT");
                                AudRegionDayAQIExists.TakeStep = d_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "TAKESTEP");
                            }
                            else
                            {
                                AudRegionDayAQIExists.Range = null;
                                AudRegionDayAQIExists.RGBValue = null;
                                AudRegionDayAQIExists.Class = null;
                                AudRegionDayAQIExists.Grade = null;
                                AudRegionDayAQIExists.HealthEffect = null;
                                AudRegionDayAQIExists.TakeStep = null;
                            }
                            AudRegionDayAQIExists.UpdateUser = "SystemSync";
                            AudRegionDayAQIExists.UpdateDateTime = DateTime.Now;

                            #endregion
                        }
                    }
                    #endregion
                    MonitorBusinessModel.SaveChanges();
                    //}
                    #endregion

                }
                log.Info("-------------------------------------------------------CalculateConBy60审核数据结束----------------------------");
                #endregion
            }
            catch (Exception ex)
            {
                log.Error("--------------------------------------------------------CalculateConBy60异常:" + ex.ToString());
            }

        }
        /// <summary>
        /// 日数据计算月数据
        /// </summary>
        public void CalculateByDay(DateTime SMTime, DateTime EMTime, DateTime SWTime, DateTime EWTime)
        {
            try
            {
                DateTime sMonthTime = SMTime.AddDays(1 - SMTime.Day);//当前月第一天
                DateTime eMonthTime = EMTime.AddDays(1 - EMTime.Day).AddMonths(1).AddDays(-1);//当前月最后一天

                //周数据时间范围：当前周第一天和当前周最后一天
                DateTime sWeekTime = GetWeekFirstDayMon(SWTime);
                DateTime eWeekTime = GetWeekLastDaySun(EWTime);

                foreach (int PointId in PointIds)
                {
                    //生成原始月数据
                    d_DAL.AddOriginalMonthData(sMonthTime, eMonthTime, PointId);

                    //生成周数据
                    d_DAL.AddAirReport_Week_Mul(sWeekTime, eWeekTime, PointId);

                    string CalculateTableName = "AirReport.TB_MonthReport_Calculate";
                    //生成月计算数据
                    d_DAL.AddAirReport_Month_Mul(sMonthTime, eMonthTime, PointId, CalculateTableName);

                    //生成月审核数据
                    string AuditTableName = "AirReport.TB_MonthReport";
                    d_DAL.AddAirReport_Month_Mul(sMonthTime, eMonthTime, PointId, AuditTableName);

                }
            }
            catch (Exception ex)
            {
                log.Error("--------------------------------------------CalculateByDay异常：" + ex.ToString());
            }
        }

        #region 周日期格式化
        /// <summary>  
        /// 得到本周第一天(以星期一为第一天)  
        /// </summary>  
        /// <param name="datetime"></param>  
        /// <returns></returns>  
        public DateTime GetWeekFirstDayMon(DateTime datetime)
        {
            //星期一为第一天  
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);

            //因为是以星期一为第一天，所以要判断weeknow等于0时，要向前推6天。  
            weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
            int daydiff = (-1) * weeknow;

            //本周第一天  
            string FirstDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
            return Convert.ToDateTime(FirstDay);
        }
        /// <summary>  
        /// 得到本周最后一天(以星期天为最后一天)  
        /// </summary>  
        /// <param name="datetime"></param>  
        /// <returns></returns>  
        public DateTime GetWeekLastDaySun(DateTime datetime)
        {
            //星期天为最后一天  
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);
            weeknow = (weeknow == 0 ? 7 : weeknow);
            int daydiff = (7 - weeknow);

            //本周最后一天  
            string LastDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
            return Convert.ToDateTime(LastDay);
        }
        #endregion

        /// <summary>
        /// 月数据计算季数据，年数据
        /// </summary>
        public void CalculateByMonth(DateTime SSTime, DateTime ESTime, DateTime SYTime, DateTime EYTime)
        {
            try
            {
                //季数据时间范围：当前季度第一天和当前季度最后一天
                DateTime sSeasonTime = Convert.ToDateTime(SSTime.AddMonths(0 - ((SSTime.Month - 1) % 3)).ToString("yyyy-MM-01"));
                DateTime eSeasonTime = Convert.ToDateTime(DateTime.Parse(ESTime.AddMonths(3 - ((ESTime.Month - 1) % 3)).ToString("yyyy-MM-01")).AddDays(-1).ToShortDateString());

                //年数据时间范围：当前年第一天和当前年最后一天
                DateTime sYearTime = Convert.ToDateTime(SYTime.ToString("yyyy-01-01"));
                DateTime eYearTime = Convert.ToDateTime(EYTime.ToString("yyyy-12-31"));

                foreach (int PointId in PointIds)
                {
                    //生成季数据
                    d_DAL.AddAirReport_Season_Mul(sSeasonTime, eSeasonTime, PointId);

                    //生成年数据
                    d_DAL.AddAirReport_Year_Mul(sSeasonTime, eSeasonTime, PointId);
                }
            }
            catch (Exception ex)
            {
                log.Error("--------------------------------------------CalculateByMonth异常：" + ex.ToString());
            }
        }

        /// <summary>
        /// 超级站仪器在线情况
        /// </summary>
        /// <param name="DataTypeUid">数据类型</param>
        /// <param name="IDLog">ID记录</param>
        public void InstrumentOnline(string DataTypeUid)
        {
            try
            {
                DateTime UpdateDateTime = DateTime.Now;
                string JGLDGuid = System.Configuration.ConfigurationManager.AppSettings["JGLDGuid"];
                string CityShootGuid = System.Configuration.ConfigurationManager.AppSettings["CityShootGuid"];
                using (BaseDataModel BaseDataModel = new BaseDataModel())
                {
                    using (MonitorBusinessModel MonitorBusinessModel = new MonitorBusinessModel())
                    {
                        //获取配置表中信息
                        DT_DataTypeConfig DataTypeConfig = BaseDataModel.DT_DataTypeConfigs.Where(p => p.EnableOrNot.Equals(true) && p.DataTypeUid.Equals(DataTypeUid)).FirstOrDefault();
                        string OriginTable = DataTypeConfig.OriginalTable;//原始表

                        #region 超级站离线
                        DataTable dtOfflineConfig = d_DAL.GetInstrumentSet(DataTypeUid);
                        foreach (DataRow dr in dtOfflineConfig.Rows)
                        {
                            string InUid = dr["InstrumentUid"].ToString();
                            string InName = dr["InstrumentName"].ToString();
                            int OffLineTimeSpan = Convert.ToInt32(dr["OffLineTimeSpan"].ToString());
                            DateTime DataDT = DateTime.Now;

                          //获取所有仪器最新数据时间
                            DataTable dtLastestData = null;
                          //城市摄影和激光雷达获取数据最新时间的方法和其他仪器不同
                            if (JGLDGuid.Equals(InUid))
                            {
                              dtLastestData = d_DAL.GetJGLDNewestData(JGLDGuid);
                            }
                            else if (CityShootGuid.Equals(InUid))
                            {
                              dtLastestData = GetCityShootNewestData(CityShootGuid);
                            }
                            else
                            {
                              dtLastestData=d_DAL.GetNewDataInstrument(OriginTable, InUid);
                            }

                            #region 报文时间格式化
                            if (DataTypeUid == "1b6367f1-5287-4c14-b120-7a35bd176db1")//小时数据
                            {
                                DataDT = Convert.ToDateTime(DataDT.ToString("yyyy-MM-dd HH:00:00"));
                            }
                            else if (DataTypeUid == "7a894b1f-e990-4cc3-87bb-be1e431c46bf")//5分钟数据
                            {
                                int minuteTimes = DataDT.Minute / 5;
                                string minute = (minuteTimes * 5).ToString();
                                DataDT = Convert.ToDateTime(DataDT.ToString("yyyy-MM-dd HH:") + minute + ":00");
                            }
                            else if (DataTypeUid == "c36398ef-2bec-49be-8fca-b491fecaa359")//1分钟数据
                            {
                                DataDT = Convert.ToDateTime(DataDT.ToString("yyyy-MM-dd HH:mm:00"));
                            }
                            #endregion

                            InstrumentDataOnline DataOnlineExists = MonitorBusinessModel.InstrumentDataOnlines.Where(p => p.InstrumentUid.Equals(InUid) && p.DataTypeUid.Equals(DataTypeUid)).FirstOrDefault();
                            if (dtLastestData!=null && dtLastestData.Rows.Count > 0)
                            {
                                DateTime DTFinish = Convert.ToDateTime(dtLastestData.Rows[0]["Tstamp"]);
                                string content = DataDT.ToString("MM月dd日HH时") + "-" + DTFinish.ToString("MM月dd日HH时") + "，" + InName + "仪器断线";
                                string SendContent = InName + ";" + DataDT.ToString("yyyy-MM-dd HH:mm:dd") + ";" + DTFinish.ToString("yyyy-MM-dd HH:mm:dd");
                                TimeSpan ts = DataDT - DTFinish;
                                if (ts.TotalMinutes > OffLineTimeSpan)
                                {
                                    TB_CreatAlarm[] CreatAlarmExists = BaseDataModel.TB_CreatAlarms.Where(p => p.Content.Equals(content)).ToArray();
                                    if (CreatAlarmExists.Count() == 0)
                                    {
                                        TB_CreatAlarm NewCreatAlarm = new TB_CreatAlarm();
                                        NewCreatAlarm.AlarmUid = Guid.NewGuid().ToString();
                                        NewCreatAlarm.ApplicationUid = ApplicationUid;
                                        NewCreatAlarm.MonitoringPointUid = "f2369c1f-95e1-4f5d-9045-9c185ce8727a";
                                        NewCreatAlarm.RecordDateTime = DataDT;
                                        NewCreatAlarm.AlarmEventUid = "9504ec80-e5f8-4436-9fa1-1768805ea1ca";
                                        NewCreatAlarm.AlarmGradeUid = "e1111f5e-6ef7-4f95-8778-bd7062ce2aae";
                                        NewCreatAlarm.DataTypeUid = DataTypeUid;
                                        NewCreatAlarm.Content = content;
                                        NewCreatAlarm.SendContent = SendContent;
                                        NewCreatAlarm.ItemCode = "Offline";
                                        NewCreatAlarm.ItemName = "断线";
                                        NewCreatAlarm.MonitoringPoint = InName;
                                        NewCreatAlarm.CreatUser = "SYSTEM";
                                        NewCreatAlarm.CreatDateTime = DateTime.Now;

                                        BaseDataModel.Add(NewCreatAlarm);
                                        BaseDataModel.SaveChanges();
                                    }
                                    if (DataOnlineExists == null)//新增
                                    {
                                        InstrumentDataOnline NewDataOnline = new InstrumentDataOnline();
                                        NewDataOnline.InstrumentUid = InUid;
                                        NewDataOnline.InstrumentName = InName;
                                        NewDataOnline.DataTypeUid = DataTypeUid;
                                        NewDataOnline.IsOnline = 0;
                                        NewDataOnline.NewDataTime = DTFinish;
                                        NewDataOnline.OffLineTime = Convert.ToInt32(ts.TotalMinutes);
                                        MonitorBusinessModel.Add(NewDataOnline);
                                    }
                                    else//更新
                                    {
                                        DataOnlineExists.IsOnline = 0;
                                        DataOnlineExists.NewDataTime = DTFinish;
                                        DataOnlineExists.OffLineTime = Convert.ToInt32(ts.TotalMinutes);
                                    }
                                }
                                else
                                {
                                    //站点在线
                                    if (DataOnlineExists == null)//新增
                                    {
                                        InstrumentDataOnline NewDataOnline = new InstrumentDataOnline();
                                        NewDataOnline.InstrumentUid = InUid;
                                        NewDataOnline.InstrumentName = InName;
                                        NewDataOnline.DataTypeUid = DataTypeUid;
                                        NewDataOnline.IsOnline = 1;
                                        NewDataOnline.NewDataTime = DTFinish;
                                        NewDataOnline.OffLineTime = null;
                                        MonitorBusinessModel.Add(NewDataOnline);
                                    }
                                    else//更新
                                    {
                                        DataOnlineExists.IsOnline = 1;
                                        DataOnlineExists.NewDataTime = DTFinish;
                                        DataOnlineExists.OffLineTime = null;
                                    }
                                }

                            }

                        }
                        #endregion
                        MonitorBusinessModel.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("------------------------------------------InstrumentOnline异常:" + ex.ToString());
            }

        }
      /// <summary>
      /// 获取城市摄影数据的最新的时间
      /// </summary>
      /// <param name="CityShootGuid">城市摄影仪器的Guid</param>
      /// <returns>城市摄影仪器的最新时间</returns>
        private DataTable GetCityShootNewestData(string CityShootGuid)
        {
          try
          {
            DataTable dt = new DataTable();
            dt.Columns.Add("Tstamp", typeof(DateTime));
            string cityShootUrl = System.Configuration.ConfigurationManager.AppSettings["CityShootUrl"];
            DataTable dtCityShoot = d_DAL.GetInstrumentData(CityShootGuid);
            if (dtCityShoot != null && dtCityShoot.Rows.Count > 0)
            {
              var query = from a in
                            (from f in Directory.GetDirectories(cityShootUrl)
                             let fi = new FileInfo(f)
                             
                             orderby GetDate(fi.Name) descending
                             select fi.Name)
                          from mm in Directory.GetFiles(cityShootUrl + "/" + a)
                          let mfi = new FileInfo(mm)
                          where IsOrNotImg(mfi.Name)
                          orderby GetCityDate(mfi.Name.Substring(2, 19)) descending
                          select a + "/" + mfi.Name
                        ;
              string[] childfilelist =query.ToArray().Distinct().ToArray();
             
              if (childfilelist != null && childfilelist.Length > 0)
              {
                DataRow dr = dt.NewRow();
                dr["Tstamp"] = GetCityDate(childfilelist[0].Substring(13, 19)).ToString("yyyy-MM-dd HH:mm:ss");

                dt.Rows.Add(dr);
              }
              
            }
            return dt;
          }
          catch (Exception ex)
          {
            log.Error("------------------------------------------GetCityShootNewestData异常:" + ex.ToString());
            throw ex;
          }
        }
      /// <summary>
      /// 查看文件是否是图片
      /// </summary>
      /// <param name="str">文件名</param>
      /// <returns>true/false</returns>
        protected bool IsOrNotImg(string str)
        {
          try
          {
            if (str.ToLower().Contains("bmp") || str.ToLower().Contains("jpeg") || str.ToLower().Contains("jpg") || str.ToLower().Contains("png") || str.ToLower().Contains("svg"))
            {
              return true;
            }
            else
            {
              return false;
            }
          }
          catch (Exception ex)
          {
            log.Error("------------------------------------------IsOrNotImg异常:" + ex.ToString());
            throw ex;
          }
        }
      /// <summary>
      /// 截取城市摄影图片的文件名中的时间
      /// </summary>
      /// <param name="str">城市摄影文件名</param>
      /// <returns>文件名中的时间</returns>
        private DateTime GetCityDate(string str)
        {
          try
          {
            DateTime dt = DateTime.Parse("1900-01-01 00:00:00");
            if (str.Length == 19)
            {
              //IFormatProvider format = new System.Globalization.CultureInfo("zh-CN");
              string FormatStr = "yyyy-MM-dd HH-mm-ss";
              dt = DateTime.ParseExact(str, FormatStr, CultureInfo.InvariantCulture);
            }
            return dt;
          }
          catch (Exception ex)
          {
            log.Error("------------------------------------------GetCityDate异常:" + ex.ToString());
            throw ex;
          }

        }
        /// <summary>
        /// 将日期格式的字符串转为日期输出
        /// </summary>
        /// <param name="str">日期格式的字符串yyyyMMdd</param>
        /// <returns></returns>
        public DateTime GetDate(string str)
        {
          DateTime dt = DateTime.Parse("1900-01-01");
          if (str.Length == 10)
          {
            //IFormatProvider format = new System.Globalization.CultureInfo("zh-CN");
            string FormatStr = "yyyy-MM-dd";
            dt = DateTime.ParseExact(str, FormatStr, CultureInfo.InvariantCulture);
          }
          return dt;
        }
        /// <summary>
        /// 执行DataTable中的查询返回新的DataTable
        /// </summary>
        /// <param name="dt">源数据DataTable</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public  DataTable GetNewDataTable(DataTable dt, string condition)
        {
            DataTable newdt = new DataTable();
            try
            {
                newdt = dt.Clone();
                DataRow[] dr = dt.Select(condition);
                for (int i = 0; i < dr.Length; i++)
                {
                    newdt.ImportRow((DataRow)dr[i]);
                }
                return newdt;//返回的查询结果
            }
            catch (Exception ex)
            {
                log.Error("--------------------------------------------GetNewDataTable异常：" + ex.ToString());
                return newdt;
            }
        }
        /// <summary>
        /// 抓取上海发布平台实时AQI
        /// </summary>
        public void ShangHaiRealAQI()
        {
            try
            {
                string Mydata = "";
                string serviceAddress = "http://www.semc.gov.cn/aqi/Home/RealTimeAirQualityChange";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceAddress);

                request.Method = "POST";
                request.ContentType = "application/json";
                StreamWriter dataStream = new StreamWriter(request.GetRequestStream());
                dataStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string encoding = response.ContentEncoding;
                if (encoding == null || encoding.Length < 1)
                {
                    encoding = "UTF-8"; //默认编码  
                }
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                Mydata = reader.ReadToEnd();
                DateTime time = DateTime.Now.AddMinutes(-25);

                Regex regexR = new Regex(@"(?<=[)([\s\S]*?)(?=]})");
                MatchCollection mcR = regexR.Matches(Mydata);
                Regex regexD = new Regex(@"(?<={)([\s\S]*?)(?=})");
                MatchCollection mcD = regexD.Matches(mcR[0].ToString());
                string[] arr=mcD[mcD.Count - 1].Value.ToString().Split(',');

                #region yaunfangfa
                //WebClient MyWebClient = new WebClient();
                //string Mydata = "";
                //string Mydata2 = "";
                //string data = "";

                //DateTime time = DateTime.Now.AddMinutes(-25);
                //MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据

                //Byte[] pageData = MyWebClient.DownloadData("http://www.semc.gov.cn/aqi/home/Index.aspx"); //从指定网站下载数据
                //string pageHtml = Encoding.UTF8.GetString(pageData); //如果获取网站页面采用的是UTF-8，则使用这句  "D:\\项目\\南通超级站\\代码\\CatchData\\CatchData\\Files\\ouput.html"
                //string url = ConfigurationManager.AppSettings["url"];
                //List<string> list = new List<string>();
                //Regex regexArea = new Regex(@"(?<=实时空气质量状况)([\s\S]*?)(?=过去24小时AQI)", RegexOptions.Multiline | RegexOptions.ExplicitCapture);//构造解析从“实时空气质量状况”到“过去24小时AQI”之间字符串的正则表达式
                //MatchCollection mcArea = regexArea.Matches(pageHtml); //执行匹配
                //foreach (Match mArea in mcArea)
                //{
                //    Regex regexR = new Regex(@"(?<=<p)([\s\S]*?)(?=</p>)");//构造解析表格行数据的正则表达式
                //    MatchCollection mcR = regexR.Matches(mArea.Groups[0].ToString()); //执行匹配
                //    foreach (Match mr in mcR)
                //    {
                //        Regex regexD = new Regex(@"(?<=<td[^>]*>[\s]*?)([\S]*)(?=[\s]*?</td>)");//构造解析表格列数据的正则表达式
                //        Regex regexSpan = new Regex(@"(?<=<span[^>]*>[\s]*?)([\S]*)(?=[\s]*?</span>)");//正则表达式获取SPAN (?i)(?<=<span.*?id=""s1"".*?>)[^<]+(?=</span>)
                //        MatchCollection mcD = regexD.Matches(mr.Groups[0].ToString()); //执行匹配
                //        MatchCollection mcS = regexSpan.Matches(mr.Groups[0].ToString());

                //        for (int i = 0; i < mcD.Count; i++)
                //        {
                //            Mydata += mcD[i].Value + "/";
                //        }

                //        for (int k = 0; k < mcS.Count; k++)
                //        {
                //            data += mcS[k].Value + ",";
                //        }
                //    }
                //}
                //string[] dt = data.Trim(',').Split(',');
                //for (int i = 0; i < dt.Length; i++)
                //{
                //    if (dt[i] == "优")
                //    {
                //        Mydata = "无/" + Mydata;
                //    }
                //    if (i == 1 || i == 3)
                //    {
                //        Mydata2 += dt[i] + "/";
                //    }
                //}

                //Mydata += Mydata2;
                //list.Add(Mydata);
                #endregion
                #region 向表里增加数据
                using (AirAutoMonitorModel AirAutoMonitorModel = new AirAutoMonitorModel())
                {
                    TB_RealAQINearby RealAQINearbyExists = AirAutoMonitorModel.TB_RealAQINearbies.Where(p => p.City.ToLower().Equals("shanghai") && p.DateTime.Equals(time) && p.Data.Equals(Mydata)).FirstOrDefault();
                    if (RealAQINearbyExists == null)//新增
                    {
                        TB_RealAQINearby NewRealAQINearby = new TB_RealAQINearby();
                        NewRealAQINearby.City = "shanghai";
                        NewRealAQINearby.DateTime = time;
                        //NewRealAQINearby.Data = mcR[0].Value.ToString();
                        NewRealAQINearby.Data = "";
                        NewRealAQINearby.CreatUser = "SystemSync";
                        NewRealAQINearby.CreatDateTime = DateTime.Now;
                        AirAutoMonitorModel.Add(NewRealAQINearby);
                        AirAutoMonitorModel.SaveChanges();
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }

        /// <summary>
        /// 计算当前时刻voc汇总数据
        /// </summary>
        public void VOCStatistics(DateTime time)
        {
            try
            {
                #region 获取因子列表

                /////一级类别因子
                ////TVOC总值
                //DataTable dtAll = d_DAL.GetVOC1PollutantCode("af6c560e-07b2-422d-8ea6-ec9dc1ca3e91");
                ////卤代烃类
                //DataTable dtl = d_DAL.GetVOC1PollutantCode("0a153ee0-c7c3-4782-953f-74db4b4c5396");
                ////非甲烷碳氢化合物
                //DataTable dtf = d_DAL.GetVOC1PollutantCode("0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1");
                ////含氧(氮)类
                //DataTable dth = d_DAL.GetVOC1PollutantCode("219651d8-3463-4de8-941e-a38aae42bf48");
                /////二级类别因子
                ////低碳烷烃
                //DataTable dtDW = d_DAL.GetVOC2PollutantCode("fb1fc34b-770f-4141-b75a-015919725e0b");
                ////低碳烯烃
                //DataTable dtDX = d_DAL.GetVOC2PollutantCode("a41db3c8-0fb4-4b0e-b1b0-e5950f04eff6");
                ////高碳烷烃
                //DataTable dtGW = d_DAL.GetVOC2PollutantCode("06a02408-6eab-4188-b442-86dd8e97654c");
                ////高碳烯烃
                //DataTable dtGX = d_DAL.GetVOC2PollutantCode("7bc5f3d8-7cb3-4273-a646-1ed7df60cdd8");
                ////炔烃
                //DataTable dtQT = d_DAL.GetVOC2PollutantCode("5b1918b9-7c92-477a-8e23-64cbae6477f6");
                ////苯系物
                //DataTable dtBXW = d_DAL.GetVOC2PollutantCode("e9607fce-75dc-4134-9a8d-af2a1eb4a7bf");
                ////氟利昂
                //DataTable dtFLA = d_DAL.GetVOC2PollutantCode("053c74fd-d1ae-4341-b258-1788079970bd");
                ////卤代烷烃
                //DataTable dtLW = d_DAL.GetVOC2PollutantCode("a0bad5d7-9eec-4fa4-9c36-828aad78041d");
                ////卤代烯烃
                //DataTable dtLX = d_DAL.GetVOC2PollutantCode("21de4143-2c28-4256-b71e-6cb5ce63e417");
                ////卤代芳香烃
                //DataTable dtLF = d_DAL.GetVOC2PollutantCode("1eaac416-0d69-48b9-aca1-9ff7904907bb");
                ////醛类有机物
                //DataTable dtQL = d_DAL.GetVOC2PollutantCode("8198b6fc-7a77-427d-8e3e-9c9228ac168c");
                ////酮类有机物
                //DataTable dtTL = d_DAL.GetVOC2PollutantCode("8c9ce5f3-4716-485e-95e1-72608b2843ce");
                ////醚类有机物
                //DataTable dtML = d_DAL.GetVOC2PollutantCode("e5f83fd9-0b77-4d1b-935f-1826fddcc343");
                ////含氮有机物
                //DataTable dtHD = d_DAL.GetVOC2PollutantCode("3bbe4b30-53e4-48a8-a884-c3b38a03b705");

                #endregion

                #region 转换为一维字符串数组
                ////一级类
                //string[] factorsAll = dtToArr(dtAll);
                //string[] factorsl = dtToArr(dtl);
                //string[] factorsf = dtToArr(dtf);
                //string[] factorsh = dtToArr(dth);
                #endregion
                ///pbb单位数据
                //TVOC总值
                UpdateVOC(time,"af6c560e-07b2-422d-8ea6-ec9dc1ca3e91");
                //非甲烷碳氢化合物
                UpdateVOC(time,"0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1");
                //卤代烃类
                UpdateVOC(time,"0a153ee0-c7c3-4782-953f-74db4b4c5396");
                //含氧(氮)类
                UpdateVOC(time,"219651d8-3463-4de8-941e-a38aae42bf48");
                //低碳烷烃
                UpdateVOC(time,"0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1", "fb1fc34b-770f-4141-b75a-015919725e0b");
                //低碳烯烃
                UpdateVOC(time,"0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1", "a41db3c8-0fb4-4b0e-b1b0-e5950f04eff6");
                //高碳烷烃
                UpdateVOC(time,"0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1", "06a02408-6eab-4188-b442-86dd8e97654c");
                //高碳烯烃
                UpdateVOC(time,"0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1", "7bc5f3d8-7cb3-4273-a646-1ed7df60cdd8");
                //炔烃
                UpdateVOC(time,"0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1", "5b1918b9-7c92-477a-8e23-64cbae6477f6");
                //苯系物
                UpdateVOC(time,"0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1", "e9607fce-75dc-4134-9a8d-af2a1eb4a7bf");
                //氟利昂
                UpdateVOC(time,"0a153ee0-c7c3-4782-953f-74db4b4c5396", "053c74fd-d1ae-4341-b258-1788079970bd");
                //卤代烷烃
                UpdateVOC(time,"0a153ee0-c7c3-4782-953f-74db4b4c5396", "a0bad5d7-9eec-4fa4-9c36-828aad78041d");
                //卤代烯烃
                UpdateVOC(time,"0a153ee0-c7c3-4782-953f-74db4b4c5396", "21de4143-2c28-4256-b71e-6cb5ce63e417");
                //卤代芳香烃
                UpdateVOC(time,"0a153ee0-c7c3-4782-953f-74db4b4c5396", "1eaac416-0d69-48b9-aca1-9ff7904907bb");
                //醛类有机物
                UpdateVOC(time,"219651d8-3463-4de8-941e-a38aae42bf48", "8198b6fc-7a77-427d-8e3e-9c9228ac168c");
                //酮类有机物
                UpdateVOC(time,"219651d8-3463-4de8-941e-a38aae42bf48", "8c9ce5f3-4716-485e-95e1-72608b2843ce");
                //醚类有机物
                UpdateVOC(time,"219651d8-3463-4de8-941e-a38aae42bf48", "e5f83fd9-0b77-4d1b-935f-1826fddcc343");
                //含氮有机物
                UpdateVOC(time,"219651d8-3463-4de8-941e-a38aae42bf48", "3bbe4b30-53e4-48a8-a884-c3b38a03b705");
                ///ug/m3单位数据
                //TVOC总值
                UpdateVOCU(time, "af6c560e-07b2-422d-8ea6-ec9dc1ca3e91");
                //非甲烷碳氢化合物
                UpdateVOCU(time, "0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1");
                //卤代烃类
                UpdateVOCU(time, "0a153ee0-c7c3-4782-953f-74db4b4c5396");
                //含氧(氮)类
                UpdateVOCU(time, "219651d8-3463-4de8-941e-a38aae42bf48");
                //低碳烷烃
                UpdateVOCU(time, "0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1", "fb1fc34b-770f-4141-b75a-015919725e0b");
                //低碳烯烃
                UpdateVOCU(time, "0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1", "a41db3c8-0fb4-4b0e-b1b0-e5950f04eff6");
                //高碳烷烃
                UpdateVOCU(time, "0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1", "06a02408-6eab-4188-b442-86dd8e97654c");
                //高碳烯烃
                UpdateVOCU(time, "0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1", "7bc5f3d8-7cb3-4273-a646-1ed7df60cdd8");
                //炔烃
                UpdateVOCU(time, "0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1", "5b1918b9-7c92-477a-8e23-64cbae6477f6");
                //苯系物
                UpdateVOCU(time, "0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1", "e9607fce-75dc-4134-9a8d-af2a1eb4a7bf");
                //氟利昂
                UpdateVOCU(time, "0a153ee0-c7c3-4782-953f-74db4b4c5396", "053c74fd-d1ae-4341-b258-1788079970bd");
                //卤代烷烃
                UpdateVOCU(time, "0a153ee0-c7c3-4782-953f-74db4b4c5396", "a0bad5d7-9eec-4fa4-9c36-828aad78041d");
                //卤代烯烃
                UpdateVOCU(time, "0a153ee0-c7c3-4782-953f-74db4b4c5396", "21de4143-2c28-4256-b71e-6cb5ce63e417");
                //卤代芳香烃
                UpdateVOCU(time, "0a153ee0-c7c3-4782-953f-74db4b4c5396", "1eaac416-0d69-48b9-aca1-9ff7904907bb");
                //醛类有机物
                UpdateVOCU(time, "219651d8-3463-4de8-941e-a38aae42bf48", "8198b6fc-7a77-427d-8e3e-9c9228ac168c");
                //酮类有机物
                UpdateVOCU(time, "219651d8-3463-4de8-941e-a38aae42bf48", "8c9ce5f3-4716-485e-95e1-72608b2843ce");
                //醚类有机物
                UpdateVOCU(time, "219651d8-3463-4de8-941e-a38aae42bf48", "e5f83fd9-0b77-4d1b-935f-1826fddcc343");
                //含氮有机物
                UpdateVOCU(time, "219651d8-3463-4de8-941e-a38aae42bf48", "3bbe4b30-53e4-48a8-a884-c3b38a03b705");
                #region 整合前代码
                //decimal? sum=0;
                ////TVOCS
                ////DateTime date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00:00"));
                //DataTable dtsAll = d_DAL.GetDataPagerVOCs("204", factorsAll, DateTime.Now, DateTime.Now, "PointId asc,tstamp desc");
                //dtsAll.Columns.Remove("PointId");
                //dtsAll.Columns.Remove("Tstamp");
                //DateTime date = DateTime.Now;
                //if (dtsAll.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dtsAll.Columns.Count; i++)
                //    {
                //        sum += dtsAll.AsEnumerable().Select(t => t.Field<decimal?>(dtsAll.Columns[i].ColumnName)).Sum();
                //    }
                //}
                //d_DAL.AddVOCStatistics("af6c560e-07b2-422d-8ea6-ec9dc1ca3e91", date, sum, DateTime.Now);

                ////卤代烃类
                //DataTable dtsl = d_DAL.GetDataPagerVOCs("204", factorsl, DateTime.Now, DateTime.Now, "PointId asc,tstamp desc");
                //dtsl.Columns.Remove("PointId");
                //dtsl.Columns.Remove("Tstamp");
                //if (dtsl.Rows.Count > 0)
                //{
                //    sum = 0;
                //    for (int i = 0; i < dtsl.Columns.Count; i++)
                //    {
                //        sum += dtsl.AsEnumerable().Select(t => t.Field<decimal?>(dtsl.Columns[i].ColumnName)).Sum();
                //    }
                //}
                //d_DAL.AddVOCStatistics("0a153ee0-c7c3-4782-953f-74db4b4c5396", date, sum, DateTime.Now);

                ////非甲烷碳氢化合物
                //DataTable dtsf = d_DAL.GetDataPagerVOCs("204", factorsf, DateTime.Now, DateTime.Now, "PointId asc,tstamp desc");
                //dtsf.Columns.Remove("PointId");
                //dtsf.Columns.Remove("Tstamp");
                //if (dtsf.Rows.Count > 0)
                //{
                //    sum = 0;
                //    for (int i = 0; i < dtsf.Columns.Count; i++)
                //    {
                //        sum += dtsf.AsEnumerable().Select(t => t.Field<decimal?>(dtsf.Columns[i].ColumnName)).Sum();
                //    }
                //}
                //d_DAL.AddVOCStatistics("0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1", date, sum, DateTime.Now);

                ////非甲烷碳氢化合物
                //DataTable dtsh = d_DAL.GetDataPagerVOCs("204", factorsh, DateTime.Now, DateTime.Now, "PointId asc,tstamp desc");
                //dtsh.Columns.Remove("PointId");
                //dtsh.Columns.Remove("Tstamp");
                //if (dtsh.Rows.Count > 0)
                //{
                //    sum = 0;
                //    for (int i = 0; i < dtsh.Columns.Count; i++)
                //    {
                //        sum += dtsh.AsEnumerable().Select(t => t.Field<decimal?>(dtsh.Columns[i].ColumnName)).Sum();
                //    }
                //}
                //d_DAL.AddVOCStatistics("219651d8-3463-4de8-941e-a38aae42bf48", date, sum, DateTime.Now);
                #endregion
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        /// <summary>
        /// 同步历史VOC汇总数据
        /// </summary>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        public void FillVOCStatisticData(string sTime, string eTime)
        {
            DateTime startTime = Convert.ToDateTime(sTime);
            //DateTime endTime = Convert.ToDateTime(eTime).AddDays(1).AddMinutes(-1);
            DateTime endTime = Convert.ToDateTime(eTime).AddDays(1).AddMinutes(-1) > DateTime.Now ? DateTime.Now.AddHours(-1) : Convert.ToDateTime(eTime).AddDays(1).AddMinutes(-1);
            for (DateTime time = startTime; time <= endTime.AddHours(1); time = time.AddHours(1))
            {
                try
                {
                    VOCStatistics(time);
                }
                catch (Exception ex)
                {
                    log.Error(ex.ToString());
                    continue;
                }
            }
        }

        /// <summary>
        /// DataTable转换为一维字符串数组
        /// </summary>
        /// <returns></returns>
        public static string[] dtToArr(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return new string[0];
            }
            else
            {
                string[] sr = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.IsDBNull(dt.Rows[i][0]))
                    {
                        sr[i] = "";
                    }
                    else
                    {
                        sr[i] = dt.Rows[i][0] + "";
                    }
                }
                return sr;
            }
        }
        //复用代码整合（更新一级类VOC数据）
        public void UpdateVOC(DateTime time,string ItemGuid)
        {
            try
            {
                DataTable dts = d_DAL.GetVOC1PollutantCode(ItemGuid);
                string[] factors = dtToArr(dts);
                DataTable dt = d_DAL.GetDataPagerVOCs("204", factors, time, time, "PointId asc,tstamp desc");
                dt.Columns.Remove("PointId");
                dt.Columns.Remove("Tstamp");
                //decimal? sum as object;
                //amc_dbtamt = DbNull.Value
                decimal? sum = null;
                //DateTime date = DateTime.Now;
                if (dt.Rows.Count > 0)
                {
                    sum = 0;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        sum += dt.AsEnumerable().Select(t => Convert.ToDecimal(t.Field<String>(dt.Columns[i].ColumnName))).Sum();
                    }
                }
                d_DAL.AddVOCStatistics(ItemGuid, time, sum, DateTime.Now);
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        //复用代码整合（更新一级类VOC数据）
        public void UpdateVOCU(DateTime time, string ItemGuid)
        {
            try
            {
                DataTable dts = d_DAL.GetVOC1PollutantCodeU(ItemGuid);
                string[] factors = dtToArr(dts);
                DataTable dt = d_DAL.GetDataPagerVOCs("204", factors, time, time, "PointId asc,tstamp desc");
                dt.Columns.Remove("PointId");
                dt.Columns.Remove("Tstamp");
                decimal? sum = null;
                if (dt.Rows.Count > 0)
                {
                    sum = 0;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        sum += dt.AsEnumerable().Select(t => Convert.ToDecimal(t.Field<String>(dt.Columns[i].ColumnName))).Sum();
                    }
                }
                d_DAL.AddVOCStatisticsU(ItemGuid, time, sum, DateTime.Now);
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        //复用代码整合（更新二级类VOC数据）
        public void UpdateVOC(DateTime time, string MainGuid, string ItemGuid)
        {
            try
            {
                DataTable dts = d_DAL.GetVOC2PollutantCode(ItemGuid);
                string[] factors = dtToArr(dts);
                DataTable dt = d_DAL.GetDataPagerVOCs("204", factors, time, time, "PointId asc,tstamp desc");
                dt.Columns.Remove("PointId");
                dt.Columns.Remove("Tstamp");
                decimal? sum = null;
                if (dt.Rows.Count > 0)
                {
                    sum = 0;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        sum += dt.AsEnumerable().Select(t => Convert.ToDecimal(t.Field<String>(dt.Columns[i].ColumnName))).Sum();
                    }
                }
                d_DAL.AddVOCStatistics(MainGuid, ItemGuid, time, sum, DateTime.Now);
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        //复用代码整合（更新二级类VOC数据）
        public void UpdateVOCU(DateTime time, string MainGuid, string ItemGuid)
        {
            try
            {
                DataTable dts = d_DAL.GetVOC2PollutantCodeU(ItemGuid);
                string[] factors = dtToArr(dts);
                DataTable dt = d_DAL.GetDataPagerVOCs("204", factors, time, time, "PointId asc,tstamp desc");
                dt.Columns.Remove("PointId");
                dt.Columns.Remove("Tstamp");
                decimal? sum = null;
                //DateTime date = DateTime.Now;
                if (dt.Rows.Count > 0)
                {
                    sum = 0;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        sum += dt.AsEnumerable().Select(t => Convert.ToDecimal(t.Field<String>(dt.Columns[i].ColumnName))).Sum();
                    }
                }
                d_DAL.AddVOCStatisticsU(MainGuid, ItemGuid, time, sum, DateTime.Now);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        #endregion

        #region 导入历史数据
        public void fillhistorydata(DateTime tstamp, int FillDays)
        {
            try
            {
                log.Info("----------------线程同步开始------------");

                DateTime startTime = tstamp;
                DateTime endTime = tstamp.AddDays(FillDays).AddSeconds(-1) > DateTime.Now ? DateTime.Now.AddHours(-1) : tstamp.AddDays(FillDays).AddSeconds(-1);

                string sTime = startTime.ToString("yyyy-MM-dd");
                string eTime = tstamp.AddDays(FillDays).AddSeconds(-1) > DateTime.Now ? DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:59:59") : endTime.ToString("yyyy-MM-dd 23:59:59");
                DateTime dts = DateTime.Now;
                //删除原有接口接入数据，原始表数据
                d_DAL.DeleteOriDataByTime(startTime, endTime);
                //删除原有小时计算表数据和小时报表数据
                d_DAL.DeleteAuditDataByTime(startTime, endTime);
                //接入常规站历史数据
                FillData(sTime, eTime);
                //批量处理insert
                FillProcessData();
                log.Info("生成原始数据成功");
                CalculateBy60(Convert.ToDateTime(sTime), Convert.ToDateTime(eTime));
                log.Info("计算AQI、日数据成功");
                CalculateByDay(Convert.ToDateTime(sTime), Convert.ToDateTime(eTime), Convert.ToDateTime(sTime), Convert.ToDateTime(eTime));
                log.Info("月数据、周数据同步成功");
                CalculateByMonth(Convert.ToDateTime(sTime), Convert.ToDateTime(eTime), Convert.ToDateTime(sTime), Convert.ToDateTime(eTime));
                log.Info("季数据、年数据同步成功");
                DateTime dte = DateTime.Now;
                TimeSpan ts = dte - dts;
                var minutes = ts.Minutes;
                log.Info(startTime + "~" + endTime + "数据同步成功！时长：" + minutes + "分钟");
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        #endregion


    }
}
