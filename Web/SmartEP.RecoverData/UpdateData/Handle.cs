﻿using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.AirAutoMonitoring;
using SmartEP.DomainModel;
using SmartEP.DomainModel.MonitoringBusiness;

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
        ILog log = LogManager.GetLogger("FileLogging");//获取一个日志记录器
        //BaseDataModel BaseDataModel = new BaseDataModel();
        //AirAutoMonitoringModel AirAutoMonitoringModel = new AirAutoMonitoringModel();
        //FrameworkModel FrameworkModel = new FrameworkModel();
        //MonitoringBusinessModel MonitoringBusinessModel = new MonitoringBusinessModel();
        string ApplicationUid = "airaaira-aira-aira-aira-airaairaaira";
        string ApplicationName = "气";
        int DataDelayTime = Convert.ToInt32(ConfigurationManager.AppSettings["DataDelayTime"]);
        DAL d_DAL = new DAL();
        AQICalculateService d_AQICalculateService = new AQICalculateService();
        string MData = string.Empty;
        //获取数据库中所有因子
        string[] WSCodes = new BaseDataModel().SYS_Factors_MappingEntities.OrderBy(p => p.Id).Select(p => p.WSCode).ToArray();

        //获取所有站点
        int[] PointIds = new BaseDataModel().MonitoringPointEntities.Select(p => p.PointId).ToArray();
        //常规站点
        int[] CommonPointIds = new BaseDataModel().V_Point_AirEntities.Where(p => p.EnableOrNot.Equals(true)).Where(p => p.SuperOrNot.Equals(false)).Select(p => p.PointId).ToArray();
        //超级站点
        int[] SuperPointIds = new BaseDataModel().V_Point_AirEntities.Where(p => p.EnableOrNot.Equals(true)).Where(p => p.SuperOrNot.Equals(true)).Select(p => p.PointId).ToArray();

        //获取数据类型
        V_CodeMainItemEntity[] DataTypeCodeMainItems = new FrameworkModel().V_CodeMainItemEntities.Where(p => p.MainGuid.Equals("0d24484d-d315-4f12-b2bc-f64552c4f6dd")).ToArray();

        //获取审核/报警字典表信息
        V_CodeMainItemEntity[] FlagTypeCodeMainItems = new FrameworkModel().V_CodeMainItemEntities.Where(p => p.MainGuid.Equals("3e8f63ea-64ea-4c23-8aba-c369129dde13")).ToArray();

        //获取报警类型字典表信息
        V_CodeMainItemEntity[] AlarmTypes = new FrameworkModel().V_CodeMainItemEntities.Where(p => p.MainGuid.Equals("7d5c609b-cc69-498f-a22e-183014c8f099")).ToArray();

        //获取空气点位区域类型Guid
        string[] m_regions = new FrameworkModel().V_CodeDictionaryEntities.Where(p => p.CodeDictionaryName.Equals("空气点位区域类型") && p.CodeName.Equals("空气点位区域类型")).OrderByDescending(p => p.SortNumber).Select(p => p.ItemGuid).ToArray();

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
            //Analysis("2017-07-31 22:59:59", n);
        }
        /// <summary>
        /// 填补数据
        /// </summary>
        public void AutoFillData()
        {
            try
            {
                DateTime startTime = Convert.ToDateTime(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd 00:00:00"));//获取前上周0点
                DateTime endTime = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));//获取昨天23点
                for (DateTime time = startTime; time <= endTime; time = time.AddHours(1))
                {
                    int n = 0;
                    Analysis(time.ToString(), n);
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
        public void FillData(string sTime, string eTime, string pid, string[] factory,out string MissData)
        {
            MissData = "接口该处无数据:";
            try
            {
                int n = 0;
                DateTime startTime = Convert.ToDateTime(sTime);
                DateTime endTime = Convert.ToDateTime(eTime);
                for (DateTime time = startTime; time <= endTime; time = time.AddHours(1))
                {
                    Analysis(time.ToString(), n, pid, factory,out MData);
                    MissData +=MData;
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
        public void Analysis(string dateTime, int n, string pid, string[] factory,out string MissData)
        {
            MissData = string.Empty;
            try
            {
                //调用数据接口
                UpdateData.ServiceReference1.AirQualityWebServiceSoapClient client = new UpdateData.ServiceReference1.AirQualityWebServiceSoapClient();
                UpdateData.ServiceReference1.MySoapHeader header = new UpdateData.ServiceReference1.MySoapHeader();
                header.UserId = UserId;
                header.Password = Password;
                //string Factors = string.Join(",", WSCodes);//字符串连接

                StringBuilder sbb = new StringBuilder();
                foreach (string fac in factory)
                {
                    var factors=new BaseDataModel().SYS_Factors_MappingEntities.Where(p => p.LocalCode.Equals(fac)).Select(p => p.WSCode).ToArray();
                    if (factors.Length >0)
                    {
                        string Fac = factors[0];
                        sbb.Append(Fac + ",");
                    }
                }
                //获取所有站点所有因子小时数据
                DataSet ds = client.getHourlyDataSet(header, sbb.ToString().TrimEnd(','), dateTime);
                DataTable dt = ds.Tables[0];
                #region 删除没有数据的行
                DataTable dtNew = ds.Tables[0];
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

                //删除没有数据的行
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    bool flag = true;//标识此行数据是否都为空
                    for (int iCol = 0; iCol < dt.Columns.Count; iCol++)
                    {
                        if (dt.Columns[iCol].ColumnName != "StationId" && dt.Columns[iCol].ColumnName != "StationName" && dt.Columns[iCol].ColumnName != "DateTime" && dt.Rows[iRow][iCol] != DBNull.Value)//遍历到因子列
                        {
                            flag = false;//该行存在非空数据
                            break;//跳出列循环
                        }
                    }
                    if (flag == true)
                    {
                        log.Info("接口该行没数据：" + dt.Rows[iRow]["StationName"] + dt.Rows[iRow]["DateTime"]);
                        //删除该行
                        MissData += dt.Rows[iRow]["StationName"].ToString() + dt.Rows[iRow]["DateTime"].ToString() + " ";
                        dt.Rows[iRow].Delete();
                        
                    }
                    else
                    {
                        continue;//继续循环下一行
                    }
                }
                dt.AcceptChanges();//提交修改
                #endregion

                using (AirAutoMonitoringModel AirAutoMonitoringModel = new AirAutoMonitoringModel())
                {
                    using (BaseDataModel BaseDataModel = new BaseDataModel())
                    {
                        for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                        {
                            //接口上传的站点id
                            string WSPointId = dt.Rows[iRow]["StationId"].ToString();
                            //到站点映射表里查找数据库对应的id
                            string LocalPointId = BaseDataModel.SYS_Point_MappingEntities.Where(p => p.WSPointId == WSPointId).Select(p => p.LocalPointId).FirstOrDefault();
                            if (string.IsNullOrWhiteSpace(LocalPointId))
                            {
                                continue;
                            }
                            //获取该站点因子对应配置
                            IQueryable<SYS_PointsFactors_MappingEntity> PointsFactors_Mappings = BaseDataModel.SYS_PointsFactors_MappingEntities.Where(p => p.WSPointId == WSPointId);
                            //获取站点监测项
                            string[] WSCodesByPoint = PointsFactors_Mappings.Select(p => p.WSCode).ToArray();

                            for (int iCol = 0; iCol < dt.Columns.Count; iCol++)
                            {
                                string WSCode = dt.Columns[iCol].ColumnName.Replace("Value", "");
                                if (WSCodesByPoint.Contains(WSCode))//遍历到因子列
                                {
                                    //到因子映射表里查找数据库对应的code
                                    string LocalCode = BaseDataModel.SYS_Factors_MappingEntities.Where(p => p.WSCode == WSCode).Select(p => p.LocalCode).ToArray()[0];
                                    if (!string.IsNullOrWhiteSpace(LocalPointId) && !string.IsNullOrWhiteSpace(LocalCode))
                                    {
                                        int localPointId = Convert.ToInt32(LocalPointId);
                                        DateTime tstamp = Convert.ToDateTime(dt.Rows[iRow]["DateTime"]);



                                        //查找数据库中是否已存在该时间点、该站点、该因子数据
                                        InfectantBy60BufferEntity InfectantBy60BufferExist = AirAutoMonitoringModel.InfectantBy60BufferEntities.Where(p => p.PointId.Equals(localPointId) && p.PollutantCode.Equals(LocalCode) && p.Tstamp.Equals(tstamp)).FirstOrDefault();
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
                                        else
                                        {
                                            #region 新增
                                            InfectantBy60BufferEntity InfectantBy60Buffer = new InfectantBy60BufferEntity();

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
                                            AirAutoMonitoringModel.Add(InfectantBy60Buffer);
                                            #endregion
                                        }

                                    }
                                    iCol++;
                                }
                            }
                        }
                    }
                    AirAutoMonitoringModel.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                n++;
                if (n <= 3)
                {
                    log.Info("第" + n + "次尝试修复");
                    Analysis(dateTime, n, pid, factory,out MissData);

                }

            }

        }

        /// <summary>
        /// 接入小时数据
        /// </summary>
        /// <param name="dateTime">时间</param>
        public void Analysis(string dateTime, int n)
        {
            try
            {
                //调用数据接口
                UpdateData.ServiceReference1.AirQualityWebServiceSoapClient client = new UpdateData.ServiceReference1.AirQualityWebServiceSoapClient();
                UpdateData.ServiceReference1.MySoapHeader header = new UpdateData.ServiceReference1.MySoapHeader();
                header.UserId = UserId;
                header.Password = Password;
                string Factors = string.Join(",", WSCodes);//字符串连接
                //获取所有站点所有因子小时数据
                DataSet ds = client.getHourlyDataSet(header, Factors, dateTime);
                DataTable dt = ds.Tables[0];
                #region 删除没有数据的行
                DataTable dtNew = ds.Tables[0];
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

                //删除没有数据的行
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    bool flag = true;//标识此行数据是否都为空
                    for (int iCol = 0; iCol < dt.Columns.Count; iCol++)
                    {
                        if (dt.Columns[iCol].ColumnName != "StationId" && dt.Columns[iCol].ColumnName != "StationName" && dt.Columns[iCol].ColumnName != "DateTime" && dt.Rows[iRow][iCol] != DBNull.Value)//遍历到因子列
                        {
                            flag = false;//该行存在非空数据
                            break;//跳出列循环
                        }
                    }
                    if (flag == true)
                    {
                        log.Info("接口该行没数据：" + dt.Rows[iRow]["StationName"] + dt.Rows[iRow]["DateTime"]);
                        //删除该行
                        dt.Rows[iRow].Delete();
                    }
                    else
                    {
                        continue;//继续循环下一行
                    }
                }
                dt.AcceptChanges();//提交修改
                #endregion

                using (AirAutoMonitoringModel AirAutoMonitoringModel = new AirAutoMonitoringModel())
                {
                    using (BaseDataModel BaseDataModel = new BaseDataModel())
                    {
                        for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                        {
                            //接口上传的站点id
                            string WSPointId = dt.Rows[iRow]["StationId"].ToString();
                            //到站点映射表里查找数据库对应的id
                            string LocalPointId = BaseDataModel.SYS_Point_MappingEntities.Where(p => p.WSPointId == WSPointId).Select(p => p.LocalPointId).FirstOrDefault();
                            if (string.IsNullOrWhiteSpace(LocalPointId))
                            {
                                continue;
                            }
                            //获取该站点因子对应配置
                            IQueryable<SYS_PointsFactors_MappingEntity> PointsFactors_Mappings = BaseDataModel.SYS_PointsFactors_MappingEntities.Where(p => p.WSPointId == WSPointId);
                            //获取站点监测项
                            string[] WSCodesByPoint = PointsFactors_Mappings.Select(p => p.WSCode).ToArray();

                            for (int iCol = 0; iCol < dt.Columns.Count; iCol++)
                            {
                                string WSCode = dt.Columns[iCol].ColumnName.Replace("Value", "");
                                if (WSCodesByPoint.Contains(WSCode))//遍历到因子列
                                {
                                    //到因子映射表里查找数据库对应的code
                                    string LocalCode = BaseDataModel.SYS_Factors_MappingEntities.Where(p => p.WSCode == WSCode).Select(p => p.LocalCode).ToArray()[0];
                                    if (!string.IsNullOrWhiteSpace(LocalPointId) && !string.IsNullOrWhiteSpace(LocalCode))
                                    {
                                        int localPointId = Convert.ToInt32(LocalPointId);
                                        DateTime tstamp = Convert.ToDateTime(dt.Rows[iRow]["DateTime"]);



                                        //查找数据库中是否已存在该时间点、该站点、该因子数据
                                        InfectantBy60BufferEntity InfectantBy60BufferExist = AirAutoMonitoringModel.InfectantBy60BufferEntities.Where(p => p.PointId.Equals(localPointId) && p.PollutantCode.Equals(LocalCode) && p.Tstamp.Equals(tstamp)).FirstOrDefault();
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
                                        else
                                        {
                                            #region 新增
                                            InfectantBy60BufferEntity InfectantBy60Buffer = new InfectantBy60BufferEntity();

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
                                            AirAutoMonitoringModel.Add(InfectantBy60Buffer);
                                            #endregion
                                        }

                                    }
                                    iCol++;
                                }
                            }
                        }
                    }
                    AirAutoMonitoringModel.SaveChanges();
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

        #region 线程方法(WEB端不适用)
        ///// <summary>
        ///// 每分钟执行
        ///// </summary>
        //public void RunBy1()
        //{
        //    try
        //    {
        //        //log.Info("------------------------------------------------------------RunBy1开始-------------------");
        //        using (FrameworkModel FrameworkModel = new FrameworkModel())
        //        {
        //            V_CodeMainItemEntity DataType = DataTypeCodeMainItems.Where(p => p.ItemValue.Equals("Min1")).FirstOrDefault();
        //            string DataTypeUid = DataType.ItemGuid;
        //            using (BaseDataModel BaseDataModel = new BaseDataModel())
        //            {
        //                DT_ConfigIDInfoEntity IDLogFlagPre = BaseDataModel.DT_ConfigIDInfoEntities.Where(p => p.Memo.Equals("InfectantBy1BufferFlag")).FirstOrDefault();
        //                long IDLogFlag = Convert.ToInt32(IDLogFlagPre.Maxid);
        //                DT_ConfigIDInfoEntity OriIDLogFlagPre = BaseDataModel.DT_ConfigIDInfoEntities.Where(p => p.Memo.Equals("InfectantBy1Flag")).FirstOrDefault();
        //                long OriIDLogFlag = Convert.ToInt32(OriIDLogFlagPre.Maxid);
        //                DT_ConfigIDInfoEntity BufferIDLogBatchProcessPre = BaseDataModel.DT_ConfigIDInfoEntities.Where(p => p.Memo.Equals("InfectantBy1BufferBatchProcess")).FirstOrDefault();
        //                long BufferIDLogBatchProcess = Convert.ToInt32(BufferIDLogBatchProcessPre.Maxid);
        //                DT_ConfigIDInfoEntity IDLogBatchProcessPre = BaseDataModel.DT_ConfigIDInfoEntities.Where(p => p.Memo.Equals("InfectantBy1BatchProcess")).FirstOrDefault();
        //                long IDLogBatchProcess = Convert.ToInt32(IDLogBatchProcessPre.Maxid);

        //                Flag(DataTypeUid, IDLogFlag, OriIDLogFlag);
        //                InstrumentOnline(DataTypeUid);

        //                using (AirAutoMonitoringModel AirAutoMonitoringModel = new AirAutoMonitoringModel())
        //                {
        //                    //获取表中最新ID
        //                    InfectantBy1BufferEntity Last_InfectantBy1Buffer = AirAutoMonitoringModel.InfectantBy1BufferEntities.OrderByDescending(p => p.Id).FirstOrDefault();
        //                    if (Last_InfectantBy1Buffer != null)
        //                    {
        //                        long NewID = Last_InfectantBy1Buffer.Id;
        //                        IDLogFlagPre.Maxid = NewID;
        //                        IDLogFlagPre.Updatedatetime = DateTime.Now;
        //                        BaseDataModel.SaveChanges();

        //                        BatchProcess(DataTypeUid, BufferIDLogBatchProcess, IDLogBatchProcess, NewID);

        //                        BufferIDLogBatchProcessPre.Maxid = NewID;
        //                        BufferIDLogBatchProcessPre.Updatedatetime = DateTime.Now;
        //                        //获取原始表中最新ID
        //                        InfectantBy1Entity Last_InfectantBy1 = AirAutoMonitoringModel.InfectantBy1Entities.OrderByDescending(p => p.Id).FirstOrDefault();
        //                        if (Last_InfectantBy1 != null)
        //                        {
        //                            long OriNewID = Last_InfectantBy1.Id;
        //                            IDLogBatchProcessPre.Maxid = OriNewID;
        //                            IDLogBatchProcessPre.Updatedatetime = DateTime.Now;

        //                            OriIDLogFlagPre.Maxid = OriNewID;
        //                            OriIDLogFlagPre.Updatedatetime = DateTime.Now;

        //                        }

        //                        BaseDataModel.SaveChanges();
        //                    }
        //                }
        //            }
        //        }
        //        //log.Info("-------------------------------------------------------------RunBy1结束-------------------");
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("-------------------------RunBy1异常：" + ex.ToString());
        //    }
        //}
        ///// <summary>
        ///// 每五分钟执行
        ///// </summary>
        //public void RunBy5()
        //{
        //    try
        //    {

        //        //log.Info("-------------------------------------------------------------RunBy5开始-------------------");
        //        using (FrameworkModel FrameworkModel = new FrameworkModel())
        //        {
        //            V_CodeMainItemEntity DataType = DataTypeCodeMainItems.Where(p => p.ItemValue.Equals("Min5")).FirstOrDefault();
        //            string DataTypeUid = DataType.ItemGuid;


        //            using (BaseDataModel BaseDataModel = new BaseDataModel())
        //            {
        //                DT_ConfigIDInfoEntity IDLogFlagPre = BaseDataModel.DT_ConfigIDInfoEntities.Where(p => p.Memo.Equals("InfectantBy5BufferFlag")).FirstOrDefault();
        //                long IDLogFlag = Convert.ToInt32(IDLogFlagPre.Maxid);
        //                DT_ConfigIDInfoEntity OriIDLogFlagPre = BaseDataModel.DT_ConfigIDInfoEntities.Where(p => p.Memo.Equals("InfectantBy5Flag")).FirstOrDefault();
        //                long OriIDLogFlag = Convert.ToInt32(OriIDLogFlagPre.Maxid);
        //                DT_ConfigIDInfoEntity BufferIDLogBatchProcessPre = BaseDataModel.DT_ConfigIDInfoEntities.Where(p => p.Memo.Equals("InfectantBy5BufferBatchProcess")).FirstOrDefault();
        //                long BufferIDLogBatchProcess = Convert.ToInt32(BufferIDLogBatchProcessPre.Maxid);
        //                DT_ConfigIDInfoEntity IDLogBatchProcessPre = BaseDataModel.DT_ConfigIDInfoEntities.Where(p => p.Memo.Equals("InfectantBy5BatchProcess")).FirstOrDefault();
        //                long IDLogBatchProcess = Convert.ToInt32(IDLogBatchProcessPre.Maxid);

        //                Flag(DataTypeUid, IDLogFlag, OriIDLogFlag);
        //                InstrumentOnline(DataTypeUid);//5分钟类型更新

        //                V_CodeMainItemEntity DataTypeMin60 = DataTypeCodeMainItems.Where(p => p.ItemValue.Equals("Min60")).FirstOrDefault();
        //                string DataTypeUidMin60 = DataTypeMin60.ItemGuid;

        //                //log.Info("60分钟仪器在线情况更新");
        //                InstrumentOnline(DataTypeUidMin60);//60分钟类型更新
        //                using (AirAutoMonitoringModel AirAutoMonitoringModel = new AirAutoMonitoringModel())
        //                {
        //                    //获取表中最新ID
        //                    InfectantBy5BufferEntity Last_InfectantBy5Buffer = AirAutoMonitoringModel.InfectantBy5BufferEntities.OrderByDescending(p => p.Id).FirstOrDefault();
        //                    if (Last_InfectantBy5Buffer != null)
        //                    {
        //                        long NewID = Last_InfectantBy5Buffer.Id;
        //                        IDLogFlagPre.Maxid = NewID;
        //                        IDLogFlagPre.Updatedatetime = DateTime.Now;
        //                        BaseDataModel.SaveChanges();

        //                        BatchProcess(DataTypeUid, BufferIDLogBatchProcess, IDLogBatchProcess, NewID);

        //                        BufferIDLogBatchProcessPre.Maxid = NewID;
        //                        BufferIDLogBatchProcessPre.Updatedatetime = DateTime.Now;
        //                        //获取原始表中最新ID
        //                        InfectantBy5Entity Last_InfectantBy5 = AirAutoMonitoringModel.InfectantBy5Entities.OrderByDescending(p => p.Id).FirstOrDefault();
        //                        if (Last_InfectantBy5 != null)
        //                        {
        //                            long OriNewID = Last_InfectantBy5.Id;
        //                            IDLogBatchProcessPre.Maxid = OriNewID;
        //                            IDLogBatchProcessPre.Updatedatetime = DateTime.Now;

        //                            OriIDLogFlagPre.Maxid = OriNewID;
        //                            OriIDLogFlagPre.Updatedatetime = DateTime.Now;

        //                        }

        //                        BaseDataModel.SaveChanges();
        //                    }
        //                }
        //            }
        //        }
        //        //log.Info("-------------------------------------------------------------RunBy5结束-------------------");
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("-------------------------RunBy5异常：" + ex.ToString());
        //    }
        //}
        ///// <summary>
        ///// 每小时执行
        ///// </summary>
        //public void RunBy60()
        //{
        //    try
        //    {
        //        //log.Info("-------------------------------------------------------------RunBy60开始-------------------");
        //        using (FrameworkModel FrameworkModel = new FrameworkModel())
        //        {
        //            V_CodeMainItemEntity DataType = DataTypeCodeMainItems.Where(p => p.ItemValue.Equals("Min60")).FirstOrDefault();
        //            string DataTypeUid = DataType.ItemGuid;
        //            using (BaseDataModel BaseDataModel = new BaseDataModel())
        //            {
        //                DT_ConfigIDInfoEntity IDLogFlagPre = BaseDataModel.DT_ConfigIDInfoEntities.Where(p => p.Memo.Equals("InfectantBy60BufferFlag")).FirstOrDefault();
        //                long IDLogFlag = Convert.ToInt32(IDLogFlagPre.Maxid);
        //                DT_ConfigIDInfoEntity OriIDLogFlagPre = BaseDataModel.DT_ConfigIDInfoEntities.Where(p => p.Memo.Equals("InfectantBy60Flag")).FirstOrDefault();
        //                long OriIDLogFlag = Convert.ToInt32(OriIDLogFlagPre.Maxid);
        //                DT_ConfigIDInfoEntity BufferIDLogBatchProcessPre = BaseDataModel.DT_ConfigIDInfoEntities.Where(p => p.Memo.Equals("InfectantBy60BufferBatchProcess")).FirstOrDefault();
        //                long BufferIDLogBatchProcess = Convert.ToInt32(BufferIDLogBatchProcessPre.Maxid);
        //                DT_ConfigIDInfoEntity IDLogBatchProcessPre = BaseDataModel.DT_ConfigIDInfoEntities.Where(p => p.Memo.Equals("InfectantBy60BatchProcess")).FirstOrDefault();
        //                long IDLogBatchProcess = Convert.ToInt32(IDLogBatchProcessPre.Maxid);
        //                try
        //                {
        //                    AnalyzeCurrentData();
        //                    Flag(DataTypeUid, IDLogFlag, OriIDLogFlag);

        //                    using (AirAutoMonitoringModel AirAutoMonitoringModel = new AirAutoMonitoringModel())
        //                    {
        //                        //获取缓存表中最新ID
        //                        InfectantBy60BufferEntity Last_InfectantBy60Buffer = AirAutoMonitoringModel.InfectantBy60BufferEntities.OrderByDescending(p => p.Id).FirstOrDefault();
        //                        if (Last_InfectantBy60Buffer != null)
        //                        {
        //                            long NewID = Last_InfectantBy60Buffer.Id;
        //                            IDLogFlagPre.Maxid = NewID;
        //                            IDLogFlagPre.Updatedatetime = DateTime.Now;
        //                            BaseDataModel.SaveChanges();

        //                            try
        //                            {
        //                                BatchProcess(DataTypeUid, BufferIDLogBatchProcess, IDLogBatchProcess, NewID);

        //                                BufferIDLogBatchProcessPre.Maxid = NewID;
        //                                BufferIDLogBatchProcessPre.Updatedatetime = DateTime.Now;
        //                                //获取原始表中最新ID
        //                                InfectantBy60Entity Last_InfectantBy60 = AirAutoMonitoringModel.InfectantBy60Entities.OrderByDescending(p => p.Id).FirstOrDefault();
        //                                if (Last_InfectantBy60 != null)
        //                                {
        //                                    long OriNewID = Last_InfectantBy60.Id;
        //                                    IDLogBatchProcessPre.Maxid = OriNewID;
        //                                    IDLogBatchProcessPre.Updatedatetime = DateTime.Now;

        //                                    OriIDLogFlagPre.Maxid = OriNewID;
        //                                    OriIDLogFlagPre.Updatedatetime = DateTime.Now;
        //                                }
        //                                BaseDataModel.SaveChanges();
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                log.Error("----------------------------BatchProcess异常：" + ex.ToString());
        //                            }

        //                        }

        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    log.Error("----------------------------Flag异常：" + ex.ToString());
        //                }

        //            }
        //        }
        //        //时间范围：计算当天和昨天数据
        //        DateTime sTime = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00"));
        //        DateTime eTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));

        //        CalculateBy60(sTime, eTime);
        //        //log.Info("-------------------------------------------------------RunBy60结束----------------------------");
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("----------------------------RunBy60异常：" + ex.ToString());
        //    }
        //}
        ///// <summary>
        ///// 每天执行
        ///// </summary>
        //public void RunByDay()
        //{
        //    try
        //    {
        //        //log.Info("-------------------------------------------------------RunByDay开始---------------------------");
        //        //月数据时间范围：上个月和这个月
        //        DateTime sMonthTime = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01 00:00:00"));
        //        DateTime eMonthTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));

        //        //周数据时间范围：上周和本周
        //        DateTime sWeekTime = Convert.ToDateTime(DateTime.Now.AddDays(-14).ToString("yyyy-MM-dd 00:00:00"));
        //        DateTime eWeekTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));

        //        CalculateByDay(sMonthTime, eMonthTime, sWeekTime, eWeekTime);
        //        //log.Info("-------------------------------------------------------RunByDay结束---------------------------");
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("------------------------------------------------RunByDay异常：" + ex.ToString());
        //    }
        //}
        ///// <summary>
        ///// 每月执行
        ///// </summary>
        //public void RunByMonth()
        //{
        //    try
        //    {
        //        //季数据时间范围：上季度和本季度
        //        DateTime sSeasonTime = Convert.ToDateTime(DateTime.Now.AddMonths(-3 - ((DateTime.Now.Month - 1) % 3)).ToString("yyyy-MM-01"));
        //        DateTime eSeasonTime = DateTime.Now;

        //        //年数据时间范围：去年和今年
        //        DateTime sYearTime = Convert.ToDateTime(DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddYears(-1).ToShortDateString());
        //        DateTime eYearTime = DateTime.Now;
        //        CalculateByMonth(sSeasonTime, eSeasonTime, sYearTime, eYearTime);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("------------------------------------------------RunByMonth异常：" + ex.ToString());
        //    }
        //}
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
                    using (MonitoringBusinessModel MonitoringBusinessModel = new MonitoringBusinessModel())
                    {
                        //获取配置表中信息
                        DT_DataTypeConfigEntity DataTypeConfig = BaseDataModel.DT_DataTypeConfigEntities.Where(p => p.EnableOrNot.Equals(true) && p.DataTypeUid.Equals(DataTypeUid)).FirstOrDefault();
                        string SY_BufferTable = DataTypeConfig.SYBufferTable;//缓存表同义词
                        string BufferTable = DataTypeConfig.BufferTable;//缓存表
                        string OriginTable = DataTypeConfig.OriginalTable;//原始表
                        string SY_OriTable = DataTypeConfig.SYOriginalTable;//缓存表同义词

                        //删除重复数据
                        d_DAL.DeleteRepeatData(IDLog, BufferTable);

                        #region 自动审核
                        //获取审核规则用途字典表信息
                        V_CodeMainItemEntity AuditCodeMainItem = FlagTypeCodeMainItems.Where(p => p.ItemText.Equals("审核")).FirstOrDefault();
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
                        V_CodeMainItemEntity AlarmCodeMainItem = FlagTypeCodeMainItems.Where(p => p.ItemText.Equals("报警")).FirstOrDefault();
                        if (AuditCodeMainItem == null)
                        {
                            log.Error("报警字典表信息未找到！");
                            return;
                        }


                        string DataFlag = "DataFlag";//对应数据表中字段名称
                        string UseForAlarmUid = AlarmCodeMainItem.ItemGuid;
                        #region 超标

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
                            V_CodeMainItemEntity HspAlarmType = AlarmTypes.Where(p => p.ItemValue.Equals(HSp)).FirstOrDefault();
                            string m_AlarmTypeUid_Hsp = HspAlarmType.ItemGuid;
                            if (ExcessiveUpper != null & m_AlarmTypeUid_Hsp != null)
                            {
                                //获取超上限数据
                                DataTable dtUpperData = d_DAL.GetUpperData(DataTypeUid, IDLog, HSp, DataFlag, UseForAlarmUid, SY_BufferTable, ApplicationUid);
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
                            V_CodeMainItemEntity LspAlarmType = AlarmTypes.Where(p => p.ItemValue.Equals(LSp)).FirstOrDefault();
                            string m_AlarmTypeUid_Lsp = LspAlarmType.ItemGuid;
                            if (ExcessiveLow != null & m_AlarmTypeUid_Lsp != null)
                            {
                                //获取超下限数据
                                DataTable dtLowerData = d_DAL.GetLowerData(DataTypeUid, IDLog, LSp, DataFlag, UseForAlarmUid, SY_BufferTable, ApplicationUid);
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
                                    V_CodeMainItemEntity RepAlarmType = AlarmTypes.Where(p => p.ItemValue.Equals("Rep")).FirstOrDefault();
                                    string m_AlarmTypeUid_Rep = RepAlarmType.ItemGuid;
                                    string content = PName + "站" + DateStart.ToString("yy月dd日HH时") + FactorName + "[" + PreFactorValue + "]连续" + (Temp_IDS + 1) + "组数据重复";
                                    CreatAlarmEntity[] CreatAlarmExists = BaseDataModel.CreatAlarmEntities.Where(p => p.Content.Equals(content)).ToArray();
                                    if (CreatAlarmExists.Count() == 0)
                                    {
                                        CreatAlarmEntity NewCreatAlarm = new CreatAlarmEntity();
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
                                    V_CodeMainItemEntity HAlarmType = AlarmTypes.Where(p => p.ItemValue.Equals("H")).FirstOrDefault();
                                    string m_AlarmTypeUid_H = HAlarmType.ItemGuid;

                                    string content = DataDT.ToString("MM月dd日HH时") + "-" + DTFinishByPoint.ToString("MM月dd日HH时") + "，" + PName + "点位" + FactorName + "数据缺失";
                                    string SendContent = PName + ";" + DataDT.ToString("yyyy-MM-dd HH:mm:dd") + ";" + DTFinishByPoint.ToString("yyyy-MM-dd HH:mm:dd") + ";" + FactorName;
                                    CreatAlarmEntity[] CreatAlarmExists = BaseDataModel.CreatAlarmEntities.Where(p => p.Content.Equals(content)).ToArray();
                                    if (CreatAlarmExists.Count() == 0)
                                    {
                                        CreatAlarmEntity NewCreatAlarm = new CreatAlarmEntity();
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
                            DataOnlineEntity DataOnlineEntityExists = MonitoringBusinessModel.DataOnlineEntities.Where(p => p.PointId.Equals(PId) && p.DataTypeUid.Equals(DataTypeUid)).FirstOrDefault();
                            if (dtLatestOnline.Rows.Count > 0 && dtLastestData.Rows.Count > 0)
                            {
                                DateTime DTFinish = Convert.ToDateTime(dtLastestData.Rows[0]["Tstamp"]);


                                TimeSpan ts = DTFinish - DataDT;
                                if (ts.TotalMinutes > OffLineTimeSpan)
                                {
                                    //站点离线
                                    V_CodeMainItemEntity OffAlarmType = AlarmTypes.Where(p => p.ItemValue.Equals("Off")).FirstOrDefault();
                                    string m_AlarmTypeUid_Off = OffAlarmType.ItemGuid;

                                    string content = DataDT.ToString("MM月dd日HH时") + "-" + DTFinish.ToString("MM月dd日HH时") + "，" + PName + "点位断线";
                                    string SendContent = PName + ";" + DataDT.ToString("yyyy-MM-dd HH:mm:dd") + ";" + DTFinish.ToString("yyyy-MM-dd HH:mm:dd");

                                    CreatAlarmEntity[] CreatAlarmExists = BaseDataModel.CreatAlarmEntities.Where(p => p.Content.Equals(content)).ToArray();
                                    if (CreatAlarmExists.Count() == 0)
                                    {
                                        CreatAlarmEntity NewCreatAlarm = new CreatAlarmEntity();
                                        NewCreatAlarm.AlarmUid = Guid.NewGuid().ToString();
                                        NewCreatAlarm.ApplicationUid = ApplicationUid;
                                        NewCreatAlarm.MonitoringPointUid = PUid;
                                        NewCreatAlarm.RecordDateTime = DataDT;
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
                                    if (DataOnlineEntityExists == null)//新增
                                    {
                                        DataOnlineEntity NewDataOnlineEntity = new DataOnlineEntity();
                                        NewDataOnlineEntity.ApplicationUid = ApplicationUid;
                                        NewDataOnlineEntity.MonitoringPointUid = PUid;
                                        NewDataOnlineEntity.PointId = PId;
                                        NewDataOnlineEntity.DataTypeUid = DataTypeUid;
                                        NewDataOnlineEntity.IsOnline = 0;
                                        NewDataOnlineEntity.NewDataTime = DataDT;
                                        NewDataOnlineEntity.OffLineTime = Convert.ToInt32(ts.TotalMinutes);
                                        NewDataOnlineEntity.Recent24HourRecords = Recent24Records;
                                        MonitoringBusinessModel.Add(NewDataOnlineEntity);
                                    }
                                    else//更新
                                    {
                                        DataOnlineEntityExists.IsOnline = 0;
                                        DataOnlineEntityExists.NewDataTime = DataDT;
                                        DataOnlineEntityExists.OffLineTime = Convert.ToInt32(ts.TotalMinutes);
                                        DataOnlineEntityExists.Recent24HourRecords = Recent24Records;

                                    }
                                }
                                else
                                {
                                    //站点在线
                                    if (DataOnlineEntityExists == null)//新增
                                    {
                                        DataOnlineEntity NewDataOnlineEntity = new DataOnlineEntity();
                                        NewDataOnlineEntity.ApplicationUid = ApplicationUid;
                                        NewDataOnlineEntity.MonitoringPointUid = PUid;
                                        NewDataOnlineEntity.PointId = PId;
                                        NewDataOnlineEntity.DataTypeUid = DataTypeUid;
                                        NewDataOnlineEntity.IsOnline = 1;
                                        NewDataOnlineEntity.NewDataTime = DataDT;
                                        NewDataOnlineEntity.OffLineTime = null;
                                        NewDataOnlineEntity.Recent24HourRecords = Recent24Records;
                                        MonitoringBusinessModel.Add(NewDataOnlineEntity);
                                    }
                                    else//更新
                                    {
                                        DataOnlineEntityExists.IsOnline = 1;
                                        DataOnlineEntityExists.NewDataTime = DataDT;
                                        DataOnlineEntityExists.OffLineTime = null;
                                        DataOnlineEntityExists.Recent24HourRecords = Recent24Records;
                                    }
                                }

                            }

                        }
                        #endregion
                        #endregion
                        MonitoringBusinessModel.SaveChanges();
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
                    DT_DataTypeConfigEntity DataTypeConfig = BaseDataModel.DT_DataTypeConfigEntities.Where(p => p.EnableOrNot.Equals(true) && p.DataTypeUid.Equals(DataTypeUid)).FirstOrDefault();
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
                    V_CodeMainItemEntity DataType = DataTypeCodeMainItems.Where(p => p.ItemValue.Equals("Min60")).FirstOrDefault();
                    string DataTypeUid = DataType.ItemGuid;
                    using (BaseDataModel BaseDataModel = new BaseDataModel())
                    {
                        DT_ConfigIDInfoEntity IDLogFlagPre = BaseDataModel.DT_ConfigIDInfoEntities.Where(p => p.Memo.Equals("TB_InfectantBy60BufferFlag")).FirstOrDefault();
                        long IDLogFlag = Convert.ToInt32(IDLogFlagPre.Maxid);
                        DT_ConfigIDInfoEntity OriIDLogFlagPre = BaseDataModel.DT_ConfigIDInfoEntities.Where(p => p.Memo.Equals("TB_InfectantBy60Flag")).FirstOrDefault();
                        long OriIDLogFlag = Convert.ToInt32(OriIDLogFlagPre.Maxid);
                        DT_ConfigIDInfoEntity BufferIDLogBatchProcessPre = BaseDataModel.DT_ConfigIDInfoEntities.Where(p => p.Memo.Equals("TB_InfectantBy60BufferBatchProcess")).FirstOrDefault();
                        long BufferIDLogBatchProcess = Convert.ToInt32(BufferIDLogBatchProcessPre.Maxid);
                        DT_ConfigIDInfoEntity IDLogBatchProcessPre = BaseDataModel.DT_ConfigIDInfoEntities.Where(p => p.Memo.Equals("TB_InfectantBy60BatchProcess")).FirstOrDefault();
                        long IDLogBatchProcess = Convert.ToInt32(IDLogBatchProcessPre.Maxid);
                        try
                        {
                            Flag(DataTypeUid, IDLogFlag, OriIDLogFlag);
                            //InstrumentOnline(DataTypeUid);
                            using (AirAutoMonitoringModel AirAutoMonitoringModel = new AirAutoMonitoringModel())
                            {
                                //获取缓存表中最新ID
                                InfectantBy60BufferEntity Last_InfectantBy60Buffer = AirAutoMonitoringModel.InfectantBy60BufferEntities.OrderByDescending(p => p.Id).FirstOrDefault();
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
                                        InfectantBy60Entity Last_InfectantBy60 = AirAutoMonitoringModel.InfectantBy60Entities.OrderByDescending(p => p.Id).FirstOrDefault();
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
        public void CalculateBy60(DateTime startTime, DateTime endTime, string[] pids)
        {
            try
            {
                //log.Info("-------------------------------------------------------------CalculateBy60开始-------------------");
                //将字符串转换成int
                List<int> pds = new List<int>();
                for (int i = 0; i < pids.Length; i++)
                {
                    pds.Add(Convert.ToInt32(pids[i]));
                }
                DateTime sTime = Convert.ToDateTime(startTime.ToString("yyyy-MM-dd 00:00:00"));
                DateTime eTime = Convert.ToDateTime(endTime > DateTime.Now ? DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss") : endTime.ToString("yyyy-MM-dd 23:59:59"));

                using (AirAutoMonitoringModel AirAutoMonitoringModel = new AirAutoMonitoringModel())
                {
                    #region 原始测点AQI
                    int[] arr = pds.ToArray();
                    foreach (int PointId in arr)
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
                            OriHourAQIEntity OriHourAQIExists = AirAutoMonitoringModel.OriHourAQIEntities.Where(p => p.PointId.Equals(PointId) && p.DateTime.Equals(Tstamp)).FirstOrDefault();
                            if (OriHourAQIExists == null)
                            {
                                #region 新增小时AQI
                                OriHourAQIEntity NewOriHourAQI = new OriHourAQIEntity();
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
                                AirAutoMonitoringModel.Add(NewOriHourAQI);
                                //AirAutoMonitoringModel.SaveChanges();
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
                                OriHourAQIExists.UpdateUser = "SystemSync";
                                OriHourAQIExists.UpdateDateTime = DateTime.Now;
                                //AirAutoMonitoringModel.SaveChanges();
                                #endregion
                            }
                        }
                        #endregion
                        AirAutoMonitoringModel.SaveChanges();

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
                            OriDayAQIEntity OriDayAQIExists = AirAutoMonitoringModel.OriDayAQIEntities.Where(p => p.PointId.Equals(PointId) && p.DateTime.Equals(Tstamp)).FirstOrDefault();
                            if (OriDayAQIExists == null)
                            {
                                #region 新增日AQI
                                OriDayAQIEntity NewOriDayAQI = new OriDayAQIEntity();
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
                                AirAutoMonitoringModel.Add(NewOriDayAQI);
                                //AirAutoMonitoringModel.SaveChanges();
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
                                OriDayAQIExists.UpdateUser = "SystemSync";
                                OriDayAQIExists.UpdateDateTime = DateTime.Now;
                                //AirAutoMonitoringModel.SaveChanges();
                                #endregion
                            }
                        }
                        #endregion
                        AirAutoMonitoringModel.SaveChanges();

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
                            OriRegionHourAQIEntity OriRegionHourAQIExists = AirAutoMonitoringModel.OriRegionHourAQIEntities.Where(p => p.MonitoringRegionUid.Equals(m_region) && p.DateTime.Equals(Tstamp)).FirstOrDefault();
                            if (OriRegionHourAQIExists == null)
                            {
                                #region 新增小时AQI
                                OriRegionHourAQIEntity NewOriHourAQI = new OriRegionHourAQIEntity();
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
                                AirAutoMonitoringModel.Add(NewOriHourAQI);
                                //AirAutoMonitoringModel.SaveChanges();
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
                                OriRegionHourAQIExists.UpdateUser = "SystemSync";
                                OriRegionHourAQIExists.UpdateDateTime = DateTime.Now;
                                //AirAutoMonitoringModel.SaveChanges();
                                #endregion
                            }
                        }
                        #endregion
                        AirAutoMonitoringModel.SaveChanges();

                        #region 原始日区域AQI
                        DataTable dtPivotOriRegionDay = d_DAL.GetPivotOriRegionDayData(m_region, sTime, eTime);
                        foreach (DataRow dr in dtPivotOriRegionDay.Rows)
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
                            OriRegionDayAQIReportEntity OriRegionDayAQIExists = AirAutoMonitoringModel.OriRegionDayAQIReportEntities.Where(p => p.MonitoringRegionUid.Equals(m_region) && p.ReportDateTime.Equals(Tstamp)).FirstOrDefault();
                            if (OriRegionDayAQIExists == null)
                            {
                                #region 新增日AQI
                                OriRegionDayAQIReportEntity NewOriDayAQI = new OriRegionDayAQIReportEntity();
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
                                AirAutoMonitoringModel.Add(NewOriDayAQI);
                                //AirAutoMonitoringModel.SaveChanges();
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
                                OriRegionDayAQIExists.UpdateUser = "SystemSync";
                                OriRegionDayAQIExists.UpdateDateTime = DateTime.Now;
                                //AirAutoMonitoringModel.SaveChanges();
                                #endregion
                            }
                        }
                        #endregion
                        AirAutoMonitoringModel.SaveChanges();
                    }
                    #endregion
                }
                using (MonitoringBusinessModel MonitoringBusinessModel = new MonitoringBusinessModel())
                {
                    #region 审核测点AQI
                    int[] arr = pds.ToArray();
                    foreach (int PointId in arr)
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

                            HourAQIEntity HourAQIExists = MonitoringBusinessModel.HourAQIEntities.Where(p => p.PointId.Equals(PointId) && p.DateTime.Equals(Tstamp)).FirstOrDefault();
                            if (HourAQIExists == null)
                            {
                                #region 新增小时AQI
                                HourAQIEntity NewAudHourAQI = new HourAQIEntity();
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
                                MonitoringBusinessModel.Add(NewAudHourAQI);
                                //MonitoringBusinessModel.SaveChanges();
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
                                HourAQIExists.UpdateUser = "SystemSync";
                                HourAQIExists.UpdateDateTime = DateTime.Now;
                                //MonitoringBusinessModel.SaveChanges();
                                #endregion
                            }
                        }
                        #endregion
                        MonitoringBusinessModel.SaveChanges();

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
                            DayAQIEntity AudDayAQIExists = MonitoringBusinessModel.DayAQIEntities.Where(p => p.PointId.Equals(PointId) && p.DateTime.Equals(Tstamp)).FirstOrDefault();
                            if (AudDayAQIExists == null)
                            {
                                #region 新增日AQI
                                DayAQIEntity NewAudDayAQI = new DayAQIEntity();
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
                                MonitoringBusinessModel.Add(NewAudDayAQI);
                                //MonitoringBusinessModel.SaveChanges();
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

                                AudDayAQIExists.UpdateUser = "SystemSync";
                                AudDayAQIExists.UpdateDateTime = DateTime.Now;
                                //MonitoringBusinessModel.SaveChanges();
                                #endregion
                            }
                        }
                        #endregion
                        MonitoringBusinessModel.SaveChanges();
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
                            RegionHourAQIEntity AudRegionHourAQIExists = MonitoringBusinessModel.RegionHourAQIEntities.Where(p => p.MonitoringRegionUid.Equals(m_region) && p.DateTime.Equals(Tstamp)).FirstOrDefault();
                            if (AudRegionHourAQIExists == null)
                            {
                                #region 新增小时AQI
                                RegionHourAQIEntity NewAudHourAQI = new RegionHourAQIEntity();
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
                                MonitoringBusinessModel.Add(NewAudHourAQI);
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
                                AudRegionHourAQIExists.UpdateUser = "SystemSync";
                                AudRegionHourAQIExists.UpdateDateTime = DateTime.Now;
                                #endregion
                            }
                        }
                        #endregion
                        MonitoringBusinessModel.SaveChanges();

                        #region 审核日区域AQI
                        DataTable dtPivotAudRegionDay = d_DAL.GetPivotAudRegionDayData(m_region, sTime, eTime);
                        foreach (DataRow dr in dtPivotAudRegionDay.Rows)
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
                            RegionDayAQIReportEntity AudRegionDayAQIExists = MonitoringBusinessModel.RegionDayAQIReportEntities.Where(p => p.MonitoringRegionUid.Equals(m_region) && p.ReportDateTime.Equals(Tstamp)).FirstOrDefault();
                            if (AudRegionDayAQIExists == null)
                            {
                                #region 新增日AQI
                                RegionDayAQIReportEntity NewAudDayAQI = new RegionDayAQIReportEntity();
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
                                MonitoringBusinessModel.Add(NewAudDayAQI);
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
                                AudRegionDayAQIExists.UpdateUser = "SystemSync";
                                AudRegionDayAQIExists.UpdateDateTime = DateTime.Now;

                                #endregion
                            }
                        }
                        #endregion
                        MonitoringBusinessModel.SaveChanges();
                    }
                    #endregion

                }
                //log.Info("-------------------------------------------------------CalculateBy60结束----------------------------");
            }
            catch (Exception ex)
            {
                log.Error("--------------------------------------------------------CalculateBy60异常:" + ex.ToString());
            }

        }
        /// <summary>
        /// 日数据计算月数据
        /// </summary>
        public void CalculateByDay(DateTime SMTime, DateTime EMTime, DateTime SWTime, DateTime EWTime, string[] pids)
        {
            try
            {
                List<int> pds = new List<int>();
                for (int i = 0; i < pids.Length; i++)
                {
                    pds.Add(Convert.ToInt32(pids[i]));
                }
                DateTime sMonthTime = SMTime.AddDays(1 - SMTime.Day);//当前月第一天
                DateTime eMonthTime = EMTime.AddDays(1 - EMTime.Day).AddMonths(1).AddDays(-1);//当前月最后一天

                //周数据时间范围：当前周第一天和当前周最后一天
                DateTime sWeekTime = GetWeekFirstDayMon(SWTime);
                DateTime eWeekTime = GetWeekLastDaySun(EWTime);
                int[] arr = pds.ToArray();
                foreach (int PointId in arr)
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
        public void CalculateByMonth(DateTime SSTime, DateTime ESTime, DateTime SYTime, DateTime EYTime, string[] pids)
        {
            try
            {
                List<int> pds = new List<int>();
                for (int i = 0; i < pids.Length; i++)
                {
                    pds.Add(Convert.ToInt32(pids[i]));
                }
                //季数据时间范围：当前季度第一天和当前季度最后一天
                DateTime sSeasonTime = Convert.ToDateTime(SSTime.AddMonths(0 - ((SSTime.Month - 1) % 3)).ToString("yyyy-MM-01"));
                DateTime eSeasonTime = Convert.ToDateTime(DateTime.Parse(ESTime.AddMonths(3 - ((ESTime.Month - 1) % 3)).ToString("yyyy-MM-01")).AddDays(-1).ToShortDateString());

                //年数据时间范围：当前年第一天和当前年最后一天
                DateTime sYearTime = Convert.ToDateTime(SYTime.ToString("yyyy-01-01"));
                DateTime eYearTime = Convert.ToDateTime(EYTime.ToString("yyyy-12-31"));
                int[] arr = pds.ToArray();
                foreach (int PointId in arr)
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
                using (BaseDataModel BaseDataModel = new BaseDataModel())
                {
                    using (MonitoringBusinessModel MonitoringBusinessModel = new MonitoringBusinessModel())
                    {
                        //获取配置表中信息
                        DT_DataTypeConfigEntity DataTypeConfig = BaseDataModel.DT_DataTypeConfigEntities.Where(p => p.EnableOrNot.Equals(true) && p.DataTypeUid.Equals(DataTypeUid)).FirstOrDefault();
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
                            DataTable dtLastestData = d_DAL.GetNewDataInstrument(OriginTable, InUid);

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

                            InstrumentDataOnlineEntity DataOnlineEntityExists = MonitoringBusinessModel.InstrumentDataOnlineEntities.Where(p => p.InstrumentUid.Equals(InUid) && p.DataTypeUid.Equals(DataTypeUid)).FirstOrDefault();
                            if (dtLastestData.Rows.Count > 0)
                            {
                                DateTime DTFinish = Convert.ToDateTime(dtLastestData.Rows[0]["Tstamp"]);
                                TimeSpan ts = DataDT - DTFinish;
                                if (ts.TotalMinutes > OffLineTimeSpan)
                                {
                                    if (DataOnlineEntityExists == null)//新增
                                    {
                                        InstrumentDataOnlineEntity NewDataOnlineEntity = new InstrumentDataOnlineEntity();
                                        NewDataOnlineEntity.InstrumentUid = InUid;
                                        NewDataOnlineEntity.InstrumentName = InName;
                                        NewDataOnlineEntity.DataTypeUid = DataTypeUid;
                                        NewDataOnlineEntity.IsOnline = 0;
                                        NewDataOnlineEntity.NewDataTime = DTFinish;
                                        NewDataOnlineEntity.OffLineTime = Convert.ToInt32(ts.TotalMinutes);
                                        MonitoringBusinessModel.Add(NewDataOnlineEntity);
                                    }
                                    else//更新
                                    {
                                        DataOnlineEntityExists.IsOnline = 0;
                                        DataOnlineEntityExists.NewDataTime = DTFinish;
                                        DataOnlineEntityExists.OffLineTime = Convert.ToInt32(ts.TotalMinutes);
                                    }
                                }
                                else
                                {
                                    //站点在线
                                    if (DataOnlineEntityExists == null)//新增
                                    {
                                        InstrumentDataOnlineEntity NewDataOnlineEntity = new InstrumentDataOnlineEntity();
                                        NewDataOnlineEntity.InstrumentUid = InUid;
                                        NewDataOnlineEntity.InstrumentName = InName;
                                        NewDataOnlineEntity.DataTypeUid = DataTypeUid;
                                        NewDataOnlineEntity.IsOnline = 1;
                                        NewDataOnlineEntity.NewDataTime = DTFinish;
                                        NewDataOnlineEntity.OffLineTime = null;
                                        MonitoringBusinessModel.Add(NewDataOnlineEntity);
                                    }
                                    else//更新
                                    {
                                        DataOnlineEntityExists.IsOnline = 1;
                                        DataOnlineEntityExists.NewDataTime = DTFinish;
                                        DataOnlineEntityExists.OffLineTime = null;
                                    }
                                }

                            }

                        }
                        #endregion
                        MonitoringBusinessModel.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("------------------------------------------InstrumentOnline异常:" + ex.ToString());
            }

        }
        /// <summary>
        /// 执行DataTable中的查询返回新的DataTable
        /// </summary>
        /// <param name="dt">源数据DataTable</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public DataTable GetNewDataTable(DataTable dt, string condition)
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
        #endregion

        #region 导入历史数据
        public void fillhistorydata(DateTime tstamp, int FillDays, string[] portIds, string[] factorCodes,out string MissData)
        {
            MissData = string.Empty;
            try
            {
                log.Info("----------------线程同步开始------------");

                DateTime startTime = tstamp;
                DateTime endTime = tstamp.AddDays(FillDays).AddSeconds(-1) > DateTime.Now ? DateTime.Now.AddHours(-1) : tstamp.AddDays(FillDays).AddSeconds(-1);
                //update by xy
                string pid = string.Join(",", portIds);
                string fac = "'" + string.Join("','", factorCodes) + "'";
                string sTime = startTime.ToString("yyyy-MM-dd");
                string eTime = tstamp.AddDays(FillDays).AddSeconds(-1) > DateTime.Now ? DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss") : endTime.ToString("yyyy-MM-dd 23:59:59");
                DateTime dts = DateTime.Now;
                //删除原有接口接入数据，原始表数据
                d_DAL.DeleteOriDataByTime(startTime, endTime, pid, fac);

                //删除原有小时计算表数据和小时报表数据
                d_DAL.DeleteAuditDataByTime(startTime, endTime, pid, fac);

                //接入常规站历史数据
                FillData(sTime, eTime, pid, factorCodes,out MissData);
                //log.Info("常规站数据接入成功");
                //批量处理insert
                FillProcessData();
                //log.Info("生成原始数据成功");
                CalculateBy60(Convert.ToDateTime(sTime), Convert.ToDateTime(eTime), portIds);
                //log.Info("计算AQI、日数据成功");
                CalculateByDay(Convert.ToDateTime(sTime), Convert.ToDateTime(eTime), Convert.ToDateTime(sTime), Convert.ToDateTime(eTime), portIds);
                //log.Info("月数据、周数据同步成功");
                CalculateByMonth(Convert.ToDateTime(sTime), Convert.ToDateTime(eTime), Convert.ToDateTime(sTime), Convert.ToDateTime(eTime), portIds);
                //log.Info("季数据、年数据同步成功");
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
