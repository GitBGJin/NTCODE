﻿
#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Security;
#endregion

namespace SmartEP.Utilities.IO.ExtensionMethods
{
    /// <summary>
    /// Serialization extensions
    /// </summary>
    public static class SerializationExtensions
    {
        #region Extension Methods

        #region ToBinary

        /// <summary>
        /// Converts an object to Binary
        /// </summary>
        /// <param name="Object">Object to convert</param>
        /// <param name="FileToSaveTo">File to save the XML to (optional)</param>
        /// <returns>The object converted to a JSON string</returns>
        public static byte[] ToBinary(this object Object, string FileToSaveTo = "")
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            byte[] Output = new byte[0];
            using (MemoryStream Stream = new MemoryStream())
            {
                BinaryFormatter Formatter = new BinaryFormatter();
                Formatter.Serialize(Stream, Object);
                Output = Stream.ToArray();
            }
            if (!string.IsNullOrEmpty(FileToSaveTo))
                new FileInfo(FileToSaveTo).Save(Output);
            return Output;
        }

        #endregion

        #region ToJSON

        /// <summary>
        /// Converts an object to JSON
        /// </summary>
        /// <param name="Object">Object to convert</param>
        /// <param name="FileToSaveTo">File to save the XML to (optional)</param>
        /// <param name="EncodingUsing">Encoding that the XML should be saved/returned as (defaults to ASCII)</param>
        /// <returns>The object converted to a JSON string</returns>
        public static string ToJSON(this object Object,string FileToSaveTo="", Encoding EncodingUsing = null)
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            if (EncodingUsing == null)
                EncodingUsing = new ASCIIEncoding();
            string JSON = "";
            using (MemoryStream Stream = new MemoryStream())
            {
                DataContractJsonSerializer Serializer = new DataContractJsonSerializer(Object.GetType());
                Serializer.WriteObject(Stream, Object);
                Stream.Flush();
                JSON=EncodingUsing.GetString(Stream.GetBuffer(), 0, (int)Stream.Position);
            }
            if (!string.IsNullOrEmpty(FileToSaveTo))
                new FileInfo(FileToSaveTo).Save(JSON);
            return JSON;
        }

        #endregion

        #region ToSOAP

        /// <summary>
        /// Converts an object to a SOAP string
        /// </summary>
        /// <param name="Object">Object to serialize</param>
        /// <param name="FileToSaveTo">File to save the XML to (optional)</param>
        /// <param name="EncodingUsing">Encoding that the XML should be saved/returned as (defaults to ASCII)</param>
        /// <returns>The serialized string</returns>
        [SecuritySafeCritical]
        public static string ToSOAP(this object Object, string FileToSaveTo = "", Encoding EncodingUsing = null)
        {
            if (Object == null)
                throw new ArgumentException("Object can not be null");
            if (EncodingUsing == null)
                EncodingUsing = new ASCIIEncoding();
            string SOAP = "";
            using (MemoryStream Stream = new MemoryStream())
            {
                SoapFormatter Serializer = new SoapFormatter();
                Serializer.Serialize(Stream, Object);
                Stream.Flush();
                SOAP = EncodingUsing.GetString(Stream.GetBuffer(), 0, (int)Stream.Position);
            }
            if (!string.IsNullOrEmpty(FileToSaveTo))
                new FileInfo(FileToSaveTo).Save(SOAP);
            return SOAP;
        }

        #endregion

        #region ToXML

        /// <summary>
        /// Converts an object to XML
        /// </summary>
        /// <param name="Object">Object to convert</param>
        /// <param name="FileToSaveTo">File to save the XML to (optional)</param>
        /// <param name="EncodingUsing">Encoding that the XML should be saved/returned as (defaults to ASCII)</param>
        /// <returns>string representation of the object in XML format</returns>
        public static string ToXML(this object Object, string FileToSaveTo , Encoding EncodingUsing = null)
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            if (EncodingUsing == null)
                EncodingUsing = new ASCIIEncoding();
            string XML = "";
            using (MemoryStream Stream = new MemoryStream())
            {
                XmlSerializer Serializer = new XmlSerializer(Object.GetType());
                Serializer.Serialize(Stream, Object);
                Stream.Flush();
                XML = EncodingUsing.GetString(Stream.GetBuffer(), 0, (int)Stream.Position);
            }
            if (!string.IsNullOrEmpty(FileToSaveTo))
                new FileInfo(FileToSaveTo).Save(XML);
            return XML;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filename">文件路径</param>
        public static bool ToXML(object obj, string filePath)
        {
            bool success = false;

            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
                success = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return success;

        }

        #endregion

        #region ToObject

        /// <summary>
        /// Converts the serialized byte array into an object
        /// </summary>
        /// <param name="Content">The byte array to convert</param>
        /// <typeparam name="T">Object type to return</typeparam>
        /// <returns>The byte array converted to the specified object type</returns>
        public static T ToObject<T>(this byte[] Content)
        {
            return (Content == null) ? default(T) : (T)Content.ToObject(typeof(T));
        }

        /// <summary>
        /// Converts the serialized byte array into an object
        /// </summary>
        /// <param name="Content">The byte array to convert</param>
        /// <param name="ObjectType">Object type to return</param>
        /// <returns>The byte array converted to the specified object type</returns>
        public static object ToObject(this byte[] Content, Type ObjectType)
        {
            if (Content == null)
                return null;
            using (MemoryStream Stream = new MemoryStream(Content))
            {
                BinaryFormatter Formatter = new BinaryFormatter();
                return Formatter.Deserialize(Stream);
            }
        }

        /// <summary>
        /// Converts the serialized XML document into an object
        /// </summary>
        /// <param name="Content">The XML document to convert</param>
        /// <param name="EncodingUsing">Encoding to use (defaults to ASCII)</param>
        /// <typeparam name="T">Object type to return</typeparam>
        /// <returns>The XML document converted to the specified object type</returns>
        public static T ToObject<T>(this XmlDocument Content, Encoding EncodingUsing = null)
        {
            return (Content == null) ? default(T) : (T)Content.InnerXml.XMLToObject(typeof(T), EncodingUsing);
        }

        /// <summary>
        /// Converts the serialized XML document into an object
        /// </summary>
        /// <param name="Content">The XML document to convert</param>
        /// <param name="EncodingUsing">Encoding to use (defaults to ASCII)</param>
        /// <param name="ObjectType">Object type to return</param>
        /// <returns>The XML document converted to the specified object type</returns>
        public static object ToObject(this XmlDocument Content,Type ObjectType, Encoding EncodingUsing = null)
        {
            return (Content == null) ? null : Content.InnerXml.XMLToObject(ObjectType, EncodingUsing);
        }

        #endregion

        #region JSONToObject

        /// <summary>
        /// Converts a JSON string to an object of the specified type
        /// </summary>
        /// <typeparam name="T">Object type to return</typeparam>
        /// <param name="Content">The string to convert</param>
        /// <param name="EncodingUsing">Encoding to use (defaults to ASCII)</param>
        /// <returns>The string converted to the specified object type</returns>
        public static T JSONToObject<T>(this string Content, Encoding EncodingUsing = null)
        {
            return string.IsNullOrEmpty(Content) ? default(T) : (T)Content.JSONToObject(typeof(T), EncodingUsing);
        }

        /// <summary>
        /// Converts a JSON file to an object of the specified type
        /// </summary>
        /// <typeparam name="T">Object type to return</typeparam>
        /// <param name="Content">The file to convert</param>
        /// <param name="EncodingUsing">Encoding to use (defaults to ASCII)</param>
        /// <returns>The file converted to the specified object type</returns>
        public static T JSONToObject<T>(this FileInfo Content, Encoding EncodingUsing = null)
        {
            return (Content == null || !Content.Exists) ? default(T) : (T)Content.Read().JSONToObject(typeof(T), EncodingUsing);
        }

        /// <summary>
        /// Converts a JSON string to an object of the specified type
        /// </summary>
        /// <param name="ObjectType">Object type to return</param>
        /// <param name="Content">The string to convert</param>
        /// <param name="EncodingUsing">Encoding to use (defaults to ASCII)</param>
        /// <returns>The string converted to the specified object type</returns>
        public static object JSONToObject(this string Content, Type ObjectType, Encoding EncodingUsing = null)
        {
            if (string.IsNullOrEmpty(Content))
                return null;
            if (EncodingUsing == null)
                EncodingUsing = new ASCIIEncoding();
            using (MemoryStream Stream = new MemoryStream(EncodingUsing.GetBytes(Content)))
            {
                DataContractJsonSerializer Serializer = new DataContractJsonSerializer(ObjectType);
                return Serializer.ReadObject(Stream);
            }
        }

        /// <summary>
        /// Converts a JSON file to an object of the specified type
        /// </summary>
        /// <param name="ObjectType">Object type to return</param>
        /// <param name="Content">The file to convert</param>
        /// <param name="EncodingUsing">Encoding to use (defaults to ASCII)</param>
        /// <returns>The file converted to the specified object type</returns>
        public static object JSONToObject(this FileInfo Content, Type ObjectType, Encoding EncodingUsing = null)
        {
            return (Content == null || !Content.Exists) ? null : Content.Read().JSONToObject(ObjectType, EncodingUsing);
        }

        #endregion

        #region SOAPToObject

        /// <summary>
        /// Converts a SOAP string to an object of the specified type
        /// </summary>
        /// <typeparam name="T">Object type to return</typeparam>
        /// <param name="Content">The string to convert</param>
        /// <param name="EncodingUsing">Encoding to use (defaults to ASCII)</param>
        /// <returns>The string converted to the specified object type</returns>
        public static T SOAPToObject<T>(this string Content, Encoding EncodingUsing = null)
        {
            return string.IsNullOrEmpty(Content) ? default(T) : (T)Content.SOAPToObject(typeof(T), EncodingUsing);
        }

        /// <summary>
        /// Converts a SOAP file to an object of the specified type
        /// </summary>
        /// <typeparam name="T">Object type to return</typeparam>
        /// <param name="Content">The file to convert</param>
        /// <param name="EncodingUsing">Encoding to use (defaults to ASCII)</param>
        /// <returns>The file converted to the specified object type</returns>
        public static T SOAPToObject<T>(this FileInfo Content, Encoding EncodingUsing = null)
        {
            return (Content == null || !Content.Exists) ? default(T) : (T)Content.Read().SOAPToObject(typeof(T), EncodingUsing);
        }

        /// <summary>
        /// Converts a SOAP string to an object of the specified type
        /// </summary>
        /// <param name="ObjectType">Object type to return</param>
        /// <param name="Content">The string to convert</param>
        /// <param name="EncodingUsing">Encoding to use (defaults to ASCII)</param>
        /// <returns>The string converted to the specified object type</returns>
        [SecuritySafeCritical]
        public static object SOAPToObject(this string Content,Type ObjectType, Encoding EncodingUsing = null)
        {
            if (string.IsNullOrEmpty(Content))
                return null;
            if (EncodingUsing == null)
                EncodingUsing = new ASCIIEncoding();
            using (MemoryStream Stream = new MemoryStream(EncodingUsing.GetBytes(Content)))
            {
                SoapFormatter Formatter = new SoapFormatter();
                return Formatter.Deserialize(Stream);
            }
        }

        /// <summary>
        /// Converts a SOAP file to an object of the specified type
        /// </summary>
        /// <param name="ObjectType">Object type to return</param>
        /// <param name="Content">The file to convert</param>
        /// <param name="EncodingUsing">Encoding to use (defaults to ASCII)</param>
        /// <returns>The file converted to the specified object type</returns>
        public static object SOAPToObject(this FileInfo Content, Type ObjectType, Encoding EncodingUsing = null)
        {
            return (Content == null || !Content.Exists) ? null : Content.Read().SOAPToObject(ObjectType, EncodingUsing);
        }

        #endregion

        #region XMLToObject

        /// <summary>
        /// Converts a string to an object of the specified type
        /// </summary>
        /// <typeparam name="T">Object type to return</typeparam>
        /// <param name="Content">The string to convert</param>
        /// <param name="EncodingUsing">Encoding to use (defaults to ASCII)</param>
        /// <returns>The string converted to the specified object type</returns>
        public static T XMLToObject<T>(this string Content, Encoding EncodingUsing = null)
        {
            return string.IsNullOrEmpty(Content) ? default(T) : (T)Content.XMLToObject(typeof(T), EncodingUsing);
        }

        /// <summary>
        /// Converts a FileInfo object to an object of the specified type
        /// </summary>
        /// <typeparam name="T">Object type to return</typeparam>
        /// <param name="Content">The file to convert</param>
        /// <param name="EncodingUsing">Encoding to use (defaults to ASCII)</param>
        /// <returns>The file converted to the specified object type</returns>
        public static T XMLToObject<T>(this FileInfo Content, Encoding EncodingUsing = null)
        {
            return (Content == null || !Content.Exists) ? default(T) : (T)Content.Read().XMLToObject(typeof(T), EncodingUsing);
        }

        /// <summary>
        /// Converts a string to an object of the specified type
        /// </summary>
        /// <param name="ObjectType">Object type to return</param>
        /// <param name="Content">The string to convert</param>
        /// <param name="EncodingUsing">Encoding to use (defaults to ASCII)</param>
        /// <returns>The string converted to the specified object type</returns>
        public static object XMLToObject(this string Content,Type ObjectType, Encoding EncodingUsing = null)
        {
            if (string.IsNullOrEmpty(Content))
                return null;
            if (EncodingUsing == null)
                EncodingUsing = new UTF8Encoding();
            using (MemoryStream Stream = new MemoryStream(EncodingUsing.GetBytes(Content)))
            {
                XmlSerializer Serializer = new XmlSerializer(ObjectType);
                return Serializer.Deserialize(Stream);
            }
        }

        /// <summary>
        /// Converts a FileInfo object to an object of the specified type
        /// </summary>
        /// <param name="ObjectType">Object type to return</param>
        /// <param name="Content">The file to convert</param>
        /// <param name="EncodingUsing">Encoding to use (defaults to ASCII)</param>
        /// <returns>The file converted to the specified object type</returns>
        public static object XMLToObject(this FileInfo Content, Type ObjectType, Encoding EncodingUsing = null)
        {
            return (Content == null || !Content.Exists) ? null : Content.Read().XMLToObject(ObjectType, EncodingUsing);
        }

        #endregion

        #endregion
    }
}
