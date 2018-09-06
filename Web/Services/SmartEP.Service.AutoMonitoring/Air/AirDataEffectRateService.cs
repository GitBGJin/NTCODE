using SmartEP.AMSRepository.Air;
using SmartEP.AMSRepository.Interfaces;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.MonitoringBusinessRepository.Air;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.AutoMonitoring.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using SmartEP.Service.Frame;
using SmartEP.Service.Core.Enums;
using SmartEP.DomainModel;
using SmartEP.BaseInfoRepository.Channel;

namespace SmartEP.Service.AutoMonitoring.Air
{
    /// <summary>
    /// 名称：AirDataEffectRateService.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-08-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：环境空气数据有效率查询类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AirDataEffectRateService : IDataEffectRate
    {
        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointAirService g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();

        /// <summary>
        /// 实时数据仓储接口
        /// </summary>
        IInfectantRepository g_IInfectantRepository;

        /// <summary>
        /// 审核数据仓储类
        /// </summary>
        HourReportRepository g_AirHourData;

        #region 实现接口
        /// <summary>
        /// 点位某一小时总有效率
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="dtmHour">日期（小时）</param>
        /// <returns></returns>
        public decimal GetHourEffectRate(int portId, DateTime dtmHour)
        {
            DateTime dtmStart = dtmHour;//开始时间
            DateTime dtmEnd = dtmHour;//结束时间
            string[] pointIds = { portId.ToString() };
            string[] factors = GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            return GetPointEffectRate(pointIds, factors, dtmStart, dtmEnd);
        }

        /// <summary>
        /// 点位某天总有效率
        /// </summary>
        /// <param name="portId">测点Id</param>
        /// <param name="dtmDay">日期（天）</param>
        /// <returns></returns>
        public decimal GetDayEffectRate(int portId, DateTime dtmDay)
        {
            DateTime dtmStart = dtmDay;//开始时间
            DateTime dtmEnd = dtmDay;//结束时间
            string[] pointIds = { portId.ToString() };
            string[] factors = GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            return GetPointEffectRate(pointIds, factors, dtmStart, dtmEnd);
        }

        /// <summary>
        /// 点位某月总有效率
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="year">年</param>
        /// <param name="monthOfYear">月</param>
        /// <returns></returns>
        public decimal GetMonthEffectRate(int portId, int year, int monthOfYear)
        {
            if (monthOfYear < 1 || monthOfYear > 12)
            {
                return 0;
            }

            DateTime dtmStart = new DateTime(year, monthOfYear, 1);//开始时间
            DateTime dtmEnd = new DateTime(year, monthOfYear + 1, 1).AddDays(-1);//结束时间
            string[] pointIds = { portId.ToString() };
            string[] factors = GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            return GetPointEffectRate(pointIds, factors, dtmStart, dtmEnd);
        }

        /// <summary>
        /// 点位某季度总有效率
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="year">年</param>
        /// <param name="seasonOfYear">季</param>
        /// <returns></returns>
        public decimal GetSeasonEffectRate(int portId, int year, int seasonOfYear)
        {
            if (seasonOfYear < 1 || seasonOfYear > 4)
            {
                return 0;
            }

            int monthStart = seasonOfYear * 3 - 2;//开始月
            int monthEnd = seasonOfYear * 3;//结束月
            DateTime dtmStart = new DateTime(year, monthStart, 1);//开始时间
            DateTime dtmEnd = (monthEnd == 12) ? new DateTime(year + 1, 1, 1).AddDays(-1) : new DateTime(year, monthEnd + 1, 1).AddDays(-1);//结束时间
            string[] pointIds = { portId.ToString() };
            string[] factors = GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            return GetPointEffectRate(pointIds, factors, dtmStart, dtmEnd);
        }

        /// <summary>
        /// 点位某年总有效率
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="year">年</param>
        /// <returns></returns>
        public decimal GetYearEffectRate(int portId, int year)
        {
            DateTime dtmStart = new DateTime(year, 1, 1);//开始时间
            DateTime dtmEnd = new DateTime(year + 1, 1, 1).AddDays(-1);//结束时间
            string[] pointIds = { portId.ToString() };
            string[] factors = GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            return GetPointEffectRate(pointIds, factors, dtmStart, dtmEnd);
        }

        /// <summary>
        /// 点位某一小时各个污染物有效率明细
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="dtmHour">日期（小时）</param>
        /// <returns></returns>
        public DataView GetHourEffectRateDetail(int portId, DateTime dtmHour)
        {
            DateTime dtmStart = dtmHour;//开始时间
            DateTime dtmEnd = dtmHour;//结束时间
            string[] pointIds = { portId.ToString() };
            string[] factors = GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            int recordTotal = 0;
            return GetPointEffectRateDetail(pointIds, factors, dtmStart, dtmEnd, int.MaxValue, 0, out recordTotal);
        }

        /// <summary>
        /// 点位某天各个污染物有效率明细
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="dtmDay">日期（天）</param>
        /// <returns></returns>
        public DataView GetDayEffectRateDetail(int portId, DateTime dtmDay)
        {
            DateTime dtmStart = dtmDay;//开始时间
            DateTime dtmEnd = dtmDay;//结束时间
            string[] pointIds = { portId.ToString() };
            string[] factors = GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            int recordTotal = 0;
            return GetPointEffectRateDetail(pointIds, factors, dtmStart, dtmEnd, int.MaxValue, 0, out recordTotal);
        }

        /// <summary>
        /// 点位某月各个污染物有效率明细
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="year">年</param>
        /// <param name="monthOfYear">月</param>
        /// <returns></returns>
        public DataView GetMonthEffectRateDetail(int portId, int year, int monthOfYear)
        {
            if (monthOfYear < 1 || monthOfYear > 12)
            {
                return null;
            }

            DateTime dtmStart = new DateTime(year, monthOfYear, 1);//开始时间
            DateTime dtmEnd = new DateTime(year, monthOfYear + 1, 1).AddDays(-1);//结束时间
            string[] pointIds = { portId.ToString() };
            string[] factors = GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            int recordTotal = 0;
            return GetPointEffectRateDetail(pointIds, factors, dtmStart, dtmEnd, int.MaxValue, 0, out recordTotal);
        }

        /// <summary>
        /// 点位某季度各个污染物有效率明细
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="year">年</param>
        /// <param name="seasonOfYear">季</param>
        /// <returns></returns>
        public DataView GetSeasonEffectRateDetail(int portId, int year, int seasonOfYear)
        {
            if (seasonOfYear < 1 || seasonOfYear > 4)
            {
                return null;
            }

            int monthStart = seasonOfYear * 3 - 2;//开始月
            int monthEnd = seasonOfYear * 3;//结束月
            DateTime dtmStart = new DateTime(year, monthStart, 1);//开始时间
            DateTime dtmEnd = (monthEnd == 12) ? new DateTime(year + 1, 1, 1).AddDays(-1) : new DateTime(year, monthEnd + 1, 1).AddDays(-1);//结束时间
            string[] pointIds = { portId.ToString() };
            string[] factors = GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            int recordTotal = 0;
            return GetPointEffectRateDetail(pointIds, factors, dtmStart, dtmEnd, int.MaxValue, 0, out recordTotal);
        }

        /// <summary>
        /// 点位某年各个污染物有效率明细
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="year">年</param>
        /// <returns></returns>
        public DataView GetYearEffectRateDetail(int portId, int year)
        {
            DateTime dtmStart = new DateTime(year, 1, 1);//开始时间
            DateTime dtmEnd = new DateTime(year + 1, 1, 1).AddDays(-1);//结束时间
            string[] pointIds = { portId.ToString() };
            string[] factors = GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            int recordTotal = 0;
            return GetPointEffectRateDetail(pointIds, factors, dtmStart, dtmEnd, int.MaxValue, 0, out recordTotal);
        }
        #endregion

        #region 总有效率
        /// <summary>
        /// 国控点数据总有效率
        /// </summary>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <returns></returns>
        public decimal GetLocusOfControlEffectRate(DateTime dtmStart, DateTime dtmEnd)
        {
            //获取国控点位列表
            IQueryable<MonitoringPointEntity> entityQueryable = g_MonitoringPointAir.RetrieveAirMPListByCountryControlled();
            string[] pointIds = GetPointIdsByPointEntitys(entityQueryable.ToArray());//根据测点数组获取测点Id列
            string[] factors = GetPollutantCodesByPointEntitys(entityQueryable.ToArray());//根据测点数组获取因子列
            return GetPointEffectRate(pointIds, factors, dtmStart, dtmEnd);
        }

        /// <summary>
        /// 获取某一时间段监测点数据总有效率（因子不可选，测点自动关联因子）
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <returns></returns>
        public decimal GetPointEffectRate(string[] portIds, DateTime dtmStart, DateTime dtmEnd)
        {
            string[] factors = GetPollutantCodesByPointIds(portIds);//根据测点Id数组获取因子列
            return GetPointEffectRate(portIds, factors, dtmStart, dtmEnd);
        }

        /// <summary>
        /// 获取某一时间段监测点数据总有效率（因子可选）
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <returns></returns>
        public decimal GetPointEffectRate(string[] portIds, string[] factors, DateTime dtmStart, DateTime dtmEnd)
        {
            dtmStart = new DateTime(dtmStart.Year, dtmStart.Month, dtmStart.Day, 0, 0, 0);
            dtmEnd = new DateTime(dtmEnd.Year, dtmEnd.Month, dtmEnd.Day, 23, 59, 59);
            DataTable dtNew = new DataTable();//新建表
            int shouldCount = ((dtmEnd - dtmStart).Days + 1) * 24;//每个因子应测条数
            int recordTotal = 0;//总行数
            int pageSize = int.MaxValue;//每页记录数
            int pageNo = 0;//当前页(从0开始)
            int allShouldCount = 0;//总应测条数
            int allEffectCount = 0;//总有效条数
            decimal allEffectRate = 0;//总有效率
            string orderBy = "PointId,Tstamp";//排序方式
            Dictionary<string, IList<string>> pointPollutantCodeList = GetPointPollutantCodeByPointIds(portIds);//根据测点Id数组获取测点Id和对应的因子Code
            g_IInfectantRepository = Singleton<InfectantBy60Repository>.GetInstance();
            g_AirHourData = Singleton<HourReportRepository>.GetInstance();

            if (g_IInfectantRepository != null)
            {
                DataTable dtOriginal = g_IInfectantRepository.GetDataPager(portIds, factors, dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy).Table;//原始数据
                DataTable dtAudit = g_AirHourData.GetDataPager(portIds, factors, dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy).Table;//审核数据

                foreach (DataColumn dcOriginal in dtOriginal.Columns)
                {
                    //当该列不是站点或时间时，才计算
                    if (dcOriginal.ColumnName != "PointId" && dcOriginal.ColumnName != "Tstamp" && !dcOriginal.ColumnName.Contains("_Status")
                        && !dcOriginal.ColumnName.Contains("_Mark"))
                    {
                        int realCount = 0;//实测条数
                        int effectCount = 0;//有效条数
                        int totalShouldCount = 0;//合计应测条数
                        //IList<string> pointList = new List<string>();//有此监测项的站点列表

                        foreach (string pointId in portIds)//循环累计应测条数
                        {
                            IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId))
                                      ? pointPollutantCodeList[pointId] : new List<string>(); //该测点对应的因子Code列
                            if (pollutantCodeList.Contains(dcOriginal.ColumnName))//当前测点中有该因子，则累加应测条数
                            {
                                totalShouldCount += shouldCount;
                            }
                        }

                        //如果合计应测条数大于0，则计算有效率
                        if (totalShouldCount > 0)
                        {
                            foreach (DataRow drOriginal in dtOriginal.Rows)
                            {
                                IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(drOriginal["PointId"].ToString()))
                                    ? pointPollutantCodeList[drOriginal["PointId"].ToString()] : new List<string>();//该测点对应的因子Code列

                                //如果该测点对应的因子列中没有当前因子，则该因子为无效因子，直接跳过
                                if (!pollutantCodeList.Contains(dcOriginal.ColumnName))
                                {
                                    continue;
                                }

                                ////如果测点列表中还没有该测点，则添加测点到列表
                                //if (!pointList.Contains(drOriginal["PointId"].ToString()))
                                //{
                                //    pointList.Add(drOriginal["PointId"].ToString());
                                //}

                                //当有数据时，则条数累加
                                if (drOriginal[dcOriginal.ColumnName].ToString() != "")
                                {
                                    realCount++;//实测条数累加

                                    //当审核数据和原始数据的测点和时间戳相同，则代表一条有效数据
                                    if (dtAudit.Columns.Contains(dcOriginal.ColumnName)
                                        && dtAudit.Select(string.Format("PointId='{0}' and Tstamp='{1}' and isnull({2},'-1')<>'-1'",
                                            drOriginal["PointId"], drOriginal["Tstamp"], dcOriginal.ColumnName)).Length > 0)
                                    {
                                        effectCount++;//有效条数累加
                                    }
                                }
                            }

                            allEffectCount += effectCount;
                            allShouldCount += totalShouldCount;
                        }
                    }
                }
                allEffectRate = (allShouldCount > 0) ? Math.Round((decimal)allEffectCount * 100 / allShouldCount, 2) : 0;
            }
            return allEffectRate;
        }

        /// <summary>
        /// 获取某一时间段苏州市区所有监测点数据总有效率
        /// <summary>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <returns></returns>
        public decimal GetSuZhouEffectRate(DateTime dtmStart, DateTime dtmEnd)
        {
            DictionaryService dictionaryService = Singleton<DictionaryService>.GetInstance();
            string guid = dictionaryService.GetValueByText(DictionaryType.Air, "点位城市类型", "苏州市");//获取苏州市区的Uid

            //根据行政区划类型获取点位列表
            IQueryable<MonitoringPointEntity> entityQueryable = g_MonitoringPointAir.RetrieveAirMPListByRegion(guid);
            string[] pointIds = GetPointIdsByPointEntitys(entityQueryable.ToArray());//根据测点数组获取测点Id列
            string[] factors = GetPollutantCodesByPointEntitys(entityQueryable.ToArray());//根据测点数组获取因子列
            return GetPointEffectRate(pointIds, factors, dtmStart, dtmEnd);
        }

        /// <summary>
        /// 获取某一时间段某一城区所有监测点数据总有效率
        /// <summary>
        /// <param name="urbanAreas">城区数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <returns></returns>
        public decimal GetUrbanAreaEffectRate(IList<string> urbanAreas, DateTime dtmStart, DateTime dtmEnd)
        {
            IList<string> portIds = new List<string>();
            IList<string> factors = new List<string>();
            foreach (string urbanArea in urbanAreas)
            {
                //根据行政区划类型获取点位列表
                IQueryable<MonitoringPointEntity> entityQueryable = g_MonitoringPointAir.RetrieveAirMPListByRegion(urbanArea);
                portIds = portIds.Union(GetPointIdsByPointEntitys(entityQueryable.ToArray()).ToList()).ToList();//根据测点数组获取测点Id列
                factors = factors.Union(GetPollutantCodesByPointEntitys(entityQueryable.ToArray()).ToList()).ToList();//根据测点数组获取因子列
            }
            return GetPointEffectRate(portIds.ToArray(), factors.ToArray(), dtmStart, dtmEnd);
        }

        /// <summary>
        /// 获取某一时间段乡镇所有监测点数据总有效率
        /// <summary>
        /// <param name="villagesTownsList">乡镇数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <returns></returns>
        public decimal GetVillagesTownsEffectRate(IList<string> villagesTownsList, DateTime dtmStart, DateTime dtmEnd)
        {
            IList<string> portIds = new List<string>();
            IList<string> factors = new List<string>();
            foreach (string villagesTowns in villagesTownsList)
            {
                //根据监测区域类型获取点位列表
                IQueryable<MonitoringPointEntity> entityQueryable = g_MonitoringPointAir.RetrieveAirMPListByMonitoringRegion(villagesTowns);
                portIds = portIds.Union(GetPointIdsByPointEntitys(entityQueryable.ToArray()).ToList()).ToList();//根据测点数组获取测点Id列
                factors = factors.Union(GetPollutantCodesByPointEntitys(entityQueryable.ToArray()).ToList()).ToList();//根据测点数组获取因子列
            }
            return GetPointEffectRate(portIds.ToArray(), factors.ToArray(), dtmStart, dtmEnd);
        }

        /// <summary>
        /// 获取某一时间段创模点所有监测点数据总有效率
        /// <summary>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <returns></returns>
        public decimal GetModelEffectRate(DateTime dtmStart, DateTime dtmEnd)
        {
            //获取创模点位列表
            IQueryable<MonitoringPointEntity> entityQueryable = g_MonitoringPointAir.RetrieveAirMPListByModel();
            string[] pointIds = GetPointIdsByPointEntitys(entityQueryable.ToArray());//根据测点数组获取测点Id列
            string[] factors = GetPollutantCodesByPointEntitys(entityQueryable.ToArray());//根据测点数组获取因子列
            return GetPointEffectRate(pointIds, factors, dtmStart, dtmEnd);
        }
        #endregion

        #region 有效率明细
        /// <summary>
        /// 国控点各个污染物有效率明细
        /// <summary>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>返回结果集
        /// 返回的列名同GetPointEffectRateDetail
        /// </returns>
        public DataView GetLocusOfControlEffectRateDetail(DateTime dtmStart, DateTime dtmEnd,
            int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            recordTotal = 0;
            try
            {
                //获取国控点位列表
                IQueryable<MonitoringPointEntity> entityQueryable = g_MonitoringPointAir.RetrieveAirMPListByCountryControlled();
                string[] pointIds = GetPointIdsByPointEntitys(entityQueryable.ToArray());//根据测点数组获取测点Id列
                string[] factors = GetPollutantCodesByPointEntitys(entityQueryable.ToArray());//根据测点数组获取因子列
                return GetPointEffectRateDetail(pointIds, factors, dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取某一时间段监测点各个污染物有效率明细（因子不可选，测点自动关联因子）
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>返回结果集
        /// 返回的列名同GetPointEffectRateDetail
        /// </returns>
        public DataView GetPointEffectRateDetail(string[] portIds, DateTime dtmStart, DateTime dtmEnd,
            int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            recordTotal = 0;
            try
            {
                string[] factors = GetPollutantCodesByPointIds(portIds);//根据测点Id数组获取因子列
                return GetPointEffectRateDetail(portIds, factors, dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取某一时间段监测点各个污染物有效率明细（因子可选）
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// PointId：测点Id
        /// Tstamp：日期
        /// 因子代码（多列，不是固定值）：如a21005
        /// 因子代码_Status（多列，不是固定值）：如a21005_Status
        /// blankspaceColumn：空白列
        /// </returns>
        public DataView GetPointEffectRateDetail(string[] portIds, string[] factors, DateTime dtmStart, DateTime dtmEnd,
            int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            recordTotal = 0;
            try
            {
                return GetEffectRateDataPager(portIds, factors, dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取某一时间段苏州市区所有监测点各个污染物有效率明细
        /// <summary>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>返回结果集
        /// 返回的列名同GetPointEffectRateDetail
        /// </returns>
        public DataView GetSuZhouEffectRateDetail(DateTime dtmStart, DateTime dtmEnd, int pageSize, int pageNo,
            out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            recordTotal = 0;
            try
            {
                DictionaryService dictionaryService = Singleton<DictionaryService>.GetInstance();
                string guid = dictionaryService.GetValueByText(DictionaryType.Air, "点位城市类型", "苏州市");//获取苏州市区的Uid

                //根据行政区划类型获取点位列表
                IQueryable<MonitoringPointEntity> entityQueryable = g_MonitoringPointAir.RetrieveAirMPListByRegion(guid);
                string[] pointIds = GetPointIdsByPointEntitys(entityQueryable.ToArray());//根据测点数组获取测点Id列
                string[] factors = GetPollutantCodesByPointEntitys(entityQueryable.ToArray());//根据测点数组获取因子列
                return GetPointEffectRateDetail(pointIds, factors, dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取某一时间段某一城区所有监测点各个污染物有效率明细
        /// <summary>
        /// <param name="urbanAreas">城区数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>返回结果集
        /// 返回的列名同GetPointEffectRateDetail
        /// </returns>
        public DataView GetUrbanAreaEffectRateDetail(IList<string> urbanAreas, DateTime dtmStart, DateTime dtmEnd,
            int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            recordTotal = 0;
            try
            {
                IList<string> portIds = new List<string>();
                IList<string> factors = new List<string>();
                foreach (string urbanArea in urbanAreas)
                {
                    //根据行政区划类型获取点位列表
                    IQueryable<MonitoringPointEntity> entityQueryable = g_MonitoringPointAir.RetrieveAirMPListByRegion(urbanArea);
                    portIds = portIds.Union(GetPointIdsByPointEntitys(entityQueryable.ToArray()).ToList()).ToList();//根据测点数组获取测点Id列
                    factors = factors.Union(GetPollutantCodesByPointEntitys(entityQueryable.ToArray()).ToList()).ToList();//根据测点数组获取因子列
                }
                return GetPointEffectRateDetail(portIds.ToArray(), factors.ToArray(), dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取某一时间段乡镇所有监测点各个污染物有效率明细
        /// <summary>
        /// <param name="villagesTowns">乡镇数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>返回结果集
        /// 返回的列名同GetPointEffectRateDetail
        /// </returns>
        public DataView GetVillagesTownsEffectRateDetail(IList<string> villagesTownsList,
            DateTime dtmStart, DateTime dtmEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            recordTotal = 0;
            try
            {
                IList<string> portIds = new List<string>();
                IList<string> factors = new List<string>();
                foreach (string villagesTowns in villagesTownsList)
                {
                    //根据监测区域类型获取点位列表
                    IQueryable<MonitoringPointEntity> entityQueryable = g_MonitoringPointAir.RetrieveAirMPListByMonitoringRegion(villagesTowns);
                    portIds = portIds.Union(GetPointIdsByPointEntitys(entityQueryable.ToArray()).ToList()).ToList();//根据测点数组获取测点Id列
                    factors = factors.Union(GetPollutantCodesByPointEntitys(entityQueryable.ToArray()).ToList()).ToList();//根据测点数组获取因子列
                }
                return GetPointEffectRateDetail(portIds.ToArray(), factors.ToArray(), dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取某一时间段创模点所有监测点各个污染物有效率明细
        /// <summary>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>返回结果集
        /// 返回的列名同GetPointEffectRateDetail
        /// </returns>
        public DataView GetModelEffectRateDetail(DateTime dtmStart, DateTime dtmEnd,
            int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            recordTotal = 0;
            try
            {
                //获取创模点位列表
                IQueryable<MonitoringPointEntity> entityQueryable = g_MonitoringPointAir.RetrieveAirMPListByModel();
                string[] pointIds = GetPointIdsByPointEntitys(entityQueryable.ToArray());//根据测点数组获取测点Id列
                string[] factors = GetPollutantCodesByPointEntitys(entityQueryable.ToArray());//根据测点数组获取因子列
                return GetPointEffectRateDetail(pointIds, factors, dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取各个测点的有效率统计数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="dvStatistical">总计行的数据
        /// 返回的列名同返回结果集，PointName的值为合计
        /// </param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// PointId：测点Id
        /// PointName：测点名称
        /// IntegratorName：集成商名称
        /// Days：天数
        /// ShouldCount：应测数据条数
        /// EffectCount：有效数据条数
        /// RealCount：实测数据条数
        /// EffectRate：有效率
        /// blankspaceColumn：空白列
        /// </returns>
        public DataView GetPointEffectRateStatisticalData(string[] portIds, DateTime dtmStart, DateTime dtmEnd, int pageSize, int pageNo,
            out int recordTotal, out DataView dvStatistical, string orderBy = "PointId,Tstamp")
        {
            int shouldDays = (dtmEnd - dtmStart).Days + 1;//应测天数
            dtmStart = new DateTime(dtmStart.Year, dtmStart.Month, dtmStart.Day, 0, 0, 0);
            dtmEnd = new DateTime(dtmEnd.Year, dtmEnd.Month, dtmEnd.Day, 23, 59, 59);
            int shouldCount = shouldDays * 24;//应测数据
            int startIndex = pageSize * pageNo;//要返回的数据的开始位置(从0开始)
            int endIndex = pageSize * (pageNo + 1);//要返回的数据的结束位置(数据取到小于这个值)
            int allEffectCount = 0;//总有效条数
            int allRealCount = 0;//总实测条数
            int allShouldCount = 0;//总应测条数
            DataTable dtNew = new DataTable();//新建表
            DataTable dtReturn;//要返回的视图的表
            DataTable dtStatistical;//统计行
            string[] factors;
            IList<string> pointListDistinct = new List<string>();
            Dictionary<string, IList<string>> pointPollutantCodeList = GetPointPollutantCodeByPointIds(portIds);//根据测点Id数组获取测点Id和对应的因子Code
            Dictionary<string, string> pointInstrumentList = GetInstrumentNamesByPointIds(portIds);//根据测点Id数组获取测点Id和集成商名称的对应关系列 
            dtNew.Columns.Add("PointId", typeof(int));//测点Id
            dtNew.Columns.Add("PointName", typeof(string));//测点名称
            dtNew.Columns.Add("IntegratorName", typeof(string));//集成商名称
            dtNew.Columns.Add("Days", typeof(int));//天数
            dtNew.Columns.Add("ShouldCount", typeof(int));//应测数据条数
            dtNew.Columns.Add("EffectCount", typeof(int));//有效数据条数
            dtNew.Columns.Add("RealCount", typeof(int));//实测数据条数
            dtNew.Columns.Add("EffectRate", typeof(string));//有效率
            dtNew.Columns.Add("blankspaceColumn", typeof(string));//空白列
            g_IInfectantRepository = Singleton<InfectantBy60Repository>.GetInstance();
            g_AirHourData = Singleton<HourReportRepository>.GetInstance();
            recordTotal = 0;
            dvStatistical = null;
            dtStatistical = dtNew.Clone();
            DataRow drStatistical = dtStatistical.NewRow();//统计行信息
            drStatistical["PointName"] = "合计";
            drStatistical["Days"] = shouldDays;
            dtStatistical.Rows.Add(drStatistical);

            if (g_IInfectantRepository != null && g_AirHourData != null)
            {
                factors = GetPollutantCodesByPointIds(portIds);//根据测点Id数组获取因子列
                DataTable dtOriginal = g_IInfectantRepository.GetDataPager(portIds, factors, dtmStart, dtmEnd, int.MaxValue, 0, out recordTotal, orderBy).Table;//原始数据
                DataTable dtAudit = g_AirHourData.GetDataPager(portIds, factors, dtmStart, dtmEnd, int.MaxValue, 0, out recordTotal, orderBy).Table;//审核数据
                for (int i = 0; i < dtOriginal.Rows.Count; i++)
                {
                    DataRow dr = dtOriginal.Rows[i];

                    //如果新表中没有该站点，则添加该站点有效率数据
                    if (dtNew.Select(string.Format("PointId='{0}'", dr["PointId"])).Length == 0)
                    {
                        //根据测点Id获取测点名称
                        int pointId = Convert.ToInt32(dr["PointId"]);
                        int totalEffectCount = 0;//合计有效条数
                        int totalRealCount = 0;//合计实测条数
                        int totalShouldCount = 0;//合计应测条数
                        string pointName = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;
                        DataRow[] drsOriginalPoint = dtOriginal.Select(string.Format("PointId='{0}'", pointId));//同一测点的集合
                        IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId.ToString()))
                                        ? pointPollutantCodeList[pointId.ToString()] : new List<string>();//该测点对应的因子Code列

                        foreach (DataColumn dcOriginal in dtOriginal.Columns)
                        {
                            //当不是站点和时间列时，才计算有效率
                            if (dcOriginal.ColumnName != "PointId" && dcOriginal.ColumnName != "Tstamp" && !dcOriginal.ColumnName.Contains("_Status")
                               && !dcOriginal.ColumnName.Contains("_Mark") && dcOriginal.ColumnName != "TotalValue")
                            {
                                int realCount = 0;//实测条数
                                int effectCount = 0;//有效条数

                                //如果该测点对应的因子列中没有当前因子，则该因子为无效因子，直接跳过
                                if (!pollutantCodeList.Contains(dcOriginal.ColumnName))
                                {
                                    continue;
                                }
                                foreach (DataRow drOriginalPoint in drsOriginalPoint)
                                {
                                    if (drOriginalPoint[dcOriginal.ColumnName].ToString() != "")
                                    {
                                        realCount++;//实测条数累加

                                        //当审核数据和原始数据的测点和时间戳相同，则代表一条有效数据
                                        if (dtAudit.Columns.Contains(dcOriginal.ColumnName)
                                            && dtAudit.Select(string.Format("PointId='{0}' and Tstamp='{1}' and isnull({2},'-1')<>'-1'",
                                                drOriginalPoint["PointId"], drOriginalPoint["Tstamp"], dcOriginal.ColumnName)).Length > 0)
                                        {
                                            effectCount++;//有效条数累加
                                        }
                                    }
                                }

                                totalEffectCount += effectCount;
                                totalRealCount += realCount;
                                totalShouldCount += shouldCount;
                            }
                        }

                        //如果有应测条数，才计算出该测点的有效率
                        if (totalShouldCount > 0)
                        {
                            decimal totalEffectRate = Math.Round((decimal)totalEffectCount * 100 / totalShouldCount, 2);//有效运行率=有效总数/应测总数
                            DataRow drNew = dtNew.NewRow();
                            drNew["PointId"] = pointId;
                            drNew["PointName"] = pointName;
                            drNew["IntegratorName"] = pointInstrumentList[pointId.ToString()];
                            drNew["Days"] = shouldDays;
                            drNew["RealCount"] = totalRealCount;
                            drNew["ShouldCount"] = totalShouldCount;
                            drNew["EffectCount"] = totalEffectCount;
                            drNew["EffectRate"] = totalEffectRate.ToString() + "%";
                            dtNew.Rows.Add(drNew);
                        }
                    }
                }
                AddDataRowsByPointInstruments(portIds, pointInstrumentList, pointPollutantCodeList, shouldCount, shouldDays, dtNew);//根据测点Id数组和集成商列表增加空白数据行
                recordTotal = dtNew.Rows.Count;
                for (int i = 0; i < dtNew.Rows.Count; i++)
                {
                    DataRow drNew = dtNew.Rows[i];
                    int rowRealCount = int.TryParse(drNew["RealCount"].ToString(), out rowRealCount) ? rowRealCount : 0;
                    int rowShouldCount = int.TryParse(drNew["ShouldCount"].ToString(), out rowShouldCount) ? rowShouldCount : 0;
                    int rowEffectCount = int.TryParse(drNew["EffectCount"].ToString(), out rowEffectCount) ? rowEffectCount : 0;
                    allEffectCount += rowEffectCount;//总有效条数
                    allRealCount += rowRealCount;//总实测条数
                    allShouldCount += rowShouldCount;//总应测条数
                }
                if (allShouldCount > 0)
                {
                    decimal allEffectRate = Math.Round((decimal)allEffectCount * 100 / allShouldCount, 2);//有效运行率=有效总数/应测总数
                    drStatistical["RealCount"] = allRealCount;
                    drStatistical["ShouldCount"] = allShouldCount;
                    drStatistical["EffectCount"] = allEffectCount;
                    drStatistical["EffectRate"] = allEffectRate.ToString() + "%";
                }
                dtReturn = GetDataPagerByPageSize(dtNew, pageSize, pageNo, "PointId,IntegratorName");
                dvStatistical = dtStatistical.AsDataView();
                return dtReturn.AsDataView();
            }
            return null;
        }
        #endregion

        #region 获取数据有效率详情数据
        /// <summary>
        /// 获取测点的有效率详情数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// PointId：测点Id
        /// PointName：测点名称
        /// IntegratorName：集成商名称
        /// PollutantCode：因子Code
        /// PollutantName：因子名称
        /// Days：天数
        /// ShouldCount：应测数据条数
        /// EffectCount：有效数据条数
        /// RealCount：实测数据条数
        /// EffectRate：有效率
        /// blankspaceColumn：空白列
        /// </returns>
        public DataView GetEffectRateDetailData(string[] portIds, DateTime dtmStart, DateTime dtmEnd, int pageSize, int pageNo,
            out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            int shouldDays = (dtmEnd - dtmStart).Days + 1;//应测天数
            dtmStart = new DateTime(dtmStart.Year, dtmStart.Month, dtmStart.Day, 0, 0, 0);
            dtmEnd = new DateTime(dtmEnd.Year, dtmEnd.Month, dtmEnd.Day, 23, 59, 59);
            int shouldCount = shouldDays * 24;//应测数据
            int startIndex = pageSize * pageNo;//要返回的数据的开始位置(从0开始)
            int endIndex = pageSize * (pageNo + 1);//要返回的数据的结束位置(数据取到小于这个值)
            int totalEffectCount = 0;//合计有效条数
            int totalRealCount = 0;//合计实测条数
            int totalShouldCount = 0;//合计应测条数
            DataTable dtNew = new DataTable();//新建表
            DataTable dtReturn;//要返回的视图的表
            Dictionary<string, IList<string>> pointPollutantCodeList = GetPointPollutantCodeByPointIds(portIds);//根据测点Id数组获取测点Id和对应的因子Code
            Dictionary<string, string> pointInstrumentList = GetInstrumentNamesByPointIds(portIds);//根据测点Id数组获取测点Id和集成商名称的对应关系列 
            IList<PollutantCodeEntity> pollutantEntityList = GetPollutantDataByPointIds(portIds);//根据测点Id数组获取因子数据列
            dtNew.Columns.Add("PointId", typeof(int));//测点Id
            dtNew.Columns.Add("PointName", typeof(string));//测点名称
            dtNew.Columns.Add("IntegratorName", typeof(string));//集成商名称
            dtNew.Columns.Add("PollutantCode", typeof(string));//因子Code
            dtNew.Columns.Add("PollutantName", typeof(string));//因子名称
            dtNew.Columns.Add("Days", typeof(int));//天数
            dtNew.Columns.Add("ShouldCount", typeof(int));//应测数据条数
            dtNew.Columns.Add("EffectCount", typeof(int));//有效数据条数
            dtNew.Columns.Add("RealCount", typeof(int));//实测数据条数
            dtNew.Columns.Add("EffectRate", typeof(string));//有效率
            dtNew.Columns.Add("blankspaceColumn", typeof(string));//空白列
            g_IInfectantRepository = Singleton<InfectantBy60Repository>.GetInstance();
            g_AirHourData = Singleton<HourReportRepository>.GetInstance();
            recordTotal = 0;

            if (g_IInfectantRepository != null && g_AirHourData != null)
            {
                DataTable dtOriginal = g_IInfectantRepository.GetDataPager(portIds, pollutantEntityList.Select(t => t.PollutantCode).ToArray(), dtmStart, dtmEnd, int.MaxValue, 0, out recordTotal, orderBy).Table;//原始数据
                DataTable dtAudit = g_AirHourData.GetDataPager(portIds, pollutantEntityList.Select(t => t.PollutantCode).ToArray(), dtmStart, dtmEnd, int.MaxValue, 0, out recordTotal, orderBy).Table;//审核数据
                for (int i = 0; i < dtOriginal.Rows.Count; i++)
                {
                    DataRow dr = dtOriginal.Rows[i];

                    //如果新表中没有该站点，则添加该站点有效率数据
                    if (dtNew.Select(string.Format("PointId='{0}'", dr["PointId"])).Length == 0)
                    {
                        //根据测点Id获取测点名称
                        int pointId = Convert.ToInt32(dr["PointId"]);
                        string pointName = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;
                        DataRow[] drsOriginalPoint = dtOriginal.Select(string.Format("PointId='{0}'", pointId));//同一测点的集合
                        IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId.ToString()))
                                        ? pointPollutantCodeList[pointId.ToString()] : new List<string>();//该测点对应的因子Code列

                        foreach (DataColumn dcOriginal in dtOriginal.Columns)
                        {
                            //当不是站点和时间列时，才计算有效率
                            if (dcOriginal.ColumnName != "PointId" && dcOriginal.ColumnName != "Tstamp" && !dcOriginal.ColumnName.Contains("_Status")
                               && !dcOriginal.ColumnName.Contains("_Mark") && dcOriginal.ColumnName != "TotalValue")
                            {
                                int realCount = 0;//实测条数
                                int effectCount = 0;//有效条数

                                //如果该测点对应的因子列中没有当前因子，则该因子为无效因子，直接跳过
                                if (!pollutantCodeList.Contains(dcOriginal.ColumnName))
                                {
                                    continue;
                                }
                                foreach (DataRow drOriginalPoint in drsOriginalPoint)
                                {
                                    if (drOriginalPoint[dcOriginal.ColumnName].ToString() != "")
                                    {
                                        realCount++;//实测条数累加

                                        //当审核数据和原始数据的测点和时间戳相同，则代表一条有效数据
                                        if (dtAudit.Columns.Contains(dcOriginal.ColumnName)
                                            && dtAudit.Select(string.Format("PointId='{0}' and Tstamp='{1}' and isnull({2},'-1')<>'-1'",
                                                drOriginalPoint["PointId"], drOriginalPoint["Tstamp"], dcOriginal.ColumnName)).Length > 0)
                                        {
                                            effectCount++;//有效条数累加
                                        }
                                    }
                                }

                                //计算出该行的有效率
                                decimal effectRate = Math.Round((decimal)effectCount * 100 / shouldCount, 2);//有效运行率=有效总数/应测总数
                                DataRow drNew = dtNew.NewRow();
                                drNew["PointId"] = pointId;
                                drNew["PointName"] = pointName;
                                drNew["IntegratorName"] = pointInstrumentList[pointId.ToString()];
                                drNew["PollutantCode"] = dcOriginal.ColumnName;
                                PollutantCodeEntity pollutantEntity = pollutantEntityList.FirstOrDefault(t => t.PollutantCode == dcOriginal.ColumnName);
                                drNew["PollutantName"] = (pollutantEntity != null) ? pollutantEntity.PollutantName : string.Empty;
                                drNew["Days"] = shouldDays;
                                drNew["RealCount"] = realCount;
                                drNew["ShouldCount"] = shouldCount;
                                drNew["EffectCount"] = effectCount;
                                drNew["EffectRate"] = effectRate.ToString() + "%";
                                dtNew.Rows.Add(drNew);

                                //有数据才加上有效条数、实测条数和应测条数
                                totalEffectCount += effectCount;
                                totalRealCount += realCount;
                                totalShouldCount += shouldCount;
                            }
                        }
                    }
                }
                AddDataRowsByPointPollutants(portIds, pollutantEntityList.Select(t => t.PollutantCode).ToArray(),
                    pollutantEntityList, pointInstrumentList, pointPollutantCodeList, shouldCount, shouldDays, dtNew);//根据测点Id数组和因子Code数组增加空白数据行
                recordTotal = dtNew.Rows.Count;
                dtReturn = GetDataPagerByPageSize(dtNew, pageSize, pageNo, "PointId,IntegratorName,PollutantCode");
                return dtReturn.AsDataView();
            }
            return null;
        }

        /// <summary>
        /// 获取测点的有效率详情数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// PointId：测点Id
        /// PointName：测点名称
        /// IntegratorName：集成商名称
        /// PollutantCode：因子Code
        /// PollutantName：因子名称
        /// Days：天数
        /// ShouldCount：应测数据条数
        /// EffectCount：有效数据条数
        /// RealCount：实测数据条数
        /// EffectRate：有效率
        /// blankspaceColumn：空白列
        /// </returns>
        public DataView GetEffectRateDetailData(string[] portIds, string[] factors, DateTime dtmStart, DateTime dtmEnd, int pageSize, int pageNo,
            out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            int shouldDays = (dtmEnd - dtmStart).Days + 1;//应测天数
            dtmStart = new DateTime(dtmStart.Year, dtmStart.Month, dtmStart.Day, 0, 0, 0);
            dtmEnd = new DateTime(dtmEnd.Year, dtmEnd.Month, dtmEnd.Day, 23, 59, 59);
            int shouldCount = shouldDays * 24;//应测数据
            int startIndex = pageSize * pageNo;//要返回的数据的开始位置(从0开始)
            int endIndex = pageSize * (pageNo + 1);//要返回的数据的结束位置(数据取到小于这个值)
            int totalEffectCount = 0;//合计有效条数
            int totalRealCount = 0;//合计实测条数
            int totalShouldCount = 0;//合计应测条数
            DataTable dtNew = new DataTable();//新建表
            DataTable dtReturn;//要返回的视图的表
            Dictionary<string, IList<string>> pointPollutantCodeList = GetPointPollutantCodeByPointIds(portIds);//根据测点Id数组获取测点Id和对应的因子Code
            Dictionary<string, string> pointInstrumentList = GetInstrumentNamesByPointIds(portIds);//根据测点Id数组获取测点Id和集成商名称的对应关系列 
            IList<PollutantCodeEntity> pollutantEntityList = GetPollutantDataByPollutantCodes(factors);//根据因子Code数组获取因子数据列
            dtNew.Columns.Add("PointId", typeof(int));//测点Id
            dtNew.Columns.Add("PointName", typeof(string));//测点名称
            dtNew.Columns.Add("IntegratorName", typeof(string));//集成商名称
            dtNew.Columns.Add("PollutantCode", typeof(string));//因子Code
            dtNew.Columns.Add("PollutantName", typeof(string));//因子名称
            dtNew.Columns.Add("Days", typeof(int));//天数
            dtNew.Columns.Add("ShouldCount", typeof(int));//应测数据条数
            dtNew.Columns.Add("EffectCount", typeof(int));//有效数据条数
            dtNew.Columns.Add("RealCount", typeof(int));//实测数据条数
            dtNew.Columns.Add("EffectRate", typeof(string));//有效率
            dtNew.Columns.Add("blankspaceColumn", typeof(string));//空白列
            g_IInfectantRepository = Singleton<InfectantBy60Repository>.GetInstance();
            g_AirHourData = Singleton<HourReportRepository>.GetInstance();
            recordTotal = 0;

            if (g_IInfectantRepository != null && g_AirHourData != null)
            {
                DataTable dtOriginal = g_IInfectantRepository.GetDataPager(portIds, factors, dtmStart, dtmEnd, int.MaxValue, 0, out recordTotal, orderBy).Table;//原始数据
                DataTable dtAudit = g_AirHourData.GetDataPager(portIds, factors, dtmStart, dtmEnd, int.MaxValue, 0, out recordTotal, orderBy).Table;//审核数据
                for (int i = 0; i < dtOriginal.Rows.Count; i++)
                {
                    DataRow dr = dtOriginal.Rows[i];

                    //如果新表中没有该站点，则添加该站点有效率数据
                    if (dtNew.Select(string.Format("PointId='{0}'", dr["PointId"])).Length == 0)
                    {
                        //根据测点Id获取测点名称
                        int pointId = Convert.ToInt32(dr["PointId"]);
                        string pointName = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;
                        DataRow[] drsOriginalPoint = dtOriginal.Select(string.Format("PointId='{0}'", pointId));//同一测点的集合
                        IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId.ToString()))
                                        ? pointPollutantCodeList[pointId.ToString()] : new List<string>();//该测点对应的因子Code列

                        foreach (DataColumn dcOriginal in dtOriginal.Columns)
                        {
                            //当不是站点和时间列时，才计算有效率
                            if (dcOriginal.ColumnName != "PointId" && dcOriginal.ColumnName != "Tstamp" && !dcOriginal.ColumnName.Contains("_Status")
                               && !dcOriginal.ColumnName.Contains("_Mark") && dcOriginal.ColumnName != "TotalValue")
                            {
                                int realCount = 0;//实测条数
                                int effectCount = 0;//有效条数

                                //如果该测点对应的因子列中没有当前因子，则该因子为无效因子，直接跳过
                                if (!pollutantCodeList.Contains(dcOriginal.ColumnName))
                                {
                                    continue;
                                }
                                foreach (DataRow drOriginalPoint in drsOriginalPoint)
                                {
                                    if (drOriginalPoint[dcOriginal.ColumnName].ToString() != "")
                                    {
                                        realCount++;//实测条数累加

                                        //当审核数据和原始数据的测点和时间戳相同，则代表一条有效数据
                                        if (dtAudit.Columns.Contains(dcOriginal.ColumnName)
                                            && dtAudit.Select(string.Format("PointId='{0}' and Tstamp='{1}' and isnull({2},'-1')<>'-1'",
                                                drOriginalPoint["PointId"], drOriginalPoint["Tstamp"], dcOriginal.ColumnName)).Length > 0)
                                        {
                                            effectCount++;//有效条数累加
                                        }
                                    }
                                }

                                //计算出该行的有效率
                                decimal effectRate = Math.Round((decimal)effectCount * 100 / shouldCount, 2);//有效运行率=有效总数/应测总数
                                DataRow drNew = dtNew.NewRow();
                                drNew["PointId"] = pointId;
                                drNew["PointName"] = pointName;
                                drNew["IntegratorName"] = pointInstrumentList[pointId.ToString()];
                                drNew["PollutantCode"] = dcOriginal.ColumnName;
                                PollutantCodeEntity pollutantEntity = pollutantEntityList.FirstOrDefault(t => t.PollutantCode == dcOriginal.ColumnName);
                                drNew["PollutantName"] = (pollutantEntity != null) ? pollutantEntity.PollutantName : string.Empty;
                                drNew["Days"] = shouldDays;
                                drNew["RealCount"] = realCount;
                                drNew["ShouldCount"] = shouldCount;
                                drNew["EffectCount"] = effectCount;
                                drNew["EffectRate"] = effectRate.ToString() + "%";
                                dtNew.Rows.Add(drNew);

                                //有数据才加上有效条数、实测条数和应测条数
                                totalEffectCount += effectCount;
                                totalRealCount += realCount;
                                totalShouldCount += shouldCount;
                            }
                        }
                    }
                }
                AddDataRowsByPointPollutants(portIds, factors, pollutantEntityList, pointInstrumentList,
                    pointPollutantCodeList, shouldCount, shouldDays, dtNew);//根据测点Id数组和因子Code数组增加空白数据行
                recordTotal = dtNew.Rows.Count;
                dtReturn = GetDataPagerByPageSize(dtNew, pageSize, pageNo, "PointId,IntegratorName,PollutantCode");
                return dtReturn.AsDataView();
            }
            return null;
        }
        #endregion

        #region 获取数据基础方法
        /// <summary>
        /// 取得虚拟分页查询数据和总行数
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// PointId：测点Id
        /// Tstamp：日期
        /// 因子代码（多列，不是固定值）：如a21005
        /// 因子代码_Status（多列，不是固定值）：如a21005_Status
        /// TotalValue：合计
        /// blankspaceColumn：空白列
        /// </returns>
        public DataView GetEffectRateDataPager(string[] portIds, string[] factors
            , DateTime dtmStart, DateTime dtmEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            int shouldDays = (dtmEnd - dtmStart).Days + 1;//应测天数
            dtmStart = new DateTime(dtmStart.Year, dtmStart.Month, dtmStart.Day, 0, 0, 0);
            dtmEnd = new DateTime(dtmEnd.Year, dtmEnd.Month, dtmEnd.Day, 23, 59, 59);
            DataTable dtNew = new DataTable();//新建表
            DataTable dtReturn;//要返回的视图的表
            int shouldCount = shouldDays * 24;//应测数据
            int startIndex = pageSize * pageNo;//要返回的数据的开始位置(从0开始)
            int endIndex = pageSize * (pageNo + 1);//要返回的数据的结束位置(数据取到小于这个值)
            Dictionary<string, IList<string>> pointPollutantCodeList = GetPointPollutantCodeByPointIds(portIds);//根据测点Id数组获取测点Id和对应的因子Code
            recordTotal = 0;
            g_IInfectantRepository = Singleton<InfectantBy60Repository>.GetInstance();
            g_AirHourData = Singleton<HourReportRepository>.GetInstance();

            if (g_IInfectantRepository != null && g_AirHourData != null)
            {
                DataTable dtOriginal = g_IInfectantRepository.GetDataPager(portIds, factors, dtmStart, dtmEnd, int.MaxValue, 0, out recordTotal, orderBy).Table;//原始数据
                DataTable dtAudit = g_AirHourData.GetDataPager(portIds, factors, dtmStart, dtmEnd, int.MaxValue, 0, out recordTotal, orderBy).Table;//审核数据
                dtNew = dtOriginal.Clone();
                SetDataColumnDataType(dtNew);
                dtNew.Columns.Remove("blankspaceColumn");
                dtNew.Columns.Add("TotalValue", typeof(string));
                dtNew.Columns.Add("blankspaceColumn", typeof(string));
                for (int i = 0; i < dtOriginal.Rows.Count; i++)
                {
                    DataRow dr = dtOriginal.Rows[i];

                    //如果新表中没有该站点，则添加该站点有效率数据
                    if (dtNew.Select(string.Format("PointId='{0}'", dr["PointId"])).Length == 0)
                    {
                        decimal rowEffectCount = 0;//行合计有效条数
                        int rowShouldCount = 0;//行合计应测条数
                        DataRow drNew = dtNew.NewRow();
                        DataRow[] drsOriginalPoint = dtOriginal.Select(string.Format("PointId='{0}'", dr["PointId"]));
                        IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(dr["PointId"].ToString()))
                                        ? pointPollutantCodeList[dr["PointId"].ToString()] : new List<string>();//该测点对应的因子Code列
                        drNew["PointId"] = dr["PointId"];
                        drNew["Tstamp"] = dr["Tstamp"];

                        foreach (DataColumn dcNew in dtNew.Columns)
                        {
                            //当不是站点和时间列时，才计算有效率
                            if (dcNew.ColumnName != "PointId" && dcNew.ColumnName != "Tstamp" && !dcNew.ColumnName.Contains("_Status")
                                && !dcNew.ColumnName.Contains("_Mark"))
                            {
                                decimal realCount = 0;//实测条数
                                decimal effectCount = 0;//有效条数

                                //如果是合计列，则计算合计数据
                                if (dcNew.ColumnName == "TotalValue")
                                {
                                    //应测条数大于0时才计算出该列的有效率
                                    if (rowShouldCount > 0)
                                    {
                                        decimal rowEffectRate = Math.Round(rowEffectCount * 100 / rowShouldCount, 2);//有效运行率=有效总数/应测总数
                                        drNew[dcNew.ColumnName] = string.Format("{0}%<br/>{1}:{2}", rowEffectRate, rowEffectCount, rowShouldCount);
                                    }
                                    continue;
                                }

                                //如果该测点对应的因子列中没有当前因子，则该因子为无效因子，直接跳过
                                if (!pollutantCodeList.Contains(dcNew.ColumnName))
                                {
                                    continue;
                                }

                                //如果有Status列，则把该列的值赋值给新表
                                if (dtOriginal.Columns.Contains(dcNew.ColumnName + "_Status") && dtNew.Columns.Contains(dcNew.ColumnName + "_Status"))
                                {
                                    drNew[dcNew.ColumnName + "_Status"] = dr[dcNew.ColumnName + "_Status"];
                                }

                                foreach (DataRow drOriginalPoint in drsOriginalPoint)
                                {
                                    //该列值不为空时，计算条数
                                    if (drOriginalPoint[dcNew.ColumnName].ToString() != "")
                                    {
                                        realCount++;//实测条数累加

                                        //当审核数据和原始数据的测点和时间戳相同，则代表一条有效数据
                                        if (dtAudit.Columns.Contains(dcNew.ColumnName)
                                            && dtAudit.Select(string.Format("PointId='{0}' and Tstamp='{1}' and isnull({2},'-1')<>'-1'",
                                                drOriginalPoint["PointId"], drOriginalPoint["Tstamp"], dcNew.ColumnName)).Length > 0)
                                        {
                                            effectCount++;//有效条数累加
                                        }
                                    }
                                }
                                if (realCount == 0)
                                {
                                    //如果没有数据，则该列值为null
                                    drNew[dcNew.ColumnName] = null;
                                }
                                else
                                {
                                    //计算出该列的有效率
                                    effectCount = (effectCount > shouldCount) ? shouldCount : effectCount;
                                    decimal effectRate = Math.Round(effectCount * 100 / shouldCount, 2);//有效运行率=有效总数/应测总数
                                    drNew[dcNew.ColumnName] = string.Format("{0}%<br/>{1}:{2}", effectRate, effectCount, shouldCount);

                                    //有数据才加上有效条数和应测条数
                                    rowEffectCount += effectCount;
                                    rowShouldCount += shouldCount;
                                }
                            }
                        }
                        dtNew.Rows.Add(drNew);
                    }
                }
                AddDataRowsByPointIds(portIds, pointPollutantCodeList, shouldCount, dtNew);//根据测点Id数组增加空白数据行
                recordTotal = dtNew.Rows.Count;
                dtReturn = GetDataPagerByPageSize(dtNew, pageSize, pageNo, orderBy);
                return dtReturn.AsDataView();
            }
            return null;
        }

        /// <summary>
        /// 取得要导出的数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// PointId：测点Id
        /// Tstamp：日期
        /// 因子代码（多列，不是固定值）：如a21005
        /// 因子代码_Status（多列，不是固定值）：如a21005_Status
        /// </returns>
        public DataView GetEffectRateExportData(string[] portIds, string[] factors
            , DateTime dtmStart, DateTime dtmEnd, string orderBy = "PointId,Tstamp")
        {
            int shouldDays = (dtmEnd - dtmStart).Days + 1;//应测天数
            dtmStart = new DateTime(dtmStart.Year, dtmStart.Month, dtmStart.Day, 0, 0, 0);
            dtmEnd = new DateTime(dtmEnd.Year, dtmEnd.Month, dtmEnd.Day, 23, 59, 59);
            DataTable dtNew = new DataTable();//新建表
            int shouldCount = shouldDays * 24;//应测数据
            int recordTotal = 0;//总行数
            Dictionary<string, IList<string>> pointPollutantCodeList = GetPointPollutantCodeByPointIds(portIds);//根据测点Id数组获取测点Id和对应的因子Code
            g_IInfectantRepository = Singleton<InfectantBy60Repository>.GetInstance();
            g_AirHourData = Singleton<HourReportRepository>.GetInstance();

            if (g_IInfectantRepository != null && g_AirHourData != null)
            {
                DataTable dtOriginal = g_IInfectantRepository.GetDataPager(portIds, factors, dtmStart, dtmEnd, int.MaxValue, 0, out recordTotal, orderBy).Table;//原始数据
                DataTable dtAudit = g_AirHourData.GetDataPager(portIds, factors, dtmStart, dtmEnd, int.MaxValue, 0, out recordTotal, orderBy).Table;//审核数据
                dtNew = dtOriginal.Clone();
                SetDataColumnDataType(dtNew);
                dtNew.Columns.Add("TotalValue", typeof(string));
                for (int i = 0; i < dtOriginal.Rows.Count; i++)
                {
                    DataRow dr = dtOriginal.Rows[i];

                    //如果新表中没有该站点，则添加该站点有效率数据
                    if (dtNew.Select(string.Format("PointId='{0}'", dr["PointId"])).Length == 0)
                    {
                        decimal rowEffectCount = 0;//行合计有效条数
                        int rowShouldCount = 0;//行合计应测条数
                        DataRow drNew = dtNew.NewRow();
                        DataRow[] drsOriginalPoint = dtOriginal.Select(string.Format("PointId='{0}'", dr["PointId"]));
                        IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(dr["PointId"].ToString()))
                                        ? pointPollutantCodeList[dr["PointId"].ToString()] : new List<string>();//该测点对应的因子Code列
                        drNew["PointId"] = dr["PointId"];
                        drNew["Tstamp"] = dr["Tstamp"];

                        foreach (DataColumn dcNew in dtNew.Columns)
                        {
                            //当不是站点和时间列时，才计算有效率
                            if (dcNew.ColumnName != "PointId" && dcNew.ColumnName != "Tstamp" && !dcNew.ColumnName.Contains("_Status")
                                && !dcNew.ColumnName.Contains("_Mark"))
                            {
                                decimal realCount = 0;//实测条数
                                decimal effectCount = 0;//有效条数

                                //如果是合计列，则计算合计数据
                                if (dcNew.ColumnName == "TotalValue")
                                {
                                    //应测条数大于0时才计算出该列的捕获率
                                    if (rowShouldCount > 0)
                                    {
                                        decimal rowEffectRate = Math.Round(rowEffectCount * 100 / rowShouldCount, 2);//有效运行率=有效总数/应测总数
                                        drNew[dcNew.ColumnName] = string.Format("{0}% \r\n{1}:{2}", rowEffectRate, rowEffectCount, rowShouldCount);
                                    }
                                    continue;
                                }

                                //如果该测点对应的因子列中没有当前因子，则该因子为无效因子，直接跳过
                                if (!pollutantCodeList.Contains(dcNew.ColumnName))
                                {
                                    continue;
                                }

                                //如果有Status列，则把该列的值赋值给新表
                                if (dtOriginal.Columns.Contains(dcNew.ColumnName + "_Status") && dtNew.Columns.Contains(dcNew.ColumnName + "_Status"))
                                {
                                    drNew[dcNew.ColumnName + "_Status"] = dr[dcNew.ColumnName + "_Status"];
                                }

                                foreach (DataRow drOriginalPoint in drsOriginalPoint)
                                {
                                    if (drOriginalPoint[dcNew.ColumnName].ToString() != "")
                                    {
                                        realCount++;//实测条数累加

                                        //当审核数据和原始数据的测点和时间戳相同，则代表一条有效数据
                                        if (dtAudit.Columns.Contains(dcNew.ColumnName)
                                            && dtAudit.Select(string.Format("PointId='{0}' and Tstamp='{1}' and isnull({2},'-1')<>'-1'",
                                                drOriginalPoint["PointId"], drOriginalPoint["Tstamp"], dcNew.ColumnName)).Length > 0)
                                        {
                                            effectCount++;//有效条数累加
                                        }
                                    }
                                }
                                if (realCount == 0)
                                {
                                    //如果没有数据，则该列值为null
                                    drNew[dcNew.ColumnName] = null;
                                }
                                else
                                {
                                    //计算出该列的有效率
                                    effectCount = (effectCount > shouldCount) ? shouldCount : effectCount;
                                    decimal effectRate = Math.Round(effectCount * 100 / shouldCount, 2);//有效运行率=有效总数/应测总数
                                    drNew[dcNew.ColumnName] = string.Format("{0}% \r\n{1}:{2}", effectRate, effectCount, shouldCount);

                                    //有数据才加上有效条数和应测条数
                                    rowEffectCount += effectCount;
                                    rowShouldCount += shouldCount;
                                }
                            }
                        }
                        dtNew.Rows.Add(drNew);
                    }
                }
                AddDataRowsByPointIds(portIds, pointPollutantCodeList, shouldCount, dtNew);//根据测点Id数组增加空白数据行
                dtNew.DefaultView.Sort = orderBy;
                dtNew = dtNew.DefaultView.ToTable();
                return dtNew.AsDataView();
            }
            return null;
        }

        /// <summary>
        /// 取得数据总行数
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <returns></returns>
        public int GetEffectRateAllDataCount(string[] portIds, string[] factors, DateTime dtmStart, DateTime dtmEnd)
        {
            dtmStart = new DateTime(dtmStart.Year, dtmStart.Month, dtmStart.Day, 0, 0, 0);
            dtmEnd = new DateTime(dtmEnd.Year, dtmEnd.Month, dtmEnd.Day, 23, 59, 59);
            DataTable dtNew = new DataTable();//新建表
            int recordTotal;
            g_IInfectantRepository = Singleton<InfectantBy60Repository>.GetInstance();
            g_AirHourData = Singleton<HourReportRepository>.GetInstance();

            if (g_IInfectantRepository != null)
            {
                DataTable dt = g_IInfectantRepository.GetDataPager(portIds, factors, dtmStart, dtmEnd, int.MaxValue, 0, out recordTotal).Table;
                dtNew = dt.Clone();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];

                    //如果新表中没有该站点，则添加该站点
                    if (dtNew.Select(string.Format("PointId='{0}'", dr["PointId"])).Length == 0)
                    {
                        DataRow drNew = dtNew.NewRow();
                        drNew["PointId"] = dr["PointId"];
                        dtNew.Rows.Add(drNew);
                    }
                }
                return dtNew.Rows.Count;
            }
            return 0;
        }

        /// <summary>
        /// 取得统计数据（合计）
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// PollutantCode：因子代码
        /// PollutantTotal：因子数据
        /// </returns>
        public DataView GetEffectRateStatisticalData(string[] portIds, string[] factors, DateTime dtmStart, DateTime dtmEnd)
        {
            dtmStart = new DateTime(dtmStart.Year, dtmStart.Month, dtmStart.Day, 0, 0, 0);
            dtmEnd = new DateTime(dtmEnd.Year, dtmEnd.Month, dtmEnd.Day, 23, 59, 59);
            DataTable dtNew = new DataTable();//新建表
            int shouldCount = ((dtmEnd - dtmStart).Days + 1) * 24;//每个因子应测条数
            int recordTotal = 0;//总行数
            int pageSize = int.MaxValue;//每页记录数
            int pageNo = 0;//当前页(从0开始)
            string orderBy = "PointId,Tstamp";//排序方式
            decimal allEffectCount = 0;//总合计有效条数
            int allShouldCount = 0;//总合计应测条数
            decimal allEffectRate = 0;//总合计有效率
            Dictionary<string, IList<string>> pointPollutantCodeList = GetPointPollutantCodeByPointIds(portIds);//根据测点Id数组获取测点Id和对应的因子Code
            g_IInfectantRepository = Singleton<InfectantBy60Repository>.GetInstance();
            g_AirHourData = Singleton<HourReportRepository>.GetInstance();

            if (g_IInfectantRepository != null)
            {
                dtNew.Columns.Add("PollutantCode", typeof(string));//增加因子代码列
                dtNew.Columns.Add("PollutantTotal", typeof(string));//增加因子合计列
                DataTable dtOriginal = g_IInfectantRepository.GetDataPager(portIds, factors, dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy).Table;//原始数据
                DataTable dtAudit = g_AirHourData.GetDataPager(portIds, factors, dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy).Table;//审核数据

                foreach (DataColumn dcOriginal in dtOriginal.Columns)
                {
                    //当不是站点和时间列时，才计算有效率
                    if (dcOriginal.ColumnName != "PointId" && dcOriginal.ColumnName != "Tstamp" && !dcOriginal.ColumnName.Contains("_Status")
                        && !dcOriginal.ColumnName.Contains("_Mark"))
                    {
                        DataRow drNew = dtNew.NewRow();
                        int totalEffectCount = 0;//合计有效条数
                        int totalRealCount = 0;//合计实测条数
                        int totalShouldCount = 0;//合计应测条数
                        //IList<string> pointList = new List<string>();//有此监测项的站点列表
                        drNew["PollutantCode"] = dcOriginal.ColumnName;
                        dtNew.Rows.Add(drNew);

                        foreach (string pointId in portIds)//循环累计应测条数
                        {
                            IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId))
                                      ? pointPollutantCodeList[pointId] : new List<string>(); //该测点对应的因子Code列
                            if (pollutantCodeList.Contains(dcOriginal.ColumnName))//当前测点中有该因子，则累加应测条数
                            {
                                totalShouldCount += shouldCount;
                            }
                        }

                        //如果合计应测条数大于0，则计算有效率
                        if (totalShouldCount > 0)
                        {
                            foreach (DataRow drOriginal in dtOriginal.Rows)
                            {
                                IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(drOriginal["PointId"].ToString()))
                                               ? pointPollutantCodeList[drOriginal["PointId"].ToString()] : new List<string>();//该测点对应的因子Code列

                                //如果该测点对应的因子列中没有当前因子，则该因子为无效因子，直接跳过
                                if (!pollutantCodeList.Contains(dcOriginal.ColumnName))
                                {
                                    continue;
                                }

                                ////如果测点列表中还没有该测点，则添加测点到列表
                                //if (!pointList.Contains(drOriginal["PointId"].ToString()))
                                //{
                                //    pointList.Add(drOriginal["PointId"].ToString());
                                //}

                                //当有数据时，则累加条数
                                if (drOriginal[dcOriginal.ColumnName].ToString() != "")
                                {
                                    totalRealCount++;//实测条数累加

                                    //当审核数据和原始数据的测点和时间戳相同，则代表一条有效数据
                                    if (dtAudit.Columns.Contains(dcOriginal.ColumnName)
                                        && dtAudit.Select(string.Format("PointId='{0}' and Tstamp='{1}' and isnull({2},'-1')<>'-1'",
                                            drOriginal["PointId"], drOriginal["Tstamp"], dcOriginal.ColumnName)).Length > 0)
                                    {
                                        totalEffectCount++;//有效条数累加
                                    }
                                }
                            }

                            //计算出该列的有效率
                            totalEffectCount = (totalEffectCount > totalShouldCount) ? totalShouldCount : totalEffectCount;
                            decimal totalEffectRate = Math.Round((decimal)totalEffectCount * 100 / totalShouldCount, 2);//合计有效率
                            drNew["PollutantTotal"] = string.Format("{0}%<br/>{1}:{2}", totalEffectRate, totalEffectCount, totalShouldCount);

                            //如果有数据才增加实测条数和应测条数
                            allEffectCount += totalEffectCount;
                            allShouldCount += totalShouldCount;
                        }
                    }
                }
                for (int i = 0; i < factors.Length; i++)
                {
                    string factor = factors[i];
                    if (dtNew.Select(string.Format("PollutantCode='{0}'", factor)).Length == 0)
                    {
                        DataRow drNew = dtNew.NewRow();
                        drNew["PollutantCode"] = factor;
                        dtNew.Rows.Add(drNew);
                    }
                }
                if (allShouldCount == 0)
                {
                    dtNew.Rows.Add("TotalValue", null);//增加总合计数据
                }
                else
                {
                    allEffectCount = (allEffectCount > allShouldCount) ? allShouldCount : allEffectCount;
                    allEffectRate = Math.Round(allEffectCount * 100 / allShouldCount, 2);
                    dtNew.Rows.Add("TotalValue", string.Format("{0}%<br/>{1}:{2}", allEffectRate, allEffectCount, allShouldCount));//增加总合计数据
                }
                return dtNew.AsDataView();
            }
            return null;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 改变列的数据类型
        /// </summary>
        /// <param name="dt">要改变的表</param>
        private void SetDataColumnDataType(DataTable dt)
        {
            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ColumnName != "PointId" && dc.ColumnName != "Tstamp" && !dc.ColumnName.Contains("_Status")
                    && !dc.ColumnName.Contains("_Mark"))
                {
                    dc.DataType = typeof(string);
                }
            }
        }

        /// <summary>
        /// 获取分页后的数据
        /// </summary>
        /// <param name="dtOld">原始表</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="orderBy">排序方式（如：PointId,Tstamp）</param>
        /// <returns></returns>
        private DataTable GetDataPagerByPageSize(DataTable dtOld, int pageSize, int pageNo, string orderBy)
        {
            if (dtOld != null)
            {
                int startIndex = pageSize * pageNo;//要返回的数据的开始位置(从0开始)
                int endIndex = pageSize * (pageNo + 1);//要返回的数据的结束位置(数据取到小于这个值)
                DataTable dtReturn = dtOld.Clone();
                endIndex = (endIndex > dtOld.Rows.Count) ? dtOld.Rows.Count : endIndex;//如果结束位置大于最大行数，则改为最大行数的值
                orderBy = (orderBy != null) ? orderBy : string.Empty;
                string[] orderByValues = orderBy.Split(',');
                bool isOrderByContains = true;
                foreach (string orderByValue in orderByValues)
                {
                    isOrderByContains = dtOld.Columns.Contains(orderByValue);
                }
                if (isOrderByContains)
                {
                    dtOld.DefaultView.Sort = orderBy;
                    dtOld = dtOld.DefaultView.ToTable();
                }
                for (int i = startIndex; i < endIndex; i++)
                {
                    DataRow dr = dtOld.Rows[i];
                    dtReturn.Rows.Add(dr.ItemArray);
                }
                return dtReturn;
            }
            return null;
        }

        /// <summary>
        /// 根据因子数据生成新表
        /// </summary>
        /// <param name="factors"></param>
        /// <returns></returns>
        private DataTable CreateNewDataTableByPollutant(IList<IPollutant> factors)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PointId", typeof(string));
            foreach (IPollutant factor in factors)
            {
                if (!dt.Columns.Contains(factor.PollutantCode))
                {
                    dt.Columns.Add(factor.PollutantCode, typeof(string));
                }
            }
            dt.Columns.Add("TotalRealCount", typeof(int));
            dt.Columns.Add("TotalShouldCount", typeof(int));
            dt.Columns.Add("TotalValue", typeof(string));
            dt.Columns.Add("blankspaceColumn", typeof(string));
            return dt;
        }

        /// <summary>
        /// 根据因子数组生成新表
        /// </summary>
        /// <param name="pollutantCodes"></param>
        /// <returns></returns>
        private DataTable CreateNewDataTableByPollutant(string[] pollutantCodes)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PointId", typeof(string));
            foreach (string pollutantCode in pollutantCodes)
            {
                if (!dt.Columns.Contains(pollutantCode))
                {
                    dt.Columns.Add(pollutantCode, typeof(string));
                }
            }
            dt.Columns.Add("TotalRealCount", typeof(int));
            dt.Columns.Add("TotalShouldCount", typeof(int));
            dt.Columns.Add("TotalValue", typeof(string));
            dt.Columns.Add("blankspaceColumn", typeof(string));
            return dt;
        }

        /// <summary>
        /// 根据测点Id数组获取因子列
        /// </summary>
        /// <param name="pointIds">测点Id数组</param>
        /// <returns></returns>
        private string[] GetPollutantCodesByPointIds(string[] pointIds)
        {
            IQueryable<MonitoringPointEntity> monitorPointQueryable = g_MonitoringPointAir.RetrieveListByPointIds(pointIds);//根据站点ID数组获取站点
            string[] pollutantCodes = GetPollutantCodesByPointEntitys(monitorPointQueryable.ToArray());

            return pollutantCodes.GroupBy(p => p).Select(p => p.Key).ToArray();//去除重复
        }

        /// <summary>
        /// 根据测点数组获取因子列
        /// </summary>
        /// <param name="monitoringPointEntitys">测点数组</param>
        /// <returns></returns>
        private string[] GetPollutantCodesByPointEntitys(MonitoringPointEntity[] monitoringPointEntitys)
        {
            IList<string> pollutantList = new List<string>();
            InstrumentChannelService instrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();//提供仪器通道信息服务
            foreach (MonitoringPointEntity monitoringPointEntity in monitoringPointEntitys)
            {
                IQueryable<PollutantCodeEntity> pollutantCodeQueryable =
                    instrumentChannelService.RetrieveChannelListByPointUid(monitoringPointEntity.MonitoringPointUid);//根据站点Uid获取所有监测通道
                foreach (PollutantCodeEntity pollutantCodeEntity in pollutantCodeQueryable)
                {
                    pollutantList.Add(pollutantCodeEntity.PollutantCode);
                }
            }
            return pollutantList.ToArray();
        }

        /// <summary>
        /// 根据测点Id数组获取因子数据列
        /// </summary>
        /// <param name="pointIds">测点Id数组</param>
        /// <returns></returns>
        private IList<PollutantCodeEntity> GetPollutantDataByPointIds(string[] pointIds)
        {
            IQueryable<MonitoringPointEntity> monitorPointQueryable = g_MonitoringPointAir.RetrieveListByPointIds(pointIds);//根据站点ID数组获取站点
            IList<PollutantCodeEntity> pollutantCodeList = null;
            InstrumentChannelService instrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();
            foreach (MonitoringPointEntity monitoringPointEntity in monitorPointQueryable)
            {
                IQueryable<PollutantCodeEntity> pollutantCodeQueryable =
                            instrumentChannelService.RetrieveChannelListByPointUid(monitoringPointEntity.MonitoringPointUid);//根据站点Uid获取所有监测通道
                if (pollutantCodeList == null)
                {
                    pollutantCodeList = pollutantCodeQueryable.ToList();
                }
                else
                {
                    pollutantCodeList = pollutantCodeList.Union(pollutantCodeQueryable).ToList();
                }
            }
            return pollutantCodeList;
        }

        /// <summary>
        /// 根据因子Code数组获取因子数据列
        /// </summary>
        /// <param name="pollutantCodes">因子Code数组</param>
        /// <returns></returns>
        private IList<PollutantCodeEntity> GetPollutantDataByPollutantCodes(string[] pollutantCodes)
        {
            PollutantCodeRepository channelRepository = new PollutantCodeRepository();
            IList<PollutantCodeEntity> pollutantCodeList = channelRepository.Retrieve(t => pollutantCodes.Contains(t.PollutantCode)).ToList();
            return pollutantCodeList;
        }

        /// <summary>
        /// 根据测点数组获取测点Id列
        /// </summary>
        /// <param name="monitoringPointEntitys">测点数组</param>
        /// <returns></returns>
        private string[] GetPointIdsByPointEntitys(MonitoringPointEntity[] monitoringPointEntitys)
        {
            IList<string> pointIdList = new List<string>();
            foreach (MonitoringPointEntity monitoringPointEntity in monitoringPointEntitys)
            {
                if (!pointIdList.Contains(monitoringPointEntity.PointId.ToString()))
                {
                    pointIdList.Add(monitoringPointEntity.PointId.ToString());
                }
            }
            return pointIdList.ToArray();
        }

        /// <summary>
        /// 根据测点Id数组获取测点Id和对应的因子Code
        /// </summary>
        /// <param name="pointIds"></param>
        /// <returns></returns>
        private Dictionary<string, IList<string>> GetPointPollutantCodeByPointIds(string[] pointIds)
        {
            DictionaryService dictionaryService = Singleton<DictionaryService>.GetInstance();
            InstrumentChannelService instrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();//提供仪器通道信息服务
            IQueryable<MonitoringPointEntity> monitorPointQueryable = g_MonitoringPointAir.RetrieveListByPointIds(pointIds);//根据站点ID数组获取站点
            Dictionary<string, IList<string>> pointPollutantCodeList = new Dictionary<string, IList<string>>();
            foreach (MonitoringPointEntity monitoringPointEntity in monitorPointQueryable)
            {
                if (!pointPollutantCodeList.ContainsKey(monitoringPointEntity.PointId.ToString()))
                {
                    IQueryable<PollutantCodeEntity> pollutantCodeQueryable =
                        instrumentChannelService.RetrieveChannelListByPointUid(monitoringPointEntity.MonitoringPointUid);//根据站点Uid获取所有监测通道
                    pointPollutantCodeList.Add(monitoringPointEntity.PointId.ToString(), pollutantCodeQueryable.Select(t => t.PollutantCode).ToList());
                }
            }
            return pointPollutantCodeList;
        }

        /// <summary>
        /// 根据测点Id数组获取测点Id和集成商名称的对应关系列 
        /// </summary>
        /// <param name="pointIds"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetInstrumentNamesByPointIds(string[] pointIds)
        {
            DictionaryService dictionaryService = Singleton<DictionaryService>.GetInstance();
            Dictionary<string, string> pointInstrumentList = new Dictionary<string, string>();
            IQueryable<MonitoringPointEntity> monitorPointQueryable = g_MonitoringPointAir.RetrieveListByPointIds(pointIds);//根据站点ID数组获取站点
            IQueryable<V_CodeMainItemEntity> codeMainItemQueryable = dictionaryService.RetrieveList(DictionaryType.AMS, "集成商类型");//获取城市类型
            foreach (MonitoringPointEntity monitoringPointEntity in monitorPointQueryable)
            {
                if (!pointInstrumentList.ContainsKey(monitoringPointEntity.PointId.ToString()))
                {
                    string instrumentName = codeMainItemQueryable.Where(t => t.ItemGuid.Equals(monitoringPointEntity.InstrumentIntegratorUid))
                                                .Select(t => t.ItemText).FirstOrDefault();//集成商名称
                    pointInstrumentList.Add(monitoringPointEntity.PointId.ToString(), instrumentName);
                }
            }
            return pointInstrumentList;
        }

        /// <summary>
        /// 根据站点Id获取IPoint列
        /// </summary>
        /// <param name="pointIds">站点Id</param>
        /// <returns></returns>
        private string[] GetPointList(params int[] pointIds)
        {
            IList<string> pointList = new List<string>();
            foreach (int pointId in pointIds)
            {
                MonitoringPointEntity monitorPointEntity = g_MonitoringPointAir.RetrieveEntityByPointId(pointId);
                if (monitorPointEntity != null)
                {
                    pointList.Add(monitorPointEntity.PointId.ToString());
                }
            }
            return pointList.ToArray();
        }

        /// <summary>
        /// 根据测点Id数组增加空白数据行
        /// </summary>
        /// <param name="pointIds">测点Id数组</param>
        /// <param name="pointPollutantCodeList">测点Id和对应的因子Code列表</param>
        /// <param name="shouldCount">应测条数</param>
        /// <param name="dt">表</param>
        private void AddDataRowsByPointIds(string[] pointIds, Dictionary<string, IList<string>> pointPollutantCodeList, int shouldCount, DataTable dt)
        {
            if (pointIds == null || pointIds.Length == 0 || dt == null)
            {
                return;
            }
            pointIds = pointIds.Distinct().ToArray();
            for (int i = 0; i < pointIds.Length; i++)
            {
                string pointId = pointIds[i];
                if (dt.Select(string.Format("PointId='{0}'", pointId)).Length == 0)
                {
                    DataRow dr = dt.NewRow();
                    IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId))
                                   ? pointPollutantCodeList[pointId] : new List<string>(); //该测点对应的因子Code列
                    int totalShouldCount = 0;//合计应测条数
                    dr["PointId"] = pointId;

                    foreach (string pollutantCode in pollutantCodeList)
                    {
                        if (dt.Columns.Contains(pollutantCode))//该因子属于该测点，才计算有效率
                        {
                            dr[pollutantCode] = string.Format("{0}%<br/>{1}:{2}", 0, 0, shouldCount);//该因子有效率
                            totalShouldCount += shouldCount;
                            dr["TotalValue"] = string.Format("{0}%<br/>{1}:{2}", 0, 0, totalShouldCount);//合计有效率
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
        }

        /// <summary>
        /// 根据测点Id数组和因子Code数组增加空白数据行 
        /// </summary>
        /// <param name="pointIds">测点Id数组</param>
        /// <param name="pollutantCodes">因子Code数组</param>
        /// <param name="pollutantCodes">因子数据列表</param>
        /// <param name="pointInstrumentList">集成商列表</param>
        /// <param name="pointPollutantCodeList">测点Id和对应的因子Code列表</param>
        /// <param name="shouldCount">应测条数</param>
        /// <param name="shouldDays">应测天数</param>
        /// <param name="dt">表</param>
        private void AddDataRowsByPointPollutants(string[] pointIds, string[] pollutantCodes,
            IList<PollutantCodeEntity> pollutantEntityList, Dictionary<string, string> pointInstrumentList,
            Dictionary<string, IList<string>> pointPollutantCodeList, int shouldCount, decimal shouldDays, DataTable dt)
        {
            if (pointIds == null || pointIds.Length == 0 || dt == null)
            {
                return;
            }
            pointIds = pointIds.Distinct().ToArray();
            for (int i = 0; i < pointIds.Length; i++)
            {
                string pointId = pointIds[i];
                for (int j = 0; j < pollutantCodes.Length; j++)
                {
                    string pollutantCode = pollutantCodes[j];
                    if (dt.Select(string.Format("PointId='{0}' and PollutantCode='{1}'", pointId, pollutantCode)).Length == 0)
                    {
                        DataRow dr = dt.NewRow();
                        MonitoringPointEntity entity = g_MonitoringPointAir.RetrieveEntityByPointId(int.Parse(pointId));//根据测点Id获取测点名称
                        PollutantCodeEntity pollutantEntity = pollutantEntityList.FirstOrDefault(t => t.PollutantCode == pollutantCode);
                        string pointName = (entity != null) ? entity.MonitoringPointName : string.Empty;
                        IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId))
                                   ? pointPollutantCodeList[pointId] : new List<string>(); //该测点对应的因子Code列
                        dr["PointId"] = pointId;
                        dr["PointName"] = pointName;
                        dr["IntegratorName"] = pointInstrumentList[pointId];
                        dr["PollutantCode"] = pollutantCode;
                        dr["PollutantName"] = (pollutantEntity != null) ? pollutantEntity.PollutantName : string.Empty;

                        if (pollutantCodeList.Contains(pollutantCode))//该因子属于该测点，才计算有效率
                        {
                            dr["Days"] = shouldDays;
                            dr["RealCount"] = 0;
                            dr["ShouldCount"] = shouldCount;
                            dr["EffectCount"] = 0;
                            dr["EffectRate"] = "0%";
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
        }

        /// <summary>
        /// 根据测点Id数组和集成商列表增加空白数据行 
        /// </summary>
        /// <param name="pointIds">测点Id数组</param>
        /// <param name="pointInstrumentList">集成商列表</param>
        /// <param name="pointPollutantCodeList">测点Id和对应的因子Code列表</param>
        /// <param name="shouldCount">应测条数</param>
        /// <param name="shouldDays">应测天数</param>
        /// <param name="dt">表</param>
        private void AddDataRowsByPointInstruments(string[] pointIds, Dictionary<string, string> pointInstrumentList,
            Dictionary<string, IList<string>> pointPollutantCodeList, int shouldCount, decimal shouldDays, DataTable dt)
        {
            if (pointIds == null || pointIds.Length == 0 || dt == null)
            {
                return;
            }
            pointIds = pointIds.Distinct().ToArray();
            for (int i = 0; i < pointIds.Length; i++)
            {
                string pointId = pointIds[i];
                if (dt.Select(string.Format("PointId='{0}'", pointId)).Length == 0)
                {
                    DataRow dr = dt.NewRow();
                    MonitoringPointEntity entity = g_MonitoringPointAir.RetrieveEntityByPointId(int.Parse(pointId));//根据测点Id获取测点名称
                    string pointName = (entity != null) ? entity.MonitoringPointName : string.Empty;
                    IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId))
                                   ? pointPollutantCodeList[pointId] : new List<string>(); //该测点对应的因子Code列
                    int rowShouldCount = shouldCount * pollutantCodeList.Count(t => dt.Columns.Contains(t));
                    dr["PointId"] = pointId;
                    dr["PointName"] = pointName;
                    dr["IntegratorName"] = pointInstrumentList[pointId];

                    if (rowShouldCount > 0)//有应测条数才计算有效率
                    {
                        dr["Days"] = shouldDays;
                        dr["RealCount"] = 0;
                        dr["ShouldCount"] = rowShouldCount;
                        dr["EffectCount"] = 0;
                        dr["EffectRate"] = "0%";
                    }
                    dt.Rows.Add(dr);
                }
            }
        }
        #endregion
    }
}
