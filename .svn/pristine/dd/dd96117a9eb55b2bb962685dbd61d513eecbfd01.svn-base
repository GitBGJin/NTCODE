using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;

namespace SmartEP.WebService.Internal.Common
{
    /// <summary>
    /// 名称：DataTurned.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-10-20
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：数据转换类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public static class DataTurned
    {
        #region 数据集合转换成Json格式
        /// <summary>   
        /// Datatable转换为Json   
        /// </summary>   
        /// <param name="table">Datatable对象</param>   
        /// <returns>Json字符串</returns>   
        public static string ToJsonBySplitJoint(this DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    string strValue = drc[i][j].ToString();
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            if (jsonString.ToString() != "[") jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }

        /// <summary>   
        /// DataSet转换为Json   
        /// </summary>   
        /// <param name="table">Datatable对象</param>   
        /// <returns>Json字符串</returns>   
        public static string ToJsonBySplitJoint(this DataSet ds)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            foreach (DataTable dt in ds.Tables)
            {
                jsonString.Append("{\"TableName\":\"" + dt.TableName + "\",");
                DataRowCollection drc = dt.Rows;
                jsonString.Append("\"Data\":[");
                for (int i = 0; i < drc.Count; i++)
                {
                    jsonString.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        string strKey = dt.Columns[j].ColumnName;
                        string strValue = drc[i][j].ToString();
                        Type type = dt.Columns[j].DataType;
                        jsonString.Append("\"" + strKey + "\":");
                        strValue = StringFormat(strValue, type);
                        if (j < dt.Columns.Count - 1)
                        {
                            jsonString.Append(strValue + ",");
                        }
                        else
                        {
                            jsonString.Append(strValue);
                        }
                    }
                    jsonString.Append("},");
                }
                if (drc.Count > 0)
                {
                    jsonString.Remove(jsonString.Length - 1, 1);
                }
                jsonString.Append("]},");
            }
            if (jsonString.ToString() != "[") jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }

        /// <summary>  
        /// DataTable转成Json   
        /// </summary>  
        /// <param name="jsonName"></param>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static string ToJsonBySplitJoint(this DataTable dt, string jsonName)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName))
                jsonName = dt.TableName;
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Type type = dt.Rows[i][j].GetType();
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + StringFormat(dt.Rows[i][j].ToString(), type));
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }

        /// <summary>  
        /// 格式化字符型、日期型、布尔型  
        /// </summary>  
        /// <param name="str"></param>  
        /// <param name="type"></param>  
        /// <returns></returns>  
        private static string StringFormat(this string str, Type type)
        {
            if (type == typeof(string) || type == typeof(Guid))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            else if (string.IsNullOrEmpty(str))
            {
                str = "null";
            }
            return str;
        }

        /// <summary>  
        /// 过滤特殊字符  
        /// </summary>  
        /// <param name="s"></param>  
        /// <returns></returns>  
        private static string String2Json(this String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }
        #endregion

        #region DataTable 转换为Json 字符串
        /// <summary>
        /// DataTable 对象 转换为Json 字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToJsonBySerialize(this DataTable dt)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
            ArrayList arrayList = new ArrayList();
            foreach (DataRow dataRow in dt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    dictionary.Add(dataColumn.ColumnName, ToStr(dataRow[dataColumn.ColumnName]));//.ToStr());
                }
                arrayList.Add(dictionary); //ArrayList集合中添加键值
            }

            return javaScriptSerializer.Serialize(arrayList);  //返回一个json字符串
        }
        #endregion

        #region Json 字符串 转换为 DataTable数据集合
        /// <summary>
        /// Json 字符串 转换为 DataTable数据集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable ToDataTableByDeserialize(this string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }

                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch
            {
            }
            result = dataTable;
            return result;
        }
        #endregion

        #region 转换为string字符串类型
        /// <summary>
        ///  转换为string字符串类型
        /// </summary>
        /// <param name="s">获取需要转换的值</param>
        /// <param name="format">需要格式化的位数</param>
        /// <returns>返回一个新的字符串</returns>
        public static string ToStr(this object s, string format = "")
        {
            string result = "";
            try
            {
                if (format == "")
                {
                    result = s.ToString();
                }
                else
                {
                    result = string.Format("{0:" + format + "}", s);
                }
            }
            catch
            {
            }
            return result;
        }
        #endregion

        #region DataContractJsonSerializer
        ///// <summary>
        ///// 对象转换成json
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="jsonObject">需要格式化的对象</param>
        ///// <returns>Json字符串</returns>
        //public static string DataContractJsonSerialize<T>(T jsonObject)
        //{
        //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
        //    string json = null;
        //    using (MemoryStream ms = new MemoryStream()) //定义一个stream用来存发序列化之后的内容
        //    {
        //        serializer.WriteObject(ms, jsonObject);
        //        json = Encoding.UTF8.GetString(ms.GetBuffer()); //将stream读取成一个字符串形式的数据，并且返回
        //        ms.Close();
        //    }
        //    return json;
        //}

        ///// <summary>
        ///// json字符串转换成对象
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="json">要转换成对象的json字符串</param>
        ///// <returns></returns>
        //public static T DataContractJsonDeserialize<T>(string json)
        //{
        //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
        //    T obj = default(T);
        //    using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
        //    {
        //        obj = (T)serializer.ReadObject(ms);
        //        ms.Close();
        //    }
        //    return obj;
        //}
        #endregion

        /// <summary>  
        /// 将JSON解析成DataSet（只限标准的JSON数据）  
        /// 例如：Json＝{t1:[{name:'数据name',type:'数据type'}]} 或 Json＝{t1:[{name:'数据name',type:'数据type'}],t2:[{id:'数据id',gx:'数据gx',val:'数据val'}]}  
        /// </summary>  
        /// <param name="json">Json字符串</param>  
        /// <returns>DataSet</returns>  
        public static DataSet JsonToDataSet(this string json)
        {
            try
            {
                var ds = new DataSet();
                var jss = new JavaScriptSerializer();
                object obj = jss.DeserializeObject(json);
                var datajson = (Dictionary<string, object>)obj;
                foreach (var item in datajson)
                {
                    var dt = new DataTable(item.Key);
                    var rows = (object[])item.Value;
                    foreach (object row in rows)
                    {
                        var val = (Dictionary<string, object>)row;
                        DataRow dr = dt.NewRow();
                        foreach (var sss in val)
                        {
                            if (!dt.Columns.Contains(sss.Key))
                            {
                                dt.Columns.Add(sss.Key);
                                dr[sss.Key] = sss.Value;
                            }
                            else
                                dr[sss.Key] = sss.Value;
                        }
                        dt.Rows.Add(dr);
                    }
                    ds.Tables.Add(dt);
                }
                return ds;
            }
            catch
            {
                return null;
            }
        }
        public static DataSet JsonToDataSetAir(this string json)
        {
            try
            {
                var ds = new DataSet();
                var jss = new JavaScriptSerializer();
                object obj = jss.DeserializeObject(json);
                var datajson = (Dictionary<string, object>)obj;
                foreach (var item in datajson)
                {
                    var dt = new DataTable(item.Key);
                    var rows = (object[])item.Value;
                    foreach (object row in rows)
                    {
                        var val = (Dictionary<string, object>)row;
                        DataRow dr = dt.NewRow();
                        foreach (var sss in val)
                        {
                            if (!dt.Columns.Contains(sss.Key))
                            {
                                if (IsGuidByArr(sss.Value.ToString()))
                                {
                                    dt.Columns.Add(sss.Key, typeof(System.Data.SqlTypes.SqlGuid));
                                    dr[sss.Key] = new Guid(sss.Value.ToString());
                                }
                                else
                                {
                                    dt.Columns.Add(sss.Key);
                                    dr[sss.Key] = sss.Value;
                                }
                            }
                            else
                            {
                                if (IsGuidByArr(sss.Value.ToString()))
                                {
                                    dr[sss.Key] = new Guid(sss.Value.ToString());
                                }
                                else
                                {
                                    dr[sss.Key] = sss.Value;
                                }
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                    ds.Tables.Add(dt);
                }
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>  
        /// 判断字符串是否为GUID格式  
        /// </summary>  
        /// <param name="strSrc">字符串</param>  
        /// <returns>DataSet</returns>  
        static bool IsGuidByArr(string strSrc)
        {
            if (String.IsNullOrEmpty(strSrc) || strSrc.Length != 36) { return false; }
            string[] arr = strSrc.Split('-');
            if (arr.Length != 5) { return false; }
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr[i].Length; j++)
                {
                    char a = arr[i][j];
                    if (!((a >= 48 && a <= 57) || (a >= 65 && a <= 90) || (a >= 97 && a <= 122)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>  
        /// 将DataSet转化成JSON数据  
        /// </summary>  
        /// <param name="ds"></param>  
        /// <returns></returns>  
        public static string DataSetToJson(this DataSet ds)
        {
            string json;
            try
            {
                if (ds.Tables.Count == 0)
                    throw new Exception("DataSet中Tables为0");
                json = "{";
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    string tableName = string.IsNullOrWhiteSpace(dt.TableName) ? "T" + (i + 1) : dt.TableName;
                    json += tableName + ":[";
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        json += "{";
                        for (int k = 0; k < dt.Columns.Count; k++)
                        {
                            json += dt.Columns[k].ColumnName + ":'" + dt.Rows[j][k] + "'";
                            if (k != dt.Columns.Count - 1)
                                json += ",";
                        }
                        json += "}";
                        if (j != dt.Rows.Count - 1)
                            json += ",";
                    }
                    json += "]";
                    if (i != ds.Tables.Count - 1)
                        json += ",";
                }
                json += "}";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return json;
        }

        /// <summary>  
        ///     json字符串转换为Xml对象  
        /// </summary>  
        /// <param name="sJson"></param>  
        /// <returns></returns>  
        public static XmlDocument JsonToXml(this string sJson)
        {
            var serializer = new JavaScriptSerializer();
            var dic = (Dictionary<string, object>)serializer.DeserializeObject(sJson);
            var doc = new XmlDocument();
            XmlDeclaration xmlDec = doc.CreateXmlDeclaration("1.0", "gb2312", "yes");
            doc.InsertBefore(xmlDec, doc.DocumentElement);
            XmlElement root = doc.CreateElement("root");
            doc.AppendChild(root);
            foreach (var item in dic)
            {
                XmlElement element = doc.CreateElement(item.Key);
                KeyValue2Xml(element, item);
                root.AppendChild(element);
            }
            return doc;
        }

        private static void KeyValue2Xml(XmlElement node, KeyValuePair<string, object> source)
        {
            object kValue = source.Value;
            if (kValue.GetType() == typeof(Dictionary<string, object>))
            {
                var dictionary = kValue as Dictionary<string, object>;
                if (dictionary != null)
                    foreach (var item in dictionary)
                    {
                        if (node.OwnerDocument != null)
                        {
                            XmlElement element = node.OwnerDocument.CreateElement(item.Key);
                            KeyValue2Xml(element, item);
                            node.AppendChild(element);
                        }
                    }
            }
            else if (kValue.GetType() == typeof(object[]))
            {
                var o = kValue as object[];
                if (o != null)
                    foreach (object t in o)
                    {
                        if (node.OwnerDocument != null)
                        {
                            XmlElement xitem = node.OwnerDocument.CreateElement("Item");
                            var item = new KeyValuePair<string, object>("Item", t);
                            KeyValue2Xml(xitem, item);
                            node.AppendChild(xitem);
                        }
                    }
            }
            else
            {
                if (node.OwnerDocument != null)
                {
                    XmlText text = node.OwnerDocument.CreateTextNode(kValue.ToString());
                    node.AppendChild(text);
                }
            }
        }

        #region 字符串和二进制流互相转换
        /// <summary>
        /// 二进制流转换为字符串
        /// </summary>
        /// <param name="byteArray">二进制流</param>
        /// <returns></returns>
        public static string ToBase64StringByConvert(this byte[] byteArray)
        {
            string strContent;
            if (byteArray == null || byteArray.Length == 0)
            {
                strContent = string.Empty;
            }
            else
            {
                strContent = Convert.ToBase64String(byteArray);
            }
            return strContent;
        }

        /// <summary>
        /// 字符串转换为二进制流
        /// </summary>
        /// <param name="strContent">字符串</param>
        /// <returns></returns>
        public static byte[] ToByteArrayByConvert(this string strContent)
        {
            byte[] byteArray;
            if (string.IsNullOrWhiteSpace(strContent))
            {
                byteArray = new byte[0];
            }
            else
            {
                byteArray = Convert.FromBase64String(strContent);//byte[] bytes = System.Text.Encoding.Default.GetBytes(content); 
            }
            return byteArray;
        }
        #endregion
    }
}