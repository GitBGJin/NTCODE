using SmartEP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SmartEP.MonitoringBusinessRepository.WaterLZ;
using SmartEP.Utilities.DataTypes.ExtensionMethods;

namespace SmartEP.Service.DataAnalyze.WaterLZ
{
    public class LZService
    {
        LZRepository rep=new LZRepository();
        public DataTable GetTopFiveAlgalDensity(DateTime STime, DateTime ETime,string[] pointIds)
        {
            DataTable dt = new DataTable();
            dt= rep.GetTopFiveAlgalDensity(STime, ETime, pointIds);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 2; j < dt.Columns.Count; j++)
                {
                    if (dt.Rows[i][j].IsNotNullOrDBNull() && Convert.ToDecimal(dt.Rows[i][j].ToString()) < 30)
                    {
                        dt.Rows[i][j] = 30;
                    }
                }
            }
            return dt;

        }
        public DataTable GetBlueAlgalSort(DateTime STime, DateTime ETime, string[] pointIds,string[] factors)
        {
            DataTable dt = new DataTable();

            dt = rep.GetBlueAlgalSort(STime, ETime, pointIds,factors);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                foreach (string factor in factors)
                {
                    if (factor == "w19011")
                    {
                        if (dt.Rows[i][factor].IsNotNullOrDBNull() && (dt.Rows[i][factor].ToString() == "7999" || dt.Rows[i][factor].ToString() == "0" || Convert.ToDecimal(dt.Rows[i][factor].ToString()) < 30))
                        {
                            dt.Rows[i][factor] = 30;
                        }
                    }
                    else
                    {
                        if (dt.Rows[i][factor].IsNotNullOrDBNull() && (dt.Rows[i][factor].ToString() == "7999" || dt.Rows[i][factor].ToString() == "0" || Convert.ToDecimal(dt.Rows[i][factor].ToString()) < 0))
                        {
                            dt.Rows[i][factor] = DBNull.Value;
                        }
                    }
                    if (dt.Rows[i][factor] != DBNull.Value)
                    {
                        if (factor == "w19011")//藻密度
                        {
                            dt.Rows[i][factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i][factor]), 0);
                        }
                        else if (factor == "w01010" || factor == "w01003" || factor == "w01014")//水温&浊度&电导率
                        {
                            dt.Rows[i][factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i][factor]), 1);
                        }
                        else if (factor == "w01009" || factor == "w01001")//溶解氧&PH值
                        {
                            dt.Rows[i][factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i][factor]), 2);
                        }
                        else if (factor == "w01016")//叶绿素a
                        {
                            dt.Rows[i][factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i][factor]), 4);
                        }
                    }
                }
                //for (int j = 2; j < dt.Columns.Count; j++)
                //{
                //    dt.Rows[i][j] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i][j]), 0);

                //    if (dt.Rows[i][j].IsNotNullOrDBNull() && Convert.ToDecimal(dt.Rows[i][j].ToString()) < 30)
                //    {
                //        dt.Rows[i][j] = 30;
                //    }
                //}
            }
            return dt;
        }

    }
}
