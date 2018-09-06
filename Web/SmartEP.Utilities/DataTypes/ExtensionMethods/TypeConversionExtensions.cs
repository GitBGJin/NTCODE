﻿/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

#region Usings
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using SmartEP.Utilities.DataTypes.Comparison;
using System.Linq;
#endregion

namespace SmartEP.Utilities.DataTypes.ExtensionMethods
{
    /// <summary>
    /// Extensions converting between types, checking if something is null, etc.
    /// </summary>
    public static class TypeConversionExtensions
    {
        #region Functions

        #region FormatToString

        /// <summary>
        /// Calls the object's ToString function passing in the formatting
        /// </summary>
        /// <param name="Input">Input object</param>
        /// <param name="Format">Format of the output string</param>
        /// <returns>The formatted string</returns>
        public static string FormatToString(this object Input, string Format)
        {
            if (Input.IsNull())
                return "";
            return !string.IsNullOrEmpty(Format) ? (string)CallMethod("ToString", Input, Format) : Input.ToString();
        }

        #endregion

        #region IsNotDefault

        /// <summary>
        /// Determines if the object is not null
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Object">The object to check</param>
        /// <param name="EqualityComparer">Equality comparer used to determine if the object is equal to default</param>
        /// <returns>False if it is null, true otherwise</returns>
        public static bool IsNotDefault<T>(this T Object, IEqualityComparer<T> EqualityComparer = null)
        {
            return !EqualityComparer.NullCheck(new GenericEqualityComparer<T>()).Equals(Object, default(T));
        }

        #endregion

        #region IsDefault

        /// <summary>
        /// Determines if the object is null
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Object">The object to check</param>
        /// <param name="EqualityComparer">Equality comparer used to determine if the object is equal to default</param>
        /// <returns>True if it is null, false otherwise</returns>
        public static bool IsDefault<T>(this T Object, IEqualityComparer<T> EqualityComparer = null)
        {
            return EqualityComparer.NullCheck(new GenericEqualityComparer<T>()).Equals(Object, default(T));
        }

        #endregion

        #region IsNotNull

        /// <summary>
        /// Determines if the object is not null
        /// </summary>
        /// <param name="Object">The object to check</param>
        /// <returns>False if it is null, true otherwise</returns>
        public static bool IsNotNull(this object Object)
        {
            return Object != null;
        }

        #endregion

        #region IsNull

        /// <summary>
        /// Determines if the object is null
        /// </summary>
        /// <param name="Object">The object to check</param>
        /// <returns>True if it is null, false otherwise</returns>
        public static bool IsNull(this object Object)
        {
            return Object == null;
        }

        #endregion

        #region IsNotNullOrDBNull

        /// <summary>
        /// Determines if the object is not null or DBNull
        /// </summary>
        /// <param name="Object">The object to check</param>
        /// <returns>False if it is null/DBNull, true otherwise</returns>
        public static bool IsNotNullOrDBNull(this object Object)
        {
            return Object != null && !Convert.IsDBNull(Object);
        }

        #endregion

        #region IsNullOrDBNull

        /// <summary>
        /// Determines if the object is null or DBNull
        /// </summary>
        /// <param name="Object">The object to check</param>
        /// <returns>True if it is null/DBNull, false otherwise</returns>
        public static bool IsNullOrDBNull(this object Object)
        {
            return Object == null || Convert.IsDBNull(Object);
        }

        #endregion

        #region NullCheck

        /// <summary>
        /// Does a null check and either returns the default value (if it is null) or the object
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Object">Object to check</param>
        /// <param name="DefaultValue">Default value to return in case it is null</param>
        /// <returns>The default value if it is null, the object otherwise</returns>
        public static T NullCheck<T>(this T Object, T DefaultValue = default(T))
        {
            return Object == null ? DefaultValue : Object;
        }
        #endregion

        #region ThrowIfDefault

        /// <summary>
        /// Determines if the object is equal to default value and throws an ArgumentNullException if it is
        /// </summary>
        /// <param name="Item">The object to check</param>
        /// <param name="EqualityComparer">Equality comparer used to determine if the object is equal to default</param>
        /// <param name="Name">Name of the argument</param>
        /// <returns>Returns Item</returns>
        public static T ThrowIfDefault<T>(this T Item, string Name, IEqualityComparer<T> EqualityComparer = null)
        {
            if (Item.IsDefault(EqualityComparer))
                throw new ArgumentNullException(Name);
            return Item;
        }

        #endregion

        #region ThrowIfNull

        /// <summary>
        /// Determines if the object is null and throws an ArgumentNullException if it is
        /// </summary>
        /// <param name="Item">The object to check</param>
        /// <param name="Name">Name of the argument</param>
        /// <returns>Returns Item</returns>
        public static T ThrowIfNull<T>(this T Item, string Name)
        {
            if (Item.IsNull())
                throw new ArgumentNullException(Name);
            return Item;
        }

        #endregion

        #region ThrowIfNullOrEmpty

        /// <summary>
        /// Determines if the IEnumerable is null or empty and throws an ArgumentNullException if it is
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="Item">The object to check</param>
        /// <param name="Name">Name of the argument</param>
        /// <returns>Returns Item</returns>
        public static IEnumerable<T> ThrowIfNullOrEmpty<T>(this IEnumerable<T> Item, string Name)
        {
            if (Item.IsNullOrEmpty())
                throw new ArgumentNullException(Name);
            return Item;
        }

        #endregion

        #region ThrowIfNullOrDBNull

        /// <summary>
        /// Determines if the object is null or DbNull and throws an ArgumentNullException if it is
        /// </summary>
        /// <param name="Item">The object to check</param>
        /// <param name="Name">Name of the argument</param>
        /// <returns>Returns Item</returns>
        public static T ThrowIfNullOrDBNull<T>(this T Item, string Name)
        {
            if (Item.IsNullOrDBNull())
                throw new ArgumentNullException(Name);
            return Item;
        }

        #endregion

        #region ToSQLDbType

        /// <summary>
        /// Converts a .Net type to SQLDbType value
        /// </summary>
        /// <param name="Type">.Net Type</param>
        /// <returns>The corresponding SQLDbType</returns>
        public static SqlDbType ToSQLDbType(this Type Type)
        {
            return Type.ToDbType().ToSqlDbType();
        }

        /// <summary>
        /// Converts a DbType to a SqlDbType
        /// </summary>
        /// <param name="Type">Type to convert</param>
        /// <returns>The corresponding SqlDbType (if it exists)</returns>
        public static SqlDbType ToSqlDbType(this DbType Type)
        {
            SqlParameter Parameter = new SqlParameter();
            Parameter.DbType = Type;
            return Parameter.SqlDbType;
        }

        #endregion


        #region ToDbType
        /*======================================================================================================   
SQL Server类型与C#类型对应关系
                                     SQLServer类型            C#类型 
                                        bit                     bool 
                                        tinyint                 byte 
                                        smallint                short 
                                        int                     int 
                                        bigint                  long 
                                        real                    float 
                                        float                   double 
                                        money                   decimal 
                                        datetime                DateTime 
                                        char                    string 
                                        varchar                 string 
                                        nchar                   string 
                                        nvarchar                string 
                                        text                    string 
                                        ntext                   string 
                                        image                   byte[] 
                                        binary                  byte[] 
                                        uniqueidentifier        Guid 
 ========================================================================================================*/
        /// <summary>
        /// Converts a .Net type to SqlDbType value
        /// 2012-11-29 by shaoxj 
        /// </summary>
        /// <param name="Type">.Net Type String</param>
        /// <returns>The corresponding DbType</returns>
        public static SqlDbType ToDbTypeFromNetType(string netType)
        {
            SqlDbType dbType = SqlDbType.Variant;//默认为Object

            switch (netType)
            {
                case "int":
                    dbType = SqlDbType.Int;
                    break;
                case "varchar":
                    dbType = SqlDbType.VarChar;
                    break;
                case "bit":
                    dbType = SqlDbType.Bit;
                    break;
                case "datetime":
                    dbType = SqlDbType.DateTime;
                    break;
                case "decimal":
                    dbType = SqlDbType.Decimal;
                    break;
                case "float":
                    dbType = SqlDbType.Float;
                    break;
                case "image":
                    dbType = SqlDbType.Image;
                    break;
                case "money":
                    dbType = SqlDbType.Money;
                    break;
                case "ntext":
                    dbType = SqlDbType.NText;
                    break;
                case "nvarchar":
                    dbType = SqlDbType.NVarChar;
                    break;
                case "smalldatetime":
                    dbType = SqlDbType.SmallDateTime;
                    break;
                case "smallint":
                    dbType = SqlDbType.SmallInt;
                    break;
                case "text":
                    dbType = SqlDbType.Text;
                    break;
                case "bigint":
                    dbType = SqlDbType.BigInt;
                    break;
                case "binary":
                    dbType = SqlDbType.Binary;
                    break;
                case "char":
                    dbType = SqlDbType.Char;
                    break;
                case "nchar":
                    dbType = SqlDbType.NChar;
                    break;
                case "numeric":
                    dbType = SqlDbType.Decimal;
                    break;
                case "real":
                    dbType = SqlDbType.Real;
                    break;
                case "smallmoney":
                    dbType = SqlDbType.SmallMoney;
                    break;
                case "sql_variant":
                    dbType = SqlDbType.Variant;
                    break;
                case "timestamp":
                    dbType = SqlDbType.Timestamp;
                    break;
                case "tinyint":
                    dbType = SqlDbType.TinyInt;
                    break;
                case "uniqueidentifier":
                    dbType = SqlDbType.UniqueIdentifier;
                    break;
                case "varbinary":
                    dbType = SqlDbType.VarBinary;
                    break;
                case "xml":
                    dbType = SqlDbType.Xml;
                    break;
            }
            return dbType;
        }

        /// <summary>
        /// Converts a .Net type to DbType value
        /// </summary>
        /// <param name="Type">.Net Type</param>
        /// <returns>The corresponding DbType</returns>
        public static DbType ToDbType(this Type Type)
        {
            if (Type == typeof(byte)) return DbType.Byte;
            else if (Type == typeof(sbyte)) return DbType.SByte;
            else if (Type == typeof(short)) return DbType.Int16;
            else if (Type == typeof(ushort)) return DbType.UInt16;
            else if (Type == typeof(int)) return DbType.Int32;
            else if (Type == typeof(uint)) return DbType.UInt32;
            else if (Type == typeof(long)) return DbType.Int64;
            else if (Type == typeof(ulong)) return DbType.UInt64;
            else if (Type == typeof(float)) return DbType.Single;
            else if (Type == typeof(double)) return DbType.Double;
            else if (Type == typeof(decimal)) return DbType.Decimal;
            else if (Type == typeof(bool)) return DbType.Boolean;
            else if (Type == typeof(string)) return DbType.String;
            else if (Type == typeof(char)) return DbType.StringFixedLength;
            else if (Type == typeof(Guid)) return DbType.Guid;
            else if (Type == typeof(DateTime)) return DbType.DateTime2;
            else if (Type == typeof(DateTimeOffset)) return DbType.DateTimeOffset;
            else if (Type == typeof(byte[])) return DbType.Binary;
            else if (Type == typeof(byte?)) return DbType.Byte;
            else if (Type == typeof(sbyte?)) return DbType.SByte;
            else if (Type == typeof(short?)) return DbType.Int16;
            else if (Type == typeof(ushort?)) return DbType.UInt16;
            else if (Type == typeof(int?)) return DbType.Int32;
            else if (Type == typeof(uint?)) return DbType.UInt32;
            else if (Type == typeof(long?)) return DbType.Int64;
            else if (Type == typeof(ulong?)) return DbType.UInt64;
            else if (Type == typeof(float?)) return DbType.Single;
            else if (Type == typeof(double?)) return DbType.Double;
            else if (Type == typeof(decimal?)) return DbType.Decimal;
            else if (Type == typeof(bool?)) return DbType.Boolean;
            else if (Type == typeof(char?)) return DbType.StringFixedLength;
            else if (Type == typeof(Guid?)) return DbType.Guid;
            else if (Type == typeof(DateTime?)) return DbType.DateTime2;
            else if (Type == typeof(DateTimeOffset?)) return DbType.DateTimeOffset;
            return DbType.Int32;
        }

        /// <summary>
        /// Converts SqlDbType to DbType
        /// </summary>
        /// <param name="Type">Type to convert</param>
        /// <returns>The corresponding DbType (if it exists)</returns>
        public static DbType ToDbType(this SqlDbType Type)
        {
            SqlParameter Parameter = new SqlParameter();
            Parameter.SqlDbType = Type;
            return Parameter.DbType;
        }

        #endregion

        #region ToList

        /// <summary>
        /// Attempts to convert the DataTable to a list of objects
        /// </summary>
        /// <typeparam name="T">Type the objects should be in the list</typeparam>
        /// <param name="Data">DataTable to convert</param>
        /// <param name="Creator">Function used to create each object</param>
        /// <returns>The DataTable converted to a list of objects</returns>
        public static System.Collections.Generic.List<T> ToList<T>(this DataTable Data, Func<T> Creator = null) where T : new()
        {
            if (Data.IsNull())
                return new List<T>();
            Creator = Creator.NullCheck(() => new T());
            Type TType = typeof(T);
            PropertyInfo[] Properties = TType.GetProperties();
            System.Collections.Generic.List<T> Results = new System.Collections.Generic.List<T>();
            for (int x = 0; x < Data.Rows.Count; ++x)
            {
                T RowObject = Creator();

                for (int y = 0; y < Data.Columns.Count; ++y)
                {
                    PropertyInfo Property = Properties.FirstOrDefault(z => z.Name == Data.Columns[y].ColumnName);
                    if (!Property.IsNull())
                        Property.SetValue(RowObject, Data.Rows[x][Data.Columns[y]].TryTo(Property.PropertyType, null), new object[] { });
                }
                Results.Add(RowObject);
            }
            return Results;
        }

        #endregion

        #region ToType

        /// <summary>
        /// Converts a SQLDbType value to .Net type
        /// </summary>
        /// <param name="Type">SqlDbType Type</param>
        /// <returns>The corresponding .Net type</returns>
        public static Type ToType(this SqlDbType Type)
        {
            return Type.ToDbType().ToType();
        }

        /// <summary>
        /// Converts a DbType value to .Net type
        /// </summary>
        /// <param name="Type">DbType</param>
        /// <returns>The corresponding .Net type</returns>
        public static Type ToType(this DbType Type)
        {
            if (Type == DbType.Byte) return typeof(byte);
            else if (Type == DbType.SByte) return typeof(sbyte);
            else if (Type == DbType.Int16) return typeof(short);
            else if (Type == DbType.UInt16) return typeof(ushort);
            else if (Type == DbType.Int32) return typeof(int);
            else if (Type == DbType.UInt32) return typeof(uint);
            else if (Type == DbType.Int64) return typeof(long);
            else if (Type == DbType.UInt64) return typeof(ulong);
            else if (Type == DbType.Single) return typeof(float);
            else if (Type == DbType.Double) return typeof(double);
            else if (Type == DbType.Decimal) return typeof(decimal);
            else if (Type == DbType.Boolean) return typeof(bool);
            else if (Type == DbType.String) return typeof(string);
            else if (Type == DbType.StringFixedLength) return typeof(char);
            else if (Type == DbType.Guid) return typeof(Guid);
            else if (Type == DbType.DateTime2) return typeof(DateTime);
            else if (Type == DbType.DateTime) return typeof(DateTime);
            else if (Type == DbType.DateTimeOffset) return typeof(DateTimeOffset);
            else if (Type == DbType.Binary) return typeof(byte[]);
            return typeof(int);
        }

        /// <summary>
        /// Converts a SqlDbType value to .Net type 
        /// 2012-11-29 by shaoxj 
        /// </summary>
        /// <param name="Type">SqlDbType</param>
        /// <returns>The corresponding .Net type</returns>
        public static Type ToTypeFromSqlType(this SqlDbType Type)
        {
            switch (Type)
            {
                case SqlDbType.BigInt:
                    return typeof(Int64);
                case SqlDbType.Binary:
                    return typeof(Object);
                case SqlDbType.Bit:
                    return typeof(Boolean);
                case SqlDbType.Char:
                    return typeof(String);
                case SqlDbType.DateTime:
                    return typeof(DateTime);
                case SqlDbType.Decimal:
                    return typeof(Decimal);
                case SqlDbType.Float:
                    return typeof(Double);
                case SqlDbType.Image:
                    return typeof(Object);
                case SqlDbType.Int:
                    return typeof(Int32);
                case SqlDbType.Money:
                    return typeof(Decimal);
                case SqlDbType.NChar:
                    return typeof(String);
                case SqlDbType.NText:
                    return typeof(String);
                case SqlDbType.NVarChar:
                    return typeof(String);
                case SqlDbType.Real:
                    return typeof(Single);
                case SqlDbType.SmallDateTime:
                    return typeof(DateTime);
                case SqlDbType.SmallInt:
                    return typeof(Int16);
                case SqlDbType.SmallMoney:
                    return typeof(Decimal);
                case SqlDbType.Text:
                    return typeof(String);
                case SqlDbType.Timestamp:
                    return typeof(Object);
                case SqlDbType.TinyInt:
                    return typeof(Byte);
                case SqlDbType.Udt://自定义的数据类型
                    return typeof(Object);
                case SqlDbType.UniqueIdentifier:
                    return typeof(Object);
                case SqlDbType.VarBinary:
                    return typeof(Object);
                case SqlDbType.VarChar:
                    return typeof(String);
                case SqlDbType.Variant:
                    return typeof(Object);
                case SqlDbType.Xml:
                    return typeof(Object);
                default:
                    return typeof(int);
            }
        }

        #endregion

        #region TryTo

        /// <summary>
        /// Attempts to convert the object to another type and returns the value
        /// </summary>
        /// <typeparam name="T">Type to convert from</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="Object">Object to convert</param>
        /// <param name="DefaultValue">Default value to return if there is an issue or it can't be converted</param>
        /// <returns>The object converted to the other type or the default value if there is an error or can't be converted</returns>
        public static R TryTo<T, R>(this T Object, R DefaultValue = default(R))
        {
            try
            {
                return (R)Object.TryTo(typeof(R), DefaultValue);
            }
            catch { }
            return DefaultValue;
        }

        /// <summary>
        /// Attempts to convert the object to another type and returns the value
        /// </summary>
        /// <typeparam name="T">Type to convert from</typeparam>
        /// <param name="ResultType">Result type</param>
        /// <param name="Object">Object to convert</param>
        /// <param name="DefaultValue">Default value to return if there is an issue or it can't be converted</param>
        /// <returns>The object converted to the other type or the default value if there is an error or can't be converted</returns>
        public static object TryTo<T>(this T Object, Type ResultType, object DefaultValue = null)
        {
            try
            {
                if (Object.IsNullOrDBNull())
                    return DefaultValue;
                if ((Object as string).IsNotNull())
                {
                    string ObjectValue = Object as string;
                    if (ResultType.IsEnum)
                        return System.Enum.Parse(ResultType, ObjectValue, true);
                    if (ObjectValue.IsNullOrEmpty())
                        return DefaultValue;
                    if (ResultType == typeof(System.Guid))
                        return new Guid(ObjectValue);
                }
                
                if ((Object as IConvertible).IsNotNull())
                    return Convert.ChangeType(Object, ResultType);
                if (ResultType.IsAssignableFrom(Object.GetType()))
                    return Object;
                TypeConverter Converter = TypeDescriptor.GetConverter(Object.GetType());
                if (Converter.CanConvertTo(ResultType))
                    return Converter.ConvertTo(Object, ResultType);
                if ((Object as string).IsNotNull())
                    return Object.ToString().TryTo<string>(ResultType, DefaultValue);
            }
            catch { }
            return DefaultValue;
        }

        #endregion

        #endregion

        #region Private Static Functions

        /// <summary>
        /// Calls a method on an object
        /// </summary>
        /// <param name="MethodName">Method name</param>
        /// <param name="Object">Object to call the method on</param>
        /// <param name="InputVariables">(Optional)input variables for the method</param>
        /// <returns>The returned value of the method</returns>
        private static object CallMethod(string MethodName, object Object, params object[] InputVariables)
        {
            if (string.IsNullOrEmpty(MethodName) || Object.IsNull())
                return null;
            Type ObjectType = Object.GetType();
            MethodInfo Method = null;
            if (InputVariables.IsNotNull())
            {
                Type[] MethodInputTypes = new Type[InputVariables.Length];
                for (int x = 0; x < InputVariables.Length; ++x)
                    MethodInputTypes[x] = InputVariables[x].GetType();
                Method = ObjectType.GetMethod(MethodName, MethodInputTypes);
                if (Method != null)
                    return Method.Invoke(Object, InputVariables);
            }
            Method = ObjectType.GetMethod(MethodName);
            return Method.IsNull() ? null : Method.Invoke(Object, null);
        }

        #endregion
    }
}