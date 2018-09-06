using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SmartEP.Utilities.Web.WebServiceHelper;

namespace SmartEP.Service.DataAnalyze.HomePageElements
{
    /// <summary>
    /// 名称：TaskManagementService.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-09-15
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：首页元件本周任务管理类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class TaskManagementService
    {
        /// <summary>
        /// 试剂标液更换表仓储层
        /// </summary>
        //PartChangeRepository r_partChange = Singleton<PartChangeRepository>.GetInstance();

        /// <summary>
        /// 质控运维WebService接口
        /// </summary>
        //TempGetDataWebServiceSoapClient sr = Singleton<TempGetDataWebServiceSoapClient>.GetInstance();

        /// <summary>
        /// 运维平台获取数据WebService路径
        /// </summary>
        private string m_OperationOMMPGetDataWebServiceUrl = System.Configuration.ConfigurationManager.AppSettings["OperationOMMPGetDataWebServiceUrl"].ToString();

        /// <summary>
        /// 运维平台任务处理WebService路径
        /// </summary>
        private string m_OperationTaskWebServiceUrl = System.Configuration.ConfigurationManager.AppSettings["OperationTaskWebServiceUrl"].ToString();

           
        /// <summary>
        /// 获取本周任务管理
        /// </summary>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// TaskTotal：任务总数
        /// DateTime：时间
        /// RoutineTask：例行任务
        /// Maintaining：维护保养
        /// MalfunctionRepair：故障维修
        /// InstrumentCheck：仪器校准
        /// SpecialTable：专项表单
        /// FinishRate：完成率
        /// </returns>
        public DataView GetTaskManagement(string[] statusValue, DateTime startWeek, DateTime endWeek, Dictionary<string, string> dicFormCodeOrName)
        {
            //Dictionary<string, string> dicFormCodeOrName = new Dictionary<string, string>();//质控的Code和Name
            List<string> yearList = new List<string>();//时间
            DataTable dtNew = null;//符合条件的全部数据
            DataTable dtFinishRateOriginal = null;//已完成（算完成率）的数据

            //参数全部为空是获取全部的数据
            object objData = WebServiceHelper.InvokeWebService(m_OperationTaskWebServiceUrl, "GetTaskWebService", "GetTaskInfo", new object[] { "", "", "" });
            DataTable dtOriginal = objData as DataTable;
            DataRow[] drOriginal = dtOriginal.Select(string.Format("EndDateActual>='{0}' and EndDateActual<='{1}' and( TaskStatus='{2}' or TaskStatus='{3}'or TaskStatus='{4}'or TaskStatus='{5}' )", startWeek.ToString("yyyy-MM-dd HH:mm:ss"), endWeek.ToString("yyyy-MM-dd HH:mm:ss"), "2", "3", "4", "5"));
            if (drOriginal.Count() > 0)
            {
                dtNew = drOriginal.CopyToDataTable();
            }
            else
            {
                dtNew = new DataTable();
                //return dtNew.DefaultView;
            }

            //完成率的数据
            DataRow[] drFinishRateOriginal = dtOriginal.Select(string.Format(" EndDateActual>='{0}' and EndDateActual<='{1}' and TaskStatus='{2}'", startWeek.ToString("yyyy-MM-dd HH:mm:ss"), endWeek.ToString("yyyy-MM-dd HH:mm:ss"), "4"));
            if (drFinishRateOriginal.Count() > 0)
            {
                dtFinishRateOriginal = drFinishRateOriginal.CopyToDataTable();
            }
            else
            {
                dtFinishRateOriginal = new DataTable();
            }
            //测试用的全部数据
            //dtNew = dtOriginal;

            // 把时间格式化为 yyyy-MM-dd 方面以后查询
            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                if (dtNew.Rows[i]["EndDateActual"] != DBNull.Value)
                {
                    dtNew.Rows[i]["EndDateActual"] = Convert.ToDateTime((dtNew.Rows[i]["EndDateActual"])).ToString("yyyy-MM-dd");
                }

            }

            //获得所有的质控任务code
            foreach (DataRow dr in dtNew.Rows)
            {
                if (dr["FormCode"] != DBNull.Value)
                {
                    if (dicFormCodeOrName.ContainsKey(dr["FormCode"].ToString()) == false)
                    {
                        dicFormCodeOrName.Add(dr["FormCode"].ToString(), dr["FormName"].ToString());
                    }
                }
                if (dr["EndDateActual"] != DBNull.Value)
                {
                    yearList.Add(dr["EndDateActual"].ToString());//质控的日期（格式为 yyyy-MM-dd）
                }
            }

            //生成本周任务管理表
            DataTable dtFil = CreateSequential(dicFormCodeOrName);

            //填充数据
            DataTable dtTian = AddNewRowToDataTable(dtNew, dtFinishRateOriginal, dtFil, dicFormCodeOrName, yearList.Distinct().ToArray());
            DataView dvTian = dtTian.DefaultView;
            dvTian.Sort = "EndDateActual Asc";
            return dvTian;

            #region 注释
            //DataTable dt = new DataTable();
            //dt.Columns.Add("TaskTotal", typeof(string));
            //dt.Columns.Add("DateTime", typeof(string));
            //dt.Columns.Add("RoutineTask", typeof(string));
            //dt.Columns.Add("Maintaining", typeof(string));
            //dt.Columns.Add("MalfunctionRepair", typeof(string));
            //dt.Columns.Add("InstrumentCheck", typeof(string));
            //dt.Columns.Add("SpecialTable", typeof(string));
            //dt.Columns.Add("FinishRate", typeof(string));

            //DataRow dr = dt.NewRow();
            //dr["TaskTotal"] = "630";
            //dr["DateTime"] = "2015-9-7";
            //dr["RoutineTask"] = "30";
            //dr["Maintaining"] = "9";
            //dr["MalfunctionRepair"] = "0";
            //dr["InstrumentCheck"] = "7";
            //dr["SpecialTable"] = "10";
            //dr["FinishRate"] = "9";
            //dt.Rows.Add(dr);

            //dr = dt.NewRow();
            //dr["TaskTotal"] = "630";
            //dr["DateTime"] = "2015-9-8";
            //dr["RoutineTask"] = "32";
            //dr["Maintaining"] = "15";
            //dr["MalfunctionRepair"] = "1";
            //dr["InstrumentCheck"] = "16";
            //dr["SpecialTable"] = "18";
            //dr["FinishRate"] = "13";
            //dt.Rows.Add(dr);

            //dr = dt.NewRow();
            //dr["TaskTotal"] = "630";
            //dr["DateTime"] = "2015-9-9";
            //dr["RoutineTask"] = "87";
            //dr["Maintaining"] = "23";
            //dr["MalfunctionRepair"] = "15";
            //dr["InstrumentCheck"] = "31";
            //dr["SpecialTable"] = "15";
            //dr["FinishRate"] = "27";
            //dt.Rows.Add(dr);

            //dr = dt.NewRow();
            //dr["TaskTotal"] = "630";
            //dr["DateTime"] = "2015-9-10";
            //dr["RoutineTask"] = "32";
            //dr["Maintaining"] = "23";
            //dr["MalfunctionRepair"] = "7";
            //dr["InstrumentCheck"] = "22";
            //dr["SpecialTable"] = "10";
            //dr["FinishRate"] = "15";
            //dt.Rows.Add(dr);

            //dr = dt.NewRow();
            //dr["TaskTotal"] = "630";
            //dr["DateTime"] = "2015-9-11";
            //dr["RoutineTask"] = "50";
            //dr["Maintaining"] = "24";
            //dr["MalfunctionRepair"] = "0";
            //dr["InstrumentCheck"] = "16";
            //dr["SpecialTable"] = "36";
            //dr["FinishRate"] = "20";
            //dt.Rows.Add(dr);

            //dr = dt.NewRow();
            //dr["TaskTotal"] = "630";
            //dr["DateTime"] = "2015-9-12";
            //dr["RoutineTask"] = "21";
            //dr["Maintaining"] = "4";
            //dr["MalfunctionRepair"] = "0";
            //dr["InstrumentCheck"] = "16";
            //dr["SpecialTable"] = "22";
            //dr["FinishRate"] = "10";
            //dt.Rows.Add(dr);

            //dr = dt.NewRow();
            //dr["TaskTotal"] = "630";
            //dr["DateTime"] = "2015-9-13";
            //dr["RoutineTask"] = "11";
            //dr["Maintaining"] = "9";
            //dr["MalfunctionRepair"] = "1";
            //dr["InstrumentCheck"] = "12";
            //dr["SpecialTable"] = "5";
            //dr["FinishRate"] = "6";
            //dt.Rows.Add(dr);

            //return dt.DefaultView;
            #endregion
        }

        /// <summary>
        /// 生成本周任务管理表 
        /// </summary>
        /// <param name="formCodeList">质控Code</param>
        /// <returns>返回的表的列如：EndDateActual、TaskTotal、1、2、3....</returns>
        private DataTable CreateSequential(Dictionary<string, string> dicFormCodeOrName)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("EndDateActual", typeof(string));//日期
            dt.Columns.Add("TaskTotal", typeof(int));//总数
            dt.Columns.Add("FinishRateTaskTotal", typeof(int));//完成的任务总数
            dt.Columns.Add("FinishRate", typeof(decimal));//完成率
            //拼接字段
            foreach (KeyValuePair<string, string> kv in dicFormCodeOrName)
            {
                dt.Columns.Add(kv.Key, typeof(decimal));//质控Code(kv.Key) 拼接成为列
            }

            return dt;
        }

        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="dtOriginal">数据源(总的任务数据)</param>
        ///<param name="drFinishRateOriginal">数据源（完成的任务数据）</param>
        /// <param name="dtNew">要填充的表</param>
        /// <param name="formCodeList">按质控Code生成表列的数组</param>
        /// <param name="yearList">日期数据</param>
        /// <returns></returns>
        private DataTable AddNewRowToDataTable(DataTable dtOriginal, DataTable dtFinishRateOriginal, DataTable dtNew, Dictionary<string, string> dicFormCodeOrName, string[] yearList)
        {

            //分组后的时间个数 去查找数据
            for (int i = 0; i < yearList.Length; i++)
            {
                int taskTotal = 0;//总的任务数量
                int finishRateTaskTotal = 0;//完成的任务数量

                DataRow drNew = dtNew.NewRow();//新行
                drNew["EndDateActual"] = yearList[i].ToString();

                DataRow[] drOriginal = dtOriginal.Select(string.Format("EndDateActual='{0}'", yearList[i].ToString()));//总的任务数据
                DataRow[] drFinishRateOriginal = null;
                if (dtFinishRateOriginal.Rows.Count > 0)
                {
                    drFinishRateOriginal = dtFinishRateOriginal.Select(string.Format("EndDateActual like '{0}%'", yearList[i].ToString()));//任务完成的数据
                }
                //dtNew  是按 质控Code生成的列  此处也要用到
                foreach (KeyValuePair<string, string> kv in dicFormCodeOrName)
                {
                    //总的数据填充
                    if (drOriginal.Length > 0)
                    {
                        if (dtNew.Columns.Contains(kv.Key))
                        {
                            //从drOriginal检索相对应的质控Code的数量
                            DataRow[] drOriginal1 = drOriginal.CopyToDataTable().Select(string.Format("FormCode='{0}'", kv.Key));
                            if (drOriginal1.Length > 0)
                            {
                                drNew[kv.Key] = drOriginal1.Count();//数量
                                taskTotal += drOriginal1.Count();//计算改行的总数
                            }
                        }
                    }
                    //任务完成数据填充
                    if (drFinishRateOriginal != null && drFinishRateOriginal.Count() > 0)
                    {
                        if (dtNew.Columns.Contains(kv.Key))
                        {
                            //从drFinishRateOriginal检索相对应的质控Code的数量
                            DataRow[] drFinishRateOriginal1 = drFinishRateOriginal.CopyToDataTable().Select(string.Format("FormCode='{0}'", kv.Key));
                            if (drFinishRateOriginal1.Length > 0)
                            {
                                finishRateTaskTotal += drFinishRateOriginal1.Count();//计算改行的总数
                            }
                        }
                    }
                    else
                    {
                        finishRateTaskTotal = 0;
                    }
                }
                drNew["TaskTotal"] = taskTotal;
                drNew["FinishRateTaskTotal"] = finishRateTaskTotal;
                if (taskTotal == 0)//总的任务为0 完成率也为0
                {
                    drNew["FinishRate"] = 0;
                }
                else
                {
                    decimal finishRate = (decimal)finishRateTaskTotal / taskTotal * 100;
                    drNew["FinishRate"] = finishRate;
                }

                //添加行
                dtNew.Rows.Add(drNew);
            }

            return dtNew;
        }
        /// <summary>
        /// 获取任务状态
        /// </summary>
        /// <returns></returns>
        public DataTable GetTaskStatus()
        {
            object objData = WebServiceHelper.InvokeWebService(m_OperationTaskWebServiceUrl, "GetTaskWebService", "GetTaskStatus", new object[] { });
            return objData as DataTable;
        }
    }
}
