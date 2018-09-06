﻿using SmartEP.Core.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace SmartEP.WebService
{
    /// <summary>
    /// 名称：WebServiceForOutData.cs
    /// 创建人：徐阳
    /// 创建日期：2017-07-03
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：提供的WebService接口
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    [WebService(Namespace = "http://218.91.209.251:1117")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class WebServiceForOutData : System.Web.Services.WebService
    {

        /// <summary>
        /// 获取数据类
        /// </summary>
        private GetDataClass m_GetDataClass = Singleton<GetDataClass>.GetInstance();

        /// <summary>
        /// 获取测点数据
        /// </summary>
        /// <returns>
        /// </returns>
        /// PointId：站点Id
        /// MonitoringPointUid：站点Guid
        /// MonitoringPointName：站点名称
        /// X：经度
        /// Y：纬度
        /// PName:国控、省控、市控、其他
        /// <returns>返回数据示例
        ///[{"PortInfo":
        ///[{"state":0,"label":"环境空气","id":"000","children":
        ///     [{"state":0,"label":"国控","id":"6fadff52-2338-4319-9f1d-7317823770ad","children":
        ///             [{"state":0,"label":"南郊","id":"186"},{"state":0,"label":"城中","id":"188"},{"state":0,"label":"虹桥","id":"187"},
        ///             {"state":0,"label":"星湖花园","id":"189"},{"state":0,"label":"紫琅学院","id":"190"}]},
        ///      {"state":0,"label":"省控","id":"bc4fca0c-745f-49d7-b9e9-0af67d3219e6","children":
        ///             [{"state":0,"label":"通州监测站","id":"191"},{"state":0,"label":"如东职校","id":"192"},{"state":0,"label":"如皋监测站","id":"193"},
        ///             {"state":0,"label":"海安监测站","id":"194"},{"state":0,"label":"海门监测站","id":"195"},{"state":0,"label":"社会福利院","id":"196"},
        ///             {"state":0,"label":"启东监测站","id":"197"}]},
        ///      {"state":0,"label":"市控","id":"b107d493-b1a3-4ebd-b991-b0e340becec1","children":
        ///             [{"state":0,"label":"三余中学","id":"199"},{"state":0,"label":"如东监测站","id":"200"},{"state":0,"label":"如皋美能得","id":"201"},
        ///             {"state":0,"label":"育才中学","id":"198"},{"state":0,"label":"海安凤凰花园","id":"202"},{"state":0,"label":"启东南苑中学","id":"203"}]},
        ///      {"state":0,"label":"其他","id":"74973ed2-1d7b-4234-82ff-8165062f096a","children":[{"state":0,"label":"紫琅地面站","id":"205"},
        ///             {"state":0,"label":"农校路边站","id":"206"},{"state":0,"label":"超级站","id":"204"}]}]}]}]
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取测点数据(树状结构)")]
        public void GetPortInfoByType()
        {
            string json = m_GetDataClass.GetMonitoringPointJson();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        /// <summary>
        /// 获取测点信息
        /// </summary>
        /// <returns>
        /// </returns>
        /// PointId：站点Id
        /// MonitoringPointUid：站点Guid
        /// MonitoringPointName：站点名称
        /// X：经度
        /// Y：纬度
        /// PName:国控、省控、市控、其他
        /// <returns>返回数据示例
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取测点信息<br />pointId：点位Id，以分号隔开,如：204;190<br />type：查询类型(AQI、IAQI、空、day),day表示日数据<br />fac：因子名称(SO2,NO2,PM10,CO,O3,Recent8HoursO3NT,PM25 不填默认PM25)<br />只有type为day时，需要传入时间,其他type为了不报错，需要传入2000-10-10<br />dt1：开始日期，2017-8-9<br />dt2：结束日期，2017-8-9")]
        public void GetPortMessage(string pId, string type, string fac, DateTime dt1, DateTime dt2)
        {
            string[] pointId;
            if (pId == "")
            {
                pointId = new string[] { "ALL" };
            }
            else
            {
                pointId = pId.Split(';');
            }
            if (type.Equals("day"))
            {
                string json = m_GetDataClass.GetPortMessage(pointId, type != "" ? type : "", fac != "" ? fac : "PM25", dt1, dt2);
                Context.Response.Clear();
                Context.Response.ContentType = "application/json";
                Context.Response.Write(json);
                Context.Response.End();
            }
            else
            {
                string json = m_GetDataClass.GetPortMessage(pointId, type != "" ? type : "", fac != "" ? fac : "PM25");
                Context.Response.Clear();
                Context.Response.ContentType = "application/json";
                Context.Response.Write(json);
                Context.Response.End();
            }
        }

        /// <summary>
        /// 获取周边城市测点信息
        /// </summary>
        /// <returns>
        /// </returns>
        /// PointId：站点Id
        /// MonitoringPointUid：站点Guid
        /// MonitoringPointName：站点名称
        /// X：经度
        /// Y：纬度
        /// PName:国控、省控、市控、其他
        /// <returns>返回数据示例
        /// </returns>
        //[ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(Description = @"获取测点信息<br />pointId：点位Id,如：204<br />type：查询类型(AQI、IAQI、空)<br />dt：时间(日：2017-05-05;小时：2017-05-05 14:00:00)")]
        //public void GetAroundPortMessage(string pointId, string type, string dt)
        //{
        //    DateTime dtime;
        //    if (!dt.Trim().Equals(""))
        //    {
        //        dtime = Convert.ToDateTime(dt);
        //    }
        //    else
        //    {
        //        dtime = DateTime.Now;
        //    }
        //    string json = m_GetDataClass.GetAroundPortMessage(pointId, type != null ? type : "", dtime);
        //    Context.Response.Clear();
        //    Context.Response.ContentType = "application/json";
        //    Context.Response.Write(json);
        //    Context.Response.End();
        //}

        /// <summary>
        /// 获取周边城市测点信息排名
        /// </summary>
        /// <returns>
        /// </returns>
        /// <returns>返回数据示例
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取测点信息<br />order：排序(好到差：1，差到好：2)<br />type：类型，日数据还是小时数据(日：1;小时：2)")]
        public void GetAroundPortMessageOrder(string order, string type)
        {
            string json = m_GetDataClass.GetAroundPortMessageOrder(order != "" ? order : "1", type != "" ? type : "1");
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        /// <summary>
        /// 获取周边城市测点因子浓度
        /// </summary>
        /// <returns>
        /// </returns>
        /// <returns>返回数据示例
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取测点信息<br />pointId,测点，如：上海1，苏州2<br />type：类型，日数据还是小时数据(日：1;小时：2)")]
        public void GetAroundPortData(string pointId, string type)
        {
            string json = m_GetDataClass.GetAroundPortData(pointId != "" ? pointId : "1", type != "" ? type : "1");
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        /// <summary>
        /// 获取所有站点分类
        /// </summary>
        /// <returns>
        /// </returns>
        /// <returns>返回数据示例
        /// [{"PortTypes":[
        ///  {"PortTypeName":"国控","PortTypeValue":"6fadff52-2338-4319-9f1d-7317823770ad"},
        ///  {"PortTypeName":"省控","PortTypeValue":"bc4fca0c-745f-49d7-b9e9-0af67d3219e6"},
        ///  {"PortTypeName":"市控","PortTypeValue":"b107d493-b1a3-4ebd-b991-b0e340becec1"},
        ///  {"PortTypeName":"其他","PortTypeValue":"74973ed2-1d7b-4234-82ff-8165062f096a"}]}]
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取所有站点分类")]
        public void GetPortTypes()
        {
            string json = m_GetDataClass.GetPortTypes();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        /// <summary>
        /// 获取因子数据
        /// </summary>
        /// <returns>[{"PollutantCode":"","PollutantName":"","Unit":""}]</returns>
        /// PollutantCode：因子编号（如，a10101、w20202）
        /// PollutantName：因子名称（如，二氧化碳、溶解氧）
        /// Unit：单位（如：mg/L）
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取因子数据")]
        public void GetPollutantCodeJson()
        {
            string json = m_GetDataClass.GetPollutantCodeDataTable();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        /// <summary>
        /// 获取参与AQI计算因子数据
        /// </summary>
        /// <returns>
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取参与AQI计算因子数据")]
        public void GetPollutantByCalAQI()
        {
            string json = m_GetDataClass.GetPollutantByCalAQI();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        /// <summary>
        /// 获取区域综合分析数据
        /// </summary>
        /// <returns></returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取区域综合分析数据<br />dtStart：开始时间,如：2017-6-20<br />dtEnd：结束时间,如：2017-6-20")]
        public void GetAreaDataAnalyze(DateTime dtStart, DateTime dtEnd)
        {
            string json = m_GetDataClass.GetAreaDataAnalyze(dtStart, dtEnd);
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        /// <summary>
        /// 根据站点类型获取某个监测因子的最新1小时IAQI分指数情况
        /// </summary>
        /// pointType：站点类型(如：国控,省控)
        /// factor：因子Code(如：a05024)
        /// <returns>返回数据示例
        /// [{"PortIAQIBySiteType":
        /// [{"PointId":"186","CO":"1.801","CO_IAQI":"19","Grade":"一级","Class":"优","DateTime":"2017/7/5 11:00:00","PrimaryPollutant":"--","HealthEffect":"空气质量令人满意，基本无空气污染","TakeStep":"各类人群可正常活动","PortName":"南郊","Unit":"mg/m3"},
        ///  {"PointId":"187","CO":"1.227","CO_IAQI":"13","Grade":"一级","Class":"优","DateTime":"2017/7/5 11:00:00","PrimaryPollutant":"--","HealthEffect":"空气质量令人满意，基本无空气污染","TakeStep":"各类人群可正常活动","PortName":"虹桥","Unit":"mg/m3"},
        ///  {"PointId":"188","CO":"0.952","CO_IAQI":"10","Grade":"一级","Class":"优","DateTime":"2017/7/5 11:00:00","PrimaryPollutant":"--","HealthEffect":"空气质量令人满意，基本无空气污染","TakeStep":"各类人群可正常活动","PortName":"城中","Unit":"mg/m3"},
        ///  {"PointId":"189","CO":"1.968","CO_IAQI":"20","Grade":"一级","Class":"优","DateTime":"2017/7/5 11:00:00","PrimaryPollutant":"--","HealthEffect":"空气质量令人满意，基本无空气污染","TakeStep":"各类人群可正常活动","PortName":"星湖花园","Unit":"mg/m3"},
        ///  {"PointId":"190","CO":"0.752","CO_IAQI":"8","Grade":"二级","Class":"良","DateTime":"2017/7/5 11:00:00","PrimaryPollutant":"PM10","HealthEffect":"空气质量可接受，但某些污染物可能对极少数异常敏感人群健康有较弱影响","TakeStep":"极少数异常敏感人群应减少户外活动","PortName":"紫琅学院","Unit":"mg/m3"}]}]
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"根据站点类型获取某个监测因子的最新1小时IAQI分指数情况<br />portType:站点类型Guid(如：国控,省控为：6fadff52-2338-4319-9f1d-7317823770ad,bc4fca0c-745f-49d7-b9e9-0af67d3219e6)<br />factor:因子Code(如：a05024)<br/>AreaGuid（当传值为All或者空时，返回所有区域的数据）")]
        public void GetIAQIByFactorAndPointType(string pointType, string factor, string AreaGuid)
        {
            string[] arraypointType;
            if (pointType == "")
            {
                arraypointType = new string[] { "ALL" };
            }
            else
            {
                arraypointType = pointType.Split(',');
            }
            string json = m_GetDataClass.GetIAQIByFactorAndPointType(arraypointType, factor, AreaGuid);
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        /// <summary>
        /// 获取因子小时数据/日数据
        /// </summary>
        /// <returns>返回数据示例
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取因子小时数据/日数据<br />portId:站点编号(如：204)<br />factor:因子Code(如：a05024)<br />type:数据类型(1为小时，2为日)<br />dt1:开始时间(小时：yyyy-MM-dd HH;日：yyyy-MM-dd)<br/>dt2:结束时间")]
        public void GetIHourOrDayData(string portId, string factor, string type, DateTime dt1, DateTime dt2)
        {
            string json = m_GetDataClass.GetIHourOrDayData(portId, factor, type, dt1, dt2);
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        /// <summary>
        /// 根据站点Id获取最新1小时AQI数据
        /// </summary>
        /// pointType：站点类型(如：国控,省控)
        /// <returns>返回数据示例
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"根据站点类型及站点Id获取最新1小时AQI数据<br />regionType:站点类型(如：国控,省控为：6fadff52-2338-4319-9f1d-7317823770ad,bc4fca0c-745f-49d7-b9e9-0af67d3219e6))<br />portType:站点Id(如：204,189) 不填写默认查询所有站点<br />AreaGuid:区域数据当传值为All或者空时，返回所有区域的数据")]
        public void GetAQIByPointId(string regionType, string pointType, string AreaGuid)
        {
            string[] arraypointType;
            string arraypointTypeRegion;
            if (pointType == "")
            {
                arraypointType = new string[] { "ALL" };
            }
            else
            {
                arraypointType = pointType.Split(',');
            }
            if (regionType == "")
            {
                arraypointTypeRegion = "ALLR";
            }
            else
            {
                arraypointTypeRegion = regionType;
            }
            string json = m_GetDataClass.GetAQIByPointId(arraypointTypeRegion, arraypointType, AreaGuid);
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        /// <summary>
        /// 根据站点Id获取最新1小时因子数据
        /// </summary>
        /// pointType：站点ID(如：204)
        /// <returns>返回数据示例
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"根据站点Id获取最新1小时因子数据<br />portType:站点Id(如：204,189) 不填写默认查询所有站点<br />")]
        public void GetFactorByPointId(string pointType)
        {
            string[] arraypointType;
            if (pointType == "")
            {
                arraypointType = new string[] { "ALL" };
            }
            else
            {
                arraypointType = pointType.Split(',');
            }
            string json = m_GetDataClass.GetFactorByPointId(arraypointType);
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        /// <summary>
        /// 获取空气质量日报数据
        /// </summary>
        /// <param name="portType">站点类型(如：国控,省控)</param>
        /// <param name="dtStart">开始时间,如：2017-6-20</param>
        /// <param name="dtEnd">结束时间，如：2017-6-30</param>
        /// <param name="qualityType">空气质量类别,如:优,良,轻度污染,中度污染,重度污染,严重污染)</param>
        /// <returns>返回数据示例
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取空气质量日报数据<br />portType:站点类型Guid(如：国控,省控为：6fadff52-2338-4319-9f1d-7317823770ad,bc4fca0c-745f-49d7-b9e9-0af67d3219e6)<br />qualityType:空气质量类别,1代表优，2代表良，3轻度污染，4中度污染，5重度污染，6严重污染(如：1;2),不填默认查询所有类别<br />dtStart:开始时间,如：2017-6-20<br />dtStart:结束时间,如：2017-6-20<br />AreaGuid:区域数据当传值为All或者空时，返回所有区域的数据")]
        public void GetDayAQIJson(string portType, string qualityType, DateTime dtStart, DateTime dtEnd, string AreaGuid)
        {
            string[] arrayID;
            if (portType == "")
            {
                arrayID = new string[] { "ALL" };
            }
            else
            {
                arrayID = portType.Split(',');
            }
            string[] arrayType;
            if (qualityType == "")
            {
                arrayType = "优,良,轻度污染,中度污染,重度污染,严重污染".Split(',');
            }
            else
            {
                arrayType = qualityType.Split(';');
            }
            string json = m_GetDataClass.GetDayAQIJson(arrayID, dtStart, dtEnd, arrayType, AreaGuid);
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        /// <summary>
        /// 根据测点获取空气质量日报数据
        /// </summary>
        /// <param name="portType">站点ID(如：204,187)</param>
        /// <param name="dtStart">开始时间,如：2017-6-20</param>
        /// <param name="dtEnd">结束时间，如：2017-6-30</param>
        /// <param name="qualityType">空气质量类别,如:优,良,轻度污染,中度污染,重度污染,严重污染)</param>
        /// <returns>返回数据示例
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"根据测点获取空气质量日报数据<br />portType:站点Id(如：204,187),不填默认查询所有点位<br />qualityType:空气质量类别,1代表优，2代表良，3轻度污染，4中度污染，5重度污染，6严重污染(如：1,2),不填默认查询所有类别<br />dtStart:开始时间,如：2017-6-20<br />dtStart:结束时间,如：2017-6-20<br />")]
        public void GetDayAQIByPointIdJson(string portType, string qualityType, DateTime dtStart, DateTime dtEnd)
        {
            string[] arrayID;
            if (portType == "")
            {
                arrayID = new string[] { "ALL" };
            }
            else
            {
                arrayID = portType.Split(',');
            }
            string[] arrayType;
            if (qualityType == "")
            {
                arrayType = "优,良,轻度污染,中度污染,重度污染,严重污染".Split(',');
            }
            else
            {
                arrayType = qualityType.Split(',');
            }
            string json = m_GetDataClass.GetDayAQIByPointIdJson(arrayID, dtStart, dtEnd, arrayType);
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        /// <summary>
        /// 获取空气质量实时报数据
        /// </summary>
        /// <param name="portIds">站点Id,如:180,204,199</param>
        /// <param name="dtStart">开始时间,如：2017-06-20 4:00:00</param>
        /// <param name="dtEnd">结束时间,如：2017-06-20 23:00:00</param>
        /// <returns>返回数据示例
        /// [{"rows":"1","PointId":"189","DateTime":"2017/6/20 4:59:59","SO2":"0.014","SO2_IAQI":"5","NO2":"0.020","NO2_IAQI":"10","PM10":"0.046","PM10_IAQI":"46","Recent24HoursPM10":"0.038","Recent24HoursPM10_IAQI":"38","CO":"0.542","CO_IAQI":"6","O3":"0.035","O3_IAQI":"11","Recent8HoursO3":"","Recent8HoursO3_IAQI":"","Recent8HoursO3NT":"0.042","Recent8HoursO3NT_IAQI":"21","PM25":"0.022","PM25_IAQI":"32","Recent24HoursPM25":"0.020","Recent24HoursPM25_IAQI":"29","AQIValue":"46","PrimaryPollutant":"--","Range":"0~50","RGBValue":"#00e400","PicturePath":"","Class":"优","Grade":"一级","HealthEffect":"空气质量令人满意，基本无空气污染","TakeStep":"各类人群可正常活动"},
        ///  {"rows":"2","PointId":"189","DateTime":"2017/6/20 5:59:59","SO2":"0.013","SO2_IAQI":"5","NO2":"0.017","NO2_IAQI":"9","PM10":"0.040","PM10_IAQI":"40","Recent24HoursPM10":"0.037","Recent24HoursPM10_IAQI":"37","CO":"0.548","CO_IAQI":"6","O3":"0.037","O3_IAQI":"12","Recent8HoursO3":"","Recent8HoursO3_IAQI":"","Recent8HoursO3NT":"0.038","Recent8HoursO3NT_IAQI":"19","PM25":"0.021","PM25_IAQI":"30","Recent24HoursPM25":"0.020","Recent24HoursPM25_IAQI":"29","AQIValue":"40","PrimaryPollutant":"--","Range":"0~50","RGBValue":"#00e400","PicturePath":"","Class":"优","Grade":"一级","HealthEffect":"空气质量令人满意，基本无空气污染","TakeStep":"各类人群可正常活动"},
        ///  {"rows":"3","PointId":"205","DateTime":"2017/6/20 4:59:59","SO2":"0.006","SO2_IAQI":"3","NO2":"0.024","NO2_IAQI":"12","PM10":"0.025","PM10_IAQI":"25","Recent24HoursPM10":"0.033","Recent24HoursPM10_IAQI":"33","CO":"0.569","CO_IAQI":"6","O3":"0.038","O3_IAQI":"12","Recent8HoursO3":"","Recent8HoursO3_IAQI":"","Recent8HoursO3NT":"0.044","Recent8HoursO3NT_IAQI":"22","PM25":"0.027","PM25_IAQI":"39","Recent24HoursPM25":"0.024","Recent24HoursPM25_IAQI":"35","AQIValue":"39","PrimaryPollutant":"--","Range":"0~50","RGBValue":"#00e400","PicturePath":"","Class":"优","Grade":"一级","HealthEffect":"空气质量令人满意，基本无空气污染","TakeStep":"各类人群可正常活动"},
        ///  {"rows":"4","PointId":"205","DateTime":"2017/6/20 5:59:59","SO2":"0.006","SO2_IAQI":"3","NO2":"0.027","NO2_IAQI":"14","PM10":"0.039","PM10_IAQI":"39","Recent24HoursPM10":"0.033","Recent24HoursPM10_IAQI":"33","CO":"0.600","CO_IAQI":"6","O3":"0.035","O3_IAQI":"11","Recent8HoursO3":"","Recent8HoursO3_IAQI":"","Recent8HoursO3NT":"0.042","Recent8HoursO3NT_IAQI":"21","PM25":"0.036","PM25_IAQI":"52","Recent24HoursPM25":"0.025","Recent24HoursPM25_IAQI":"36","AQIValue":"52","PrimaryPollutant":"PM2.5","Range":"51~100","RGBValue":"#ffff00","PicturePath":"","Class":"良","Grade":"二级","HealthEffect":"空气质量可接受，但某些污染物可能对极少数异常敏感人群健康有较弱影响","TakeStep":"极少数异常敏感人群应减少户外活动"}]
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取空气质量小时数据<br />portIds:站点Id,如:180,204,199<br />dtStart:开始时间,如：2017-06-20 4:00:00<br />dtStart:结束时间,如：2017-06-20 23:00:00<br />不填写时间则查询所有小时数据")]
        public void GetHourAQIJson(string portIds, string dtStart, string dtEnd)
        {
            DateTime dt1;
            DateTime dt2;
            if (dtStart == "" && dtEnd == "")
            {
                dt1 = Convert.ToDateTime("2000-01-01 00:00:00");
                dt2 = Convert.ToDateTime("3000-01-01 00:00:00");
            }
            else
            {
                dt1 = Convert.ToDateTime(dtStart);
                dt2 = Convert.ToDateTime(dtEnd);
            }
            string[] arrayID = portIds.Split(',');
            string json = m_GetDataClass.GetHourAQIJson(arrayID, dt1, dt2);
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        /// <summary>
        /// 获取优良数据统计(默认显示南通市区，可选测点)
        /// </summary>
        /// <param name="portIds">站点Id,如:180,204,199</param>
        /// <param name="dtStart">开始时间,如：2017-06-20 4:00:00</param>
        /// <param name="dtEnd">结束时间,如：2017-06-20 23:00:00</param>
        /// <returns>返回数据示例
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取优良数据统计(默认显示南通市区，可选测点)<br />portIds:测点如180,204,不填则为默认值<br />dtStart:开始时间,如：2017-06-20 4:00:00<br />dtEnd:结束时间,如：2017-06-20 4:00:00")]
        public void GetCountData(string portIds, DateTime dtStart, DateTime dtEnd)
        {
            string[] ids;
            if (portIds == "")
            {
                ids = new string[] { "Default" };
            }
            else
            {
                ids = portIds.Split(',');
            }
            string json = m_GetDataClass.GetCountData(ids, dtStart, dtEnd);
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }




        /// <summary>
        /// 获取周边环境点位分布
        /// </summary>
        /// <returns>
        /// </returns>
        /// PointId：站点Id
        /// MonitoringPointUid：站点Guid
        /// MonitoringPointName：站点名称
        /// X：经度
        /// Y：纬度
        /// PName:国控、省控、市控、其他
        /// <returns>返回数据示例
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取周边环境点位分布<br />pId：点位Id,0-上海，1-苏州，2-张家港,3-南通<br />type：查询类型(AQI、IAQI、空、day),day表示日数据<br />fac：因子名称(SO2,NO2,PM10,CO,O3,PM2.5 不填默认PM2.5)<br />只有type为day时，需要传入时间,其他type为了不报错，需要传入2000-10-10<br />dt1：开始日期，2017-8-9<br />dt2：结束日期，2017-8-9")]
        public void GetPortMessageAround(string pId, string type, string fac, DateTime dt1, DateTime dt2)
        {
            if (type.Equals("day"))
            {
                string json = m_GetDataClass.GetPortMessageAround(pId, type != "" ? type : "", fac != "" ? fac : "PM2.5", dt1, dt2);
                Context.Response.Clear();
                Context.Response.ContentType = "application/json";
                Context.Response.Write(json);
                Context.Response.End();
            }
            else
            {
                string json = m_GetDataClass.GetPortMessageAround(pId, type != "" ? type : "", fac != "" ? fac : "PM2.5");
                Context.Response.Clear();
                Context.Response.ContentType = "application/json";
                Context.Response.Write(json);
                Context.Response.End();
            }
        }

        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取周边环境点位分布<br />pId：点位Id,0-上海，1-苏州，2-张家港,3-南通<br />type：查询类型(AQI、IAQI、空、day),day表示日数据<br />fac：因子名称(SO2,NO2,PM10,CO,O3,PM2.5 不填默认PM2.5)<br />只有type为day时，需要传入时间,其他type为了不报错，需要传入2000-10-10<br />dt1：开始日期，2017-8-9<br />dt2：结束日期，2017-8-9")]
        public void GetPortMessageAround_ForAPP(string pId, string type, string fac, DateTime dt1, DateTime dt2)
        {
            if (type.Equals("day"))
            {
                string json = m_GetDataClass.GetPortMessageAround(pId, type != "" ? type : "", fac != "" ? fac : "PM2.5", dt1, dt2).TrimStart('[').TrimEnd(']');
                Context.Response.Clear();
                Context.Response.ContentType = "application/json";
                Context.Response.Write(json);
                Context.Response.End();
            }
            else
            {
                string json = m_GetDataClass.GetPortMessageAround(pId, type != "" ? type : "", fac != "" ? fac : "PM2.5").TrimStart('[').TrimEnd(']');
                Context.Response.Clear();
                Context.Response.ContentType = "application/json";
                Context.Response.Write(json);
                Context.Response.End();
            }
        }

        /// <summary>
        /// 获取所有因子信息
        /// </summary>
        /// <returns>
        /// </returns>
        /// <returns>返回数据示例
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取所有因子信息<br />type：点位类型,1-南通，2-周边城市<br />pointId:站点Id")]
        public void GetFactorByType(string pointId, string id)
        {
            string json = m_GetDataClass.GetFactorByType(pointId, id);
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        /// <summary>
        /// 获取实时浓度
        /// </summary>
        /// <returns>
        /// </returns>
        /// <returns>返回数据示例
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取实时浓度<br />type：1-南通，2-周边城市<br/>pointId:点位Id,0-上海，1-苏州，2-张家港，3-周边城市南通<br />AreaGuid:区域Uid")]
        public void GetLastHourDataByType(string type, string pointId, string AreaGuid)
        {
            string json = m_GetDataClass.GetLastHourDataByType(type, pointId, AreaGuid);
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取实时浓度<br />type：1-南通，2-周边城市<br/>pointId:点位Id,0-上海，1-苏州，2-张家港，3-周边城市南通<br />AreaGuid:区域Uid")]
        public void GetLastHourDataByType_ForAPP(string type, string pointId, string AreaGuid)
        {
            string json = m_GetDataClass.GetLastHourDataByType(type, pointId, AreaGuid).TrimStart('[').TrimEnd(']');
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        /// <summary>
        /// 获取日浓度
        /// </summary>
        /// <returns>
        /// </returns>
        /// <returns>返回数据示例
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取日浓度<br />type：点位Id,0-上海，1-苏州，2-张家港,3-南通,4-周边城市南通，南通查询南通各点位<br />AreaGuid:区域数据当传值为All或者空时，返回所有区域的数据")]
        public void GetLastDayDataByType(string pointId, string AreaGuid)
        {
            string json = m_GetDataClass.GetLastDayDataByType(pointId, AreaGuid);
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        /// <summary>
        /// 获取周边城市的日AQI
        /// </summary>
        /// <returns>
        /// </returns>
        /// <returns>返回数据示例
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取周边城市的日AQI<br />portType：点位Id,0-上海，1-苏州，2-张家港，3-南通<br />qualityType:空气质量类别,1代表优，2代表良，3轻度污染，4中度污染，5重度污染，6严重污染(如：1;2),不填默认查询所有类别<br />dtStart:开始时间,如：2017-6-20<br />dtStart:结束时间,如：2017-6-20<br />OrderType:1-正序，2-倒序")]
        public void GetDayAQIForArount(string portType, string qualityType, DateTime dtStart, DateTime dtEnd, string OrderType)
        {
            string[] arrayType;
            if (qualityType == "")
            {
                arrayType = "优,良,轻度污染,中度污染,重度污染,严重污染".Split(',');
            }
            else
            {
                arrayType = qualityType.Split(';');
            }
            string json = m_GetDataClass.GetDayAQIForArount(portType, arrayType, dtStart, dtEnd, OrderType);
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        /// <summary>
        /// 获取日数据信息
        /// </summary>
        /// <returns>
        /// </returns>
        /// <returns>返回数据示例
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取日数据信息<br />PortId：点位Id,0-上海，1-苏州，2-张家港<br />")]
        public void GetLastDayDataByPortIdForArount(string PortId)
        {
            string json = m_GetDataClass.GetLastDayDataByPortIdForArount(PortId);
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        /// <summary>
        /// 获取周边城市实时AQI
        /// </summary>
        /// <returns>
        /// </returns>
        /// <returns>返回数据示例
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取周边城市实时AQI<br />PortId：点位Id,0-上海，1-苏州，2-张家港,3-南通<br />OrderType:1-正序，2-倒序")]
        public void GetHourAQIForArount(string PortId, string OrderType)
        {
            string json = m_GetDataClass.GetHourAQIForArount(PortId, OrderType);
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取周边城市实时AQI<br />PortId：点位Id,0-上海，1-苏州，2-张家港,3-南通<br />OrderType:1-正序，2-倒序")]
        public void GetHourAQIForArount_ForAPP(string PortId, string OrderType)
        {
            string json = m_GetDataClass.GetHourAQIForArount(PortId, OrderType).TrimStart('[').TrimEnd(']');
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        /// <summary>
        /// 获取点位因子浓度
        /// </summary>
        /// <returns>
        /// </returns>
        /// <returns>返回数据示例
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取点位因子浓度<br />type：1-南通，2-周边城市<br/>pointId:点位Id,0-上海，1-苏州，2-张家港,3-周边城市南通<br />")]
        public void GetFactorByPortId(string type, string PortId)
        {
            string json = string.Empty;
            if (PortId == "0" || PortId == "1" || PortId == "2" || PortId == "3")
            {
                json = m_GetDataClass.GetFactorByPortId(type, PortId);
            }
            else
            {
                json = m_GetDataClass.GetFactorByPortIdNew("1", PortId);
            }
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        /// <summary>
        /// 获取周边城市分类
        /// </summary>
        /// <returns>
        /// </returns>
        /// <returns>返回数据示例
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取周边城市分类<br />")]
        public void GetCityType()
        {
            string json = m_GetDataClass.GetCityType();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取周边城市分类<br />")]
        public void GetCityType_ForAPP()
        {
            string json = m_GetDataClass.GetCityType().TrimStart('[').TrimEnd(']');
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }
        /// <summary>
        /// 获取点位因子日浓度
        /// </summary>
        /// <returns>
        /// </returns>
        /// <returns>返回数据示例
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取点位因子日浓度<br />PointId:点位编号")]
        public void GetDayFactorByPointId(string PointId)
        {
            string json = m_GetDataClass.GetDayFactorByPointId(PointId);
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        /// <summary>
        /// 获取区域
        /// </summary>
        /// <returns>
        /// </returns>
        /// <returns>返回数据示例
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取区域<br />")]
        public void GetArea()
        {
            string json = m_GetDataClass.GetArea();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }

        /// <summary>
        /// 获取周边城市点位因子日均值浓度
        /// </summary>
        /// <returns>
        /// </returns>
        /// <returns>返回数据示例
        /// </returns>
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = @"获取周边城市点位因子日均值浓度<br />pointId:点位Id,0-上海，1-苏州，2-张家港,3-周边城市南通")]
        public void GetDayFactorByPortId(string pointId)
        {
            string json = m_GetDataClass.GetDayFactorByPortId(pointId);
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(json);
            Context.Response.End();
        }
    }
}