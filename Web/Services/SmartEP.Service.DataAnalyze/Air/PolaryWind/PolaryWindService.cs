﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Utilities.DataTypes.ExtensionMethods;

namespace SmartEP.Service.DataAnalyze.Air.PolaryWind
{
    /// <summary>
    /// 名称：PolaryWindService.aspx.cs
    /// 创建人：
    /// 创建日期：
    /// 维护人员：
    /// 最新维护人员：徐阳
    /// 最新维护日期：2017-06-12
    /// 功能摘要：
    /// 玫瑰图数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class PolaryData
    {
        public string FactorCoce { get; set; }
        public string FactorName { get; set; }
        public string FX { get; set; }
        public Decimal? ND { get; set; }
        public Decimal? FS { get; set; }
        public Decimal? FactorCount { get; set; }
    }

    public class PolaryWindService
    {
        /// <summary>
        /// 获取风向数据
        /// </summary>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        public ObservableCollection<PolaryData> GetPolaryDirectionData(ObservableCollection<decimal?> B, string factor)
        {
            ObservableCollection<string> deg = new ObservableCollection<string>();
            foreach (Decimal? i in B)
            {
                if (factor == "Sixteen")
                    deg.Add(SixteenGetWindName(Convert.ToDouble(i)));
                else
                    deg.Add(EightGetWindName(Convert.ToDouble(i)));
            }
            ObservableCollection<int> count = new ObservableCollection<int>();
            #region 计算风向次数
            if (factor == "Sixteen")
            {
                string[] windName = { "北风", "东北偏北风", "东北风", "东北偏东风", "东风", "东南偏东风", "东南风", "东南偏南风", "南风", "西南偏南风", "西南风", "西南偏西风", "西风", "西北偏西风", "西北风", "西北偏北风" };
                for (int i = 0; i < windName.Length; i++)
                {
                    int wcount = 0;
                    for (int j = 0; j < deg.Count; j++)
                    {
                        if (windName[i] == deg[j])
                        {
                            wcount++;
                        }
                    }
                    count.Add(wcount);
                }
            }
            else
            {
                string[] windName = { "北风", "东北风", "东风", "东南风", "南风", "西南风", "西风", "西北风" };
                for (int i = 0; i < windName.Length; i++)
                {
                    int wcount = 0;
                    for (int j = 0; j < deg.Count; j++)
                    {
                        if (windName[i] == deg[j])
                        {
                            wcount++;
                        }
                    }
                    count.Add(wcount);
                }
            }
            #endregion
            #region 排除多余
            //排除0
            for (int i = 0; i < count.Count; i++)
            {
                for (int j = count.Count - 1; j >= i; j--)
                {
                    if (count[j] == 0 && count[i] == count[j])
                    {
                        count.RemoveAt(j);
                    }
                }
            }
            //排除重复风向
            for (int i = 0; i < deg.Count; i++)
            {
                for (int j = deg.Count - 1; j > i; j--)
                {

                    if (deg[i] == deg[j])
                    {
                        deg.RemoveAt(j);
                    }
                }
            }
            //风向按顺序排列
            string[] WName = { "北风", "东北偏北风", "东北风", "东北偏东风", "东风", "东南偏东风", "东南风", "东南偏南风", "南风", "西南偏南风", "西南风", "西南偏西风", "西风", "西北偏西风", "西北风", "西北偏北风" };
            List<string> newlist = new List<string>();

            for (int i = 0; i < WName.Length; i++)
            {
                for (int j = 0; j < deg.Count; j++)
                {
                    if (deg[j] == WName[i])
                    {
                        newlist.Add(deg[j]);
                    }
                }
            }
            #endregion
            ObservableCollection<PolaryData> AllData = new ObservableCollection<PolaryData>();
            PolaryData da = null;
            for (int i = 0; i < deg.Count; i++)
            {
                da = new PolaryData();
                da.FX = newlist[i];
                da.FactorCount = count[i];
                AllData.Add(da);
            }
            return AllData;
        }

        #region 注释
        //public ObservableCollection<PolaryData> GetPolaryDirectionData(ObservableCollection<decimal?> B, string factor)
        //{
        //    ObservableCollection<string> deg = new ObservableCollection<string>();
        //    foreach (Decimal? i in B)
        //    {
        //        if (factor == "Sixteen")
        //            deg.Add(SixteenGetWindName(Convert.ToDouble(i)));
        //        else
        //            deg.Add(EightGetWindName(Convert.ToDouble(i)));
        //    }
        //    ObservableCollection<int> count = new ObservableCollection<int>();
        //    #region 计算风向次数
        //    if (factor == "Sixteen")
        //    {
        //        string[] windName = { "北风", "东北偏北风", "东北风", "东北偏东风", "东风", "东南偏东风", "东南风", "东南偏南风", "南风", "西南偏南", "西南风", "西南偏西", "西风", "西北偏西风", "西北风", "西北偏北风" };
        //        for (int i = 0; i < windName.Length; i++)
        //        {
        //            int wcount = 0;
        //            for (int j = 0; j < deg.Count; j++)
        //            {
        //                if (windName[i] == deg[j])
        //                {
        //                    wcount++;
        //                }
        //            }
        //            count.Add(wcount);
        //        }
        //    }
        //    else
        //    {
        //        string[] windName = { "北风", "东北风", "东风", "东南风", "南风", "西南风", "西风", "西北风" };
        //        for (int i = 0; i < windName.Length; i++)
        //        {
        //            int wcount = 0;
        //            for (int j = 0; j < deg.Count; j++)
        //            {
        //                if (windName[i] == deg[j])
        //                {
        //                    wcount++;
        //                }
        //            }
        //            count.Add(wcount);
        //        }
        //    }
        //    #endregion
        //    #region 排除多余
        //    //排除0
        //    for (int i = 0; i < count.Count; i++)
        //    {
        //        for (int j = count.Count - 1; j >= i; j--)
        //        {
        //            if (count[j] == 0 && count[i] == count[j])
        //            {
        //                count.RemoveAt(j);
        //            }
        //        }
        //    }
        //    //排除重复风向
        //    for (int i = 0; i < deg.Count; i++)
        //    {
        //        for (int j = deg.Count - 1; j > i; j--)
        //        {

        //            if (deg[i] == deg[j])
        //            {
        //                deg.RemoveAt(j);
        //            }
        //        }
        //    }
        //    //风向按顺序排列
        //    string[] WName = { "北风", "东北偏北风", "东北风", "东北偏东风", "东风", "东南偏东风", "东南风", "东南偏南风", "南风", "西南偏南", "西南风", "西南偏西", "西风", "西北偏西风", "西北风", "西北偏北风" };
        //    List<string> newlist = new List<string>();

        //    for (int i = 0; i < WName.Length; i++)
        //    {
        //        for (int j = 0; j < deg.Count; j++)
        //        {
        //            if (deg[j] == WName[i])
        //            {
        //                newlist.Add(deg[j]);
        //            }
        //        }
        //    }
        //    #endregion
        //    ObservableCollection<PolaryData> AllData = new ObservableCollection<PolaryData>();
        //    PolaryData da = null;
        //    for (int i = 0; i < deg.Count; i++)
        //    {
        //        da = new PolaryData();
        //        da.FX = newlist[i];
        //        da.ND = count[i];
        //        AllData.Add(da);
        //    }
        //    return AllData;
        //}
        #endregion

        /// <summary>
        /// 获取风速数据
        /// </summary>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        //public ObservableCollection<PolaryData> GetPolarySpeedData(ObservableCollection<decimal?> B, ObservableCollection<decimal?> C, string factor)
        //{
        //    ObservableCollection<string> deg = new ObservableCollection<string>();
        //    foreach (Decimal? i in B)
        //    {
        //        if (factor == "Sixteen")
        //            deg.Add(SixteenGetWindName(Convert.ToDouble(i)));
        //        else
        //            deg.Add(EightGetWindName(Convert.ToDouble(i)));
        //    }
        //    ObservableCollection<PolaryData> AllData = new ObservableCollection<PolaryData>();
        //    PolaryData da = null;
        //    for (int i = 0; i < deg.Count; i++)
        //    {
        //        da = new PolaryData();
        //        da.FX = deg[i];
        //        da.ND = C[i];
        //        AllData.Add(da);
        //    }
        //    var WindName = (from a in AllData select a.FX).Distinct();
        //    ObservableCollection<string> CoWindName = new ObservableCollection<string>(WindName.ToList());
        //    ObservableCollection<decimal?> AvgSpeed = new ObservableCollection<decimal?>();
        //    foreach (string i in CoWindName)
        //    {
        //        var Speed = (from b in AllData where b.FX == i select b.ND).Average();
        //        AvgSpeed.Add(Speed);
        //    }
        //    ObservableCollection<PolaryData> GetAllData = new ObservableCollection<PolaryData>();
        //    PolaryData Ob = null;
        //    for (int i = 0; i < CoWindName.Count; i++)
        //    {
        //        Ob = new PolaryData();
        //        Ob.FX = CoWindName[i];
        //        Ob.ND = Convert.ToDecimal(Convert.ToDecimal(AvgSpeed[i]).ToString("0.00"));
        //        GetAllData.Add(Ob);
        //    }
        //    return GetAllData;
        //}

        /// <summary>
        /// 污染物分布数据
        /// </summary>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        //public ObservableCollection<PolaryData> GetPolaryFactorData(ObservableCollection<decimal?> B, ObservableCollection<decimal?> C, string factor)
        //{
        //    //风向转换
        //    ObservableCollection<string> deg = new ObservableCollection<string>();
        //    foreach (Decimal? i in B)
        //    {
        //        if (factor == "Sixteen")
        //            deg.Add(SixteenGetWindName(Convert.ToDouble(i)));
        //        else
        //            deg.Add(EightGetWindName(Convert.ToDouble(i)));
        //    }
        //    //需要的数据绑定到新的集合
        //    ObservableCollection<PolaryData> AllData = new ObservableCollection<PolaryData>();
        //    PolaryData da = null;
        //    for (int i = 0; i < deg.Count; i++)
        //    {
        //        da = new PolaryData();
        //        da.FX = deg[i];
        //        da.ND = C[i];
        //        AllData.Add(da);
        //    }
        //    var WindName = (from a in AllData select a.FX).Distinct();
        //    ObservableCollection<string> CoWindName = new ObservableCollection<string>(WindName.ToList());
        //    ObservableCollection<decimal?> AvgND = new ObservableCollection<decimal?>();
        //    foreach (string i in CoWindName)
        //    {
        //        var Speed = (from b in AllData where b.FX == i select b.ND).Sum();
        //        var c = (from d in AllData where d.FX == i select d.ND).Count();
        //        decimal? s = Speed / c * 1000;
        //        AvgND.Add(s);
        //    }
        //    ObservableCollection<PolaryData> GetAllData = new ObservableCollection<PolaryData>();
        //    PolaryData Ob = null;
        //    for (int i = 0; i < CoWindName.Count; i++)
        //    {
        //        Ob = new PolaryData();
        //        Ob.FX = CoWindName[i];
        //        Ob.ND = AvgND[i];
        //        GetAllData.Add(Ob);
        //    }
        //    return GetAllData;
        //}

        /// <summary>
        /// 玫瑰图绘图数据整合
        /// </summary>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        //public ObservableCollection<PolaryData> GetPolaryAllData(ObservableCollection<decimal?> B, ObservableCollection<decimal?> S, ObservableCollection<decimal?> C, string windtype)
        //{
        //    ObservableCollection<string> deg = new ObservableCollection<string>();
        //    foreach (Decimal? i in B)
        //    {
        //        if (windtype == "Sixteen")
        //        {
        //            if (i == null)
        //            {
        //                deg.Add(null);
        //            }
        //            else
        //            {
        //                deg.Add(SixteenGetWindName(Convert.ToDouble(i)));
        //            }
        //        }
        //        else
        //        {
        //            if (i == null)
        //            {
        //                deg.Add(null);
        //            }
        //            else
        //            {
        //                deg.Add(EightGetWindName(Convert.ToDouble(i)));
        //            }
        //        }
        //    }
        //    ObservableCollection<int> count = new ObservableCollection<int>();
        //    ObservableCollection<decimal?> totalFac = new ObservableCollection<decimal?>();
        //    ObservableCollection<decimal?> totalSpeed = new ObservableCollection<decimal?>();
        //    string[] windName;
        //    if (windtype == "Sixteen")
        //    {
        //        windName = new string[] { "北风", "东北偏北风", "东北风", "东北偏东风", "东风", "东南偏东风", "东南风", "东南偏南风", "南风", "西南偏南风", "西南风", "西南偏西风", "西风", "西北偏西风", "西北风", "西北偏北风" };
        //        for (int i = 0; i < windName.Length; i++)
        //        {
        //            int wcount = 0;
        //            decimal? facCount = 0;
        //            decimal? speedCount = 0;
        //            for (int j = 0; j < deg.Count; j++)
        //            {
        //                if (windName[i] == deg[j])
        //                {
        //                    wcount++;
        //                    facCount += C[j];
        //                    speedCount += S[j];
        //                }
        //            }
        //            count.Add(wcount);
        //            totalFac.Add(facCount);
        //            totalSpeed.Add(speedCount);
        //        }
        //    }
        //    else
        //    {
        //        windName = new string[] { "北风", "东北风", "东风", "东南风", "南风", "西南风", "西风", "西北风" };
        //        for (int i = 0; i < windName.Length; i++)
        //        {
        //            int wcount = 0;
        //            decimal? facCount = 0;
        //            decimal? speedCount = 0;
        //            for (int j = 0; j < deg.Count; j++)
        //            {
        //                if (windName[i] == deg[j])
        //                {
        //                    wcount++;
        //                    facCount += C[j];
        //                    speedCount += S[j];
        //                }
        //            }
        //            count.Add(wcount);
        //            totalFac.Add(facCount);
        //            totalSpeed.Add(speedCount);
        //        }
        //    }

        //    ObservableCollection<PolaryData> AllData = new ObservableCollection<PolaryData>();
        //    PolaryData da = null;
        //    for (int i = 0; i < windName.ToList().Count; i++)
        //    {
        //        da = new PolaryData();
        //        da.FX = windName.ToList()[i];
        //        da.FactorCount = count[i];
        //        da.ND = 0;
        //        da.FS = 0;
        //        if (count[i] != 0)
        //        {
        //            da.FS = Convert.ToDecimal((Convert.ToDecimal(totalSpeed[i] / count[i])).ToString("0.00"));
        //            da.ND = totalFac[i] / count[i] * 1000;
        //        }
        //        else
        //        {
        //            da.FS = 0;
        //            da.ND = 0;
        //        }
        //        AllData.Add(da);
        //    }
        //    return AllData;


        //}

        /// <summary>
        /// 玫瑰图绘图数据整合(可选多站点)
        /// </summary>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        //public ObservableCollection<PolaryData> GetPolaryAllData(DataView dv, int pointcount, DataView dvWind, string factor, string pointId, string windtype)
        //{
        //    string factorCodeWindDir = "a01008";//风向
        //    string factorCodeWindSpeed = "a01007";//风速
        //    AirPollutantService airPollutantService = new AirPollutantService();
        //    int? n = airPollutantService.RetrieveEntityByCode("a01007").DecimalDigit;
        //    int pid = Convert.ToInt32(pointId);

        //    if (dvWind.Count > 0)
        //    {
        //        var listDir = from i in dvWind.ToTable().AsEnumerable() where i.Field<int>("PointId") == pid select i.Field<decimal?>(factorCodeWindDir);
        //        ObservableCollection<decimal?> B = new ObservableCollection<decimal?>(listDir);
        //        var listSpeed = from i in dvWind.ToTable().AsEnumerable() where i.Field<int>("PointId") == pid select i.Field<decimal?>(factorCodeWindSpeed);
        //        ObservableCollection<decimal?> S = new ObservableCollection<decimal?>(listSpeed);
        //        var list = from i in dv.ToTable().AsEnumerable() select i.Field<decimal?>(factor);
        //        ObservableCollection<decimal?> C = new ObservableCollection<decimal?>(list);

        //        List<decimal?> listAvg = new List<decimal?>();          //计算多个点位同一个因子的平均值
        //        for (int i = 0; i < listSpeed.Count(); i++)
        //        {
        //            decimal? x = 0;
        //            for (int j = 0; j < pointcount; j++)
        //            {
        //                x += C[(listSpeed.Count() * j + i)] == null ? 0 : C[(listSpeed.Count() * j + i)];
        //            }
        //            listAvg.Add(x);
        //        }
        //        ObservableCollection<decimal?> D = new ObservableCollection<decimal?>(listAvg);

        //        ObservableCollection<string> deg = new ObservableCollection<string>();
        //        foreach (Decimal? i in B)
        //        {
        //            if (windtype == "Sixteen")
        //            {
        //                if (i == null)
        //                {
        //                    deg.Add(null);
        //                }
        //                else
        //                {
        //                    deg.Add(SixteenGetWindName(Convert.ToDouble(i)));
        //                }
        //            }
        //            else
        //            {
        //                if (i == null)
        //                {
        //                    deg.Add(null);
        //                }
        //                else
        //                {
        //                    deg.Add(EightGetWindName(Convert.ToDouble(i)));
        //                }
        //            }
        //        }
        //        ObservableCollection<int> count = new ObservableCollection<int>();
        //        ObservableCollection<decimal?> totalFac = new ObservableCollection<decimal?>();
        //        ObservableCollection<decimal?> totalSpeed = new ObservableCollection<decimal?>();
        //        string[] windName;
        //        if (windtype == "Sixteen")
        //        {
        //            windName = new string[] { "北风", "东北偏北风", "东北风", "东北偏东风", "东风", "东南偏东风", "东南风", "东南偏南风", "南风", "西南偏南风", "西南风", "西南偏西风", "西风", "西北偏西风", "西北风", "西北偏北风" };
        //            for (int i = 0; i < windName.Length; i++)
        //            {
        //                int wcount = 0;
        //                decimal? facCount = 0;
        //                decimal? speedCount = 0;
        //                for (int j = 0; j < deg.Count; j++)
        //                {
        //                    if (windName[i] == deg[j])
        //                    {
        //                        wcount++;
        //                        facCount += D[j];
        //                        speedCount += S[j];
        //                    }
        //                }
        //                count.Add(wcount);
        //                totalFac.Add(facCount);
        //                totalSpeed.Add(speedCount);
        //            }
        //        }
        //        else
        //        {
        //            windName = new string[] { "北风", "东北风", "东风", "东南风", "南风", "西南风", "西风", "西北风" };
        //            for (int i = 0; i < windName.Length; i++)
        //            {
        //                int wcount = 0;
        //                decimal? facCount = 0;
        //                decimal? speedCount = 0;
        //                for (int j = 0; j < deg.Count; j++)
        //                {
        //                    if (windName[i] == deg[j])
        //                    {
        //                        wcount++;
        //                        facCount += D[j];
        //                        speedCount += S[j];
        //                    }
        //                }
        //                count.Add(wcount);
        //                totalFac.Add(facCount);
        //                totalSpeed.Add(speedCount);
        //            }
        //        }

        //        ObservableCollection<PolaryData> AllData = new ObservableCollection<PolaryData>();
        //        PolaryData da = null;
        //        for (int i = 0; i < windName.ToList().Count; i++)
        //        {
        //            da = new PolaryData();
        //            da.FX = windName.ToList()[i];
        //            da.FactorCount = count[i];
        //            da.ND = null;
        //            da.FS = null;
        //            if (count[i] != 0)
        //            {
        //                da.FS = DecimalExtension.GetPollutantValue(Convert.ToDecimal(totalSpeed[i] / count[i]), Convert.ToInt32(n));
        //                if (factor.Equals("a21005"))
        //                {
        //                    da.ND = DecimalExtension.GetPollutantValue(Convert.ToDecimal(totalFac[i] / count[i]), 1);
        //                }
        //                else
        //                {
        //                    da.ND = Convert.ToDecimal((DecimalExtension.GetPollutantValue(Convert.ToDecimal(totalFac[i] / count[i]), 3) * 1000).ToString("G0"));
        //                } 
        //            }
        //            else
        //            {
        //                da.FS = 0;
        //                da.ND = 0;
        //            }
        //            AllData.Add(da);
        //        }
        //        return AllData;
        //    }
        //    else
        //    {
        //        var list = from i in dv.ToTable().AsEnumerable() select i.Field<decimal?>(factor);

        //        ObservableCollection<PolaryData> AllData = new ObservableCollection<PolaryData>();
        //        PolaryData da = null;
        //        for (int i = 0; i < (list.Count() / pointcount); i++)
        //        {
        //            da = new PolaryData();
        //            da.FX = null;
        //            da.FactorCount = null;
        //            da.ND = 0;
        //            da.FS = 0;
        //            AllData.Add(da);
        //        }
        //        return AllData;
        //    }
        //}

        public ObservableCollection<PolaryData> GetPolaryAllData(DataView dv, int pointcount, DataView dvWind, string factor, string pointId, string windtype, int dayOrHour)
        {
            string factorCodeWindDir = "a01008";//风向
            string factorCodeWindSpeed = "a01007";//风速
            AirPollutantService airPollutantService = new AirPollutantService();
            int? n = airPollutantService.RetrieveEntityByCode("a01007").DecimalDigit;
            int pid = Convert.ToInt32(pointId);

            if (dvWind.Count > 0 && dv.Count > 0)
            {
                try
                {
                    dvWind.Table.Columns.Add("windName", typeof(string));
                    dvWind.Table.Columns.Add(factor, typeof(decimal));
                    string dtformat = string.Empty;
                    foreach (DataRowView drv in dvWind)
                    {
                        if (windtype == "Sixteen")
                        {
                            if (drv["a01008"] != DBNull.Value && drv["a01008"].ToString() != "")
                            {
                                drv["windName"] = SixteenGetWindName(Convert.ToDouble(drv["a01008"]));
                            }
                        }
                        else
                        {
                            if (drv["a01008"] != DBNull.Value && drv["a01008"].ToString() != "")
                            {
                                drv["windName"] = EightGetWindName(Convert.ToDouble(drv["a01008"]));
                            }
                        }
                        if (dayOrHour == 0)
                        {
                            dv.RowFilter = factor + " is not null and Tstamp = '" + drv["Tstamp"] + "'";
                            object avg = DBNull.Value;
                            if (dv.Count > 0)
                            {
                                //object a = dv.Table.Columns[factor].DataType;
                                //avg = Convert.ToDecimal(dv.Table.Compute("avg(" + factor + ")", factor + " is not null and Tstamp = '" + drv["Tstamp"] + "'"));
                                decimal avgg = 0;
                                foreach (DataRowView drvv in dv)
                                {
                                    avgg += Convert.ToDecimal(drvv[factor].ToString());
                                }
                                drv[factor] = avgg / dv.Count;
                                dv.RowFilter = "";
                            }
                            else
                            {
                                drv[factor] = avg;
                                dv.RowFilter = "";
                            }
                        }
                        if (dayOrHour == 1)
                        {
                            //dv.RowFilter = factor + " is not null and DateTime = '" + drv["DateTime"] + "'";
                            object avg = DBNull.Value;
                            if (dv.Count > 0)
                            {
                                avg = Convert.ToDecimal(dv.Table.Compute("avg(" + factor + ")", factor + " is not null and DateTime = '" + drv["DateTime"] + "'"));
                            }
                            drv[factor] = avg;
                            dv.RowFilter = "";
                        }
                    }
                }
                catch
                {
                }
                List<int> count = new List<int>();
                List<int> countND = new List<int>();
                List<decimal?> totalFac = new List<decimal?>();
                List<decimal?> totalSpeed = new List<decimal?>();
                string[] windName;
                if (windtype == "Sixteen")
                {
                    windName = new string[] { "北风", "东北偏北风", "东北风", "东北偏东风", "东风", "东南偏东风", "东南风", "东南偏南风", "南风", "西南偏南风", "西南风", "西南偏西风", "西风", "西北偏西风", "西北风", "西北偏北风" };
                    for (int i = 0; i < windName.Length; i++)
                    {
                        int wcount = 0;
                        int NDcount = 0;
                        decimal? facCount = 0;
                        decimal? speedCount = 0;
                        foreach (DataRowView drv in dvWind)
                        {
                            if (drv["windName"].ToString().Equals(windName[i]))
                            {
                                wcount++;
                                NDcount += drv[factor] != DBNull.Value ? 1 : 0;
                                facCount += drv[factor] != DBNull.Value ? Convert.ToDecimal(drv[factor]) : 0;
                                speedCount += drv["a01007"] != DBNull.Value ? Convert.ToDecimal(drv["a01007"]) : 0;
                            }
                        }
                        count.Add(wcount);
                        countND.Add(NDcount);
                        totalFac.Add(facCount);
                        totalSpeed.Add(speedCount);
                    }
                }
                else
                {
                    windName = new string[] { "北风", "东北风", "东风", "东南风", "南风", "西南风", "西风", "西北风" };
                    for (int i = 0; i < windName.Length; i++)
                    {
                        int wcount = 0;
                        int NDcount = 0;
                        decimal? facCount = 0;
                        decimal? speedCount = 0;
                        foreach (DataRowView drv in dvWind)
                        {
                            if (drv["windName"].ToString().Equals(windName[i]))
                            {
                                wcount++;
                                NDcount += drv[factor] != DBNull.Value ? 1 : 0;
                                facCount += drv[factor] != DBNull.Value ? Convert.ToDecimal(drv[factor]) : 0;
                                speedCount += drv["a01007"] != DBNull.Value ? Convert.ToDecimal(drv["a01007"]) : 0;
                            }
                        }
                        count.Add(wcount);
                        countND.Add(NDcount);
                        totalFac.Add(facCount);
                        totalSpeed.Add(speedCount);
                    }
                }
                dvWind.Table.Columns.Remove("windName");
                ObservableCollection<PolaryData> AllData = new ObservableCollection<PolaryData>();
                PolaryData da = null;
                for (int i = 0; i < windName.ToList().Count; i++)
                {
                    da = new PolaryData();
                    da.FX = windName.ToList()[i];
                    da.FactorCount = count[i];
                    da.ND = null;
                    da.FS = null;
                    if (count[i] != 0)
                    {
                        da.FS = DecimalExtension.GetPollutantValue(Convert.ToDecimal(totalSpeed[i] / count[i]), Convert.ToInt32(n));
                        if (countND[i] != 0)
                        {
                            if (factor.Equals("a21005"))
                            {
                                da.ND = DecimalExtension.GetPollutantValue(Convert.ToDecimal(totalFac[i] / countND[i]), 1);
                            }
                            else
                            {
                                da.ND = Convert.ToDecimal((DecimalExtension.GetPollutantValue(Convert.ToDecimal(totalFac[i] / countND[i]), 3) * 1000).ToString("G0"));
                            }
                        }
                    }
                    else
                    {
                        da.FS = 0;
                        da.ND = 0;
                    }
                    AllData.Add(da);
                }
                return AllData;
            }
            else
            {
                var list = from i in dv.ToTable().AsEnumerable() select i.Field<decimal?>(factor);

                ObservableCollection<PolaryData> AllData = new ObservableCollection<PolaryData>();
                PolaryData da = null;
                for (int i = 0; i < (list.Count() / pointcount); i++)
                {
                    da = new PolaryData();
                    da.FX = null;
                    da.FactorCount = null;
                    da.ND = 0;
                    da.FS = 0;
                    AllData.Add(da);
                }
                return AllData;
            }
        }

        #region 通用
        #region 16角度转换
        public string SixteenGetWindName(double degree)
        {
            string[] windName = {   "北风", 
                                    "东北偏北风", 
                                    "东北风", 
                                    "东北偏东风", 
                                    "东风",
                                    "东南偏东风",
                                    "东南风", 
                                    "东南偏南风", 
                                    "南风",
                                    "西南偏南风",
                                    "西南风",
                                    "西南偏西风", 
                                    "西风", 
                                    "西北偏西风", 
                                    "西北风", 
                                    "西北偏北风"
                                };
            double i = Math.Floor(degree % 360 / 11.25);
            if (i == 31) i = -1;
            i++;
            return windName[(int)Math.Floor(i / 2)];
        }
        #endregion

        #region 8角度转换
        public string EightGetWindName(double degree)
        {
            string[] windName = {   "北风", 
                                    "东北风", 
                                    "东风",
                                    "东南风", 
                                    "南风",
                                    "西南风",
                                    "西风", 
                                    "西北风", 
                                };
            double i = Math.Floor(degree % 360 / 22.5);
            if (i == 15) i = -1;
            i++;
            return windName[(int)Math.Floor(i / 2)];
        }
        #endregion

        #endregion
    }
}
