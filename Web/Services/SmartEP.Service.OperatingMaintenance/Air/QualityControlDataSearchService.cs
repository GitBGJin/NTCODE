using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.MonitoringBusinessRepository.QualityControlOperation.Air;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Utilities.Web.WebServiceHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.OperatingMaintenance.Air
{
    public class QualityControlDataSearchService
    {
        /// <summary>
        /// 仓储层
        /// </summary>
        QualityControlDataSearchRepository r_ControlDataSearch = new QualityControlDataSearchRepository();
        /// <summary>
        /// 站点类
        /// </summary>
        MonitoringPointAirService g_MonitoringPointAir = null;
        /// <summary>
        /// 运维平台获取数据WebService路径
        /// </summary>
        private string m_OperationOMMPGetDataWebServiceUrl = System.Configuration.ConfigurationManager.AppSettings["OperationOMMPGetDataWebServiceUrl"].ToString();

        /// <summary>
        /// 运维平台任务处理WebService路径
        /// </summary>
        private string m_OperationTaskWebServiceUrl = System.Configuration.ConfigurationManager.AppSettings["OperationTaskWebServiceUrl"].ToString();

        /// <summary>
        /// 根据主键guid获取数据
        /// </summary>
        /// <param name="TaskGuid">任务</param>
        /// <returns></returns>
        public IQueryable<QC_Report_PMSharp5030FlowCheckCaliEntity> Retrieve(Guid guid)
        {
            return r_ControlDataSearch.Retrieve(p => p.TaskGuid == guid);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="customID"></param>
        /// <returns></returns>
        public IQueryable<QC_Report_PMSharp5030FlowCheckCaliEntity> GetPages(string[] AnaDevSN, DateTime dtBegin, DateTime dtEnd)
        {
            return r_ControlDataSearch.GetPages(AnaDevSN, dtBegin, dtEnd);
        }
        /// <summary>
        /// 根据主键DutyID获取数据
        /// </summary>
        /// <param name="customID"></param>
        /// <returns></returns>
        public IQueryable<QC_Report_PMSharp5030FlowCheckCaliEntity> GetList(Guid dutyId)
        {
            return r_ControlDataSearch.GetList(dutyId);
        }
        /// <summary>
        /// 通过站点ID获取站点内的所有仪器
        /// </summary>
        /// <param name="pointId">站点ID</param>
        /// <returns></returns>
        public DataTable GetInstrument()
        {
            object objData = WebServiceHelper.InvokeWebService(m_OperationOMMPGetDataWebServiceUrl, "TempGetDataWebService", "GetInsInfoByType", new object[] { "" });
            return objData as DataTable;
        }
        /// <summary>
        /// 通过站点ID获取站点内的所有仪器
        /// </summary>
        /// <param name="pointId">站点ID</param>
        /// <returns></returns>
        public DataTable GetInstrumentType(string guid)
        {
            object objData = WebServiceHelper.InvokeWebService(m_OperationOMMPGetDataWebServiceUrl, "TempGetDataWebService", "GetInstanceStatus", new object[] { guid });
            return objData as DataTable;
        }
        /// <summary>
        /// 获取所有仪器
        /// </summary>
        /// <param name="pointId">站点ID</param>
        /// <returns></returns>
        public DataView GetDataBasePager()
        {
            return r_ControlDataSearch.GetDataBasePager("1");
        }
        /// <summary>
        /// 获取站点信息
        /// </summary>
        /// <param name="ObjectType">测点类型：1水2气</param>
        /// <returns></returns>
        public DataView GetDataPointGuid(string[] rowGuid)
        {
            return r_ControlDataSearch.GetDataPointGuid(rowGuid);
        }
        /// <summary>
        /// 获取站点信息
        /// </summary>
        /// <param name="ObjectType">测点类型：1水2气</param>
        /// <returns></returns>
        public DataView GetDataPointName(string[] PointNames)
        {
            return r_ControlDataSearch.GetDataPointName(PointNames);
        }
        /// <summary>
        /// 获取仪器信息
        /// </summary>
        /// <param name="ObjectType">测点类型：1水2气</param>
        /// <returns></returns>
        public DataView GetDataByObjectType(string ObjectType)
        {
            return r_ControlDataSearch.GetDataByObjectType(ObjectType);
        }
        /// <summary>
        /// 获取仪器实例信息
        /// </summary>
        /// <param name="ObjectType">测点类型：1水2气</param>
        /// <returns></returns>
        public DataView GetInstanceDataByObjectType(string ObjectType, string IsSpareParts="")
        {
            return r_ControlDataSearch.GetInstanceDataByObjectType(ObjectType, IsSpareParts);
        }
        /// <summary>
        /// 获取所有仪器对应的状态
        /// </summary>
        /// <param name="pointId">站点ID</param>
        /// <returns></returns>
        public DataView GetDataPager(string objectType)
        {
            return r_ControlDataSearch.GetDataPager(objectType);
        }
        /// <summary>
        /// 获取所有仪器对应的状态
        /// </summary>
        /// <param name="pointId">站点ID</param>
        /// <returns></returns>
        public DataView GetDataByOperations()
        {
            return r_ControlDataSearch.GetDataByOperations();
        }
        /// <summary>
        /// 获取站点对应的所有仪器状态
        /// </summary>
        /// <param name="pointId">站点ID</param>
        /// <returns></returns>
        public DataView GetDataPointGuidPager(string objectType,string pointGuid)
        {
            return r_ControlDataSearch.GetDataPointGuidPager(objectType, pointGuid);
        }
        /// <summary>
        /// 获取所有仪器对应的状态
        /// </summary>
        /// <param name="pointId">站点ID</param>
        /// <returns></returns>
        public DataView GetDataPointPager(string objectType, string[] rowGuid)
        {
            return r_ControlDataSearch.GetDataPointPager(objectType, rowGuid);
        }
        string portIds = "";
        /// <summary>
        /// 仪器信息查询
        /// </summary>
        /// <param name="pointId">站点ID</param>
        /// <returns></returns>
        public DataView GetAllDataPager(string[] pointIds, string instrumentName, string[] SN, string[] inState, string[] Operators, DateTime dtBegin, DateTime dtEnd)
        {

            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            IQueryable<MonitoringPointEntity> entity = g_MonitoringPointAir.RetrieveListByPointIds(pointIds);
            if (pointIds != null)
            {
                for (int i = 0; i < pointIds.Length; i++)
                {
                    int pointId = Convert.ToInt32(pointIds[i]);
                    portIds += (entity.Where(x => x.PointId == pointId).Select(t => t.MonitoringPointUid).FirstOrDefault() + ",");
                }
            }
            portIds = portIds.Trim(',');
            string[] PointGuids = portIds.Split(',');
            DataView dv = r_ControlDataSearch.GetAllDataPager(PointGuids, instrumentName, SN, inState, Operators, dtBegin, dtEnd);
            DataTable dt = dv.ToTable();
            dt.Columns.Add("UserInfo", typeof(string));
            DataRow dr = dt.NewRow();
            DataView dvUser = r_ControlDataSearch.GetDataUserPager();
            for (var i = 0; i < dv.Count; i++)
            {
                string guid = dv[i]["TeamGuid"].ToString();
                dvUser.RowFilter = "TeamGuid='" + guid + "'";
                string name = "";
                for (var j = 0; j < dvUser.Count; j++)
                {
                    if (dvUser[j]["UserName"].ToString() != "" && dvUser[j]["Mobile"].ToString() != "")
                        name += dvUser[j]["UserName"].ToString() + "/" + dvUser[j]["Mobile"].ToString() + ",";
                    else
                        name += dvUser[j]["UserName"].ToString() + dvUser[j]["Mobile"].ToString() + ",";
                }
                name = name.Trim(',');
                dt.Rows[i]["UserInfo"] = name;

            }
            return dt.DefaultView;

        }
        /// <summary>
        ///仪器状态信息
        /// </summary>
        /// <returns></returns>
        public DataView GetDataStatePager()
        {
            return r_ControlDataSearch.GetDataStatePager();
        }
        /// <summary>
        ///Sharp5030颗粒物检查校准记录表
        /// </summary>
        /// <returns></returns>
        public DataView GetPMSharpDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return r_ControlDataSearch.GetPMSharpDataPager(SN, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///Thermo1400、1405颗粒物检查校准记录表
        /// </summary>
        /// <returns></returns>
        public DataView GetPMTeomSharpDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return r_ControlDataSearch.GetPMTeomSharpDataPager(SN, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///标准流量计检定核查记录
        /// </summary>
        /// <returns></returns>
        public DataView GetStdFlowMeterDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return r_ControlDataSearch.GetStdFlowMeterDataPager(SN, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///臭氧校准仪校准记录
        /// </summary>
        /// <returns></returns>
        public DataView GetO3HappenDevDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return r_ControlDataSearch.GetO3HappenDevDataPager(SN, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///氮氧化物分析仪动态校准记录
        /// </summary>
        /// <returns></returns>
        public DataView GetNOxDevDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return r_ControlDataSearch.GetNOxDevDataPager(SN, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///动态校准仪流量（标准气/稀释气）检查记录表
        /// </summary>
        /// <returns></returns>
        public DataView GetCaliDevDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return r_ControlDataSearch.GetCaliDevDataPager(SN, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///零气纯度检查记录
        /// </summary>
        /// <returns></returns>
        public DataView GetZeroGasDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return r_ControlDataSearch.GetZeroGasDataPager(SN, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///气体分析仪精密度检查记录
        /// </summary>
        /// <returns></returns>
        public DataView GetAnaDevPrecisionDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return r_ControlDataSearch.GetAnaDevPrecisionDataPager(SN, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///气体分析仪零点、跨度检查与调节记录
        /// </summary>
        /// <returns></returns>
        public DataView GetZeroAndSpanDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            DataView dv = r_ControlDataSearch.GetZeroAndSpanDataPager(SN, dtBegin, dtEnd, MissionName);
            DataTable dt = new DataTable();
            dt.Columns.Add("任务编号", typeof(string));
            dt.Columns.Add("仪器编号", typeof(string));
            dt.Columns.Add("日期", typeof(DateTime));
            dt.Columns.Add("污染物", typeof(string));
            dt.Columns.Add("零气", typeof(string));
            dt.Columns.Add("零气调节前", typeof(string));
            dt.Columns.Add("零气调节后", typeof(string));
            dt.Columns.Add("跨度气", typeof(string));
            dt.Columns.Add("跨度气调节前", typeof(string));
            dt.Columns.Add("跨度气调节后", typeof(string));
            dt.Columns.Add("斜率调节前", typeof(string));
            dt.Columns.Add("斜率调节后", typeof(string));
            dt.Columns.Add("截距调节前", typeof(string));
            dt.Columns.Add("截距调节后", typeof(string));
            dt.Columns.Add("增益调节前", typeof(string));
            dt.Columns.Add("增益调节后", typeof(string));
            dt.Columns.Add("备注", typeof(string));

            for (int i = 0; i < dv.Count; i++)
            {
                DataRow newRow = dt.NewRow();
                newRow["任务编号"] = dv[i]["TaskCode"].ToString();
                newRow["仪器编号"] = dv[i]["CaliDevSN"].ToString();
                newRow["日期"] = Convert.ToDateTime(dv[i]["FinishDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                newRow["污染物"] = "SO<SUB>2</SUB>";
                newRow["零气"] = dv[i]["SO2Zero"].ToString();
                newRow["零气调节前"] = dv[i]["SO2ZeroModifyBefore"].ToString();
                newRow["零气调节后"] = dv[i]["SO2ZeroModifyAfter"].ToString();
                newRow["跨度气"] = dv[i]["SO2Span"].ToString();
                newRow["跨度气调节前"] = dv[i]["SO2SpanModifyBefore"].ToString();
                newRow["跨度气调节后"] = dv[i]["SO2SpanModifyAfter"].ToString();
                newRow["斜率调节前"] = dv[i]["SO2SlopeModifyBefore"].ToString();
                newRow["斜率调节后"] = dv[i]["SO2SlopeModifyAfter"].ToString();
                newRow["截距调节前"] = dv[i]["SO2InterceptModifyBefore"].ToString();
                newRow["截距调节后"] = dv[i]["SO2InterceptModifyAfter"].ToString();
                newRow["增益调节前"] = dv[i]["SO2GainModifyBefore"].ToString();
                newRow["增益调节后"] = dv[i]["SO2GainModifyAfter"].ToString();
                newRow["备注"] = dv[i]["Memo"].ToString();
                dt.Rows.Add(newRow);
                DataRow drRow = dt.NewRow();
                drRow["任务编号"] = dv[i]["TaskCode"].ToString();
                drRow["仪器编号"] = dv[i]["CaliDevSN"].ToString();
                drRow["日期"] = Convert.ToDateTime(dv[i]["FinishDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                drRow["污染物"] = "CO";
                drRow["零气"] = dv[i]["COZero"].ToString();
                drRow["零气调节前"] = dv[i]["COZeroModifyBefore"].ToString();
                drRow["零气调节后"] = dv[i]["COZeroModifyAfter"].ToString();
                drRow["跨度气"] = dv[i]["COSpan"].ToString();
                drRow["跨度气调节前"] = dv[i]["COSpanModifyBefore"].ToString();
                drRow["跨度气调节后"] = dv[i]["COSpanModifyAfter"].ToString();
                drRow["斜率调节前"] = dv[i]["COSlopeModifyBefore"].ToString();
                drRow["斜率调节后"] = dv[i]["COSlopeModifyAfter"].ToString();
                drRow["截距调节前"] = dv[i]["COInterceptModifyBefore"].ToString();
                drRow["截距调节后"] = dv[i]["COInterceptModifyAfter"].ToString();
                drRow["增益调节前"] = dv[i]["COGainModifyBefore"].ToString();
                drRow["增益调节后"] = dv[i]["COGainModifyAfter"].ToString();
                drRow["备注"] = dv[i]["Memo"].ToString();
                dt.Rows.Add(drRow);
                DataRow drRowO3 = dt.NewRow();
                drRowO3["任务编号"] = dv[i]["TaskCode"].ToString();
                drRowO3["仪器编号"] = dv[i]["CaliDevSN"].ToString();
                drRowO3["日期"] = Convert.ToDateTime(dv[i]["FinishDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                drRowO3["污染物"] = "O<SUB>3</SUB>";
                drRowO3["零气"] = dv[i]["O3Zero"].ToString();
                drRowO3["零气调节前"] = dv[i]["O3ZeroModifyBefore"].ToString();
                drRowO3["零气调节后"] = dv[i]["O3ZeroModifyAfter"].ToString();
                drRowO3["跨度气"] = dv[i]["O3Span"].ToString();
                drRowO3["跨度气调节前"] = dv[i]["O3SpanModifyBefore"].ToString();
                drRowO3["跨度气调节后"] = dv[i]["O3SpanModifyAfter"].ToString();
                drRowO3["斜率调节前"] = dv[i]["O3SlopeModifyBefore"].ToString();
                drRowO3["斜率调节后"] = dv[i]["O3SlopeModifyAfter"].ToString();
                drRowO3["截距调节前"] = dv[i]["O3InterceptModifyBefore"].ToString();
                drRowO3["截距调节后"] = dv[i]["O3InterceptModifyAfter"].ToString();
                drRowO3["增益调节前"] = dv[i]["O3GainModifyBefore"].ToString();
                drRowO3["增益调节后"] = dv[i]["O3GainModifyAfter"].ToString();
                drRowO3["备注"] = dv[i]["Memo"].ToString();
                dt.Rows.Add(drRowO3);
                DataRow drRowNO = dt.NewRow();
                drRowNO["任务编号"] = dv[i]["TaskCode"].ToString();
                drRowNO["仪器编号"] = dv[i]["CaliDevSN"].ToString();
                drRowNO["日期"] = Convert.ToDateTime(dv[i]["FinishDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                drRowNO["污染物"] = "NO";
                drRowNO["零气"] = dv[i]["NOZero"].ToString();
                drRowNO["零气调节前"] = dv[i]["NOZeroModifyBefore"].ToString();
                drRowNO["零气调节后"] = dv[i]["NOZeroModifyAfter"].ToString();
                drRowNO["跨度气"] = dv[i]["NOSpan"].ToString();
                drRowNO["跨度气调节前"] = dv[i]["NOSpanModifyBefore"].ToString();
                drRowNO["跨度气调节后"] = dv[i]["NOSpanModifyAfter"].ToString();
                drRowNO["斜率调节前"] = dv[i]["NOSlopeModifyBefore"].ToString();
                drRowNO["斜率调节后"] = dv[i]["NOSlopeModifyAfter"].ToString();
                drRowNO["截距调节前"] = dv[i]["NOInterceptModifyBefore"].ToString();
                drRowNO["截距调节后"] = dv[i]["NOInterceptModifyAfter"].ToString();
                drRowNO["增益调节前"] = dv[i]["NOGainModifyBefore"].ToString();
                drRowNO["增益调节后"] = dv[i]["NOGainModifyAfter"].ToString();
                drRowNO["备注"] = dv[i]["Memo"].ToString();
                dt.Rows.Add(drRowNO);
                DataRow drRowNOx = dt.NewRow();
                drRowNOx["任务编号"] = dv[i]["TaskCode"].ToString();
                drRowNOx["仪器编号"] = dv[i]["CaliDevSN"].ToString();
                drRowNOx["日期"] = Convert.ToDateTime(dv[i]["FinishDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                drRowNOx["污染物"] = "NOx";
                drRowNOx["零气"] = dv[i]["NOxZero"].ToString();
                drRowNOx["零气调节前"] = dv[i]["NOxZeroModifyBefore"].ToString();
                drRowNOx["零气调节后"] = dv[i]["NOxZeroModifyAfter"].ToString();
                drRowNOx["跨度气"] = dv[i]["NOxSpan"].ToString();
                drRowNOx["跨度气调节前"] = dv[i]["NOxSpanModifyBefore"].ToString();
                drRowNOx["跨度气调节后"] = dv[i]["NOxSpanModifyAfter"].ToString();
                drRowNOx["斜率调节前"] = dv[i]["NOxSlopeModifyBefore"].ToString();
                drRowNOx["斜率调节后"] = dv[i]["NOxSlopeModifyAfter"].ToString();
                drRowNOx["截距调节前"] = dv[i]["NOxInterceptModifyBefore"].ToString();
                drRowNOx["截距调节后"] = dv[i]["NOxInterceptModifyAfter"].ToString();
                drRowNOx["增益调节前"] = dv[i]["NOxGainModifyBefore"].ToString();
                drRowNOx["增益调节后"] = dv[i]["NOxGainModifyAfter"].ToString();
                drRowNOx["备注"] = dv[i]["Memo"].ToString();
                dt.Rows.Add(drRowNOx);
            }
            return dt.DefaultView;
        }
        /// <summary>
        ///气体分析仪零漂、标漂检查记录
        /// </summary>
        /// <returns></returns>
        public DataView GetAnaDevDriftDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return r_ControlDataSearch.GetAnaDevDriftDataPager(SN, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///气体分析仪准确度审核记录
        /// </summary>
        /// <returns></returns>
        public DataView GetAnaDevDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return r_ControlDataSearch.GetAnaDevDataPager(SN, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///多点线性校准记录
        /// </summary>
        /// <returns></returns>
        public DataView GetMultiPointDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return r_ControlDataSearch.GetMultiPointDataPager(SN, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        /// 获取区域信息
        /// </summary>
        /// <returns></returns>
        public DataView GetDataAreaPager()
        {
            return r_ControlDataSearch.GetDataAreaPager();
        }
        /// <summary>
        /// 获取站点信息
        /// </summary>
        /// <returns></returns>
        public DataView GetDataPointPager()
        {
            return r_ControlDataSearch.GetDataPointPager();
        }
        #region new
        /// <summary>
        ///Sharp5030颗粒物检查校准记录表
        /// </summary>
        /// <returns></returns>
        public DataView GetPMSharpDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return r_ControlDataSearch.GetPMSharpDataNewPager(SN, pointIds, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///Thermo1400、1405颗粒物检查校准记录表
        /// </summary>
        /// <returns></returns>
        public DataView GetPMTeomSharpDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return r_ControlDataSearch.GetPMTeomSharpDataNewPager(SN, pointIds, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///标准流量计检定核查记录
        /// </summary>
        /// <returns></returns>
        public DataView GetStdFlowMeterDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return r_ControlDataSearch.GetStdFlowMeterDataNewPager(SN, pointIds, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///臭氧校准仪校准记录
        /// </summary>
        /// <returns></returns>
        public DataView GetO3HappenDevDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return r_ControlDataSearch.GetO3HappenDevDataNewPager(SN, pointIds, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///氮氧化物分析仪动态校准记录
        /// </summary>
        /// <returns></returns>
        public DataView GetNOxDevDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return r_ControlDataSearch.GetNOxDevDataNewPager(SN, pointIds, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///动态校准仪流量（标准气/稀释气）检查记录表
        /// </summary>
        /// <returns></returns>
        public DataView GetCaliDevDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return r_ControlDataSearch.GetCaliDevDataNewPager(SN, pointIds, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///零气纯度检查记录
        /// </summary>
        /// <returns></returns>
        public DataView GetZeroGasDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return r_ControlDataSearch.GetZeroGasDataNewPager(SN, pointIds, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///气体分析仪精密度检查记录
        /// </summary>
        /// <returns></returns>
        public DataView GetAnaDevPrecisionDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            DataTable dt = r_ControlDataSearch.GetAnaDevPrecisionDataNewPager(SN, pointIds, dtBegin, dtEnd, MissionName).ToTable();
            dt.Columns.Add("Code", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Code"] = dt.Rows[i]["污染物"].ToString();
            }
            return dt.DefaultView;
        }
        /// <summary>
        ///气体分析仪零点、跨度检查与调节记录
        /// </summary>
        /// <returns></returns>
        public DataView GetZeroAndSpanDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            DataView dv = r_ControlDataSearch.GetZeroAndSpanDataNewPager(SN, pointIds, dtBegin, dtEnd, MissionName);
            DataTable dt = new DataTable();
            dt.Columns.Add("测点", typeof(string));
            dt.Columns.Add("时间", typeof(DateTime));
            dt.Columns.Add("Code", typeof(string));
            dt.Columns.Add("污染物", typeof(string));
            dt.Columns.Add("零气", typeof(string));
            dt.Columns.Add("零气调节前", typeof(string));
            dt.Columns.Add("零气调节后", typeof(string));
            dt.Columns.Add("跨度气", typeof(string));
            dt.Columns.Add("跨度气调节前", typeof(string));
            dt.Columns.Add("跨度气调节后", typeof(string));
            dt.Columns.Add("斜率调节前", typeof(string));
            dt.Columns.Add("斜率调节后", typeof(string));
            dt.Columns.Add("截距调节前", typeof(string));
            dt.Columns.Add("截距调节后", typeof(string));
            dt.Columns.Add("增益调节前", typeof(string));
            dt.Columns.Add("增益调节后", typeof(string));
            dt.Columns.Add("仪器编号", typeof(string));
            dt.Columns.Add("任务编号", typeof(string));
            dt.Columns.Add("执行人", typeof(string));

            for (int i = 0; i < dv.Count; i++)
            {
                DataRow newRow = dt.NewRow();
                newRow["测点"] = dv[i]["PointName"].ToString();
                newRow["时间"] = Convert.ToDateTime(dv[i]["FinishDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                newRow["Code"] = "SO2";
                newRow["污染物"] = "SO2";
                newRow["零气"] = dv[i]["SO2Zero"].ToString();
                newRow["零气调节前"] = dv[i]["SO2ZeroModifyBefore"].ToString();
                newRow["零气调节后"] = dv[i]["SO2ZeroModifyAfter"].ToString();
                newRow["跨度气"] = dv[i]["SO2Span"].ToString();
                newRow["跨度气调节前"] = dv[i]["SO2SpanModifyBefore"].ToString();
                newRow["跨度气调节后"] = dv[i]["SO2SpanModifyAfter"].ToString();
                newRow["斜率调节前"] = dv[i]["SO2SlopeModifyBefore"].ToString();
                newRow["斜率调节后"] = dv[i]["SO2SlopeModifyAfter"].ToString();
                newRow["截距调节前"] = dv[i]["SO2InterceptModifyBefore"].ToString();
                newRow["截距调节后"] = dv[i]["SO2InterceptModifyAfter"].ToString();
                newRow["增益调节前"] = dv[i]["SO2GainModifyBefore"].ToString();
                newRow["增益调节后"] = dv[i]["SO2GainModifyAfter"].ToString();
                newRow["仪器编号"] = dv[i]["CaliDevSN"].ToString();
                newRow["任务编号"] = dv[i]["TaskCode"].ToString();
                newRow["执行人"] = dv[i]["ActionUserName"].ToString();
                dt.Rows.Add(newRow);
                DataRow drRow = dt.NewRow();
                drRow["测点"] = dv[i]["PointName"].ToString();
                drRow["时间"] = Convert.ToDateTime(dv[i]["FinishDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                drRow["Code"] = "CO";
                drRow["污染物"] = "CO";
                drRow["零气"] = dv[i]["COZero"].ToString();
                drRow["零气调节前"] = dv[i]["COZeroModifyBefore"].ToString();
                drRow["零气调节后"] = dv[i]["COZeroModifyAfter"].ToString();
                drRow["跨度气"] = dv[i]["COSpan"].ToString();
                drRow["跨度气调节前"] = dv[i]["COSpanModifyBefore"].ToString();
                drRow["跨度气调节后"] = dv[i]["COSpanModifyAfter"].ToString();
                drRow["斜率调节前"] = dv[i]["COSlopeModifyBefore"].ToString();
                drRow["斜率调节后"] = dv[i]["COSlopeModifyAfter"].ToString();
                drRow["截距调节前"] = dv[i]["COInterceptModifyBefore"].ToString();
                drRow["截距调节后"] = dv[i]["COInterceptModifyAfter"].ToString();
                drRow["增益调节前"] = dv[i]["COGainModifyBefore"].ToString();
                drRow["增益调节后"] = dv[i]["COGainModifyAfter"].ToString();
                drRow["仪器编号"] = dv[i]["CaliDevSN"].ToString();
                drRow["任务编号"] = dv[i]["TaskCode"].ToString();
                drRow["执行人"] = dv[i]["ActionUserName"].ToString();
                dt.Rows.Add(drRow);
                DataRow drRowO3 = dt.NewRow();
                drRowO3["测点"] = dv[i]["PointName"].ToString();
                drRowO3["时间"] = Convert.ToDateTime(dv[i]["FinishDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                drRowO3["Code"] = "O3";
                drRowO3["污染物"] = "O3";
                drRowO3["零气"] = dv[i]["O3Zero"].ToString();
                drRowO3["零气调节前"] = dv[i]["O3ZeroModifyBefore"].ToString();
                drRowO3["零气调节后"] = dv[i]["O3ZeroModifyAfter"].ToString();
                drRowO3["跨度气"] = dv[i]["O3Span"].ToString();
                drRowO3["跨度气调节前"] = dv[i]["O3SpanModifyBefore"].ToString();
                drRowO3["跨度气调节后"] = dv[i]["O3SpanModifyAfter"].ToString();
                drRowO3["斜率调节前"] = dv[i]["O3SlopeModifyBefore"].ToString();
                drRowO3["斜率调节后"] = dv[i]["O3SlopeModifyAfter"].ToString();
                drRowO3["截距调节前"] = dv[i]["O3InterceptModifyBefore"].ToString();
                drRowO3["截距调节后"] = dv[i]["O3InterceptModifyAfter"].ToString();
                drRowO3["增益调节前"] = dv[i]["O3GainModifyBefore"].ToString();
                drRowO3["增益调节后"] = dv[i]["O3GainModifyAfter"].ToString();
                drRowO3["仪器编号"] = dv[i]["CaliDevSN"].ToString();
                drRowO3["任务编号"] = dv[i]["TaskCode"].ToString();
                drRowO3["执行人"] = dv[i]["ActionUserName"].ToString();
                dt.Rows.Add(drRowO3);
                DataRow drRowNO = dt.NewRow();
                drRowNO["测点"] = dv[i]["PointName"].ToString();
                drRowNO["时间"] = Convert.ToDateTime(dv[i]["FinishDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                drRowNO["Code"] = "NO";
                drRowNO["污染物"] = "NO";
                drRowNO["零气"] = dv[i]["NOZero"].ToString();
                drRowNO["零气调节前"] = dv[i]["NOZeroModifyBefore"].ToString();
                drRowNO["零气调节后"] = dv[i]["NOZeroModifyAfter"].ToString();
                drRowNO["跨度气"] = dv[i]["NOSpan"].ToString();
                drRowNO["跨度气调节前"] = dv[i]["NOSpanModifyBefore"].ToString();
                drRowNO["跨度气调节后"] = dv[i]["NOSpanModifyAfter"].ToString();
                drRowNO["斜率调节前"] = dv[i]["NOSlopeModifyBefore"].ToString();
                drRowNO["斜率调节后"] = dv[i]["NOSlopeModifyAfter"].ToString();
                drRowNO["截距调节前"] = dv[i]["NOInterceptModifyBefore"].ToString();
                drRowNO["截距调节后"] = dv[i]["NOInterceptModifyAfter"].ToString();
                drRowNO["增益调节前"] = dv[i]["NOGainModifyBefore"].ToString();
                drRowNO["增益调节后"] = dv[i]["NOGainModifyAfter"].ToString();
                drRowNO["仪器编号"] = dv[i]["CaliDevSN"].ToString();
                drRowNO["任务编号"] = dv[i]["TaskCode"].ToString();
                drRowNO["执行人"] = dv[i]["ActionUserName"].ToString();
                dt.Rows.Add(drRowNO);
                DataRow drRowNOx = dt.NewRow();
                drRowNOx["测点"] = dv[i]["PointName"].ToString();
                drRowNOx["时间"] = Convert.ToDateTime(dv[i]["FinishDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                drRowNOx["Code"] = "NOx";
                drRowNOx["污染物"] = "NOx";
                drRowNOx["零气"] = dv[i]["NOxZero"].ToString();
                drRowNOx["零气调节前"] = dv[i]["NOxZeroModifyBefore"].ToString();
                drRowNOx["零气调节后"] = dv[i]["NOxZeroModifyAfter"].ToString();
                drRowNOx["跨度气"] = dv[i]["NOxSpan"].ToString();
                drRowNOx["跨度气调节前"] = dv[i]["NOxSpanModifyBefore"].ToString();
                drRowNOx["跨度气调节后"] = dv[i]["NOxSpanModifyAfter"].ToString();
                drRowNOx["斜率调节前"] = dv[i]["NOxSlopeModifyBefore"].ToString();
                drRowNOx["斜率调节后"] = dv[i]["NOxSlopeModifyAfter"].ToString();
                drRowNOx["截距调节前"] = dv[i]["NOxInterceptModifyBefore"].ToString();
                drRowNOx["截距调节后"] = dv[i]["NOxInterceptModifyAfter"].ToString();
                drRowNOx["增益调节前"] = dv[i]["NOxGainModifyBefore"].ToString();
                drRowNOx["增益调节后"] = dv[i]["NOxGainModifyAfter"].ToString();
                drRowNOx["仪器编号"] = dv[i]["CaliDevSN"].ToString();
                drRowNOx["任务编号"] = dv[i]["TaskCode"].ToString();
                drRowNOx["执行人"] = dv[i]["ActionUserName"].ToString();
                dt.Rows.Add(drRowNOx);
            }
            return dt.DefaultView;
        }
        /// <summary>
        ///气体分析仪零漂、标漂检查记录
        /// </summary>
        /// <returns></returns>
        public DataView GetAnaDevDriftDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return r_ControlDataSearch.GetAnaDevDriftDataNewPager(SN, pointIds, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///气体分析仪准确度审核记录
        /// </summary>
        /// <returns></returns>
        public DataView GetAnaDevDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return r_ControlDataSearch.GetAnaDevDataNewPager(SN, pointIds, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///多点线性校准记录
        /// </summary>
        /// <returns></returns>
        public DataView GetMultiPointDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return r_ControlDataSearch.GetMultiPointDataNewPager(SN, pointIds, dtBegin, dtEnd, MissionName);
        }
        #endregion
    }
}
