using SmartEP.BaseInfoRepository.Dictionary;
using SmartEP.DomainModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.OperatingMaintenance.Water
{
    public class ReagentConfigRecordService
    {
        ReagentConfigRecordRepository r = new ReagentConfigRecordRepository();

        /// <summary>
        /// 返回查询数据
        /// </summary>
        /// <param name="staDate">配置开始时间</param>
        /// <param name="endDate">配置结束时间</param>
        /// <param name="points">站点</param>
        /// <param name="factors">因子</param>
        /// <returns></returns>
        public DataTable GetData(DateTime staDate, DateTime endDate, string points, string factors)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ManufactureDate", typeof(DateTime));
                dt.Columns.Add("CheckFactor", typeof(string));
                dt.Columns.Add("ReagentName", typeof(string));
                dt.Columns.Add("ConfigAmount", typeof(string));
                dt.Columns.Add("pointName", typeof(string));
                dt.Columns.Add("ConfigPeople", typeof(string));

                DataTable org = r.GetData(staDate, endDate, points, factors);

                if (org.Rows.Count > 0)
                {
                    //获取日期
                    DataTable days = org.DefaultView.ToTable(true, "ManufactureDate");

                    //获取试剂名称
                    DataTable reagents = org.DefaultView.ToTable(true, "ReagentName");

                    //获取试剂名称
                    DataTable f = org.DefaultView.ToTable(true, "CheckFactor");

                    for (int i = 0; i < days.Rows.Count; i++)
                    {
                        for (int y = 0; y < f.Rows.Count; y++)
                        {
                            for (int j = 0; j < reagents.Rows.Count; j++)
                            {
                                DateTime ManufactureDate = Convert.ToDateTime(days.Rows[i][0]);
                                string ReagentName = reagents.Rows[j][0].ToString();
                                string checkFactor = f.Rows[y][0].ToString();
                                DataRow[] dr_org = org.Select("ManufactureDate='" + ManufactureDate + "' and ReagentName='" + ReagentName + "' and CheckFactor='" + checkFactor + "'");
                                //string CheckFactor = dr_org[0]["CheckFactor"].ToString();
                                if (dr_org.Length > 0)
                                {
                                    string CheckFactor = checkFactor;
                                    string ConfigAmount = dr_org[0]["ConfigAmount"].ToString();
                                    string ConfigPeople = dr_org[0]["ConfigPeople"].ToString();
                                    string pointName = "";
                                    for (int k = 0; k < dr_org.Length; k++)
                                    {
                                        pointName = pointName + dr_org[k]["PointName"].ToString() + "(" + Convert.ToDateTime(dr_org[k]["ChangeDate"]).ToString("MM-dd") + ")\n";
                                    }

                                    DataRow dr = dt.NewRow();
                                    dr["ManufactureDate"] = ManufactureDate;
                                    dr["CheckFactor"] = CheckFactor;
                                    dr["ReagentName"] = ReagentName;
                                    dr["ConfigAmount"] = ConfigAmount;
                                    dr["pointName"] = pointName;
                                    dr["ConfigPeople"] = ConfigPeople;

                                    dt.Rows.Add(dr);
                                }
                            }
                        }
                    }
                }
                return dt;
            }
            catch
            {
                return null;
            }
        }
    }
}
