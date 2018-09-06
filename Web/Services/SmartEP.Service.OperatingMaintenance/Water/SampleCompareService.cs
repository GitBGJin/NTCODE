using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water;
using SmartEP.Utilities.Web.WebServiceHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SmartEP.Service.OperatingMaintenance.Water
{
    /// <summary>
    /// 名称：SampleCompareService.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-10-21
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 实样比对服务层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class SampleCompareService
    {
        /// <summary>
        /// 实验室比对记录表仓储层
        /// </summary>
        RealSamplesRepository r_RealSamples = new RealSamplesRepository();

        /// <summary>
        /// 质控任务记录表仓储层
        /// </summary>
        SampleCompareRepository r_SampleCompare = new SampleCompareRepository();
        /// <summary>
        /// 运维平台获取数据WebService路径
        /// </summary>
        private string m_OperationOMMPGetDataWebServiceUrl = System.Configuration.ConfigurationManager.AppSettings["OperationOMMPGetDataWebServiceUrl"].ToString();

        /// <summary>
        /// 运维平台任务处理WebService路径
        /// </summary>
        private string m_OperationTaskWebServiceUrl = System.Configuration.ConfigurationManager.AppSettings["OperationTaskWebServiceUrl"].ToString();

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="e_StandardSolutionCheck"></param>
        /// <param name="e_RealSample"></param>
        public void Add(StandardSolutionCheckEntity e_StandardSolutionCheck)
        {
            r_SampleCompare.AddEntity(e_StandardSolutionCheck);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="e_StandardSolutionCheck"></param>
        /// <param name="e_RealSample"></param>
        public void RealAdd(RealSampleEntity[] e_RealSample)
        {
            r_RealSamples.BatchAdd(e_RealSample.ToList());
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="e_StandardSolutionCheck"></param>
        /// <param name="e_RealSample"></param>
        public void RealUpdate(RealSampleEntity[] e_RealSample)
        {
            r_RealSamples.BatchUpdate(e_RealSample.ToList());
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="e_StandardSolutionCheck"></param>
        /// <param name="e_RealSample"></param>
        public void Delete(StandardSolutionCheckEntity e_StandardSolutionCheck)
        {
            r_SampleCompare.delete(e_StandardSolutionCheck);
        }

        public void Delete(int id)
        {
            StandardSolutionCheckEntity model;
            model = r_SampleCompare.RetrieveFirstOrDefault(p => p.Id == id);
            r_SampleCompare.Delete(model);
        }
        /// <summary>
        /// 获取实验室比对数据
        /// </summary>
        /// <param name="SampleNumber">样品编号</param>
        /// <param name="Tstamp">时间戳</param>
        /// <returns></returns>
        public List<RealSampleEntity> GetRealSamplesData(string TaskCode)
        {
            return r_RealSamples.RetrieveData(TaskCode).ToList<RealSampleEntity>();
        }
        /// <summary>
        /// 获取实验室比对数据
        /// </summary>
        /// <param name="SampleNumber">样品编号</param>
        /// <param name="Tstamp">时间戳</param>
        /// <returns></returns>
        public List<RealSampleEntity> GetRealSamplesDataNew(Guid Id)
        {
            return r_RealSamples.RetrieveDataNew(Id).ToList<RealSampleEntity>();
        }
        /// <summary>
        /// 获取质控任务数据
        /// </summary>
        /// <param name="taskId">任务id</param>
        /// <param name="Tstamp">时间戳</param>
        /// <returns></returns>
        public List<StandardSolutionCheckEntity> GetSampleCompareData(string taskId, int pointId, DateTime dtBegin, DateTime dtEnd)
        {
            return r_SampleCompare.RetrieveData(taskId, pointId, dtBegin, dtEnd).ToList<StandardSolutionCheckEntity>();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="e_StandardSolutionCheck"></param>
        /// <param name="e_RealSample"></param>
        public DataView GetDataPagers(string taskId, int pointId, DateTime dtBegin, DateTime dtEnd)
        {

            List<StandardSolutionCheckEntity> Standard = GetSampleCompareData(taskId, pointId, dtBegin, dtEnd);
            DataTable Standardt = new DataTable();
            DataTable Realdt = new DataTable();
            DataTable newdtb = new DataTable();
            newdtb.Columns.Add("Id", typeof(int));
            newdtb.Columns.Add("PointName", typeof(string));
            newdtb.Columns.Add("Tstamp", typeof(string));
            newdtb.Columns.Add("PollutantName", typeof(string));
            newdtb.Columns.Add("SampleNumber", typeof(string));
            newdtb.Columns.Add("LabValue", typeof(string));
            newdtb.Columns.Add("PollutantValue", typeof(string));
            newdtb.Columns.Add("RelativeOffset", typeof(string));
            newdtb.Columns.Add("CompareLimitValue", typeof(string));
            newdtb.Columns.Add("Tester", typeof(string));
            newdtb.Columns.Add("Evaluate", typeof(string));
            newdtb.Columns["Id"].AutoIncrement = true;
            string TaskCode = "";
            DateTime Tstamp = DateTime.Now;
            //检查实体集合不能为空
            if (Standard == null || Standard.Count < 1)
            {
                Standardt = null;
            }
            //取出第一个实体的所有Propertie
            else
            {
                Type StandardType = Standard[0].GetType();
                PropertyInfo[] StandardProperties = StandardType.GetProperties();

                //生成DataTable的structure
                //生产代码中，应将生成的DataTable结构Cache起来，此处略

                for (int i = 0; i < StandardProperties.Length; i++)
                {
                    Standardt.Columns.Add(StandardProperties[i].Name);
                }
                //将所有entity添加到DataTable中
                foreach (object Sentity in Standard)
                {
                    //检查所有的的实体都为同一类型
                    if (Sentity.GetType() != StandardType)
                    {
                        throw new Exception("要转换的集合元素类型不一致");
                    }
                    object[] StandardValues = new object[StandardProperties.Length];
                    for (int i = 0; i < StandardProperties.Length; i++)
                    {
                        StandardValues[i] = StandardProperties[i].GetValue(Sentity, null);
                    }
                    Standardt.Rows.Add(StandardValues);
                }
            }

            List<RealSampleEntity> RealSample = null;
            if (Standardt != null)
            {
                DataView Standardv = new DataView(Standardt);
                Standardt.Columns.Add("LabValue", typeof(string));
                Standardt.Columns.Add("CompareLimitValue", typeof(string));
                for (int i = 0; i < Standardv.Count; i++)
                {
                    TaskCode = Standardv[i]["TaskCode"].ToString();
                    Tstamp = Convert.ToDateTime(Standardv[i]["Tstamp"]);
                    RealSample = GetRealSamplesData(TaskCode);

                    //检查实体集合不能为空
                    if (RealSample == null || RealSample.Count < 1)
                    {
                        Realdt = null;
                    }
                    if (Realdt != null)
                    {
                        Standardt.Rows[i]["LabValue"] = RealSample[0].LabValue;
                        Standardt.Rows[i]["CompareLimitValue"] = RealSample[0].CompareLimitValue;
                    }
                    else
                    {
                        Standardt.Rows[i]["LabValue"] = "";
                        Standardt.Rows[i]["CompareLimitValue"] = "";
                    }
                }
            }
            else
            {
                return new DataView(newdtb);
            }
            DataView newdv = new DataView(Standardt);
            return newdv;
        }

        /// <summary>
        /// 通过站点ID获取站点内的所有仪器
        /// </summary>
        /// <param name="pointId">站点ID</param>
        /// <returns></returns>
        public DataTable GetInstrument(string pointId)
        {
            object objData = WebServiceHelper.InvokeWebService(m_OperationOMMPGetDataWebServiceUrl, "TempGetDataWebService", "GetInsTypeByObjectID", new object[] { pointId });
            return objData as DataTable;
            //return sr.GetInsTypeByObjectID(pointId);
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns>获取符合条件的数据</returns>
        public DataTable GetList(string taskCode, string ActionID = null, DateTime? dtimeStart = null, DateTime? dtimeEnd = null)
        {
            //string strWhere = "TaskCode='" + taskCode + "' and MissionID='" + missionId + "' and  PointId='" + pointId + "' and Tstamp>='" + datetime + "' and Tstamp<='" + endtime + "'";
            //if (!string.IsNullOrWhiteSpace(aciontId))
            //{
            //    strWhere += " and  ActionID='" + aciontId + "'";
            //}
            string strWhere = " 1=1 ";
            if (!string.IsNullOrWhiteSpace(taskCode))
            {
                strWhere += string.Format(" and TaskCode ='{0}' ", taskCode);
            }
            if (!string.IsNullOrWhiteSpace(ActionID))
            {
                strWhere += string.Format(" and ActionID ='{0}' ", ActionID);
            }
            if (dtimeStart != null)
            {
                strWhere += string.Format(" and Tstamp <='{0}' ", dtimeStart);
            }
            if (dtimeEnd != null)
            {
                strWhere += string.Format(" and Tstamp >='{0}' ", dtimeEnd);
            }
            if (strWhere == " 1=1 ")
            {
                strWhere += " and TaskCode='' ";
            }
            return r_SampleCompare.GetList(strWhere);
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns>获取符合条件的数据</returns>
        public DataTable GetLists(string taskCode)
        {
            string strWhere = " 1=1 ";
            if (!string.IsNullOrWhiteSpace(taskCode))
            {
                strWhere += string.Format(" and a.TaskCode ='{0}' ", taskCode);
            }
            if (strWhere == " 1=1 ")
            {
                strWhere += " and a.TaskCode='' ";
            }
            return r_SampleCompare.GetLists(strWhere);
        }
    }
}
