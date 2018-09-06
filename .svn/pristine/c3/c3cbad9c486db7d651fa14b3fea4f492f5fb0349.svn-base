using SmartEP.Core.Generic;
using SmartEP.MonitoringBusinessRepository.Air;
using SmartEP.Service.BaseData.Standard;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Service.DataAnalyze.Interfaces;
using SmartEP.Service.Frame;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


namespace SmartEP.Service.DataAnalyze.Air.AQIReport
{
    public class YearAQIService : IDayAQI
    {
        /// <summary>
        /// 区域日AllStandardRate
        /// </summary>
        RegionDayAQIRepository regionDayAQI = null;

        /// <summary>
        /// 区域名称
        /// </summary>
        DictionaryService g_DictionaryService = new DictionaryService();

        DataQueryByDayService m_DataQueryByDayService = Singleton<DataQueryByDayService>.GetInstance();
        /// <summary>
        /// 空气污染指数
        /// </summary>
        AQIService s_AQIService = new AQIService();

        #region 根据区域统计
        /// <summary>
        /// 根据key主键判断记录是否存在
        /// </summary>
        /// <param name="strKey">主键值</param>
        /// <returns></returns>
        public bool RIsExist(string strKey)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
                return regionDayAQI.IsExist(strKey);
            return false;
        }
        /// <summary>
        /// 年报
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        public DataView GetRegionsAllData(DateTime dateStart, DateTime dateEnd)
        {
            string[] regionguids = { "7e05b94c-bbd4-45c3-919c-42da2e63fd43"     //市区           
			                ,"66d2abd1-ca39-4e39-909f-da872704fbfd"			    //张家港市
                            ,"d7d7a1fe-493a-4b3f-8504-b1850f6d9eff"			    //常熟市
			                ,"57b196ed-5038-4ad0-a035-76faee2d7a98"				//太仓市
			                ,"2e2950cd-dbab-43b3-811d-61bd7569565a"				//昆山市	
			                ,"2fea3cb2-8b95-45e6-8a71-471562c4c89c"	            //吴江区
                                  };
            DateTime dtSta = dateStart.AddYears(-1);
            DateTime dtEn = dateEnd.AddYears(-1);
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
            {
                DataTable dt = new DataTable();   //2015
                DataTable dtNew = new DataTable();  //2014
                dt = regionDayAQI.GetRegionsAllData(regionguids, dateStart, dateEnd).Table;  //2015
                dt.Columns.Add("regionName", typeof(string)).SetOrdinal(0);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["regionName"] = g_DictionaryService.GetCodeDictionaryTextByValue(dt.Rows[i]["MonitoringRegionUid"].ToString());
                }
                dtNew = regionDayAQI.GetRegionsAllData(regionguids, dtSta, dtEn).Table;   //2014
                dtNew.Columns.Add("regionName", typeof(string)).SetOrdinal(0);
                for (int i = 0; i < dtNew.Rows.Count; i++)
                {
                    dtNew.Rows[i]["regionName"] = g_DictionaryService.GetCodeDictionaryTextByValue(dtNew.Rows[i]["MonitoringRegionUid"].ToString());
                }

                DataTable dtAQI = new DataTable();  //2015全市AQI均值
                DataTable dtNewt = new DataTable();  //2015
                DataTable dtNewn = new DataTable();  //2014
                dtAQI = regionDayAQI.GetRegionsYearAQIData(dateStart, dateEnd).Table; //2015全市AQI均值
                dtNewt = regionDayAQI.GetRegionsHalfYearData(dateStart, dateEnd).Table;  //2015 全市达标率
                dtNewn = regionDayAQI.GetRegionsYearAQIData(dtSta, dtEn).Table;   //2014  全市AQI均值

                DataTable newdt = new DataTable();  //2014
                newdt.Columns.Add("M4", typeof(string));
                newdt.Columns.Add("M5", typeof(string));
                newdt.Columns.Add("M6", typeof(string));
                newdt.Columns.Add("M7", typeof(string));
                newdt.Columns.Add("M8", typeof(string));
                newdt.Columns.Add("M9", typeof(string));
                newdt.Columns.Add("M10", typeof(string));
                newdt.Columns.Add("M11", typeof(string));
                newdt.Columns.Add("M12", typeof(string));
                newdt.Columns.Add("M13", typeof(string));
                newdt.Columns.Add("M14", typeof(string));
                newdt.Columns.Add("M15", typeof(string));
                newdt.Columns.Add("M16", typeof(string));
                newdt.Columns.Add("M17", typeof(string));
                newdt.Columns.Add("M18", typeof(string));
                newdt.Columns.Add("M19", typeof(string));
                newdt.Columns.Add("M20", typeof(string));
                newdt.Columns.Add("M21", typeof(string));
                newdt.Columns.Add("M22", typeof(string));
                newdt.Columns.Add("M23", typeof(string));

                DataRow newRow = newdt.NewRow();

                decimal AQIValue = 0;
                decimal AQIValueL = 0;
                //全市AQI均值
                if (dtAQI.Rows.Count > 0)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        string factorCode = dtAQI.Columns[j].ColumnName;
                        int num = 24;
                        if (factorCode == "a05024")
                        {
                            num = 8;
                        }
                        decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtAQI.Rows[0][j]), 4);
                        decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factorCode, Con, num), 0);
                        if (AQIValue < temp)
                        {
                            AQIValue = temp;
                        }
                    }
                }
                //全市达标率
                //DataTable test = new DataTable();
                //test.Columns.Add("value", typeof(string));

                if (dtNewt.Rows.Count > 0)
                {
                    decimal StandardCount = 0;
                    decimal ValidCount = 0;
                    for (int i = 0; i < dtNewt.Rows.Count; i++)
                    {
                        //DataRow tr = test.NewRow();
                        decimal AQIcount = 0;

                        for (int j = 1; j < 7; j++)
                        {
                            string factorCode = dtNewt.Columns[j].ColumnName;
                            int num = 24;
                            if (factorCode == "a05024")
                            {
                                num = 8;
                            }
                            decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNewt.Rows[i][j]), 4);
                            decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factorCode, Con, num), 0);
                            if (AQIcount < temp)
                            {
                                AQIcount = temp;
                            }
                        }
                        if (AQIcount > 0)
                        {
                            ValidCount++;
                            if (AQIcount <= 100)
                                StandardCount++;
                        }
                        //tr["value"] = AQIcount.ToString();
                        //test.Rows.Add(tr);
                    }

                    if (ValidCount != 0)
                    {
                        newRow["M4"] = DecimalExtension.GetRoundValue(StandardCount / ValidCount * 100, 1) + "%";
                    }
                }
                //上年全市AQI均值
                if (dtNewn.Rows.Count > 0)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        string factorCode = dtNewn.Columns[j].ColumnName;
                        int num = 24;
                        if (factorCode == "a05024")
                        {
                            num = 8;
                        }
                        decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNewn.Rows[0][j]), 4);
                        decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factorCode, Con, num), 0);
                        if (AQIValueL < temp)
                        {
                            AQIValueL = temp;
                        }
                    }
                }

                newRow["M7"] = AQIValue.ToString();
                newRow["M9"] = AQIValueL.ToString();
                if (AQIValue > AQIValueL)
                {
                    newRow["M8"] = "略有上升";
                }
                else if (AQIValue < AQIValueL)
                {
                    newRow["M8"] = "略有下降";
                }
                else if (AQIValue == AQIValueL)
                {
                    newRow["M8"] = "持平";
                }
                decimal[] AllStandardRate = { 0 };    //全市各区达标率
                string[] names = { "" };   //全市各区名称
                string[,] PM25 = { };  //各区PM2.5浓度 
                string[,] PM10 = { };  //各区PM10浓度 
                string[,] O3 = { };    //各区O3浓度 
                string[,] O3AQI = { };    //各区O3AQI
                string[,] SO2 = { };   //各区SO2浓度 
                string[,] NO2 = { };   //各区NO2浓度 
                string[,] CO = { };    //各区CO浓度 
                decimal value = 0;
                decimal value1 = 0;
                int[] days = new int[6];
                string[] factorNames = new string[6];
                if (dt.Rows.Count > 0)
                {
                    AllStandardRate = new decimal[dt.Rows.Count];   //全市各区达标率
                    names = new string[dt.Rows.Count];   //全市各区名称
                    PM25 = new string[dt.Rows.Count, 2];  //各区PM2.5浓度 
                    PM10 = new string[dt.Rows.Count, 2];  //各区PM10浓度 
                    O3 = new string[dt.Rows.Count, 2];    //各区O3浓度 
                    O3AQI = new string[dt.Rows.Count, 2];    //各区O3AQI
                    SO2 = new string[dt.Rows.Count, 2];   //各区SO2浓度 
                    NO2 = new string[dt.Rows.Count, 2];   //各区NO2浓度 
                    CO = new string[dt.Rows.Count, 2];    //各区CO浓度 
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["regionName"].ToString() == "苏州市区")
                            names[i] = "市区";
                        if (dt.Rows[i]["regionName"].ToString() == "太仓市")
                            names[i] = "太仓";
                        if (dt.Rows[i]["regionName"].ToString() == "吴江区")
                            names[i] = "吴江";
                        if (dt.Rows[i]["regionName"].ToString() == "昆山市")
                            names[i] = "昆山";
                        if (dt.Rows[i]["regionName"].ToString() == "常熟市")
                            names[i] = "常熟";
                        if (dt.Rows[i]["regionName"].ToString() == "张家港市")
                            names[i] = "张家港";
                        PM25[i, 0] = names[i];
                        PM25[i, 1] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["a34004"]) * 1000, 1).ToString();
                        PM10[i, 0] = names[i];
                        PM10[i, 1] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["a34002"]) * 1000, 0).ToString();
                        O3[i, 0] = names[i];
                        O3[i, 1] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["a05024"]) * 1000, 0).ToString();
                        SO2[i, 0] = names[i];
                        SO2[i, 1] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["a21026"]) * 1000, 0).ToString();
                        NO2[i, 0] = names[i];
                        NO2[i, 1] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["a21004"]) * 1000, 0).ToString();
                        CO[i, 0] = names[i];
                        CO[i, 1] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["a21005"]), 2).ToString();
                        if (dt.Rows[i]["O3AQICount"].IsNotNullOrDBNull() && Convert.ToDecimal(dt.Rows[i]["O3AQICount"]) != 0)
                        {
                            if (dt.Rows[i]["O3AQI"].IsNotNullOrDBNull())
                            {
                                O3AQI[i, 0] = names[i];
                                O3AQI[i, 1] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["O3AQI"]) / Convert.ToDecimal(dt.Rows[i]["O3AQICount"]) * 100, 1).ToString();
                            }
                        }
                        if (dt.Rows[i]["ValidCount"].IsNotNullOrDBNull() && dt.Rows[i]["ValidCount"].ToString() != "" && Convert.ToDecimal(dt.Rows[i]["ValidCount"]) != 0)
                        {
                            if (dt.Rows[i]["StandardCount"].IsNotNullOrDBNull() && dt.Rows[i]["StandardCount"].ToString() != "")
                            {
                                AllStandardRate[i] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["StandardCount"]) / Convert.ToDecimal(dt.Rows[i]["ValidCount"]) * 100, 1);
                                if (dt.Rows[i]["MonitoringRegionUid"].ToString() == "7e05b94c-bbd4-45c3-919c-42da2e63fd43")
                                {
                                    newRow["M10"] = AllStandardRate[i].ToString() + "%";
                                    if (Convert.ToDecimal(dt.Rows[i]["StandardCount"]) != 0)
                                    {
                                        newRow["M11"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["Optimal"]) / Convert.ToDecimal(dt.Rows[i]["ValidCount"]) * 100, 1).ToString() + "%";
                                        newRow["M12"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["benign"]) / Convert.ToDecimal(dt.Rows[i]["ValidCount"]) * 100, 1).ToString() + "%";
                                    }
                                    if (dt.Rows[i]["ExcessiveCount"].IsNotNullOrDBNull() && dt.Rows[i]["ExcessiveCount"].ToString() != "")
                                    {
                                        newRow["M13"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["ExcessiveCount"]) / Convert.ToDecimal(dt.Rows[i]["ValidCount"]) * 100, 1).ToString() + "%";
                                        decimal lightPollution = 0;
                                        decimal moderatePollution = 0;
                                        decimal highPollution = 0;
                                        decimal seriousPollution = 0;
                                        lightPollution = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["LightPollution"]) / Convert.ToDecimal(dt.Rows[i]["ValidCount"]) * 100, 1);
                                        if (lightPollution != 0)
                                        {
                                            newRow["M13"] = newRow["M13"] + ",其中轻度污染占" + lightPollution.ToString() + "%";
                                        }
                                        moderatePollution = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["ModeratePollution"]) / Convert.ToDecimal(dt.Rows[i]["ValidCount"]) * 100, 1);
                                        if (moderatePollution != 0)
                                        {
                                            newRow["M13"] = newRow["M13"] + ",中度污染占" + moderatePollution.ToString() + "%";
                                        }
                                        highPollution = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["HighPollution"]) / Convert.ToDecimal(dt.Rows[i]["ValidCount"]) * 100, 1);
                                        if (highPollution != 0)
                                        {
                                            newRow["M13"] = newRow["M13"] + ",重度污染占" + highPollution.ToString() + "%";
                                        }
                                        seriousPollution = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["SeriousPollution"]) / Convert.ToDecimal(dt.Rows[i]["ValidCount"]) * 100, 1);
                                        if (seriousPollution != 0)
                                        {
                                            newRow["M13"] = newRow["M13"] + ",严重污染占" + seriousPollution.ToString() + "%";
                                        }
                                    }
                                    for (int j = 2; j < 8; j++)
                                    {
                                        string factorCode = dt.Columns[j].ColumnName;
                                        int num = 24;
                                        if (factorCode == "a05024")
                                        {
                                            num = 8;
                                        }
                                        decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i][j]), 4);
                                        decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factorCode, Con, num), 0);
                                        if (value < temp)
                                        {
                                            value = temp;
                                        }
                                    }
                                    days[0] = Convert.ToInt32(dt.Rows[i]["PM25"]);
                                    days[1] = Convert.ToInt32(dt.Rows[i]["PM10"]);
                                    days[2] = Convert.ToInt32(dt.Rows[i]["O3"]);
                                    days[3] = Convert.ToInt32(dt.Rows[i]["SO2"]);
                                    days[4] = Convert.ToInt32(dt.Rows[i]["NO2"]);
                                    days[5] = Convert.ToInt32(dt.Rows[i]["CO"]);
                                    factorNames[0] = "细颗粒物";
                                    factorNames[1] = "可吸入颗粒物";
                                    factorNames[2] = "臭氧";
                                    factorNames[3] = "二氧化硫";
                                    factorNames[4] = "二氧化氮";
                                    factorNames[5] = "一氧化碳";
                                }
                            }
                        }
                    }
                }

                if (dtNew.Rows.Count > 0)
                {
                    string[] namesNew = new string[dtNew.Rows.Count];   //全市各区名称
                    string[,] PM25New = new string[dtNew.Rows.Count, 2];  //各区PM2.5浓度 
                    string[,] PM10New = new string[dtNew.Rows.Count, 2];  //各区PM10浓度 
                    string[,] O3New = new string[dtNew.Rows.Count, 2];    //各区O3浓度 
                    string[,] O3AQINew = new string[dtNew.Rows.Count, 2];    //各区O3AQI
                    string[,] SO2New = new string[dtNew.Rows.Count, 2];   //各区SO2浓度 
                    string[,] NO2New = new string[dtNew.Rows.Count, 2];   //各区NO2浓度 
                    string[,] CONew = new string[dtNew.Rows.Count, 2];    //各区CO浓度 
                    for (int i = 0; i < dtNew.Rows.Count; i++)
                    {
                        if (dtNew.Rows[i]["regionName"].ToString() == "苏州市区")
                            namesNew[i] = "市区";
                        if (dtNew.Rows[i]["regionName"].ToString() == "太仓市")
                            namesNew[i] = "太仓";
                        if (dtNew.Rows[i]["regionName"].ToString() == "吴江区")
                            namesNew[i] = "吴江";
                        if (dtNew.Rows[i]["regionName"].ToString() == "昆山市")
                            namesNew[i] = "昆山";
                        if (dtNew.Rows[i]["regionName"].ToString() == "常熟市")
                            namesNew[i] = "常熟";
                        if (dtNew.Rows[i]["regionName"].ToString() == "张家港市")
                            namesNew[i] = "张家港";
                        PM25New[i, 0] = namesNew[i];
                        PM25New[i, 1] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[i]["a34004"]) * 1000, 1).ToString();
                        PM10New[i, 0] = namesNew[i];
                        PM10New[i, 1] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[i]["a34002"]) * 1000, 0).ToString();
                        O3New[i, 0] = namesNew[i];
                        O3New[i, 1] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[i]["a05024"]) * 1000, 0).ToString();
                        SO2New[i, 0] = namesNew[i];
                        SO2New[i, 1] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[i]["a21026"]) * 1000, 0).ToString();
                        NO2New[i, 0] = namesNew[i];
                        NO2New[i, 1] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[i]["a21004"]) * 1000, 0).ToString();
                        CONew[i, 0] = namesNew[i];
                        CONew[i, 1] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[i]["a21005"]), 2).ToString();
                        if (dtNew.Rows[i]["O3AQICount"].IsNotNullOrDBNull() && Convert.ToDecimal(dtNew.Rows[i]["O3AQICount"]) != 0)
                        {
                            if (dtNew.Rows[i]["O3AQI"].IsNotNullOrDBNull())
                            {
                                O3AQINew[i, 0] = namesNew[i];
                                O3AQINew[i, 1] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[i]["O3AQI"]) / Convert.ToDecimal(dtNew.Rows[i]["O3AQICount"]) * 100, 1).ToString();
                            }
                        }

                        if (dtNew.Rows[i]["MonitoringRegionUid"].ToString() == "7e05b94c-bbd4-45c3-919c-42da2e63fd43")
                        {
                            for (int j = 2; j < 8; j++)
                            {
                                string factorCode = dtNew.Columns[j].ColumnName;
                                int num = 24;
                                if (factorCode == "a05024")
                                {
                                    num = 8;
                                }
                                decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[i][j]), 4);
                                decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factorCode, Con, num), 0);
                                if (value1 < temp)
                                {
                                    value1 = temp;
                                }
                            }
                        }
                    }
                    newRow["M14"] = value;
                    newRow["M16"] = value1;
                    if (value > value1)
                    {
                        newRow["M15"] = "略有上升";
                    }
                    else if (AQIValue < AQIValueL)
                    {
                        newRow["M15"] = "略有下降";
                    }
                    else
                    {
                        newRow["M15"] = "持平";
                    }
                    for (int j = 0; j < AllStandardRate.Length - 1; j++)
                    {
                        for (int i = j + 1; i < AllStandardRate.Length; i++)
                        {
                            if (AllStandardRate[j] > AllStandardRate[i])
                            {
                                decimal t = AllStandardRate[j];
                                AllStandardRate[j] = AllStandardRate[i];
                                AllStandardRate[i] = t;
                                string name = names[j];
                                names[j] = names[i];
                                names[i] = name;
                            }
                        }
                    }
                    newRow["M5"] = AllStandardRate[0].ToString() + "%~" + AllStandardRate[AllStandardRate.Length - 1].ToString() + "%";
                    string strName = "";
                    for (int i = names.Length - 1; i >= 0; i--)
                    {
                        if (i != 0)
                            strName += (names[i] + "、");
                        else
                        {
                            strName = strName.Trim('、');
                            strName = strName + "和" + names[i];
                        }
                    }
                    newRow["M6"] = strName.Trim('、');
                    for (int j = 0; j < 6 - 1; j++)
                    {
                        for (int i = j + 1; i < 6; i++)
                        {
                            if (days[j] < days[i])
                            {
                                int temp = days[j];
                                days[j] = days[i];
                                days[i] = temp;
                                string name = factorNames[j];
                                factorNames[j] = factorNames[i];
                                factorNames[i] = name;
                            }
                        }
                    }
                    string wuRan = "";
                    if (days[0] != 0)

                        wuRan = "影响环境空气质量的主要污染物为";

                    for (int i = 0; i < 6; i++)
                    {
                        if (days[i] != 0)
                        {
                            wuRan += factorNames[i] + "(作为首要污染物的有" + days[i].ToString() + "天)、";
                        }

                    }
                    if (wuRan != "")
                    {
                        wuRan = wuRan.Trim('、');
                        newRow["M17"] = wuRan + "。";
                    }
                    else
                        newRow["M17"] = wuRan;
                    decimal PM25max = 0;
                    decimal PM25min = 0;
                    string PM25name = "";
                    if (PM25.Length / 2 > 0)
                    {
                        PM25max = Convert.ToDecimal(PM25[0, 1]);
                        PM25min = Convert.ToDecimal(PM25[0, 1]);
                        for (int i = 0; i < PM25.Length / 2; i++)
                        {
                            if (PM25max < Convert.ToDecimal(PM25[i, 1]))
                            {
                                PM25max = Convert.ToDecimal(PM25[i, 1]);
                                PM25name = PM25[i, 0];
                            }
                            if (PM25min > Convert.ToDecimal(PM25[i, 1]))
                            {
                                PM25min = Convert.ToDecimal(PM25[i, 1]);
                            }

                        }
                    }
                    decimal PM10max = 0;
                    decimal PM10min = 0;
                    if (PM10.Length / 2 > 0)
                    {
                        PM10max = Convert.ToInt32(PM10[0, 1]);
                        PM10min = Convert.ToInt32(PM10[0, 1]);
                        for (int i = 0; i < PM10.Length / 2; i++)
                        {
                            if (PM10max < Convert.ToInt32(PM10[i, 1]))
                                PM10max = Convert.ToInt32(PM10[i, 1]);
                            if (PM10min > Convert.ToInt32(PM10[i, 1]))
                                PM10min = Convert.ToInt32(PM10[i, 1]);
                        }
                    }
                    decimal SO2max = 0;
                    decimal SO2min = 0;
                    if (SO2.Length / 2 > 0)
                    {
                        SO2max = Convert.ToInt32(SO2[0, 1]);
                        SO2min = Convert.ToInt32(SO2[0, 1]);
                        for (int i = 0; i < SO2.Length / 2; i++)
                        {
                            if (SO2max < Convert.ToInt32(SO2[i, 1]))
                                SO2max = Convert.ToInt32(SO2[i, 1]);
                            if (SO2min > Convert.ToInt32(SO2[i, 1]))
                                SO2min = Convert.ToInt32(SO2[i, 1]);
                        }
                    }
                    decimal NO2max = 0;
                    decimal NO2min = 0;
                    if (NO2.Length / 2 > 0)
                    {
                        NO2max = Convert.ToInt32(NO2[0, 1]);
                        NO2min = Convert.ToInt32(NO2[0, 1]);
                        for (int i = 0; i < NO2.Length / 2; i++)
                        {
                            if (NO2max < Convert.ToInt32(NO2[i, 1]))
                                NO2max = Convert.ToInt32(NO2[i, 1]);
                            if (NO2min > Convert.ToInt32(NO2[i, 1]))
                                NO2min = Convert.ToInt32(NO2[i, 1]);
                        }
                    }
                    decimal O3AQImax = 0;
                    decimal O3AQImin = 0;
                    if (O3AQI.Length / 2 > 0)
                    {
                        O3AQImax = Convert.ToDecimal(O3AQI[0, 1]);
                        O3AQImin = Convert.ToDecimal(O3AQI[0, 1]);
                        for (int i = 0; i < O3AQI.Length / 2; i++)
                        {
                            if (O3AQImax < Convert.ToDecimal(O3AQI[i, 1]))
                                O3AQImax = Convert.ToDecimal(O3AQI[i, 1]);
                            if (O3AQImin > Convert.ToDecimal(O3AQI[i, 1]) && Convert.ToDecimal(O3AQI[i, 1]) != 0)
                                O3AQImin = Convert.ToDecimal(O3AQI[i, 1]);
                        }
                    }
                    decimal COmax = 0;
                    decimal COmin = 0;
                    if (CO.Length / 2 > 0)
                    {
                        COmax = Convert.ToDecimal(CO[0, 1]);
                        COmin = Convert.ToDecimal(CO[0, 1]);
                        for (int i = 0; i < CO.Length / 2; i++)
                        {
                            if (COmax < Convert.ToDecimal(CO[i, 1]))
                                COmax = Convert.ToDecimal(CO[i, 1]);
                            if (COmin > Convert.ToDecimal(CO[i, 1]))
                                COmin = Convert.ToDecimal(CO[i, 1]);
                        }
                    }
                    string PM25drop = "";
                    string PM25down = "";
                    string PM25ping = "";
                    for (int i = 0; i < PM25.Length / 2; i++)
                    {
                        for (int j = 0; j < PM25New.Length / 2; j++)
                            if (PM25[i, 0] == PM25New[j, 0])
                            {
                                if (Convert.ToDecimal(PM25[i, 1]) < Convert.ToDecimal(PM25New[j, 1]))
                                    PM25down += (PM25[i, 0] + "、");
                                else if (Convert.ToDecimal(PM25[i, 1]) > Convert.ToDecimal(PM25New[j, 1]))
                                    PM25drop += (PM25[i, 0] + "、");
                                else
                                    PM25ping += (PM25[i, 0] + "、");
                            }
                    }
                    if (PM25down != "")
                    {
                        if (PM25ping != "" || PM25drop != "")
                        {
                            PM25down = PM25down.Trim('、');
                            PM25down = "除" + PM25down + "细颗粒物浓度略有降低外，";
                        }
                        else
                            PM25down = "全市各地细颗粒物浓度均有所降低";
                    }
                    if (PM25ping != "")
                    {
                        if (PM25down != "" || PM25drop != "")
                        {
                            PM25ping = PM25ping.Trim('、');
                            PM25ping = PM25ping + "细颗粒物浓度持平，";
                        }
                        else
                            PM25ping = "全市各地细颗粒物浓度均持平";
                    }
                    if (PM25drop != "")
                    {
                        if (PM25down != "" || PM25ping != "")
                        {
                            PM25drop = PM25drop.Trim('、');
                            PM25drop = "其余各地平均浓度均有不同程度上升";
                        }
                        else
                            PM25drop = "全市各地平均浓度均有不同程度上升";
                    }
                    string PM25Str = "";
                    if (PM25min > 35)
                        PM25Str = "均超过年均浓度标准限值。";
                    else if (PM25max <= 35)
                        PM25Str = "均达到年均浓度标准限值。";
                    string PM25chao = "";
                    if (PM25max > 35)
                    {
                        PM25chao = "超标" + DecimalExtension.GetRoundValue(Convert.ToDecimal(PM25max - 35) / Convert.ToDecimal(35), 2).ToString() + "倍。";
                    }
                    else
                        PM25chao = "未超标。";
                    newRow["M18"] = PM25min.ToString() + "~" + PM25max.ToString() + "微克/立方米之间，" + PM25Str + "其中，" + PM25name + "最高，为" + PM25max.ToString() + "微克/立方米，" + PM25chao + "与上年同期相比，" + PM25down + PM25ping + PM25drop;
                    string PM10drop = "";
                    string PM10down = "";
                    string PM10ping = "";
                    for (int i = 0; i < PM10.Length / 2; i++)
                    {
                        for (int j = 0; j < PM10New.Length / 2; j++)
                            if (PM10[i, 0] == PM10New[j, 0])
                            {
                                if (Convert.ToInt32(PM10[i, 1]) < Convert.ToInt32(PM10New[j, 1]))
                                    PM10down += (PM10[i, 0] + "、");
                                else if (Convert.ToInt32(PM10[i, 1]) > Convert.ToInt32(PM10New[j, 1]))
                                    PM10drop += (PM10[i, 0] + "、");
                                else
                                    PM10ping += (PM10[i, 0] + "、");
                            }
                    }
                    if (PM10down != "")
                    {
                        if (PM10ping != "" || PM10drop != "")
                        {
                            PM10down = PM10down.Trim('、');
                            PM10down = "除" + PM10down + "可吸入颗粒物浓度有所降低，";
                        }
                        else
                            PM10down = "全市各地可吸入颗粒物浓度均有所降低";
                    }
                    if (PM10ping != "")
                    {
                        if (PM10down != "" || PM10drop != "")
                        {
                            PM10ping = PM10ping.Trim('、');
                            PM10ping = PM10ping + "可吸入颗粒物浓度持平，";
                        }
                        else
                            PM10ping = "全市各地可吸入颗粒物浓度均持平";
                    }
                    if (PM10drop != "")
                    {
                        if (PM10down != "" || PM10ping != "")
                        {
                            PM10drop = PM10drop.Trim('、');
                            PM10drop = "其余各地平均浓度均有不同程度上升";
                        }
                        else
                        {
                            PM10drop = "全市各地平均浓度均有不同程度上升";
                        }
                    }
                    string PM10Str = "";
                    if (PM10min > 70)
                        PM10Str = "均超过年均浓度标准限值。";
                    else if (PM10max <= 70)
                        PM10Str = "均达到年均浓度标准限值。";
                    newRow["M19"] = PM10min.ToString() + "~" + PM10max.ToString() + "微克/立方米之间，" + PM10Str + "与上年同期相比，" + PM10down + PM10ping + PM10drop;

                    string NO2drop = "";
                    string NO2down = "";
                    string NO2ping = "";
                    for (int i = 0; i < NO2.Length / 2; i++)
                    {
                        for (int j = 0; j < NO2New.Length / 2; j++)
                            if (NO2[i, 0] == NO2New[j, 0])
                            {
                                if (Convert.ToInt32(NO2[i, 1]) < Convert.ToInt32(NO2New[j, 1]))
                                    NO2down += (NO2[i, 0] + "、");
                                else if (Convert.ToInt32(NO2[i, 1]) > Convert.ToInt32(NO2New[j, 1]))
                                    NO2drop += (NO2[i, 0] + "、");
                                else
                                    NO2ping += (NO2[i, 0] + "、");
                            }
                    }
                    if (NO2down != "")
                    {
                        if (NO2ping != "" || NO2drop != "")
                        {
                            NO2down = NO2down.Trim('、');
                            NO2down = "除" + NO2down + "二氧化氮浓度有所降低，";
                        }
                        else
                            NO2down = "全市各地二氧化氮浓度均有所降低";
                    }
                    if (NO2ping != "")
                    {
                        if (NO2down != "" || NO2drop != "")
                        {
                            NO2ping = NO2ping.Trim('、');
                            NO2ping = NO2ping + "二氧化氮浓度持平，";
                        }
                        else
                            NO2ping = "全市各地二氧化氮浓度均持平";
                    }
                    if (NO2drop != "")
                    {
                        if (NO2down != "" || NO2ping != "")
                        {
                            NO2drop = NO2drop.Trim('、');
                            NO2drop = "，其余各地平均浓度均有不同程度上升";
                        }
                        else
                        {
                            NO2drop = "各地平均浓度均有不同程度上升";
                        }
                    }
                    string NO2Str = "";
                    if (NO2min > 40)
                        NO2Str = "均超过年均浓度标准限值。";
                    else if (NO2max <= 40)
                        NO2Str = "均达到年均浓度标准限值。";
                    newRow["M20"] = NO2min.ToString() + "~" + NO2max.ToString() + "微克/立方米之间，" + NO2Str + "与上年同期相比，" + NO2down + NO2ping + NO2drop;

                    string SO2drop = "";
                    string SO2down = "";
                    string SO2ping = "";
                    for (int i = 0; i < SO2.Length / 2; i++)
                    {
                        for (int j = 0; j < SO2New.Length / 2; j++)
                            if (SO2[i, 0] == SO2New[j, 0])
                            {
                                if (Convert.ToInt32(SO2[i, 1]) < Convert.ToInt32(SO2New[j, 1]))
                                    SO2down += (SO2[i, 0] + "、");
                                else if (Convert.ToInt32(SO2[i, 1]) > Convert.ToInt32(SO2New[j, 1]))
                                    SO2drop += (SO2[i, 0] + "、");
                                else
                                    SO2ping += (SO2[i, 0] + "、");
                            }
                    }
                    if (SO2down != "")
                    {
                        if (SO2ping != "" || SO2drop != "")
                        {
                            SO2down = SO2down.Trim('、');
                            SO2down = SO2down + "二氧化硫浓度有不同程度降低，";
                        }
                        else
                            SO2down = "全市各地二氧化硫浓度均有不同程度降低";
                    }
                    if (SO2ping != "")
                    {
                        if (SO2down != "" || SO2drop != "")
                        {
                            SO2ping = SO2ping.Trim('、');
                            SO2ping = SO2ping + "二氧化硫浓度持平，";
                        }
                        else
                            SO2ping = "全市各地二氧化硫浓度均持平";
                    }
                    if (SO2drop != "")
                    {
                        if (SO2down != "" || SO2ping != "")
                        {
                            SO2drop = SO2drop.Trim('、');
                            SO2drop = SO2drop + "平均浓度有所上升";
                        }
                        else
                            SO2drop = "全市各地二氧化硫平均浓度均有所上升";
                    }
                    string SO2Str = "";
                    if (SO2min > 60)
                        SO2Str = "均超过年均浓度标准。";
                    else if (SO2max <= 60)
                        SO2Str = "均达到年均浓度标准。";
                    newRow["M21"] = SO2min.ToString() + "~" + SO2max.ToString() + "微克/立方米之间，" + SO2Str + "与上年同期相比，" + SO2down + SO2ping + SO2drop;


                    string O3AQIdrop = "";
                    string O3AQIdown = "";
                    string O3AQIping = "";
                    string O3AQIChao = "";
                    for (int i = 0; i < O3AQI.Length / 2; i++)
                    {
                        for (int j = 0; j < O3AQINew.Length / 2; j++)
                            if (O3AQI[i, 0] == O3AQINew[j, 0])
                            {
                                if (Convert.ToDecimal(O3AQI[i, 1]) < Convert.ToDecimal(O3AQINew[j, 1]))
                                    O3AQIdown += (O3AQI[i, 0] + "、");
                                else if (Convert.ToDecimal(O3AQI[i, 1]) > Convert.ToDecimal(O3AQINew[j, 1]))
                                    O3AQIdrop += (O3AQI[i, 0] + "、");
                                else if (Convert.ToDecimal(O3AQI[i, 1]) != 0)
                                    O3AQIping += (O3AQI[i, 0] + "、");
                                else
                                    O3AQIChao += (O3AQI[i, 0] + "、");
                            }
                    }
                    if (O3AQIdown != "")
                    {
                        if (O3AQIping != "" || O3AQIdrop != "")
                        {
                            O3AQIdown = O3AQIdown.Trim('、');
                            O3AQIdown = "除" + O3AQIdown + "臭氧超标率有所降低外，";
                        }
                        else
                            O3AQIdown = "全市各地臭氧超标率均有所降低";
                    }
                    if (O3AQIping != "")
                    {
                        if (O3AQIdown != "" || O3AQIdrop != "")
                        {
                            O3AQIping = O3AQIping.Trim('、');
                            O3AQIping = O3AQIping + "臭氧超标率持平，";
                        }
                        else
                            O3AQIping = "全市各地臭氧超标率均持平";
                    }
                    if (O3AQIdrop != "")
                    {
                        if (O3AQIdown != "" || O3AQIping != "")
                        {

                            O3AQIdrop = "其余各地超标率均有不同程度上升";
                        }
                        else if (O3AQIChao != "")
                        {
                            O3AQIdrop = O3AQIdrop.Trim('、');
                            O3AQIdrop = O3AQIdrop + "臭氧超标率均有不同程度上升";
                        }
                        else
                            O3AQIdrop = "全市各地臭氧超标率均有不同程度上升";
                    }
                    string O3Str = "";
                    if (O3AQIChao != "")
                    {
                        O3AQIChao = O3AQIChao.Trim('、');
                        O3Str = "除" + O3AQIChao + "臭氧日最大8小时平均浓度未超标外，其余各地均出现超标现象，超标率分布在";
                    }
                    else
                        O3Str = "全市各地臭氧日最大8小时平均浓度均出现超标现象，超标率分布在";
                    newRow["M22"] = O3Str + O3AQImin.ToString() + "%~" + O3AQImax.ToString() + "%之间，与上年同期相比，" + O3AQIdown + O3AQIping + O3AQIdrop;

                    string COdrop = "";
                    string COdown = "";
                    string COping = "";
                    for (int i = 0; i < CO.Length / 2; i++)
                    {
                        for (int j = 0; j < CONew.Length / 2; j++)
                            if (CO[i, 0] == CONew[j, 0])
                            {
                                if (Convert.ToDecimal(CO[i, 1]) < Convert.ToDecimal(CONew[j, 1]))
                                    COdown += (CO[i, 0] + "、");
                                else if (Convert.ToDecimal(CO[i, 1]) > Convert.ToDecimal(CONew[j, 1]))
                                    COdrop += (CO[i, 0] + "、");
                                else
                                    COping += (CO[i, 0] + "、");
                            }
                    }
                    if (COdown != "")
                    {
                        if (COping != "" || COdrop != "")
                        {
                            COdown = COdown.Trim('、');
                            COdown = COdown + "一氧化碳浓度有不同程度降低，";
                        }
                        else
                            COdown = "全市各地一氧化碳浓度均有不同程度降低";
                    }
                    if (COping != "")
                    {
                        if (COdown != "" || COdrop != "")
                        {
                            COping = COping.Trim('、');
                            COping = COping + "一氧化碳浓度持平，";
                        }
                        else
                            COping = "全市各地一氧化碳浓度均持平";
                    }
                    if (COdrop != "")
                    {
                        if (COdown != "" || COping != "")
                        {
                            COdrop = COdrop.Trim('、');
                            COdrop = COdrop + "平均浓度有不同程度上升";
                        }
                        else
                            COdrop = "全市各地平均浓度均有不同程度上升";
                    }
                    string COStr = "";
                    if (COmin > 4)
                        COStr = "均超过标准限值。";
                    else if (COmax <= 4)
                        COStr = "各地一氧化碳日均浓度均达标。";
                    newRow["M23"] = COmin.ToString() + "~" + COmax.ToString() + "毫克/立方米之间，" + COStr + "与上年同期相比，" + COdown + COping + COdrop;
                }


                newdt.Rows.Add(newRow);
                return newdt.DefaultView;
            }
            return null;
        }
        public DataTable getRegionData(DateTime dateStart, DateTime dateEnd, int year)
        {
            string[] regionguids = { "7e05b94c-bbd4-45c3-919c-42da2e63fd43"     //市区           
			                ,"66d2abd1-ca39-4e39-909f-da872704fbfd"			    //张家港市
                            ,"d7d7a1fe-493a-4b3f-8504-b1850f6d9eff"			    //常熟市
			                ,"57b196ed-5038-4ad0-a035-76faee2d7a98"				//太仓市
			                ,"2e2950cd-dbab-43b3-811d-61bd7569565a"				//昆山市	
			                ,"2fea3cb2-8b95-45e6-8a71-471562c4c89c"	            //吴江区
                                  };
            string[] factors = { "a34004", "a34002", "a21026", "a21004", "a21005", "a05024", "standrate" };
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("factor", typeof(string));
            dtNew.Columns.Add("市区", typeof(decimal));
            dtNew.Columns.Add("张家港", typeof(decimal));
            dtNew.Columns.Add("常熟", typeof(decimal));
            dtNew.Columns.Add("太仓", typeof(decimal));
            dtNew.Columns.Add("昆山", typeof(decimal));
            dtNew.Columns.Add("吴江", typeof(decimal));
            dtNew.Columns.Add("全市", typeof(decimal));
            dtNew.Columns.Add("year", typeof(string));
            //dtNew.Columns.Add("regionName", typeof(string));
            //dtNew.Columns.Add("regionName", typeof(string));
            //dtNew.Columns.Add("regionName", typeof(string));
            DateTime dtSta = dateStart.AddYears(-1);
            DateTime dtEn = dateEnd.AddYears(-1);
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();

            DataTable dt = regionDayAQI.GetRegionsAllData(regionguids, dateStart, dateEnd).Table;  //今年
            DataTable lastdt = regionDayAQI.GetRegionsAllData(regionguids, dtSta, dtEn).Table;  //去年
            DataTable dtNewt = regionDayAQI.GetRegionsHalfYearData(dateStart, dateEnd).Table;  //2015 全市dabiao
            DataTable dtNewn = regionDayAQI.GetRegionsHalfYearData(dtSta, dtEn).Table;   //2014  全市dabiao

            foreach (string factor in factors)
            {
                decimal dit = 1000;
                int intx = 0;
                if (factor == "a21005")
                {
                    dit = 1;
                    intx = 2;
                }
                if (factor == "a34004")
                {
                    intx = 1;
                }
                if (factor != "standrate")
                {
                    DataRow lastrow = dtNew.NewRow();
                    lastrow["factor"] = factor;
                    if (year == 1)
                        lastrow["year"] = dateEnd.AddYears(-1).Year.ToString() + "年" + dateStart.Month + "-" + dateEnd.Month + "月";
                    else
                        lastrow["year"] = dateEnd.AddYears(-1).Year.ToString() + "年";
                    foreach (string regionguid in regionguids)
                    {
                        DataRow[] drarry = lastdt.Select("MonitoringRegionUid='" + regionguid + "'");
                        if (drarry.Length > 0)
                        {
                            if (factor != "a05024")
                            {
                                if (regionguid == "7e05b94c-bbd4-45c3-919c-42da2e63fd43")
                                {
                                    if (drarry[0][factor] != DBNull.Value)
                                    {
                                        lastrow["市区"] = DecimalExtension.GetRoundValue(decimal.Parse(drarry[0][factor].ToString()) * dit, intx);
                                    }
                                }
                                else if (regionguid == "66d2abd1-ca39-4e39-909f-da872704fbfd")
                                {
                                    if (drarry[0][factor] != DBNull.Value)
                                    {
                                        lastrow["张家港"] = DecimalExtension.GetRoundValue(decimal.Parse(drarry[0][factor].ToString()) * dit, intx);
                                    }
                                }
                                else if (regionguid == "d7d7a1fe-493a-4b3f-8504-b1850f6d9eff")
                                {
                                    if (drarry[0][factor] != DBNull.Value)
                                    {
                                        lastrow["常熟"] = DecimalExtension.GetRoundValue(decimal.Parse(drarry[0][factor].ToString()) * dit, intx);
                                    }
                                }
                                else if (regionguid == "57b196ed-5038-4ad0-a035-76faee2d7a98")
                                {
                                    if (drarry[0][factor] != DBNull.Value)
                                    {
                                        lastrow["太仓"] = DecimalExtension.GetRoundValue(decimal.Parse(drarry[0][factor].ToString()) * dit, intx);
                                    }
                                }
                                else if (regionguid == "2e2950cd-dbab-43b3-811d-61bd7569565a")
                                {
                                    if (drarry[0][factor] != DBNull.Value)
                                    {
                                        lastrow["昆山"] = DecimalExtension.GetRoundValue(decimal.Parse(drarry[0][factor].ToString()) * dit, intx);
                                    }
                                }
                                else if (regionguid == "2fea3cb2-8b95-45e6-8a71-471562c4c89c")
                                {
                                    if (drarry[0][factor] != DBNull.Value)
                                    {
                                        lastrow["吴江"] = DecimalExtension.GetRoundValue(decimal.Parse(drarry[0][factor].ToString()) * dit, intx);
                                    }
                                }
                            }
                            else
                            {
                                if (regionguid == "7e05b94c-bbd4-45c3-919c-42da2e63fd43")
                                {
                                    if (drarry[0]["O3AQICount"] != DBNull.Value && Convert.ToDecimal(drarry[0]["O3AQICount"]) != 0)
                                    {
                                        if (drarry[0]["O3AQI"] != DBNull.Value)
                                            lastrow["市区"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(drarry[0]["O3AQI"]) / Convert.ToDecimal(drarry[0]["O3AQICount"]) * 100, 1);
                                    }
                                }
                                else if (regionguid == "66d2abd1-ca39-4e39-909f-da872704fbfd")
                                {
                                    if (drarry[0]["O3AQICount"] != DBNull.Value && Convert.ToDecimal(drarry[0]["O3AQICount"]) != 0)
                                    {
                                        if (drarry[0]["O3AQI"] != DBNull.Value)
                                            lastrow["张家港"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(drarry[0]["O3AQI"]) / Convert.ToDecimal(drarry[0]["O3AQICount"]) * 100, 1);
                                    }
                                }
                                else if (regionguid == "d7d7a1fe-493a-4b3f-8504-b1850f6d9eff")
                                {
                                    if (drarry[0]["O3AQICount"] != DBNull.Value && Convert.ToDecimal(drarry[0]["O3AQICount"]) != 0)
                                    {
                                        if (drarry[0]["O3AQI"] != DBNull.Value)
                                            lastrow["常熟"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(drarry[0]["O3AQI"]) / Convert.ToDecimal(drarry[0]["O3AQICount"]) * 100, 1);
                                    }
                                }
                                else if (regionguid == "57b196ed-5038-4ad0-a035-76faee2d7a98")
                                {
                                    if (drarry[0]["O3AQICount"] != DBNull.Value && Convert.ToDecimal(drarry[0]["O3AQICount"]) != 0)
                                    {
                                        if (drarry[0]["O3AQI"] != DBNull.Value)
                                            lastrow["太仓"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(drarry[0]["O3AQI"]) / Convert.ToDecimal(drarry[0]["O3AQICount"]) * 100, 1);
                                    }
                                }
                                else if (regionguid == "2e2950cd-dbab-43b3-811d-61bd7569565a")
                                {
                                    if (drarry[0]["O3AQICount"] != DBNull.Value && Convert.ToDecimal(drarry[0]["O3AQICount"]) != 0)
                                    {
                                        if (drarry[0]["O3AQI"] != DBNull.Value)
                                            lastrow["昆山"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(drarry[0]["O3AQI"]) / Convert.ToDecimal(drarry[0]["O3AQICount"]) * 100, 1);
                                    }
                                }
                                else if (regionguid == "2fea3cb2-8b95-45e6-8a71-471562c4c89c")
                                {
                                    if (drarry[0]["O3AQICount"] != DBNull.Value && Convert.ToDecimal(drarry[0]["O3AQICount"]) != 0)
                                    {
                                        if (drarry[0]["O3AQI"] != DBNull.Value)
                                            lastrow["吴江"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(drarry[0]["O3AQI"]) / Convert.ToDecimal(drarry[0]["O3AQICount"]) * 100, 1);
                                    }
                                }
                            }
                        }
                    }
                    dtNew.Rows.Add(lastrow);

                    DataRow thisrow = dtNew.NewRow();
                    thisrow["factor"] = factor;
                    if (year == 1)
                        thisrow["year"] = dateEnd.Year.ToString() + "年" + dateStart.Month + "-" + dateEnd.Month + "月";
                    else
                        thisrow["year"] = dateEnd.Year.ToString() + "年";
                    foreach (string regionguid in regionguids)
                    {
                        DataRow[] drarry = dt.Select("MonitoringRegionUid='" + regionguid + "'");
                        if (drarry.Length > 0)
                        {
                            if (factor != "a05024")
                            {
                                if (regionguid == "7e05b94c-bbd4-45c3-919c-42da2e63fd43")
                                {
                                    if (drarry[0][factor] != DBNull.Value)
                                    {
                                        thisrow["市区"] = DecimalExtension.GetRoundValue(decimal.Parse(drarry[0][factor].ToString()) * dit, intx);
                                    }
                                }
                                else if (regionguid == "66d2abd1-ca39-4e39-909f-da872704fbfd")
                                {
                                    if (drarry[0][factor] != DBNull.Value)
                                    {
                                        thisrow["张家港"] = DecimalExtension.GetRoundValue(decimal.Parse(drarry[0][factor].ToString()) * dit, intx);
                                    }
                                }
                                else if (regionguid == "d7d7a1fe-493a-4b3f-8504-b1850f6d9eff")
                                {
                                    if (drarry[0][factor] != DBNull.Value)
                                    {
                                        thisrow["常熟"] = DecimalExtension.GetRoundValue(decimal.Parse(drarry[0][factor].ToString()) * dit, intx);
                                    }
                                }
                                else if (regionguid == "57b196ed-5038-4ad0-a035-76faee2d7a98")
                                {
                                    if (drarry[0][factor] != DBNull.Value)
                                    {
                                        thisrow["太仓"] = DecimalExtension.GetRoundValue(decimal.Parse(drarry[0][factor].ToString()) * dit, intx);
                                    }
                                }
                                else if (regionguid == "2e2950cd-dbab-43b3-811d-61bd7569565a")
                                {
                                    if (drarry[0][factor] != DBNull.Value)
                                    {
                                        thisrow["昆山"] = DecimalExtension.GetRoundValue(decimal.Parse(drarry[0][factor].ToString()) * dit, intx);
                                    }
                                }
                                else if (regionguid == "2fea3cb2-8b95-45e6-8a71-471562c4c89c")
                                {
                                    if (drarry[0][factor] != DBNull.Value)
                                    {
                                        thisrow["吴江"] = DecimalExtension.GetRoundValue(decimal.Parse(drarry[0][factor].ToString()) * dit, intx);
                                    }
                                }
                            }
                            else
                            {
                                if (regionguid == "7e05b94c-bbd4-45c3-919c-42da2e63fd43")
                                {
                                    if (drarry[0]["O3AQICount"] != DBNull.Value && Convert.ToDecimal(drarry[0]["O3AQICount"]) != 0)
                                    {
                                        if (drarry[0]["O3AQI"] != DBNull.Value)
                                            thisrow["市区"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(drarry[0]["O3AQI"]) / Convert.ToDecimal(drarry[0]["O3AQICount"]) * 100, 1);
                                    }
                                }
                                else if (regionguid == "66d2abd1-ca39-4e39-909f-da872704fbfd")
                                {
                                    if (drarry[0]["O3AQICount"] != DBNull.Value && Convert.ToDecimal(drarry[0]["O3AQICount"]) != 0)
                                    {
                                        if (drarry[0]["O3AQI"] != DBNull.Value)
                                            thisrow["张家港"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(drarry[0]["O3AQI"]) / Convert.ToDecimal(drarry[0]["O3AQICount"]) * 100, 1);
                                    }
                                }
                                else if (regionguid == "d7d7a1fe-493a-4b3f-8504-b1850f6d9eff")
                                {
                                    if (drarry[0]["O3AQICount"] != DBNull.Value && Convert.ToDecimal(drarry[0]["O3AQICount"]) != 0)
                                    {
                                        if (drarry[0]["O3AQI"] != DBNull.Value)
                                            thisrow["常熟"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(drarry[0]["O3AQI"]) / Convert.ToDecimal(drarry[0]["O3AQICount"]) * 100, 1);
                                    }
                                }
                                else if (regionguid == "57b196ed-5038-4ad0-a035-76faee2d7a98")
                                {
                                    if (drarry[0]["O3AQICount"] != DBNull.Value && Convert.ToDecimal(drarry[0]["O3AQICount"]) != 0)
                                    {
                                        if (drarry[0]["O3AQI"] != DBNull.Value)
                                            thisrow["太仓"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(drarry[0]["O3AQI"]) / Convert.ToDecimal(drarry[0]["O3AQICount"]) * 100, 1);
                                    }
                                }
                                else if (regionguid == "2e2950cd-dbab-43b3-811d-61bd7569565a")
                                {
                                    if (drarry[0]["O3AQICount"] != DBNull.Value && Convert.ToDecimal(drarry[0]["O3AQICount"]) != 0)
                                    {
                                        if (drarry[0]["O3AQI"] != DBNull.Value)
                                            thisrow["昆山"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(drarry[0]["O3AQI"]) / Convert.ToDecimal(drarry[0]["O3AQICount"]) * 100, 1);
                                    }
                                }
                                else if (regionguid == "2fea3cb2-8b95-45e6-8a71-471562c4c89c")
                                {
                                    if (drarry[0]["O3AQICount"] != DBNull.Value && Convert.ToDecimal(drarry[0]["O3AQICount"]) != 0)
                                    {
                                        if (drarry[0]["O3AQI"] != DBNull.Value)
                                            thisrow["吴江"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(drarry[0]["O3AQI"]) / Convert.ToDecimal(drarry[0]["O3AQICount"]) * 100, 1);
                                    }
                                }
                            }
                        }
                    }
                    dtNew.Rows.Add(thisrow);
                }
                else
                {
                    DataRow lastrow = dtNew.NewRow();
                    lastrow["factor"] = factor;
                    if (year == 1)
                        lastrow["year"] = dateEnd.AddYears(-1).Year.ToString() + "年" + dateStart.Month + "-" + dateEnd.Month + "月";
                    else
                        lastrow["year"] = dateEnd.AddYears(-1).Year.ToString() + "年";
                    foreach (string regionguid in regionguids)
                    {
                        DataRow[] drarry = lastdt.Select("MonitoringRegionUid='" + regionguid + "'");
                        if (drarry.Length > 0)
                        {
                            if (regionguid == "7e05b94c-bbd4-45c3-919c-42da2e63fd43")
                            {
                                if (drarry[0]["ValidCount"] != DBNull.Value)
                                {
                                    lastrow["市区"] = decimal.Parse(drarry[0]["ValidCount"].ToString()) != 0 ? DecimalExtension.GetRoundValue((decimal.Parse(drarry[0]["StandardCount"].ToString()) / decimal.Parse(drarry[0]["ValidCount"].ToString())) * 100, 1) : 0;
                                }
                            }
                            else if (regionguid == "66d2abd1-ca39-4e39-909f-da872704fbfd")
                            {
                                if (drarry[0]["ValidCount"] != DBNull.Value)
                                {
                                    lastrow["张家港"] = decimal.Parse(drarry[0]["ValidCount"].ToString()) != 0 ? DecimalExtension.GetRoundValue((decimal.Parse(drarry[0]["StandardCount"].ToString()) / decimal.Parse(drarry[0]["ValidCount"].ToString())) * 100, 1) : 0;
                                }
                            }
                            else if (regionguid == "d7d7a1fe-493a-4b3f-8504-b1850f6d9eff")
                            {
                                if (drarry[0]["ValidCount"] != DBNull.Value)
                                {
                                    lastrow["常熟"] = decimal.Parse(drarry[0]["ValidCount"].ToString()) != 0 ? DecimalExtension.GetRoundValue((decimal.Parse(drarry[0]["StandardCount"].ToString()) / decimal.Parse(drarry[0]["ValidCount"].ToString())) * 100, 1) : 0;
                                }
                            }
                            else if (regionguid == "57b196ed-5038-4ad0-a035-76faee2d7a98")
                            {
                                if (drarry[0]["ValidCount"] != DBNull.Value)
                                {
                                    lastrow["太仓"] = decimal.Parse(drarry[0]["ValidCount"].ToString()) != 0 ? DecimalExtension.GetRoundValue((decimal.Parse(drarry[0]["StandardCount"].ToString()) / decimal.Parse(drarry[0]["ValidCount"].ToString())) * 100, 1) : 0;
                                }
                            }
                            else if (regionguid == "2e2950cd-dbab-43b3-811d-61bd7569565a")
                            {
                                if (drarry[0]["ValidCount"] != DBNull.Value)
                                {
                                    lastrow["昆山"] = decimal.Parse(drarry[0]["ValidCount"].ToString()) != 0 ? DecimalExtension.GetRoundValue((decimal.Parse(drarry[0]["StandardCount"].ToString()) / decimal.Parse(drarry[0]["ValidCount"].ToString())) * 100, 1) : 0;
                                }
                            }
                            else if (regionguid == "2fea3cb2-8b95-45e6-8a71-471562c4c89c")
                            {
                                if (drarry[0]["ValidCount"] != DBNull.Value)
                                {
                                    lastrow["吴江"] = decimal.Parse(drarry[0]["ValidCount"].ToString()) != 0 ? DecimalExtension.GetRoundValue((decimal.Parse(drarry[0]["StandardCount"].ToString()) / decimal.Parse(drarry[0]["ValidCount"].ToString())) * 100, 1) : 0;
                                }
                            }
                        }
                    }
                    decimal StandardRateL = 0;
                    if (dtNewn.Rows.Count > 0)
                    {
                        decimal StandardCount = 0;
                        decimal ValidCount = 0;
                        for (int i = 0; i < dtNewn.Rows.Count; i++)
                        {
                            decimal AQIcount = 0;

                            for (int j = 1; j < 7; j++)
                            {
                                string factorCode = dtNewn.Columns[j].ColumnName;
                                int num = 24;
                                if (factorCode == "a05024")
                                {
                                    num = 8;
                                }
                                decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNewn.Rows[i][j]), 4);
                                decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factorCode, Con, num), 0);
                                if (AQIcount < temp)
                                {
                                    AQIcount = temp;
                                }
                            }
                            if (AQIcount > 0)
                            {
                                ValidCount++;
                                if (AQIcount <= 100)
                                    StandardCount++;
                            }
                        }
                        if (ValidCount != 0)
                        {
                            StandardRateL = DecimalExtension.GetRoundValue(StandardCount / ValidCount * 100, 1);
                        }
                        lastrow["全市"] = StandardRateL.ToString();
                    }
                    dtNew.Rows.Add(lastrow);

                    DataRow thisrow = dtNew.NewRow();
                    thisrow["factor"] = factor;
                    if (year == 1)
                        thisrow["year"] = dateEnd.Year.ToString() + "年" + dateStart.Month + "-" + dateEnd.Month + "月";
                    else
                        thisrow["year"] = dateEnd.Year.ToString() + "年";
                    foreach (string regionguid in regionguids)
                    {
                        DataRow[] drarry = dt.Select("MonitoringRegionUid='" + regionguid + "'");
                        if (drarry.Length > 0)
                        {
                            if (regionguid == "7e05b94c-bbd4-45c3-919c-42da2e63fd43")
                            {
                                if (drarry[0]["ValidCount"] != DBNull.Value)
                                {
                                    thisrow["市区"] = decimal.Parse(drarry[0]["ValidCount"].ToString()) != 0 ? DecimalExtension.GetRoundValue((decimal.Parse(drarry[0]["StandardCount"].ToString()) / decimal.Parse(drarry[0]["ValidCount"].ToString())) * 100, 1) : 0;
                                }
                            }
                            else if (regionguid == "66d2abd1-ca39-4e39-909f-da872704fbfd")
                            {
                                if (drarry[0]["ValidCount"] != DBNull.Value)
                                {
                                    thisrow["张家港"] = decimal.Parse(drarry[0]["ValidCount"].ToString()) != 0 ? DecimalExtension.GetRoundValue((decimal.Parse(drarry[0]["StandardCount"].ToString()) / decimal.Parse(drarry[0]["ValidCount"].ToString())) * 100, 1) : 0;
                                }
                            }
                            else if (regionguid == "d7d7a1fe-493a-4b3f-8504-b1850f6d9eff")
                            {
                                if (drarry[0]["ValidCount"] != DBNull.Value)
                                {
                                    thisrow["常熟"] = decimal.Parse(drarry[0]["ValidCount"].ToString()) != 0 ? DecimalExtension.GetRoundValue((decimal.Parse(drarry[0]["StandardCount"].ToString()) / decimal.Parse(drarry[0]["ValidCount"].ToString())) * 100, 1) : 0;
                                }
                            }
                            else if (regionguid == "57b196ed-5038-4ad0-a035-76faee2d7a98")
                            {
                                if (drarry[0]["ValidCount"] != DBNull.Value)
                                {
                                    thisrow["太仓"] = decimal.Parse(drarry[0]["ValidCount"].ToString()) != 0 ? DecimalExtension.GetRoundValue((decimal.Parse(drarry[0]["StandardCount"].ToString()) / decimal.Parse(drarry[0]["ValidCount"].ToString())) * 100, 1) : 0;
                                }
                            }
                            else if (regionguid == "2e2950cd-dbab-43b3-811d-61bd7569565a")
                            {
                                if (drarry[0]["ValidCount"] != DBNull.Value)
                                {
                                    thisrow["昆山"] = decimal.Parse(drarry[0]["ValidCount"].ToString()) != 0 ? DecimalExtension.GetRoundValue((decimal.Parse(drarry[0]["StandardCount"].ToString()) / decimal.Parse(drarry[0]["ValidCount"].ToString())) * 100, 1) : 0;
                                }
                            }
                            else if (regionguid == "2fea3cb2-8b95-45e6-8a71-471562c4c89c")
                            {
                                if (drarry[0]["ValidCount"] != DBNull.Value)
                                {
                                    thisrow["吴江"] = decimal.Parse(drarry[0]["ValidCount"].ToString()) != 0 ? DecimalExtension.GetRoundValue((decimal.Parse(drarry[0]["StandardCount"].ToString()) / decimal.Parse(drarry[0]["ValidCount"].ToString())) * 100, 1) : 0;
                                }
                            }
                        }
                    }
                    decimal StandardRate = 0;
                    //全市达标率
                    if (dtNewt.Rows.Count > 0)
                    {
                        decimal StandardCount = 0;
                        decimal ValidCount = 0;
                        for (int i = 0; i < dtNewt.Rows.Count; i++)
                        {
                            decimal AQIcount = 0;

                            for (int j = 1; j < 7; j++)
                            {
                                string factorCode = dtNewt.Columns[j].ColumnName;
                                int num = 24;
                                if (factorCode == "a05024")
                                {
                                    num = 8;
                                }
                                decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNewt.Rows[i][j]), 4);
                                decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factorCode, Con, num), 0);
                                if (AQIcount < temp)
                                {
                                    AQIcount = temp;
                                }
                            }
                            if (AQIcount > 0)
                            {
                                ValidCount++;
                                if (AQIcount <= 100)
                                    StandardCount++;
                            }
                        }
                        if (ValidCount != 0)
                        {
                            StandardRate = DecimalExtension.GetRoundValue(StandardCount / ValidCount * 100, 1);
                        }
                        thisrow["全市"] = StandardRate.ToString();
                    }
                    dtNew.Rows.Add(thisrow);
                }
            }
            return dtNew;

        }
        public DataTable GetMonthAvgAllData(DateTime dateStart, DateTime dateEnd)
        {
            DataTable dt = regionDayAQI.GetMonthAvgAllData(dateStart, dateEnd).Table;  //今年

            DateTime dtSta = dateStart.AddYears(-1);
            DateTime dtEn = dateEnd.AddYears(-1);
            DataTable lastdt = regionDayAQI.GetMonthAvgAllData(dtSta, dtEn).Table;  //去年

            DataTable dtNew = new DataTable();
            for (int i = 1; i <= dateEnd.Month; i++)
                dtNew.Columns.Add(i + "月", typeof(decimal));
            dtNew.Columns.Add("year", typeof(string));
            if (lastdt.Rows.Count > 0)
            {
                DataRow lastrow = dtNew.NewRow();
                lastrow["year"] = dateEnd.AddYears(-1).Year.ToString() + "年";
                DataRow[] drarry = lastdt.Select();
                if (drarry.Length > 0)
                {
                    for (int i = 1; i <= dateEnd.Month; i++)
                        if (drarry[0][i.ToString() + "月"] != DBNull.Value)
                        {
                            lastrow[i.ToString() + "月"] = DecimalExtension.GetRoundValue(decimal.Parse(drarry[0][i.ToString() + "月"].ToString()), 1);
                        }
                }
                dtNew.Rows.Add(lastrow);
            }
            if (dt.Rows.Count > 0)
            {
                DataRow thisrow = dtNew.NewRow();
                thisrow["year"] = dateEnd.Year.ToString() + "年";

                DataRow[] drarry = dt.Select();
                if (drarry.Length > 0)
                {
                    for (int i = 1; i <= dateEnd.Month; i++)
                        if (drarry[0][i.ToString() + "月"] != DBNull.Value)
                        {
                            thisrow[i.ToString() + "月"] = DecimalExtension.GetRoundValue(decimal.Parse(drarry[0][i.ToString() + "月"].ToString()), 1);
                        }
                }
                dtNew.Rows.Add(thisrow);
            }
            return dtNew;
        }
        public DataTable GetLevelAllData(DateTime dateStart, DateTime dateEnd)
        {
            string[] regionguids = { "7e05b94c-bbd4-45c3-919c-42da2e63fd43"     //市区           
			                ,"66d2abd1-ca39-4e39-909f-da872704fbfd"			    //张家港市
                            ,"d7d7a1fe-493a-4b3f-8504-b1850f6d9eff"			    //常熟市
			                ,"57b196ed-5038-4ad0-a035-76faee2d7a98"				//太仓市
			                ,"2e2950cd-dbab-43b3-811d-61bd7569565a"				//昆山市	
			                ,"2fea3cb2-8b95-45e6-8a71-471562c4c89c"	            //吴江区
                                  };
            DataTable dt = new DataTable();   //2015
            dt = regionDayAQI.GetRegionsAllData(regionguids, dateStart, dateEnd).Table;  //2015
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("优", typeof(decimal));
            dtNew.Columns.Add("良", typeof(decimal));
            dtNew.Columns.Add("轻度污染", typeof(decimal));
            dtNew.Columns.Add("中度污染", typeof(decimal));
            dtNew.Columns.Add("重度污染", typeof(decimal));
            dtNew.Columns.Add("严重污染", typeof(decimal));
            DataRow lastrow = dtNew.NewRow();
            string regionguid = "7e05b94c-bbd4-45c3-919c-42da2e63fd43";
            DataRow[] drarry = dt.Select("MonitoringRegionUid='" + regionguid + "'");
            decimal Optimal = 0;
            decimal benign = 0;
            decimal LightPollution = 0;
            decimal ModeratePollution = 0;
            decimal HighPollution = 0;
            decimal SeriousPollution = 0;
            if (drarry.Length > 0)
            {

                if (drarry[0]["ValidCount"] != DBNull.Value && Convert.ToDecimal(drarry[0]["ValidCount"]) != 0)
                {

                    Optimal = DecimalExtension.GetRoundValue(Convert.ToDecimal(drarry[0]["Optimal"]) / Convert.ToDecimal(drarry[0]["ValidCount"]), 3);
                    lastrow["优"] = Optimal;
                    benign = DecimalExtension.GetRoundValue(Convert.ToDecimal(drarry[0]["benign"]) / Convert.ToDecimal(drarry[0]["ValidCount"]), 3);
                    lastrow["良"] = benign;
                    LightPollution = DecimalExtension.GetRoundValue(Convert.ToDecimal(drarry[0]["LightPollution"]) / Convert.ToDecimal(drarry[0]["ValidCount"]), 3);
                    lastrow["轻度污染"] = LightPollution;
                    ModeratePollution = DecimalExtension.GetRoundValue(Convert.ToDecimal(drarry[0]["ModeratePollution"]) / Convert.ToDecimal(drarry[0]["ValidCount"]), 3);
                    lastrow["中度污染"] = ModeratePollution;
                    HighPollution = DecimalExtension.GetRoundValue(Convert.ToDecimal(drarry[0]["HighPollution"]) / Convert.ToDecimal(drarry[0]["ValidCount"]), 3);
                    lastrow["重度污染"] = HighPollution;
                    SeriousPollution = DecimalExtension.GetRoundValue(Convert.ToDecimal(drarry[0]["SeriousPollution"]) / Convert.ToDecimal(drarry[0]["ValidCount"]), 3);
                    lastrow["严重污染"] = SeriousPollution;
                }
                dtNew.Rows.Add(lastrow);
            }
            if (Optimal == 0)
                dtNew.Columns.Remove("优");
            if (benign == 0)
                dtNew.Columns.Remove("良");
            if (LightPollution == 0)
                dtNew.Columns.Remove("轻度污染");
            if (ModeratePollution == 0)
                dtNew.Columns.Remove("中度污染");
            if (HighPollution == 0)
                dtNew.Columns.Remove("重度污染");
            if (SeriousPollution == 0)
                dtNew.Columns.Remove("严重污染");
            return dtNew;
        }
        /// <summary>
        /// 年报
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        public DataView GetRegionsYearAllData(DateTime dateStart, DateTime dateEnd, string year, string target)
        {
            DateTime dtSta = dateStart.AddYears(-1);
            DateTime dtEn = dateEnd.AddYears(-1);
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            string[] regionguids = { "7e05b94c-bbd4-45c3-919c-42da2e63fd43"     //市区           
			                ,"66d2abd1-ca39-4e39-909f-da872704fbfd"			    //张家港市
                            ,"d7d7a1fe-493a-4b3f-8504-b1850f6d9eff"			    //常熟市
			                ,"57b196ed-5038-4ad0-a035-76faee2d7a98"				//太仓市
			                ,"2e2950cd-dbab-43b3-811d-61bd7569565a"				//昆山市	
			                ,"2fea3cb2-8b95-45e6-8a71-471562c4c89c"	            //吴江区
                                  };
            if (regionDayAQI != null)
            {
                DataTable dt = new DataTable();   //2015
                DataTable dtNew = new DataTable();  //2014
                DataTable dtN = new DataTable();  //2013
                dt = regionDayAQI.GetRegionsAllData(regionguids, dateStart, dateEnd).Table;  //2015
                dt.Columns.Add("regionName", typeof(string)).SetOrdinal(0);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["regionName"] = g_DictionaryService.GetCodeDictionaryTextByValue(dt.Rows[i]["MonitoringRegionUid"].ToString());
                }

                dtNew = regionDayAQI.GetRegionsAllData(regionguids, dtSta, dtEn).Table;   //2014
                dtNew.Columns.Add("regionName", typeof(string)).SetOrdinal(0);
                for (int i = 0; i < dtNew.Rows.Count; i++)
                {
                    dtNew.Rows[i]["regionName"] = g_DictionaryService.GetCodeDictionaryTextByValue(dtNew.Rows[i]["MonitoringRegionUid"].ToString());
                }
                if (year != "")
                {
                    dtN = m_DataQueryByDayService.GetRegionYearBaseData(dateStart, dateEnd, year).Table;
                    //dtN = regionDayAQI.GetRegionsAllData(dtSta.AddYears(-1), dtEn.AddYears(-1)).Table;   //2014
                    dtN.Columns.Add("regionName", typeof(string)).SetOrdinal(0);
                    for (int i = 0; i < dtN.Rows.Count; i++)
                    {
                        dtN.Rows[i]["regionName"] = g_DictionaryService.GetCodeDictionaryTextByValue(dtN.Rows[i]["MonitoringRegionUid"].ToString());
                    }
                }

                DataTable dtNewt = new DataTable();  //2015
                DataTable dtAQI = new DataTable();
                DataTable dtNewn = new DataTable();  //2014
                DataTable dtAQIN = new DataTable();
                dtNewt = regionDayAQI.GetRegionsHalfYearData(dateStart, dateEnd).Table;  //2015 全市dabiao
                dtAQI = regionDayAQI.GetRegionsYearAQIData(dateStart, dateEnd).Table;  //2015 全市AQI
                dtNewn = regionDayAQI.GetRegionsHalfYearData(dtSta, dtEn).Table;   //2014  全市
                dtAQIN = regionDayAQI.GetRegionsYearAQIData(dtSta, dtEn).Table;  //2015 全市AQI
                DataTable newdt = new DataTable();  //2014
                newdt.Columns.Add("M1", typeof(string));
                newdt.Columns.Add("M2", typeof(string));
                newdt.Columns.Add("M3", typeof(string));
                newdt.Columns.Add("M4", typeof(string));
                newdt.Columns.Add("M5", typeof(string));
                newdt.Columns.Add("M6", typeof(string));
                newdt.Columns.Add("M7", typeof(string));
                newdt.Columns.Add("M8", typeof(string));
                newdt.Columns.Add("M9", typeof(string));
                newdt.Columns.Add("M10", typeof(string));
                newdt.Columns.Add("M11", typeof(string));
                newdt.Columns.Add("M12", typeof(string));
                newdt.Columns.Add("M13", typeof(string));
                newdt.Columns.Add("M14", typeof(string));
                newdt.Columns.Add("M15", typeof(string));
                newdt.Columns.Add("M16", typeof(string));
                newdt.Columns.Add("M31", typeof(string));
                newdt.Columns.Add("M32", typeof(string));
                newdt.Columns.Add("M33", typeof(string));
                newdt.Columns.Add("M34", typeof(string));
                newdt.Columns.Add("M35", typeof(string));
                newdt.Columns.Add("M36", typeof(string));
                newdt.Columns.Add("M37", typeof(string));
                newdt.Columns.Add("M41", typeof(string));
                newdt.Columns.Add("M42", typeof(string));
                newdt.Columns.Add("M43", typeof(string));
                newdt.Columns.Add("M44", typeof(string));
                newdt.Columns.Add("M45", typeof(string));
                newdt.Columns.Add("M46", typeof(string));
                newdt.Columns.Add("M47", typeof(string));
                newdt.Columns.Add("M51", typeof(string));
                newdt.Columns.Add("M52", typeof(string));
                newdt.Columns.Add("M53", typeof(string));
                newdt.Columns.Add("M54", typeof(string));
                newdt.Columns.Add("M55", typeof(string));
                newdt.Columns.Add("M56", typeof(string));
                newdt.Columns.Add("M57", typeof(string));
                newdt.Columns.Add("M61", typeof(string));
                newdt.Columns.Add("M62", typeof(string));
                newdt.Columns.Add("M63", typeof(string));
                newdt.Columns.Add("M64", typeof(string));
                newdt.Columns.Add("M65", typeof(string));
                newdt.Columns.Add("M66", typeof(string));
                newdt.Columns.Add("M67", typeof(string));
                DataRow newRow = newdt.NewRow();

                decimal AQIValue = 0;  //全市AQI
                decimal AQIValueL = 0; //上年全市AQI
                decimal StandardRate = 0;   //全市达标率
                decimal StandardRateL = 0;   //上年全市达标率
                string bijiao = "";
                //全市AQI均值
                if (dtAQI.Rows.Count > 0)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        string factorCode = dtAQI.Columns[j].ColumnName;
                        int num = 24;
                        if (factorCode == "a05024")
                        {
                            num = 8;
                        }
                        if (dtAQI.Rows[0][j] != DBNull.Value)
                        {
                            decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtAQI.Rows[0][j]), 4);
                            decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factorCode, Con, num), 0);
                            if (AQIValue < temp)
                            {
                                AQIValue = temp;
                            }
                        }
                    }
                }
                //全市达标率
                if (dtNewt.Rows.Count > 0)
                {
                    decimal StandardCount = 0;
                    decimal ValidCount = 0;
                    for (int i = 0; i < dtNewt.Rows.Count; i++)
                    {
                        decimal AQIcount = 0;

                        for (int j = 1; j < 7; j++)
                        {
                            string factorCode = dtNewt.Columns[j].ColumnName;
                            int num = 24;
                            if (factorCode == "a05024")
                            {
                                num = 8;
                            }
                            if (dtNewt.Rows[i][j] != DBNull.Value)
                            {
                                decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNewt.Rows[i][j]), 4);
                                decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factorCode, Con, num), 0);
                                if (AQIcount < temp)
                                {
                                    AQIcount = temp;
                                }
                            }
                        }
                        if (AQIcount > 0)
                        {
                            ValidCount++;
                            if (AQIcount <= 100)
                                StandardCount++;
                        }
                    }
                    if (ValidCount != 0)
                    {
                        StandardRate = DecimalExtension.GetRoundValue(StandardCount / ValidCount * 100, 1);
                    }
                    newRow["M51"] = StandardCount.ToString();
                    newRow["M61"] = StandardRate.ToString() + "%";
                }
                //上年全市AQI均值
                if (dtAQIN.Rows.Count > 0)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        string factorCode = dtAQIN.Columns[j].ColumnName;
                        int num = 24;
                        if (factorCode == "a05024")
                        {
                            num = 8;
                        }
                        if (dtAQIN.Rows[0][j] != DBNull.Value)
                        {
                            decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtAQIN.Rows[0][j]), 4);
                            decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factorCode, Con, num), 0);
                            if (AQIValueL < temp)
                            {
                                AQIValueL = temp;
                            }
                        }
                    }
                }
                if (dtNewn.Rows.Count > 0)
                {
                    decimal StandardCount = 0;
                    decimal ValidCount = 0;
                    for (int i = 0; i < dtNewn.Rows.Count; i++)
                    {
                        decimal AQIcount = 0;

                        for (int j = 1; j < 7; j++)
                        {
                            string factorCode = dtNewn.Columns[j].ColumnName;
                            int num = 24;
                            if (factorCode == "a05024")
                            {
                                num = 8;
                            }
                            if (dtNewn.Rows[i][j] != DBNull.Value)
                            {
                                decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNewn.Rows[i][j]), 4);
                                decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factorCode, Con, num), 0);
                                if (AQIcount < temp)
                                {
                                    AQIcount = temp;
                                }
                            }

                        }
                        if (AQIcount > 0)
                        {
                            ValidCount++;
                            if (AQIcount <= 100)
                                StandardCount++;
                        }
                    }
                    if (ValidCount != 0)
                    {
                        StandardRateL = DecimalExtension.GetRoundValue(StandardCount / ValidCount * 100, 1);
                    }
                    newRow["M31"] = StandardCount.ToString();
                    newRow["M41"] = StandardRateL.ToString() + "%";
                }
                if (StandardRate > StandardRateL)
                {
                    bijiao = "略高于" + dtSta.Year.ToString() + "年。";
                }
                else if (StandardRate < StandardRateL)
                {
                    bijiao = "略低于" + dtSta.Year.ToString() + "年。";
                }
                else if (StandardRate == StandardRateL)
                {
                    bijiao = "与" + dtSta.Year.ToString() + "年持平。";
                }
                newRow["M1"] = StandardRate.ToString() + "%，" + bijiao;

                decimal[] AllStandardRate = { 0 };    //全市各区达标率
                string[] names = { "" };   //全市各区名称
                string[,] PM25 = { };  //各区PM2.5浓度 
                string[,] PM10 = { };  //各区PM10浓度 
                string[,] O3 = { };    //各区O3浓度 
                string[,] SO2 = { };   //各区SO2浓度 
                string[,] NO2 = { };   //各区NO2浓度 
                string[,] CO = { };    //各区CO浓度 
                string[,] O3AQI = { };    //各区O3AQI 
                decimal value = 0;
                decimal value1 = 0;
                int[] days = new int[6];
                string[] factorNames = new string[6];
                decimal SStandardRate = 0;   //市区达标率
                decimal SStandardRateL = 0;   //上年市区达标率
                int SExcessiveCount = 0;  //市区超标天数
                int SExcessiveCountL = 0;  //上年市区超标天数
                decimal PM25max = 0;
                decimal PM25min = 0;
                string PM25name = "";
                decimal PM10max = 0;
                decimal PM10min = 0;
                decimal SO2max = 0;
                decimal SO2min = 0;
                decimal NO2max = 0;
                decimal NO2min = 0;
                decimal O3AQImax = 0;
                decimal O3AQImin = 0;
                decimal COmax = 0;
                decimal COmin = 0;
                decimal PM25C = 0;
                decimal PM25CBase = 0;
                string PM10C = "";
                string NO2C = "";
                string SO2C = "";
                string O3C = "";
                string O3AQIC = "";
                string COC = "";
                if (dtN.Rows.Count > 0)
                {
                    for (int i = 0; i < dtN.Rows.Count; i++)
                    {
                        if (dtN.Rows[i]["MonitoringRegionUid"].ToString() == "7e05b94c-bbd4-45c3-919c-42da2e63fd43")
                        {
                            if (dtN.Rows[i]["PM25_C"] != DBNull.Value)
                            {
                                PM25CBase = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtN.Rows[i]["PM25_C"]), 0);
                            }
                        }
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    AllStandardRate = new decimal[dt.Rows.Count];   //全市各区达标率
                    names = new string[dt.Rows.Count];   //全市各区名称
                    PM25 = new string[dt.Rows.Count, 2];  //各区PM2.5浓度 
                    PM10 = new string[dt.Rows.Count, 2];  //各区PM10浓度 
                    O3 = new string[dt.Rows.Count, 2];    //各区O3浓度 
                    SO2 = new string[dt.Rows.Count, 2];   //各区SO2浓度 
                    NO2 = new string[dt.Rows.Count, 2];   //各区NO2浓度 
                    CO = new string[dt.Rows.Count, 2];    //各区CO浓度 
                    O3AQI = new string[dt.Rows.Count, 2];    //各区O3AQI 
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["regionName"].ToString() == "苏州市区")
                            names[i] = "市区";
                        if (dt.Rows[i]["regionName"].ToString() == "太仓市")
                            names[i] = "太仓";
                        if (dt.Rows[i]["regionName"].ToString() == "吴江区")
                            names[i] = "吴江";
                        if (dt.Rows[i]["regionName"].ToString() == "昆山市")
                            names[i] = "昆山";
                        if (dt.Rows[i]["regionName"].ToString() == "常熟市")
                            names[i] = "常熟";
                        if (dt.Rows[i]["regionName"].ToString() == "张家港市")
                            names[i] = "张家港";
                        PM25[i, 0] = names[i];
                        PM25[i, 1] = dt.Rows[i]["a34004"] != DBNull.Value ? DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["a34004"]) * 1000, 0).ToString() : "";
                        PM10[i, 0] = names[i];
                        PM10[i, 1] = dt.Rows[i]["a34002"] != DBNull.Value ? DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["a34002"]) * 1000, 0).ToString() : "";
                        O3[i, 0] = names[i];
                        O3[i, 1] = dt.Rows[i]["a05024"] != DBNull.Value ? DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["a05024"]) * 1000, 0).ToString() : "";
                        SO2[i, 0] = names[i];
                        SO2[i, 1] = dt.Rows[i]["a21026"] != DBNull.Value ? DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["a21026"]) * 1000, 0).ToString() : "";
                        NO2[i, 0] = names[i];
                        NO2[i, 1] = dt.Rows[i]["a21004"] != DBNull.Value ? DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["a21004"]) * 1000, 0).ToString() : "";
                        CO[i, 0] = names[i];
                        CO[i, 1] = dt.Rows[i]["a21005"] != DBNull.Value ? DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["a21005"]), 1).ToString() : "";
                        if (dt.Rows[i]["O3AQICount"].IsNotNullOrDBNull() && Convert.ToDecimal(dt.Rows[i]["O3AQICount"]) != 0)
                        {
                            if (dt.Rows[i]["O3AQI"].IsNotNullOrDBNull())
                            {
                                O3AQI[i, 0] = names[i];
                                O3AQI[i, 1] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["O3AQI"]) / Convert.ToDecimal(dt.Rows[i]["O3AQICount"]) * 100, 1).ToString();
                            }
                        }
                        if (dt.Rows[i]["ValidCount"].IsNotNullOrDBNull() && Convert.ToDecimal(dt.Rows[i]["ValidCount"]) != 0)
                        {
                            if (dt.Rows[i]["StandardCount"].IsNotNullOrDBNull())
                            {
                                AllStandardRate[i] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["StandardCount"]) / Convert.ToDecimal(dt.Rows[i]["ValidCount"]) * 100, 1);
                                if (dt.Rows[i]["MonitoringRegionUid"].ToString() == "7e05b94c-bbd4-45c3-919c-42da2e63fd43")
                                {
                                    PM25C = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["a34004"]) * 1000, 0);
                                    PM10C = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["a34002"]) * 1000, 0).ToString();
                                    NO2C = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["a05024"]) * 1000, 0).ToString();
                                    SO2C = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["a21026"]) * 1000, 0).ToString();
                                    O3C = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["a21004"]) * 1000, 0).ToString();
                                    COC = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["a21005"]), 1).ToString();
                                    if (dt.Rows[i]["O3AQICount"].IsNotNullOrDBNull() && Convert.ToDecimal(dt.Rows[i]["O3AQICount"]) != 0)
                                    {
                                        if (dt.Rows[i]["O3AQI"].IsNotNullOrDBNull())
                                        {
                                            O3AQIC = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["O3AQI"]) / Convert.ToDecimal(dt.Rows[i]["O3AQICount"]) * 100, 1).ToString();
                                        }
                                    }
                                    newRow["M3"] = AllStandardRate[i].ToString() + "%，";  //市区达标率
                                    newRow["M57"] = Convert.ToInt32(dt.Rows[i]["StandardCount"]).ToString();
                                    newRow["M67"] = AllStandardRate[i].ToString() + "%";
                                    SExcessiveCount = Convert.ToInt32(dt.Rows[i]["ExcessiveCount"]);
                                    newRow["M5"] = SExcessiveCount.ToString() + "天，";   //市区超标天数
                                    SStandardRate = AllStandardRate[i];

                                    for (int j = 2; j < 8; j++)
                                    {
                                        string factorCode = dt.Columns[j].ColumnName;
                                        int num = 24;
                                        if (factorCode == "a05024")
                                        {
                                            num = 8;
                                        }
                                        decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[0][j]), 4);
                                        decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factorCode, Con, num), 0);
                                        if (value < temp)
                                        {
                                            value = temp;
                                        }
                                    }
                                    //if (dt.Rows[i]["AQIValue"].IsNotNullOrDBNull() && dt.Rows[i]["AQIValue"].ToString() != "")
                                    //{
                                    //    value = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[i]["AQIValue"]), 0);  //市区AQI均值
                                    //}
                                    days[0] = Convert.ToInt32(dt.Rows[i]["PM25"]);
                                    days[1] = Convert.ToInt32(dt.Rows[i]["PM10"]);
                                    days[2] = Convert.ToInt32(dt.Rows[i]["O3"]);
                                    days[3] = Convert.ToInt32(dt.Rows[i]["SO2"]);
                                    days[4] = Convert.ToInt32(dt.Rows[i]["NO2"]);
                                    days[5] = Convert.ToInt32(dt.Rows[i]["CO"]);
                                    factorNames[0] = "细颗粒物";
                                    factorNames[1] = "可吸入颗粒物";
                                    factorNames[2] = "臭氧";
                                    factorNames[3] = "二氧化硫";
                                    factorNames[4] = "二氧化氮";
                                    factorNames[5] = "一氧化碳";
                                }
                                //张家港
                                if (dt.Rows[i]["MonitoringRegionUid"].ToString() == "66d2abd1-ca39-4e39-909f-da872704fbfd")
                                {
                                    newRow["M52"] = Convert.ToInt32(dt.Rows[i]["StandardCount"]).ToString();
                                    newRow["M62"] = AllStandardRate[i].ToString() + "%";
                                }
                                //常熟
                                if (dt.Rows[i]["MonitoringRegionUid"].ToString() == "d7d7a1fe-493a-4b3f-8504-b1850f6d9eff")
                                {
                                    newRow["M53"] = Convert.ToInt32(dt.Rows[i]["StandardCount"]).ToString();
                                    newRow["M63"] = AllStandardRate[i].ToString() + "%";
                                }
                                //太仓
                                if (dt.Rows[i]["MonitoringRegionUid"].ToString() == "57b196ed-5038-4ad0-a035-76faee2d7a98")
                                {
                                    newRow["M54"] = Convert.ToInt32(dt.Rows[i]["StandardCount"]).ToString();
                                    newRow["M64"] = AllStandardRate[i].ToString() + "%";
                                }
                                //昆山
                                if (dt.Rows[i]["MonitoringRegionUid"].ToString() == "2e2950cd-dbab-43b3-811d-61bd7569565a")
                                {
                                    newRow["M55"] = Convert.ToInt32(dt.Rows[i]["StandardCount"]).ToString();
                                    newRow["M65"] = AllStandardRate[i].ToString() + "%";
                                }
                                //吴江
                                if (dt.Rows[i]["MonitoringRegionUid"].ToString() == "2fea3cb2-8b95-45e6-8a71-471562c4c89c")
                                {
                                    newRow["M56"] = Convert.ToInt32(dt.Rows[i]["StandardCount"]).ToString();
                                    newRow["M66"] = AllStandardRate[i].ToString() + "%";
                                }
                            }
                        }
                    }
                    for (int j = 0; j < AllStandardRate.Length - 1; j++)
                    {
                        for (int i = j + 1; i < AllStandardRate.Length; i++)
                        {
                            if (AllStandardRate[j] > AllStandardRate[i])
                            {
                                decimal t = AllStandardRate[j];
                                AllStandardRate[j] = AllStandardRate[i];
                                AllStandardRate[i] = t;
                                string name = names[j];
                                names[j] = names[i];
                                names[i] = name;

                            }
                        }
                    }
                    string strName = "";
                    for (int i = names.Length - 1; i >= 0; i--)
                    {
                        if (i != 0)
                            strName += (names[i] + "、");
                        else
                        {
                            strName = strName.Trim('、');
                            strName = strName + "和" + names[i];
                        }
                    }
                    strName = strName.Trim('、');
                    newRow["M2"] = "各地达标天数比例介于" + AllStandardRate[0].ToString() + "%~" + AllStandardRate[AllStandardRate.Length - 1].ToString() + "%之间；达标天数比例由高到低依次为" + strName + "。" +
                        "全市空气质量指数（AQI）年均值为" + AQIValue + "。";

                    for (int j = 0; j < 6 - 1; j++)
                    {
                        for (int i = j + 1; i < 6; i++)
                        {
                            if (days[j] < days[i])
                            {
                                int temp = days[j];
                                days[j] = days[i];
                                days[i] = temp;
                                string name = factorNames[j];
                                factorNames[j] = factorNames[i];
                                factorNames[i] = name;
                            }
                        }
                    }
                    string wuRanName = "";  //首要污染物
                    string wuRanDay = "";  //首要污染物天数

                    for (int i = 0; i < 6; i++)
                    {
                        if (days[i] != 0)
                        {
                            wuRanName += (factorNames[i] + "、");
                            wuRanDay += (days[i].ToString() + "天" + "、");
                        }

                    }
                    if (wuRanName != "" && wuRanDay != "")
                    {
                        wuRanName = wuRanName.Trim('、');
                        wuRanDay = wuRanDay.Trim('、');
                        newRow["M7"] = wuRanName + "为首要污染物的天数分别为" + wuRanDay + "。";
                    }
                    newRow["M8"] = "市区空气质量指数（AQI）年均值为" + value + "。";
                    if (PM25.Length / 2 > 0)
                    {
                        PM25max = Convert.ToInt32(PM25[0, 1]);
                        PM25min = Convert.ToInt32(PM25[0, 1]);
                        for (int i = 0; i < PM25.Length / 2; i++)
                        {
                            if (PM25max < Convert.ToInt32(PM25[i, 1]))
                                PM25max = Convert.ToInt32(PM25[i, 1]);
                            if (PM25min > Convert.ToInt32(PM25[i, 1]))
                            {
                                PM25min = Convert.ToInt32(PM25[i, 1]);
                                PM25name = PM25[i, 0];
                            }

                        }
                    }

                    if (PM10.Length / 2 > 0)
                    {
                        PM10max = Convert.ToInt32(PM10[0, 1]);
                        PM10min = Convert.ToInt32(PM10[0, 1]);
                        for (int i = 0; i < PM10.Length / 2; i++)
                        {
                            if (PM10max < Convert.ToInt32(PM10[i, 1]))
                                PM10max = Convert.ToInt32(PM10[i, 1]);
                            if (PM10min > Convert.ToInt32(PM10[i, 1]))
                                PM10min = Convert.ToInt32(PM10[i, 1]);
                        }
                    }

                    if (SO2.Length / 2 > 0)
                    {
                        SO2max = Convert.ToInt32(SO2[0, 1]);
                        SO2min = Convert.ToInt32(SO2[0, 1]);
                        for (int i = 0; i < SO2.Length / 2; i++)
                        {
                            if (SO2max < Convert.ToInt32(SO2[i, 1]))
                                SO2max = Convert.ToInt32(SO2[i, 1]);
                            if (SO2min > Convert.ToInt32(SO2[i, 1]))
                                SO2min = Convert.ToInt32(SO2[i, 1]);
                        }
                    }

                    if (NO2.Length / 2 > 0)
                    {
                        NO2max = Convert.ToInt32(NO2[0, 1]);
                        NO2min = Convert.ToInt32(NO2[0, 1]);
                        for (int i = 0; i < NO2.Length / 2; i++)
                        {
                            if (NO2max < Convert.ToInt32(NO2[i, 1]))
                                NO2max = Convert.ToInt32(NO2[i, 1]);
                            if (NO2min > Convert.ToInt32(NO2[i, 1]))
                                NO2min = Convert.ToInt32(NO2[i, 1]);
                        }
                    }

                    if (CO.Length / 2 > 0)
                    {
                        COmax = Convert.ToDecimal(CO[0, 1]);
                        COmin = Convert.ToDecimal(CO[0, 1]);
                        for (int i = 0; i < CO.Length / 2; i++)
                        {
                            if (COmax < Convert.ToDecimal(CO[i, 1]))
                                COmax = Convert.ToDecimal(CO[i, 1]);
                            if (COmin > Convert.ToDecimal(CO[i, 1]))
                                COmin = Convert.ToDecimal(CO[i, 1]);
                        }
                    }

                    if (O3AQI.Length / 2 > 0)
                    {
                        O3AQImax = Convert.ToDecimal(O3AQI[0, 1]);
                        O3AQImin = Convert.ToDecimal(O3AQI[0, 1]);
                        for (int i = 0; i < O3AQI.Length / 2; i++)
                        {
                            if (O3AQImax < Convert.ToDecimal(O3AQI[i, 1]))
                                O3AQImax = Convert.ToDecimal(O3AQI[i, 1]);
                            if (O3AQImin > Convert.ToDecimal(O3AQI[i, 1]))
                                O3AQImin = Convert.ToDecimal(O3AQI[i, 1]);
                        }
                    }
                }    //2015年

                if (dtNew.Rows.Count > 0)
                {
                    string[] namesNew = new string[dtNew.Rows.Count];   //全市各区名称
                    string[,] PM25New = new string[dtNew.Rows.Count, 2];  //各区PM2.5浓度 
                    string[,] PM10New = new string[dtNew.Rows.Count, 2];  //各区PM10浓度 
                    string[,] O3New = new string[dtNew.Rows.Count, 2];    //各区O3浓度 
                    string[,] O3AQINew = new string[dtNew.Rows.Count, 2];    //各区O3AQI 
                    string[,] SO2New = new string[dtNew.Rows.Count, 2];   //各区SO2浓度 
                    string[,] NO2New = new string[dtNew.Rows.Count, 2];   //各区NO2浓度 
                    string[,] CONew = new string[dtNew.Rows.Count, 2];    //各区CO浓度 
                    for (int i = 0; i < dtNew.Rows.Count; i++)
                    {
                        if (dtNew.Rows[i]["regionName"].ToString() == "苏州市区")
                            namesNew[i] = "市区";
                        if (dtNew.Rows[i]["regionName"].ToString() == "太仓市")
                            namesNew[i] = "太仓";
                        if (dtNew.Rows[i]["regionName"].ToString() == "吴江区")
                            namesNew[i] = "吴江";
                        if (dtNew.Rows[i]["regionName"].ToString() == "昆山市")
                            namesNew[i] = "昆山";
                        if (dtNew.Rows[i]["regionName"].ToString() == "常熟市")
                            namesNew[i] = "常熟";
                        if (dtNew.Rows[i]["regionName"].ToString() == "张家港市")
                            namesNew[i] = "张家港";
                        PM25New[i, 0] = namesNew[i];
                        PM25New[i, 1] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[i]["a34004"]) * 1000, 0).ToString();
                        PM10New[i, 0] = namesNew[i];
                        PM10New[i, 1] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[i]["a34002"]) * 1000, 0).ToString();
                        O3New[i, 0] = namesNew[i];
                        O3New[i, 1] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[i]["a05024"]) * 1000, 0).ToString();
                        SO2New[i, 0] = namesNew[i];
                        SO2New[i, 1] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[i]["a21026"]) * 1000, 0).ToString();
                        NO2New[i, 0] = namesNew[i];
                        NO2New[i, 1] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[i]["a21004"]) * 1000, 0).ToString();
                        CONew[i, 0] = namesNew[i];
                        CONew[i, 1] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[i]["a21005"]), 1).ToString();
                        if (dtNew.Rows[i]["O3AQICount"].IsNotNullOrDBNull() && Convert.ToDecimal(dtNew.Rows[i]["O3AQICount"]) != 0)
                        {
                            if (dtNew.Rows[i]["O3AQI"].IsNotNullOrDBNull())
                            {
                                O3AQINew[i, 0] = namesNew[i];
                                O3AQINew[i, 1] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[i]["O3AQI"]) / Convert.ToDecimal(dtNew.Rows[i]["O3AQICount"]) * 100, 1).ToString();
                            }
                        }
                        if (dtNew.Rows[i]["MonitoringRegionUid"].ToString() == "7e05b94c-bbd4-45c3-919c-42da2e63fd43")
                        {
                            if (dtNew.Rows[i]["ExcessiveCount"].IsNotNullOrDBNull())
                                SExcessiveCountL = Convert.ToInt32(dtNew.Rows[i]["ExcessiveCount"]);

                            for (int j = 2; j < 8; j++)
                            {
                                string factorCode = dtNew.Columns[j].ColumnName;
                                int num = 24;
                                if (factorCode == "a05024")
                                {
                                    num = 8;
                                }
                                decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[0][j]), 4);
                                decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factorCode, Con, num), 0);
                                if (value1 < temp)
                                {
                                    value1 = temp;
                                }
                            }
                        }
                        if (dtNew.Rows[i]["ValidCount"].IsNotNullOrDBNull() && Convert.ToDecimal(dtNew.Rows[i]["ValidCount"]) != 0)
                        {
                            if (dtNew.Rows[i]["StandardCount"].IsNotNullOrDBNull())
                            {
                                //市区
                                if (dtNew.Rows[i]["MonitoringRegionUid"].ToString() == "7e05b94c-bbd4-45c3-919c-42da2e63fd43")
                                {
                                    SStandardRateL = DecimalExtension.GetRoundValue((Convert.ToDecimal(dtNew.Rows[i]["StandardCount"]) / Convert.ToDecimal(dtNew.Rows[i]["ValidCount"])) * 100, 1);
                                    newRow["M37"] = Convert.ToInt32(dtNew.Rows[i]["StandardCount"]).ToString();
                                    newRow["M47"] = SStandardRateL.ToString() + "%";
                                }
                                //张家港
                                if (dtNew.Rows[i]["MonitoringRegionUid"].ToString() == "66d2abd1-ca39-4e39-909f-da872704fbfd")
                                {
                                    newRow["M32"] = Convert.ToInt32(dtNew.Rows[i]["StandardCount"]).ToString();
                                    newRow["M42"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[i]["StandardCount"]) / Convert.ToDecimal(dtNew.Rows[i]["ValidCount"]) * 100, 1).ToString() + "%";
                                }
                                //常熟
                                if (dtNew.Rows[i]["MonitoringRegionUid"].ToString() == "d7d7a1fe-493a-4b3f-8504-b1850f6d9eff")
                                {
                                    newRow["M33"] = Convert.ToInt32(dtNew.Rows[i]["StandardCount"]).ToString();
                                    newRow["M43"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[i]["StandardCount"]) / Convert.ToDecimal(dtNew.Rows[i]["ValidCount"]) * 100, 1).ToString() + "%";
                                }
                                //太仓
                                if (dtNew.Rows[i]["MonitoringRegionUid"].ToString() == "57b196ed-5038-4ad0-a035-76faee2d7a98")
                                {
                                    newRow["M34"] = Convert.ToInt32(dtNew.Rows[i]["StandardCount"]).ToString();
                                    newRow["M44"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[i]["StandardCount"]) / Convert.ToDecimal(dtNew.Rows[i]["ValidCount"]) * 100, 1).ToString() + "%";
                                }
                                //昆山
                                if (dtNew.Rows[i]["MonitoringRegionUid"].ToString() == "2e2950cd-dbab-43b3-811d-61bd7569565a")
                                {
                                    newRow["M35"] = Convert.ToInt32(dtNew.Rows[i]["StandardCount"]).ToString();
                                    newRow["M45"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[i]["StandardCount"]) / Convert.ToDecimal(dtNew.Rows[i]["ValidCount"]) * 100, 1).ToString() + "%";
                                }
                                //吴江
                                if (dtNew.Rows[i]["MonitoringRegionUid"].ToString() == "2fea3cb2-8b95-45e6-8a71-471562c4c89c")
                                {
                                    newRow["M36"] = Convert.ToInt32(dtNew.Rows[i]["StandardCount"]).ToString();
                                    newRow["M46"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[i]["StandardCount"]) / Convert.ToDecimal(dtNew.Rows[i]["ValidCount"]) * 100, 1).ToString() + "%";
                                }
                            }
                        }
                    }
                    //市区达标率比较结果   
                    if (SStandardRate > SStandardRateL)
                    {
                        newRow["M4"] = "与上年相比上升" + (SStandardRate - SStandardRateL).ToString() + "个百分点；";
                        newRow["M16"] = "同比上升" + (SStandardRate - SStandardRateL).ToString() + "个百分点。";
                    }
                    else if (SStandardRate < SStandardRateL)
                    {
                        newRow["M4"] = "与上年相比下降" + (SStandardRateL - SStandardRate).ToString() + "个百分点；";
                        newRow["M16"] = "同比下降" + (SStandardRateL - SStandardRate).ToString() + "个百分点。";
                    }
                    else if (SStandardRate == SStandardRateL)
                    {
                        newRow["M4"] = "与上年相比持平；";
                        newRow["M16"] = "同比持平。";
                    }
                    //市区超标天数比较结果   
                    if (SExcessiveCount > SExcessiveCountL)
                    {
                        newRow["M6"] = "与上年相比增加了" + (SExcessiveCount - SExcessiveCountL).ToString() + "天，";
                    }
                    else if (SExcessiveCount < SExcessiveCountL)
                    {
                        newRow["M6"] = "与上年相比减少了" + (SExcessiveCountL - SExcessiveCount).ToString() + "天，";
                    }
                    else if (SExcessiveCount == SExcessiveCountL)
                    {
                        newRow["M6"] = "与上年相比持平";
                    }


                    string PM25drop = "";
                    string PM25down = "";
                    string PM25ping = "";
                    for (int i = 0; i < PM25.Length / 2; i++)
                    {
                        for (int j = 0; j < PM25New.Length / 2; j++)
                            if (PM25[i, 0] == PM25New[j, 0])
                            {
                                if (Convert.ToInt32(PM25[i, 1]) < Convert.ToInt32(PM25New[j, 1]))
                                    PM25down += (PM25[i, 0] + "、");
                                else if (Convert.ToInt32(PM25[i, 1]) > Convert.ToInt32(PM25New[j, 1]))
                                    PM25drop += (PM25[i, 0] + "、");
                                else
                                    PM25ping += (PM25[i, 0] + "、");
                            }
                    }
                    if (PM25drop != "")
                    {
                        if (PM25ping != "" || PM25down != "")
                        {
                            PM25drop = PM25drop.Trim('、');
                            PM25drop = PM25drop + "细颗粒物年均浓度有所上升，";
                        }
                        else
                            PM25drop = "全市各地细颗粒物年均浓度均有所上升。";
                    }
                    if (PM25ping != "")
                    {
                        if (PM25drop != "" || PM25down != "")
                        {
                            PM25ping = PM25ping.Trim('、');
                            PM25ping = PM25ping + "细颗粒物年均浓度持平，";
                        }
                        else
                        {
                            PM25ping = "全市各地细颗粒物年均浓度均持平。";
                        }
                    }

                    if (PM25down != "")
                    {
                        if (PM25drop != "" || PM25ping != "")
                        {
                            PM25down = PM25down.Trim('、');
                            PM25down = "其余各地细颗粒物年均浓度均有所下降。";
                        }
                        else
                            PM25down = "全市各地细颗粒物年均浓度均有所下降。";
                    }
                    string PM25Str = "";
                    if (PM25min > 35)
                        PM25Str = "均超过标准限值。";
                    else if (PM25max <= 35)
                        PM25Str = "均达到标准要求。";
                    newRow["M9"] = PM25min.ToString() + "~" + PM25max.ToString() + "微克/立方米之间，" + PM25Str + "与上年相比，" + PM25drop + PM25ping + PM25down;
                    string bilistr = "";
                    decimal bili = 0;
                    if (PM25CBase != 0)
                    {
                        bili = DecimalExtension.GetRoundValue((PM25CBase - PM25C) / PM25CBase * 100, 1);
                        if (bili > 0)
                            bilistr = "上升" + bili.ToString() + "%，未完成了" + target + "%的年度考核目标。";
                        else if (bili < 0 && bili <= -Convert.ToInt32(target))
                            bilistr = "下降" + bili.ToString() + "%，完成了" + target + "%的年度考核目标。";
                        else if (bili < 0 && bili > -Convert.ToInt32(target))
                            bilistr = "下降" + bili.ToString() + "%，未完成了" + target + "%的年度考核目标。";
                        else
                            bilistr = "持平" + "，未完成了" + target + "%的年度考核目标。";
                    }

                    newRow["M10"] = "市区细颗粒物年均浓度为" + PM25C.ToString() + "微克/立方米，与" + year + "年相比" + bilistr;
                    string PM10drop = "";
                    string PM10down = "";
                    string PM10ping = "";
                    for (int i = 0; i < PM10.Length / 2; i++)
                    {
                        for (int j = 0; j < PM10New.Length / 2; j++)
                            if (PM10[i, 0] == PM10New[j, 0])
                            {
                                if (Convert.ToInt32(PM10[i, 1]) < Convert.ToInt32(PM10New[j, 1]))
                                    PM10down += (PM10[i, 0] + "、");
                                else if (Convert.ToInt32(PM10[i, 1]) > Convert.ToInt32(PM10New[j, 1]))
                                    PM10drop += (PM10[i, 0] + "、");
                                else
                                    PM10ping += (PM10[i, 0] + "、");
                            }
                    }
                    if (PM10drop != "")
                    {
                        if (PM10ping != "" || PM10down != "")
                        {
                            PM10drop = PM10drop.Trim('、');
                            PM10drop = PM10drop + "可吸入颗粒物年均浓度有所上升，";
                        }
                        else
                            PM10drop = "全市各地可吸入颗粒物年均浓度均有所上升。";
                    }
                    if (PM10ping != "")
                    {
                        if (PM10drop != "" || PM10down != "")
                        {
                            PM10ping = PM10ping.Trim('、');
                            PM10ping = PM10ping + "可吸入颗粒物年均浓度持平，";
                        }
                        else
                            PM10ping = "全市各地可吸入颗粒物年均浓度均持平。";
                    }

                    if (PM10down != "")
                    {
                        if (PM10drop != "" || PM10ping != "")
                        {
                            PM10down = PM10down.Trim('、');
                            PM10down = "其余各地均有所下降。";
                        }
                        else
                        {
                            PM10down = "全市各地可吸入颗粒物年均浓度均有所下降。";
                        }
                    }
                    string PM10Str = "";
                    if (PM10min > 70)
                        PM10Str = "均超过标准限值。";
                    else if (PM10max <= 70)
                        PM10Str = "均达到标准要求。";
                    newRow["M11"] = PM10min.ToString() + "~" + PM10max.ToString() + "微克/立方米之间，" + PM10Str + "与上年相比，" + PM10drop + PM10ping + PM10down +
                        "市区可吸入颗粒物年均浓度为" + PM10C.ToString() + "微克/立方米。";

                    string NO2drop = "";
                    string NO2down = "";
                    string NO2ping = "";
                    for (int i = 0; i < NO2.Length / 2; i++)
                    {
                        for (int j = 0; j < NO2New.Length / 2; j++)
                            if (NO2[i, 0] == NO2New[j, 0])
                            {
                                if (Convert.ToInt32(NO2[i, 1]) < Convert.ToInt32(NO2New[j, 1]))
                                    NO2down += (NO2[i, 0] + "、");
                                else if (Convert.ToInt32(NO2[i, 1]) > Convert.ToInt32(NO2New[j, 1]))
                                    NO2drop += (NO2[i, 0] + "、");
                                else
                                    NO2ping += (NO2[i, 0] + "、");
                            }
                    }
                    if (NO2down != "")
                    {
                        if (NO2ping != "" || NO2drop != "")
                        {
                            NO2down = NO2down.Trim('、');
                            NO2down = NO2down + "二氧化氮年均浓度有所下降，";
                        }
                        else
                            NO2down = "全市各地二氧化氮年均浓度均有所下降。";
                    }
                    if (NO2ping != "")
                    {
                        if (NO2down != "" || NO2drop != "")
                        {
                            NO2ping = NO2ping.Trim('、');
                            NO2ping = NO2ping + "二氧化氮年均浓度持平，";
                        }
                        else
                            NO2ping = "全市各地二氧化氮年均浓度均持平。";
                    }
                    if (NO2drop != "")
                    {
                        if (NO2down != "" || NO2ping != "")
                        {
                            NO2drop = NO2drop.Trim('、');
                            NO2drop = "其余各地有所上升。";
                        }
                        else
                        {
                            NO2drop = "全市各地二氧化氮年均浓度均有所上升。";
                        }
                    }
                    string NO2Str = "";
                    if (NO2min > 40)
                        NO2Str = "均超过标准限值。";
                    else if (NO2max <= 40)
                        NO2Str = "均达到标准要求。";
                    newRow["M12"] = NO2min.ToString() + "~" + NO2max.ToString() + "微克/立方米之间，" + NO2Str + "与上年相比，" + NO2down + NO2ping + NO2drop +
                        "市区二氧化氮年均浓度为" + NO2C.ToString() + "微克/立方米。";

                    string SO2drop = "";
                    string SO2down = "";
                    string SO2ping = "";
                    for (int i = 0; i < SO2.Length / 2; i++)
                    {
                        for (int j = 0; j < SO2New.Length / 2; j++)
                            if (SO2[i, 0] == SO2New[j, 0])
                            {
                                if (Convert.ToInt32(SO2[i, 1]) < Convert.ToInt32(SO2New[j, 1]))
                                    SO2down += (SO2[i, 0] + "、");
                                else if (Convert.ToInt32(SO2[i, 1]) > Convert.ToInt32(SO2New[j, 1]))
                                    SO2drop += (SO2[i, 0] + "、");
                                else
                                    SO2ping += (SO2[i, 0] + "、");
                            }
                    }
                    if (SO2down != "")
                    {
                        if (SO2ping != "" || SO2drop != "")
                        {
                            SO2down = SO2down.Trim('、');
                            SO2down = SO2down + "二氧化硫年均浓度有不同程度降低，";
                        }
                        else
                            SO2down = "全市各地二氧化硫年均浓度均有不同程度降低。";
                    }
                    if (SO2ping != "")
                    {
                        if (SO2down != "" || SO2drop != "")
                        {
                            SO2ping = SO2ping.Trim('、');
                            SO2ping = SO2ping + "二氧化硫年均浓度持平，";
                        }
                        else
                            SO2ping = "全市各地二氧化硫年均浓度均持平。";
                    }
                    if (SO2drop != "")
                    {
                        if (SO2down != "" || SO2ping != "")
                        {
                            SO2drop = SO2drop.Trim('、');
                            SO2drop = "其余各地二氧化硫年均浓度有所上升。";
                        }
                        else
                            SO2drop = "全市各地二氧化硫年均浓度均有所上升。";
                    }
                    string SO2Str = "";
                    if (SO2min > 60)
                        SO2Str = "均超过标准限值。";
                    else if (SO2max <= 60)
                        SO2Str = "均达到标准要求。";
                    newRow["M13"] = SO2min.ToString() + "~" + SO2max.ToString() + "微克/立方米之间，" + SO2Str + "与上年相比，" + SO2down + SO2ping + SO2drop +
                        "市区二氧化硫年均浓度为" + SO2C + "微克/立方米。";


                    string O3AQIdrop = "";
                    string O3AQIdown = "";
                    string O3AQIping = "";
                    string O3AQIChao = "";
                    for (int i = 0; i < O3AQI.Length / 2; i++)
                    {
                        for (int j = 0; j < O3AQINew.Length / 2; j++)
                            if (O3AQI[i, 0] == O3AQINew[j, 0])
                            {
                                if (Convert.ToDecimal(O3AQI[i, 1]) < Convert.ToDecimal(O3AQINew[j, 1]))
                                    O3AQIdown += (O3AQI[i, 0] + "、");
                                else if (Convert.ToDecimal(O3AQI[i, 1]) > Convert.ToDecimal(O3AQINew[j, 1]))
                                    O3AQIdrop += (O3AQI[i, 0] + "、");
                                else if (Convert.ToDecimal(O3AQI[i, 1]) != 0)
                                    O3AQIping += (O3AQI[i, 0] + "、");
                                else
                                    O3AQIChao += (O3AQI[i, 0] + "、");
                            }
                    }
                    if (O3AQIdown != "")
                    {
                        if (O3AQIping != "" || O3AQIdrop != "")
                        {
                            O3AQIdown = O3AQIdown.Trim('、');
                            O3AQIdown = O3AQIdown + "臭氧日最大8小时平均浓度超标率有所降低，";
                        }
                        else
                            O3AQIdown = "全市各地臭氧日最大8小时平均浓度超标率均有所降低。";
                    }
                    if (O3AQIping != "")
                    {
                        if (O3AQIdown != "" || O3AQIdrop != "")
                        {
                            O3AQIping = O3AQIping.Trim('、');
                            O3AQIping = O3AQIping + "臭氧日最大8小时平均浓度超标率持平，";
                        }
                        else
                            O3AQIping = "全市各地臭氧日最大8小时平均浓度超标率均持平。";
                    }
                    if (O3AQIdrop != "")
                    {
                        if (O3AQIdown != "" || O3AQIping != "")
                        {

                            O3AQIdrop = "其余各地均有所上升。";
                        }
                        else if (O3AQIChao != "")
                        {
                            O3AQIdrop = O3AQIdrop.Trim('、');
                            O3AQIdrop = O3AQIdrop + "臭氧日最大8小时平均浓度超标率均有所上升。";
                        }
                        else
                            O3AQIdrop = "全市各地臭氧日最大8小时平均浓度超标率均有所上升。";
                    }
                    string O3Str = "";
                    if (O3AQIChao != "")
                    {
                        O3AQIChao = O3AQIChao.Trim('、');
                        O3Str = "除" + O3AQIChao + "臭氧日最大8小时平均浓度未超标外，其余各地均出现超标现象，超标率分布在";
                    }
                    else
                        O3Str = "全市各地臭氧日最大8小时平均浓度均出现超标现象，超标率分布在";
                    newRow["M14"] = O3Str + O3AQImin.ToString() + "%~" + O3AQImax.ToString() + "%之间。与上年相比，" + O3AQIdown + O3AQIping + O3AQIdrop + "市区日最大8小时平均浓度超标率为" + O3AQIC + "%。";

                    string COdrop = "";
                    string COdown = "";
                    string COping = "";
                    for (int i = 0; i < CO.Length / 2; i++)
                    {
                        for (int j = 0; j < CONew.Length / 2; j++)
                            if (CO[i, 0] == CONew[j, 0])
                            {
                                if (Convert.ToDecimal(CO[i, 1]) < Convert.ToDecimal(CONew[j, 1]))
                                    COdown += (CO[i, 0] + "、");
                                else if (Convert.ToDecimal(CO[i, 1]) > Convert.ToDecimal(CONew[j, 1]))
                                    COdrop += (CO[i, 0] + "、");
                                else
                                    COping += (CO[i, 0] + "、");
                            }
                    }
                    if (COdown != "")
                    {
                        if (COping != "" || COdrop != "")
                        {
                            COdown = COdown.Trim('、');
                            COdown = COdown + "一氧化碳年均浓度有所下降，";
                        }
                        else
                        {
                            COdown = "全市各地一氧化碳年均浓度均有所下降。";
                        }
                    }
                    if (COping != "")
                    {
                        if (COdown != "" || COdrop != "")
                        {
                            COping = COping.Trim('、');
                            COping = COping + "一氧化碳年均浓度持平，";
                        }
                        else
                        {
                            COping = "全市各地一氧化碳年均浓度均持平。";
                        }
                    }
                    if (COdrop != "")
                    {
                        if (COdown != "" || COping != "")
                        {
                            COdrop = COdrop.Trim('、');
                            COdrop = COdrop + "平均浓度有不同程度上升。";
                        }
                        else
                        {
                            COdrop = "全市各地一氧化碳年均浓度均上升。";
                        }
                    }
                    string COStr = "";
                    if (COmin > 4)
                        COStr = "均超过标准限值。";
                    else if (COmax <= 4)
                        COStr = "均达到标准要求。";
                    newRow["M15"] = COmin.ToString() + "~" + COmax.ToString() + "毫克/立方米之间，" + COStr + "与上年相比，" + COdown + COping + COdrop + "市区一氧化碳年均浓度为" + COC.ToString() + "毫克/立方米。";
                }  //上年


                newdt.Rows.Add(newRow);
                return newdt.DefaultView;
            }
            return null;
        }

        /// <summary>
        /// 站点污染持续天数及污染程度简表
        /// </summary>
        /// <param name="portsId">站点</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// 返回字段：DateTime，ContinuousDays，LightPollution，ModeratePollution，HighPollution，SeriousPollution
        /// </returns>
        public DataView GetContinuousDaysTable(DateTime dateStart, DateTime dateEnd)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
            {
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("DateTime", typeof(string));
            dt.Columns.Add("ContinuousDays", typeof(int));
            dt.Columns.Add("LightPollution", typeof(int));
            dt.Columns.Add("ModeratePollution", typeof(int));
            dt.Columns.Add("HighPollution", typeof(int));
            dt.Columns.Add("SeriousPollution", typeof(int));
            dt.Columns.Add("LightPollutionDay", typeof(string));
            dt.Columns.Add("ModeratePollutionDay", typeof(string));
            dt.Columns.Add("HighPollutionDay", typeof(string));
            dt.Columns.Add("SeriousPollutionDay", typeof(string));
            int record = 0;
            string[] regionGuids = { "7e05b94c-bbd4-45c3-919c-42da2e63fd43" };
            DataTable AllData = regionDayAQI.GetDataPager(regionGuids, dateStart, dateEnd, 999999, 0, out record, "ReportDateTime Asc").Table;
            DataTable NewExceedingDays = AllData.Clone();
            DataRow[] AllExceedingDays = AllData.Select("convert(AQIValue,'System.Int32')>100");

            if (AllExceedingDays.Length > 0)
            {
                NewExceedingDays = AllExceedingDays.CopyToDataTable();
                DataView dv = NewExceedingDays.DefaultView;
                dv.Sort = "ReportDateTime asc";
                List<List<DateTime>> ContinuousDaysList = new List<List<DateTime>>();
                List<DateTime> ContinuousDays = new List<DateTime>();

                for (int i = 1; i < dv.Count; i++)
                {
                    DateTime firstValue = Convert.ToDateTime(dv[i - 1]["ReportDateTime"]);
                    DateTime secondValue = Convert.ToDateTime(dv[i]["ReportDateTime"]);
                    int poor = (secondValue - firstValue).Days;
                    if (poor.Equals(1))
                    {
                        if (!ContinuousDays.Contains(firstValue))
                        {
                            ContinuousDays.Add(firstValue);
                        }
                        if (!ContinuousDays.Contains(secondValue))
                        {
                            ContinuousDays.Add(secondValue);
                        }
                        if (i == dv.Count - 1)
                        {
                            ContinuousDaysList.Add(ContinuousDays);
                        }
                    }
                    else
                    {
                        if (ContinuousDays.Count > 0)
                        {
                            ContinuousDaysList.Add(ContinuousDays);
                        }
                        ContinuousDays = new List<DateTime>();
                    }
                }

                for (int i = 0; i < ContinuousDaysList.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    List<DateTime> dateTimeArray = ContinuousDaysList[i];
                    DataTable ExceedingDays = NewExceedingDays.Clone();
                    ExceedingDays = NewExceedingDays.Select("ReportDateTime>='" + dateTimeArray[0] + "' and ReportDateTime<='" + dateTimeArray[dateTimeArray.Count - 1] + "'").CopyToDataTable();
                    DataRow[] LightPollution = ExceedingDays.Select("convert(AQIValue,'System.Int32')>=101 and convert(AQIValue,'System.Int32')<=150", "ReportDateTime");
                    DataRow[] ModeratePollution = ExceedingDays.Select("convert(AQIValue,'System.Int32')>=151 and convert(AQIValue,'System.Int32')<=200", "ReportDateTime");
                    DataRow[] HighPollution = ExceedingDays.Select("convert(AQIValue,'System.Int32')>=201 and convert(AQIValue,'System.Int32')<=300", "ReportDateTime");
                    DataRow[] SeriousPollution = ExceedingDays.Select("convert(AQIValue,'System.Int32')>300", "ReportDateTime");

                    int outDays = dateTimeArray.Count;
                    int LightPollutionDays = LightPollution.Length;
                    int ModeratePollutionDays = ModeratePollution.Length;
                    int HighPollutionDays = HighPollution.Length;
                    int SeriousPollutionDays = SeriousPollution.Length;
                    string LightPollutionday = "";
                    foreach (DataRow light in LightPollution)
                    {
                        if (light["ReportDateTime"].IsNotNullOrDBNull())
                        {
                            LightPollutionday += light["ReportDateTime"] + "\n";
                        }
                    }
                    string ModeratePollutionday = "";
                    foreach (DataRow Moderate in ModeratePollution)
                    {
                        if (Moderate["ReportDateTime"].IsNotNullOrDBNull())
                        {
                            ModeratePollutionday += Moderate["ReportDateTime"] + "\n";
                        }
                    }
                    string HighPollutionday = "";
                    foreach (DataRow High in HighPollution)
                    {
                        if (High["ReportDateTime"].IsNotNullOrDBNull())
                        {
                            HighPollutionday += High["ReportDateTime"] + "\n";
                        }
                    }
                    string SeriousPollutionday = "";
                    foreach (DataRow Serious in SeriousPollution)
                    {
                        if (Serious["ReportDateTime"].IsNotNullOrDBNull())
                        {
                            SeriousPollutionday += Serious["ReportDateTime"] + "\n";
                        }
                    }
                    dr["DateTime"] = dateTimeArray[0].ToString("MM-dd") + "—" + dateTimeArray[dateTimeArray.Count - 1].ToString("MM-dd");
                    dr["ContinuousDays"] = outDays;
                    dr["LightPollution"] = LightPollutionDays;
                    dr["ModeratePollution"] = ModeratePollutionDays;
                    dr["HighPollution"] = HighPollutionDays;
                    dr["SeriousPollution"] = SeriousPollutionDays;
                    dr["LightPollutionDay"] = LightPollutionday;
                    dr["ModeratePollutionDay"] = ModeratePollutionday;
                    dr["HighPollutionDay"] = HighPollutionday;
                    dr["SeriousPollutionDay"] = SeriousPollutionday;
                    dt.Rows.Add(dr);
                }
                //return dt.DefaultView;
            }
            return dt.DefaultView;
        }
        /// <summary>
        /// 年报
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        public DataView GetRegionsSeasonAllData(DateTime dateStart, DateTime dateEnd, string year)
        {
            DateTime dtSta = dateStart.AddYears(-1);
            DateTime dtEn = dateEnd.AddYears(-1);
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
            {
                DataTable dt = new DataTable();   //2015
                DataTable dtNew = new DataTable();  //2014
                DataTable dtN = new DataTable();  //2013

                dt = regionDayAQI.GetRegionsSeasonData(dateStart, dateEnd).Table;  //2015
                dt.Columns.Add("regionName", typeof(string)).SetOrdinal(0);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["regionName"] = g_DictionaryService.GetCodeDictionaryTextByValue(dt.Rows[i]["MonitoringRegionUid"].ToString());
                }

                dtNew = regionDayAQI.GetRegionsSeasonData(dtSta, dtEn).Table;   //2014
                dtNew.Columns.Add("regionName", typeof(string)).SetOrdinal(0);
                for (int i = 0; i < dtNew.Rows.Count; i++)
                {
                    dtNew.Rows[i]["regionName"] = g_DictionaryService.GetCodeDictionaryTextByValue(dtNew.Rows[i]["MonitoringRegionUid"].ToString());
                }
                if (year != "")
                {

                    dtN = m_DataQueryByDayService.GetRegionSeasonBaseData(dateStart, dateEnd, year).Table;
                    //dtN = regionDayAQI.GetRegionsAllData(dtSta.AddYears(-1), dtEn.AddYears(-1)).Table;   //2014
                    dtN.Columns.Add("regionName", typeof(string)).SetOrdinal(0);
                    for (int i = 0; i < dtN.Rows.Count; i++)
                    {
                        dtN.Rows[i]["regionName"] = g_DictionaryService.GetCodeDictionaryTextByValue(dtN.Rows[i]["MonitoringRegionUid"].ToString());
                    }
                }
                DataTable NewTable = GetContinuousDaysTable(dateStart, dateEnd).ToTable();
                DataTable newdt = new DataTable();  //2014
                newdt.Columns.Add("M1", typeof(string));
                newdt.Columns.Add("M2", typeof(string));
                newdt.Columns.Add("M21", typeof(string));
                newdt.Columns.Add("M22", typeof(string));
                newdt.Columns.Add("M23", typeof(string));
                newdt.Columns.Add("M24", typeof(string));
                newdt.Columns.Add("M25", typeof(string));
                newdt.Columns.Add("M3", typeof(string));
                newdt.Columns.Add("M4", typeof(string));
                newdt.Columns.Add("M5", typeof(string));
                newdt.Columns.Add("M6", typeof(string));
                newdt.Columns.Add("M7", typeof(string));
                newdt.Columns.Add("M8", typeof(string));
                newdt.Columns.Add("M9", typeof(string));
                newdt.Columns.Add("M10", typeof(string));
                DataRow newRow = newdt.NewRow();

                int[] days = new int[6];
                string[] factorNames = new string[6];
                if (dt.Rows.Count > 0)
                {
                    newRow["M1"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[0]["AQIMin"]), 0) + "~" + DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[0]["AQIMax"]), 0) + "之间，";
                    decimal max = 0;
                    decimal Con = 0;
                    decimal ConLast = 0;
                    decimal ConBase = 0;
                    string Constr = "";
                    string name = "";
                    string names = "";
                    string nameStr = "";
                    string sname = "";
                    if (dt.Rows[0]["PM25"] != DBNull.Value && dt.Rows[0]["PM25"].ToString() != "")
                    {
                        max = Convert.ToDecimal(dt.Rows[0]["PM25"]);
                        Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[0]["PM25Avg"]), 0);
                        Constr = Con.ToString() + "微克/立方米，";
                        nameStr = "细颗粒物（PM";
                        name = "PM2.5";
                        names = "2.5";
                        sname = "PM";
                    }
                    if (dt.Rows[0]["PM10"] != DBNull.Value && dt.Rows[0]["PM10"].ToString() != "")
                    {
                        if (max < Convert.ToDecimal(dt.Rows[0]["PM10"]))
                        {
                            max = Convert.ToDecimal(dt.Rows[0]["PM10"]);
                            Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[0]["PM10Avg"]), 0);
                            Constr = Con.ToString() + "微克/立方米，";
                            nameStr = "可吸入颗粒物（PM";
                            name = "PM10";
                            names = "10";
                            sname = "PM";
                        }
                    }
                    if (dt.Rows[0]["O3"] != DBNull.Value && dt.Rows[0]["O3"].ToString() != "")
                    {
                        if (max < Convert.ToDecimal(dt.Rows[0]["O3"]))
                        {
                            max = Convert.ToDecimal(dt.Rows[0]["O3"]);
                            Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[0]["O3Avg"]), 0);
                            Constr = Con.ToString() + "微克/立方米，";
                            nameStr = "臭氧（O";
                            name = "O3";
                            names = "3";
                            sname = "O";
                        }
                    }
                    if (dt.Rows[0]["SO2"] != DBNull.Value && dt.Rows[0]["SO2"].ToString() != "")
                    {
                        if (max < Convert.ToDecimal(dt.Rows[0]["SO2"]))
                        {
                            max = Convert.ToDecimal(dt.Rows[0]["SO2"]);
                            Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[0]["SO2Avg"]), 0);
                            Constr = Con.ToString() + "微克/立方米，";
                            nameStr = "二氧化硫（SO";
                            name = "SO2";
                            names = "2";
                            sname = "SO";
                        }
                    }
                    if (dt.Rows[0]["NO2"] != DBNull.Value && dt.Rows[0]["NO2"].ToString() != "")
                    {
                        if (max < Convert.ToDecimal(dt.Rows[0]["NO2"]))
                        {
                            max = Convert.ToDecimal(dt.Rows[0]["NO2"]);
                            Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[0]["NO2Avg"]), 0);
                            Constr = Con.ToString() + "微克/立方米，";
                            nameStr = "二氧化氮（NO";
                            name = "NO2";
                            names = "2";
                            sname = "NO";
                        }
                    }
                    if (dt.Rows[0]["CO"] != DBNull.Value && dt.Rows[0]["CO"].ToString() != "")
                    {
                        if (max < Convert.ToDecimal(dt.Rows[0]["CO"]))
                        {
                            max = Convert.ToDecimal(dt.Rows[0]["CO"]);
                            Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[0]["COAvg"]), 1);
                            Constr = Con.ToString() + "毫克/立方米，";
                            nameStr = "一氧化碳（CO";
                            name = "CO";
                            names = "";
                            sname = "CO";
                        }
                    }
                    newRow["M2"] = "影响环境空气质量的首要污染物主要为" + nameStr;
                    newRow["M21"] = names;
                    newRow["M22"] = ")，" + sname;
                    newRow["M23"] = "平均浓度为" + Constr + "与去年同期相比，" + sname;
                    if (name == "PM2.5" || name == "PM10")
                        newRow["M24"] = "与2013年同期相比，" + sname;
                    newRow["M25"] = "，苏州市区" + sname;
                    decimal dabiaoLast = 0;
                    string wuranLast = "";
                    decimal wuDays = 0;
                    if (dtNew.Rows.Count > 0)
                    {
                        if (name == "PM2.5")
                        {
                            ConLast = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[0]["PM25Avg"]), 0);
                        }
                        if (name == "PM10")
                        {
                            ConLast = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[0]["PM10Avg"]), 0);
                        }
                        if (name == "O3")
                        {
                            ConLast = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[0]["O3Avg"]), 0);
                        }
                        if (name == "SO2")
                        {
                            ConLast = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[0]["SO2Avg"]), 0);
                        }
                        if (name == "NO2")
                        {
                            ConLast = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[0]["NO2Avg"]), 0);
                        }
                        if (name == "CO")
                        {
                            ConLast = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[0]["COAvg"]), 1);
                        }

                        if (dtNew.Rows[0]["ValidCount"] != DBNull.Value && Convert.ToDecimal(dtNew.Rows[0]["ValidCount"]) != 0)
                        {
                            dabiaoLast = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNew.Rows[0]["StandardCount"]) / Convert.ToDecimal(dtNew.Rows[0]["ValidCount"]) * 100, 1);
                        }

                        if (dtNew.Rows[0]["LightPollution"] != DBNull.Value && Convert.ToDecimal(dtNew.Rows[0]["LightPollution"]) != 0)
                        {
                            wuranLast += "轻度污染为" + Convert.ToDecimal(dtNew.Rows[0]["LightPollution"]).ToString() + "天，";
                        }
                        if (dtNew.Rows[0]["ModeratePollution"] != DBNull.Value && Convert.ToDecimal(dtNew.Rows[0]["ModeratePollution"]) != 0)
                        {
                            wuranLast += "中度污染为" + Convert.ToDecimal(dtNew.Rows[0]["ModeratePollution"]).ToString() + "天，";
                        }
                        if (dtNew.Rows[0]["HighPollution"] != DBNull.Value && Convert.ToDecimal(dtNew.Rows[0]["HighPollution"]) != 0)
                        {
                            wuranLast += "重度污染为" + Convert.ToDecimal(dtNew.Rows[0]["HighPollution"]).ToString() + "天，";
                        }
                        if (dtNew.Rows[0]["SeriousPollution"] != DBNull.Value && Convert.ToDecimal(dtNew.Rows[0]["SeriousPollution"]) != 0)
                        {
                            wuranLast += "严重污染为" + Convert.ToDecimal(dtNew.Rows[0]["SeriousPollution"]).ToString() + "天，";
                        }
                        wuranLast = wuranLast.Trim('，');

                        if (dtNew.Rows[0]["ExcessiveCount"] != DBNull.Value)
                            wuDays = Convert.ToDecimal(dtNew.Rows[0]["ExcessiveCount"]);
                    }
                    if (dtN.Rows.Count > 0)
                    {
                        if (name == "PM2.5")
                        {
                            if (dtN.Rows[0]["PM25Avg"].IsNotNullOrDBNull())
                                ConBase = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtN.Rows[0]["PM25Avg"]), 0);
                        }
                        if (name == "PM10")
                        {
                            if (dtN.Rows[0]["PM10Avg"].IsNotNullOrDBNull())
                                ConBase = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtN.Rows[0]["PM10Avg"]), 0);
                        }
                        if (name == "O3")
                        {
                            if (dtN.Rows[0]["O3Avg"].IsNotNullOrDBNull())
                                ConBase = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtN.Rows[0]["O3Avg"]), 0);
                        }
                        if (name == "SO2")
                        {
                            if (dtN.Rows[0]["SO2Avg"].IsNotNullOrDBNull())
                                ConBase = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtN.Rows[0]["SO2Avg"]), 0);
                        }
                        if (name == "NO2")
                        {
                            if (dtN.Rows[0]["NO2Avg"].IsNotNullOrDBNull())
                                ConBase = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtN.Rows[0]["NO2Avg"]), 0);
                        }
                        if (name == "CO")
                        {
                            if (dtN.Rows[0]["COAvg"].IsNotNullOrDBNull())
                                ConBase = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtN.Rows[0]["COAvg"]), 1);
                        }
                    }
                    decimal bili = 0;
                    string target = ""; ;
                    if (ConBase != 0)
                    {
                        if (DecimalExtension.GetRoundValue((Con - ConBase) / ConBase * 100, 1) > 0)
                        {
                            newRow["M4"] = "浓度上升了" + DecimalExtension.GetRoundValue((Con - ConBase) / ConBase * 100, 1).ToString() + "%。";
                        }
                        if (DecimalExtension.GetRoundValue((Con - ConBase) / ConBase * 100, 1) < 0)
                        {
                            bili = DecimalExtension.GetRoundValue((ConBase - Con) / ConLast * 100, 1);
                            newRow["M4"] = "浓度下降了" + DecimalExtension.GetRoundValue((ConBase - Con) / ConBase * 100, 1).ToString() + "%。";
                        }
                        if (DecimalExtension.GetRoundValue((Con - ConBase) / ConBase * 100, 1) == 0)
                        {
                            newRow["M4"] = "浓度持平。";
                        }
                    }
                    if (ConLast != 0)
                    {
                        if (DecimalExtension.GetRoundValue((Con - ConLast) / ConLast * 100, 1) > 0)
                        {
                            newRow["M3"] = "浓度上升了" + DecimalExtension.GetRoundValue((Con - ConLast) / ConLast * 100, 1).ToString() + "%；";
                            if (name == "PM2.5")
                                target = "未达到考核目标要求（考核目标比2013年同期浓度下降7%）。";
                            newRow["M8"] = "相比去年同期略有上升，" + target;
                        }
                        if (DecimalExtension.GetRoundValue((Con - ConLast) / ConLast * 100, 1) < 0)
                        {
                            newRow["M3"] = "浓度下降了" + DecimalExtension.GetRoundValue((ConLast - Con) / ConLast * 100, 1).ToString() + "%；";
                            if (name == "PM2.5" && bili >= 7)
                                target = "达到了考核目标要求（考核目标比2013年同期浓度下降7%）。";
                            else if (name == "PM2.5" && bili < 7)
                                target = "未达到考核目标要求（考核目标比2013年同期浓度下降7%）。";
                            newRow["M8"] = "相比去年同期略有下降，" + target;
                        }
                        if (DecimalExtension.GetRoundValue((Con - ConLast) / ConLast * 100, 1) == 0)
                        {
                            newRow["M3"] = "浓度持平；";
                            if (name == "PM2.5")
                                target = "未达到考核目标要求（考核目标比2013年同期浓度下降7%）。";
                            newRow["M8"] = "相比去年同期持平，" + target;
                        }
                    }

                    decimal dabiao = 0;
                    if (dt.Rows[0]["ValidCount"] != DBNull.Value && Convert.ToDecimal(dt.Rows[0]["ValidCount"]) != 0)
                    {
                        dabiao = DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[0]["StandardCount"]) / Convert.ToDecimal(dt.Rows[0]["ValidCount"]) * 100, 1);
                    }
                    string wuran = "";
                    decimal Sday = 0;//严重污染天数
                    if (dt.Rows[0]["LightPollution"] != DBNull.Value && Convert.ToDecimal(dt.Rows[0]["LightPollution"]) != 0)
                    {
                        wuran += "轻度污染为" + Convert.ToDecimal(dt.Rows[0]["LightPollution"]).ToString() + "天，";
                    }
                    if (dt.Rows[0]["ModeratePollution"] != DBNull.Value && Convert.ToDecimal(dt.Rows[0]["ModeratePollution"]) != 0)
                    {
                        wuran += "中度污染为" + Convert.ToDecimal(dt.Rows[0]["ModeratePollution"]).ToString() + "天，";
                    }
                    if (dt.Rows[0]["HighPollution"] != DBNull.Value && Convert.ToDecimal(dt.Rows[0]["HighPollution"]) != 0)
                    {
                        wuran += "重度污染为" + Convert.ToDecimal(dt.Rows[0]["HighPollution"]).ToString() + "天，";
                    }
                    if (dt.Rows[0]["SeriousPollution"] != DBNull.Value && Convert.ToDecimal(dt.Rows[0]["SeriousPollution"]) != 0)
                    {
                        wuran += "严重污染为" + Convert.ToDecimal(dt.Rows[0]["SeriousPollution"]).ToString() + "天，";
                        Sday = Convert.ToDecimal(dt.Rows[0]["SeriousPollution"]);
                    }
                    wuran = wuran.Trim('，');
                    string dabiaoStr = "";
                    if (dabiao > dabiaoLast)
                    {
                        dabiaoStr = "与去年同期相比达标天数比例上升了" + (dabiao - dabiaoLast).ToString() + "%";
                        newRow["M9"] = "达标天数比例比去年同期相比有所上升，";
                    }
                    if (dabiao < dabiaoLast)
                    {
                        dabiaoStr = "与去年同期相比达标天数比例下降了" + (dabiaoLast - dabiao).ToString() + "%";
                        newRow["M9"] = "达标天数比例比去年同期相比有所下降，";
                    }
                    if (dabiao == dabiaoLast)
                    {
                        dabiaoStr = "与去年同期相比达标天数持平";
                        newRow["M9"] = "达标天数比例比去年同期相比持平，";
                    }
                    decimal month = Convert.ToDecimal(dt.Rows[0][dateStart.Month + "月"]);
                    string monthstr = "尤其以" + dateStart.Month + "月份超标最为严重。";
                    for (int i = dateStart.Month; i <= dateEnd.Month; i++)
                    {
                        if (dt.Rows[0][i + "月"] != DBNull.Value)
                        {
                            if (month < Convert.ToDecimal(dt.Rows[0][i + "月"]))
                                monthstr = "尤其以" + i + "月份超标最为严重。";
                        }

                    }
                    newRow["M5"] = "，苏州市区达标天数比例为" + dabiao.ToString() + " %，污染天数累计为" + dt.Rows[0]["ExcessiveCount"].ToString() + "天（" + wuran + "），";
                    newRow["M6"] = monthstr + dabiaoStr + "（去年同期污染天数为" + dtNew.Rows[0]["ExcessiveCount"].ToString() + "天，其中" + wuranLast + "）";
                    decimal maxDay = 0;
                    decimal minDay = 0;
                    decimal Day = 0;
                    int count = 0;
                    decimal sum = 0;
                    decimal SeriousPollution = 0;
                    if (NewTable.Rows.Count > 0)
                    {
                        minDay = Convert.ToDecimal(NewTable.Rows[0]["ContinuousDays"]);
                        for (int i = 0; i < NewTable.Rows.Count; i++)
                        {
                            if (NewTable.Rows[i]["ContinuousDays"] != DBNull.Value)
                            {
                                sum += Convert.ToDecimal(NewTable.Rows[i]["ContinuousDays"]);
                                Day = Convert.ToDecimal(NewTable.Rows[i]["ContinuousDays"]);
                                if (maxDay < Day)
                                    maxDay = Day;
                                if (minDay > Day)
                                    minDay = Day;
                                if (Day >= minDay)
                                    count++;
                            }
                            if (NewTable.Rows[i]["SeriousPollution"] != DBNull.Value)
                            {
                                SeriousPollution += Convert.ToDecimal(NewTable.Rows[i]["SeriousPollution"]);
                            }
                        }
                    }
                    decimal Wubili = 0;
                    if (dt.Rows[0]["ExcessiveCount"] != DBNull.Value && Convert.ToDecimal(dt.Rows[0]["ExcessiveCount"]) != 0)
                        Wubili = DecimalExtension.GetRoundValue(sum / Convert.ToDecimal(dt.Rows[0]["ExcessiveCount"]) * 100, 1);
                    string Swuran = "";
                    if (Sday == 0)
                        Swuran = "严重污染0次";
                    else
                        Swuran = Sday.ToString() + "次严重污染中有" + SeriousPollution.ToString() + "次出现在污染持续过程中";
                    newRow["M7"] = "共出现连续" + minDay.ToString() + "天以上污染天气共" + count.ToString() + "次，最长连续污染过程持续了" + maxDay.ToString() + "天，持续污染累计为" + sum.ToString() + "天（占总污染天数的" + Wubili.ToString() + "%），" + Swuran + "，具体污染持续天数及污染程度详见表1。";
                    newRow["M10"] = name;
                }  //上年


                newdt.Rows.Add(newRow);
                return newdt.DefaultView;
            }
            return null;
        }
        #endregion

        #region 接口实现
        /// <summary>
        /// 获取某段时间内某一监测点AllStandardRate
        /// </summary>
        /// <returns></returns>
        public int GetTimePortAQI()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内苏州市区所有监测点AllStandardRate
        /// </summary>
        /// <returns></returns>
        public int GetTimeSzPortsAQI()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一城区所有监测点AllStandardRate
        /// </summary>
        /// <returns></returns>
        public int GetTimeAreaPortsAQI()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一监测点AllStandardRate污染等级统计
        /// </summary>
        /// <returns></returns>
        public int GetTimePortAQIGrade()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内苏州市区所有监测点AllStandardRate污染等级统计
        /// </summary>
        /// <returns></returns>
        public int GetTimeSzPortsAQIGrade()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一城区所有监测点AllStandardRate污染等级统计
        /// </summary>
        /// <returns></returns>
        public int GetTimeAreaPortsAQIGrade()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一监测点首要污染物统计
        /// </summary>
        /// <returns></returns>
        public int GetTimePortPrimaryPollutant()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内苏州市区所有监测点首要污染物统计
        /// </summary>
        /// <returns></returns>
        public int GetTimeSzPortsPrimaryPollutant()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一城区所有监测点首要污染物统计
        /// </summary>
        /// <returns></returns>
        public int GetTimeAreaPortsPrimaryPollutant()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一监测点超标天数统计
        /// </summary>
        /// <returns></returns>
        public int GetTimePortOutDay()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内苏州市区所有监测点超标天数统计
        /// </summary>
        /// <returns></returns>
        public int GetTimeSzPortsOutDay()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一城区所有监测点超标天数统计
        /// </summary>
        /// <returns></returns>
        public int GetTimeAreaPortsOutDay()
        {
            return 0;
        }
        #endregion
    }
}
