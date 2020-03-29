﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using TlhPlatform.Core.Reflection;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Xml.Linq;
using System.Data;
using System.Collections;
using System.Linq.Expressions;
using System.Xml;
using Newtonsoft.Json.Linq;

namespace TlhPlatform.Core.Extensions
{
    /// <summary>
    /// 基类型<see cref="Object"/>扩展辅助操作类
    /// </summary>
    public static class ObjectExtensions
    {
        #region 公共方法
        /// <summary>
        /// 构造函数
        /// </summary>
        static ObjectExtensions()
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
            {
                //日期类型默认格式化处理
                setting.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
                setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                return setting;
            });
        }

        /// <summary>
        /// 把对象类型转化为指定类型
        /// </summary>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <param name="value"> 要转化的源对象 </param>
        /// <returns> 转化后的指定类型的对象，转化失败引发异常。 </returns>
        public static T CastTo<T>(this object value)
        {
            if (value == null && default(T) == null)
            {
                return default(T);
            }
            if (value.GetType() == typeof(T))
            {
                return (T)value;
            }
            object result = CastTo(value, typeof(T));
            return (T)result;
        }

        /// <summary>
        /// 把对象类型转化为指定类型，转化失败时返回指定的默认值
        /// </summary>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <param name="value"> 要转化的源对象 </param>
        /// <param name="defaultValue"> 转化失败返回的指定默认值 </param>
        /// <returns> 转化后的指定类型对象，转化失败时返回指定的默认值 </returns>
        public static T CastTo<T>(this object value, T defaultValue)
        {
            try
            {
                return CastTo<T>(value);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static T JsonClone<T>(this T source)
        {
            var jObject = JObject.FromObject(source);
            var result = jObject.ToObject<T>();
            return result;
        }


        /// <summary>
        /// 判断当前值是否介于指定范围内
        /// </summary>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <param name="value"> 动态类型对象 </param>
        /// <param name="start"> 范围起点 </param>
        /// <param name="end"> 范围终点 </param>
        /// <param name="leftEqual"> 是否可等于上限（默认等于） </param>
        /// <param name="rightEqual"> 是否可等于下限（默认等于） </param>
        /// <returns> 是否介于 </returns>
        public static bool IsBetween<T>(this IComparable<T> value, T start, T end, bool leftEqual = true, bool rightEqual = true) where T : IComparable
        {
            bool flag = leftEqual ? value.CompareTo(start) >= 0 : value.CompareTo(start) > 0;
            return flag && (rightEqual ? value.CompareTo(end) <= 0 : value.CompareTo(end) < 0);
        }

        /// <summary>
        /// 判断当前值是否介于指定范围内
        /// </summary>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <param name="value"> 动态类型对象 </param>
        /// <param name="min">范围小值</param>
        /// <param name="max">范围大值</param>
        /// <param name="minEqual">是否可等于小值（默认等于）</param>
        /// <param name="maxEqual">是否可等于大值（默认等于）</param>
        public static bool IsInRange<T>(this IComparable<T> value, T min, T max, bool minEqual = true, bool maxEqual = true) where T : IComparable
        {
            bool flag = minEqual ? value.CompareTo(min) >= 0 : value.CompareTo(min) > 0;
            return flag && (maxEqual ? value.CompareTo(max) <= 0 : value.CompareTo(max) < 0);
        }

        /// <summary>
        /// 是否存在于
        /// </summary>
        public static bool IsIn<T>(this T value, params T[] source)
        {
            return source.Contains(value);
        }

        /// <summary>
        /// 将对象[主要是匿名对象]转换为dynamic
        /// </summary>
        public static dynamic ToDynamic(this object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();
            Type type = value.GetType();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(type);
            foreach (PropertyDescriptor property in properties)
            {
                var val = property.GetValue(value);
                if (property.PropertyType.FullName != null && property.PropertyType.FullName.StartsWith("<>f__AnonymousType"))
                {
                    dynamic dval = val.ToDynamic();
                    expando.Add(property.Name, dval);
                }
                else
                {
                    expando.Add(property.Name, val);
                }
            }
            return expando as ExpandoObject;
        }

        /// <summary>
        /// 对象深度拷贝，复制出一个数据一样，但地址不一样的新版本
        /// </summary>
        public static T DeepClone<T>(this T obj) where T : class
        {
            if (obj == null)
            {
                return default(T);
            }
            if (typeof(T).HasAttribute<SerializableAttribute>())
            {
                throw new NotSupportedException("当前对象未标记特性“{0}”，无法进行DeepClone操作".FormatWith(typeof(SerializableAttribute)));
            }
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, obj);
                ms.Seek(0L, SeekOrigin.Begin);
                return (T)formatter.Deserialize(ms);
            }
        }
        /// <summary>
        /// 深复制
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        //public static T DeepClone<T>(this T obj) where T : class
        //{
        //    if (obj == null)
        //        return null;

        //    return obj.ToJson().ToObject<T>();
        //}

        #endregion


        /// <summary>
        /// Converts given object to a value type using <see cref="Convert.ChangeType(object,System.TypeCode)"/> method.
        /// </summary>
        /// <param name="obj">Object to be converted</param>
        /// <typeparam name="T">Type of the target object</typeparam>
        /// <returns>Converted object</returns>
        public static T To<T>(this object obj)
            where T : struct
        {
            if (typeof(T) == typeof(Guid))
            {
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(obj.ToString());
            }

            return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Used to simplify and beautify casting an object to a type. 
        /// </summary>
        /// <typeparam name="T">Type to be casted</typeparam>
        /// <param name="obj">Object to cast</param>
        /// <returns>Casted object</returns>
        public static T As<T>(this object obj)
            where T : class
        {
            return (T)obj;
        }


        /// <summary>
        /// 将一个object对象序列化，返回一个byte[]         
        /// </summary> 
        /// <param name="obj">能序列化的对象</param>
        /// <returns></returns> 
        public static byte[] ToBytes(this object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                return ms.GetBuffer();
            }
        }

        /// <summary>
        /// 判断是否为Null或者空
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object obj)
        {
            if (obj == null)
                return true;
            else
            {
                string objStr = obj.ToString();
                return string.IsNullOrEmpty(objStr);
            }
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 实体类转json数据，速度快
        /// </summary>
        /// <param name="t">实体类</param>
        /// <returns></returns>
        public static string EntityToJson(this object t)
        {
            if (t == null)
                return null;
            string jsonStr = "";
            jsonStr += "{";
            PropertyInfo[] infos = t.GetType().GetProperties();
            for (int i = 0; i < infos.Length; i++)
            {
                jsonStr = jsonStr + "\"" + infos[i].Name + "\":\"" + infos[i].GetValue(t).ToString() + "\"";
                if (i != infos.Length - 1)
                    jsonStr += ",";
            }
            jsonStr += "}";
            return jsonStr;
        }

        

        /// <summary>
        /// 将对象序列化为XML字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string ToXmlStr<T>(this T obj)
        {
            var jsonStr = obj.ToJson();
            var xmlDoc = JsonConvert.DeserializeXmlNode(jsonStr);
            string xmlDocStr = xmlDoc.InnerXml;

            return xmlDocStr;
        }

        /// <summary>
        /// 将对象序列化为XML字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象</param>
        /// <param name="rootNodeName">根节点名(建议设为xml)</param>
        /// <returns></returns>
        public static string ToXmlStr<T>(this T obj, string rootNodeName)
        {
            var jsonStr = obj.ToJson();
            var xmlDoc = JsonConvert.DeserializeXmlNode(jsonStr, rootNodeName);
            string xmlDocStr = xmlDoc.InnerXml;

            return xmlDocStr;
        }


        /// <summary>
        /// 将集合转换为数据集。
        /// </summary>
        /// <typeparam name="T">转换的元素类型。</typeparam>
        /// <param name="list">集合。</param>
        /// <param name="generic">是否生成泛型数据集。</param>
        /// <returns>数据集。</returns>
        public static DataSet ToDataSet<T>(this IEnumerable<T> list, bool generic = true)
        {
            return ListToDataSet(list, generic);
        }
        /// <summary>
        /// DataTable转List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IList<T> ToIList<T>(this DataTable dt) where T : class, new()
        {
            // 定义集合    
            IList<T> ts = new List<T>();

            // 获得此模型的类型   
            Type type = typeof(T);
            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性      
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;  // 检查DataTable是否包含此列    

                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter      
                        if (!pi.CanWrite) continue;

                        object value = dr[tempName];
                        if (value != DBNull.Value)
                        {
                            pi.SetValue(t, ChangeType(value, pi.PropertyType), null);
                        }
                    }
                }
                ts.Add(t);
            }
            return ts;
        }

        /// <summary>
        /// 类型转换（包含Nullable<>和非Nullable<>转换）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        private static object ChangeType(object value, Type conversionType)
        {
            // Note: This if block was taken from Convert.ChangeType as is, and is needed here since we're
            // checking properties on conversionType below.
            if (conversionType == null)
            {
                throw new ArgumentNullException("conversionType");
            } // end if

            // If it's not a nullable type, just pass through the parameters to Convert.ChangeType

            if (conversionType.IsGenericType &&
                conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                } // end if

                // It's a nullable type, and not null, so that means it can be converted to its underlying type,
                // so overwrite the passed-in conversion type with this underlying type
                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);

                conversionType = nullableConverter.UnderlyingType;
            } // end if

            // Now that we've guaranteed conversionType is something Convert.ChangeType can handle (i.e. not a
            // nullable type), pass the call on to Convert.ChangeType
            return Convert.ChangeType(value, conversionType);
        }
        /// <summary>
        /// 将集合转换为数据集。
        /// </summary>
        /// <param name="list">集合。</param>
        /// <param name="generic">是否生成泛型数据集。</param>
        /// <returns>数据集。</returns>
        public static DataSet ToDataSet(this IEnumerable list, bool generic = true)
        {
            return ListToDataSet(list, generic);
        }

        /// <summary>
        /// 将集合转换为数据集。
        /// </summary>
        /// <typeparam name="T">转换的元素类型。</typeparam>
        /// <param name="list">集合。</param>
        /// <param name="generic">是否生成泛型数据集。</param>
        /// <returns>数据集。</returns>
        public static DataSet ToDataSet<T>(this IEnumerable list, bool generic = true)
        {
            return ListToDataSet(list, typeof(T), generic);
        }

        /// <summary>
        /// 将实例转换为集合数据集。
        /// </summary>
        /// <typeparam name="T">实例类型。</typeparam>
        /// <param name="o">实例。</param>
        /// <param name="generic">是否生成泛型数据集。</param>
        /// <returns>数据集。</returns>
        public static DataSet ToListSet<T>(this T o, bool generic = true)
        {
            if (o is IEnumerable)
            {
                return ListToDataSet(o as IEnumerable, generic);
            }
            else
            {
                return ListToDataSet(new T[] { o }, generic);
            }
        }

        /// <summary>
        /// 将可序列化实例转换为XmlDocument。
        /// </summary>
        /// <typeparam name="T">实例类型。</typeparam>
        /// <param name="o">实例。</param>
        /// <returns>XmlDocument。</returns>
        public static XmlDocument ToXmlDocument<T>(this T o)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.InnerXml = o.ToListSet().GetXml();
            return xmlDocument;
        }

        /// <summary>
        /// 将集合转换为数据集。
        /// </summary>
        /// <param name="list">集合。</param>
        /// <param name="t">转换的元素类型。</param>
        /// <param name="generic">是否生成泛型数据集。</param>
        /// <returns>转换后的数据集。</returns>
        private static DataSet ListToDataSet(IEnumerable list, Type t, bool generic)
        {
            DataSet ds = new DataSet("Data");
            if (t == null)
            {
                if (list != null)
                {
                    foreach (var i in list)
                    {
                        if (i == null)
                        {
                            continue;
                        }
                        t = i.GetType();
                        break;
                    }
                }
                if (t == null)
                {
                    return ds;
                }
            }
            ds.Tables.Add(t.Name);
            //如果集合中元素为DataSet扩展涉及到的基本类型时，进行特殊转换。
            if (t.IsValueType || t == typeof(string))
            {
                ds.Tables[0].TableName = "Info";
                ds.Tables[0].Columns.Add(t.Name);
                if (list != null)
                {
                    foreach (var i in list)
                    {
                        DataRow addRow = ds.Tables[0].NewRow();
                        addRow[t.Name] = i;
                        ds.Tables[0].Rows.Add(addRow);
                    }
                }
                return ds;
            }
            //处理模型的字段和属性。
            var fields = t.GetFields();
            var properties = t.GetProperties();
            foreach (var j in fields)
            {
                if (!ds.Tables[0].Columns.Contains(j.Name))
                {
                    if (generic)
                    {
                        ds.Tables[0].Columns.Add(j.Name, j.FieldType);
                    }
                    else
                    {
                        ds.Tables[0].Columns.Add(j.Name);
                    }
                }
            }
            foreach (var j in properties)
            {
                if (!ds.Tables[0].Columns.Contains(j.Name))
                {
                    if (generic)
                    {
                        ds.Tables[0].Columns.Add(j.Name, j.PropertyType);
                    }
                    else
                    {
                        ds.Tables[0].Columns.Add(j.Name);
                    }
                }
            }
            if (list == null)
            {
                return ds;
            }
            //读取list中元素的值。
            foreach (var i in list)
            {
                if (i == null)
                {
                    continue;
                }
                DataRow addRow = ds.Tables[0].NewRow();
                foreach (var j in fields)
                {
                    MemberExpression field = Expression.Field(Expression.Constant(i), j.Name);
                    LambdaExpression lambda = Expression.Lambda(field, new ParameterExpression[] { });
                    Delegate func = lambda.Compile();
                    object value = func.DynamicInvoke();
                    addRow[j.Name] = value;
                }
                foreach (var j in properties)
                {
                    MemberExpression property = Expression.Property(Expression.Constant(i), j);
                    LambdaExpression lambda = Expression.Lambda(property, new ParameterExpression[] { });
                    Delegate func = lambda.Compile();
                    object value = func.DynamicInvoke();
                    addRow[j.Name] = value;
                }
                ds.Tables[0].Rows.Add(addRow);
            }
            return ds;
        }

        /// <summary>
        /// 将集合转换为数据集。
        /// </summary>
        /// <typeparam name="T">转换的元素类型。</typeparam>
        /// <param name="list">集合。</param>
        /// <param name="generic">是否生成泛型数据集。</param>
        /// <returns>数据集。</returns>
        private static DataSet ListToDataSet<T>(IEnumerable<T> list, bool generic)
        {
            return ListToDataSet(list, typeof(T), generic);
        }

        /// <summary>
        /// 将集合转换为数据集。
        /// </summary>
        /// <param name="list">集合。</param>
        /// <param name="generic">是否转换为字符串形式。</param>
        /// <returns>转换后的数据集。</returns>
        private static DataSet ListToDataSet(IEnumerable list, bool generic)
        {
            return ListToDataSet(list, null, generic);
        }

        /// <summary>
        /// 获取DataSet第一表，第一行，第一列的值。
        /// </summary>
        /// <param name="ds">DataSet数据集。</param>
        /// <returns>值。</returns>
        public static object GetData(this DataSet ds)
        {
            if (
                ds == null
                || ds.Tables.Count == 0
                )
            {
                return string.Empty;
            }
            else
            {
                return ds.Tables[0].GetData();
            }
        }

        /// <summary>
        /// 获取DataTable第一行，第一列的值。
        /// </summary>
        /// <param name="dt">DataTable数据集表。</param>
        /// <returns>值。</returns>
        public static object GetData(this DataTable dt)
        {
            if (
                dt.Columns.Count == 0
                || dt.Rows.Count == 0
                )
            {
                return string.Empty;
            }
            else
            {
                return dt.Rows[0][0];
            }
        }

        /// <summary>
        /// 获取DataSet第一个匹配columnName的值。
        /// </summary>
        /// <param name="ds">数据集。</param>
        /// <param name="columnName">列名。</param>
        /// <returns>值。</returns>
        public static object GetData(this DataSet ds, string columnName)
        {
            if (
                ds == null
                || ds.Tables.Count == 0
                )
            {
                return string.Empty;
            }
            foreach (DataTable dt in ds.Tables)
            {
                object o = dt.GetData(columnName);
                if (!string.IsNullOrEmpty(o.ToString()))
                {
                    return o;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取DataTable第一个匹配columnName的值。
        /// </summary>
        /// <param name="dt">数据表。</param>
        /// <param name="columnName">列名。</param>
        /// <returns>值。</returns>
        public static object GetData(this DataTable dt, string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
            {
                return GetData(dt);
            }
            if (
                dt.Columns.Count == 0
                || dt.Columns.IndexOf(columnName) == -1
                || dt.Rows.Count == 0
                )
            {
                return string.Empty;
            }
            return dt.Rows[0][columnName];
        }

        /// <summary>
        /// 将object转换为string类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <param name="t">默认值。</param>
        /// <returns>string。</returns>
        public static string ToString(this object o, string t)
        {
            string info = string.Empty;
            if (o == null)
            {
                info = t;
            }
            else
            {
                info = o.ToString();
            }
            return info;
        }

        /// <summary>
        /// 将DateTime?转换为string类型信息。
        /// </summary>
        /// <param name="o">DateTime?。</param>
        /// <param name="format">标准或自定义日期和时间格式的字符串。</param>
        /// <param name="t">默认值。</param>
        /// <returns>string。</returns>
        public static string ToString(this DateTime? o, string format, string t)
        {
            string info = string.Empty;
            if (o == null)
            {
                info = t;
            }
            else
            {
                info = o.Value.ToString(format);
            }
            return info;
        }

        /// <summary>
        /// 将TimeSpan?转换为string类型信息。
        /// </summary>
        /// <param name="o">TimeSpan?。</param>
        /// <param name="format">标准或自定义时间格式的字符串。</param>
        /// <param name="t">默认值。</param>
        /// <returns>string。</returns>
        public static string ToString(this TimeSpan? o, string format, string t)
        {
            string info = string.Empty;
            if (o == null)
            {
                info = t;
            }
            else
            {
                info = o.Value.ToString(format);
            }
            return info;
        }

        /// <summary>
        /// 将object转换为截取后的string类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <param name="startIndex">此实例中子字符串的起始字符位置（从零开始）。</param>
        /// <param name="length">子字符串中的字符数。</param>
        /// <param name="suffix">后缀。如果没有截取则不添加。</param>
        /// <returns>截取后的string类型信息。</returns>
        public static string ToSubString(this object o, int startIndex, int length, string suffix = null)
        {
            string inputString = o.ToString(string.Empty);
            startIndex = Math.Max(startIndex, 0);
            startIndex = Math.Min(startIndex, (inputString.Length - 1));
            length = Math.Max(length, 1);
            if (startIndex + length > inputString.Length)
            {
                length = inputString.Length - startIndex;
            }
            if (inputString.Length == startIndex + length)
            {
                return inputString;
            }
            else
            {
                return inputString.Substring(startIndex, length) + suffix;
            }
        }

        /// <summary>
        /// 将object转换为byte类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <param name="t">默认值。</param>
        /// <returns>byte。</returns>
        public static byte ToByte(this object o, byte t = default(byte))
        {
            byte info;
            if (!byte.TryParse(o.ToString(string.Empty), out info))
            {
                info = t;
            }
            return info;
        }

        public static object ToObject(this byte[] source)
        {
            using (var memStream = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                memStream.Write(source, 0, source.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = bf.Deserialize(memStream);
                return obj;
            }
        }

        /// <summary>
        /// 将object转换为char类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <param name="t">默认值。</param>
        /// <returns>char。</returns>
        public static char ToChar(this object o, char t = default(char))
        {
            char info;
            if (!char.TryParse(o.ToString(string.Empty), out info))
            {
                info = t;
            }
            return info;
        }

        /// <summary>
        /// 将object转换为int类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <param name="t">默认值。</param>
        /// <returns>int。</returns>
        public static int ToInt(this object o, int t = default(int))
        {
            int info;
            if (!int.TryParse(o.ToString(string.Empty), out info))
            {
                info = t;
            }
            return info;
        }

        /// <summary>
        /// 将object转换为double类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <param name="t">默认值。</param>
        /// <returns>double。</returns>
        public static double ToDouble(this object o, double t = default(double))
        {
            double info;
            if (!double.TryParse(o.ToString(string.Empty), out info))
            {
                info = t;
            }
            return info;
        }

        /// <summary>
        /// 将object转换为decimal类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <param name="t">默认值。</param>
        /// <returns>decimal。</returns>
        public static decimal ToDecimal(this object o, decimal t = default(decimal))
        {
            decimal info;
            if (!decimal.TryParse(o.ToString(string.Empty), out info))
            {
                info = t;
            }
            return info;
        }

        /// <summary>
        /// 将object转换为float类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <param name="t">默认值。</param>
        /// <returns>float。</returns>
        public static float ToFloat(this object o, float t = default(float))
        {
            float info;
            if (!float.TryParse(o.ToString(string.Empty), out info))
            {
                info = t;
            }
            return info;
        }

        /// <summary>
        /// 将object转换为long类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <param name="t">默认值。</param>
        /// <returns>long。</returns>
        public static long ToLong(this object o, long t = default(long))
        {
            long info;
            if (!long.TryParse(o.ToString(string.Empty), out info))
            {
                info = t;
            }
            return info;
        }

        /// <summary>
        /// 将object转换为bool类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <param name="t">默认值。</param>
        /// <returns>bool。</returns>
        public static bool ToBool(this object o, bool t = default(bool))
        {
            bool info;
            if (!bool.TryParse(o.ToString(string.Empty), out info))
            {
                info = t;
            }
            return info;
        }

        /// <summary>
        /// 将object转换为sbyte类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <param name="t">默认值。</param>
        /// <returns>sbyte。</returns>
        public static sbyte ToSbyte(this object o, sbyte t = default(sbyte))
        {
            sbyte info;
            if (!sbyte.TryParse(o.ToString(string.Empty), out info))
            {
                info = t;
            }
            return info;
        }

        /// <summary>
        /// 将object转换为short类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <param name="t">默认值。</param>
        /// <returns>short。</returns>
        public static short ToShort(this object o, short t = default(short))
        {
            short info;
            if (!short.TryParse(o.ToString(string.Empty), out info))
            {
                info = t;
            }
            return info;
        }

        /// <summary>
        /// 将object转换为ushort类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <param name="t">默认值。</param>
        /// <returns>ushort。</returns>
        public static ushort ToUShort(this object o, ushort t = default(ushort))
        {
            ushort info;
            if (!ushort.TryParse(o.ToString(string.Empty), out info))
            {
                info = t;
            }
            return info;
        }

        /// <summary>
        /// 将object转换为ulong类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <param name="t">默认值。</param>
        /// <returns>ulong。</returns>
        public static ulong ToULong(this object o, ulong t = default(ulong))
        {
            ulong info;
            if (!ulong.TryParse(o.ToString(string.Empty), out info))
            {
                info = t;
            }
            return info;
        }

        /// <summary>
        /// 将object转换为Enum[T]类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <param name="t">默认值。</param>
        /// <returns>Enum[T]。</returns>
        public static T ToEnum<T>(this object o, T t = default(T))
            where T : struct
        {
            T info;
            if (!Enum.TryParse(o.ToString(string.Empty), out info))
            {
                info = t;
            }
            return info;
        }

        /// <summary>
        /// 将object转换为DateTime类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <param name="t">默认值。</param>
        /// <returns>DateTime。</returns>
        public static DateTime ToDateTime(this object o, DateTime t = default(DateTime))
        {
            if (t == default(DateTime))
            {
                t = new DateTime(1753, 1, 1);
            }
            DateTime info;
            if (!DateTime.TryParse(o.ToString(string.Empty), out info))
            {
                info = t;
            }
            return info;
        }

        /// <summary>
        /// 将object转换为TimeSpan类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <param name="t">默认值。</param>
        /// <returns>TimeSpan。</returns>
        public static TimeSpan ToTimeSpan(this object o, TimeSpan t = default(TimeSpan))
        {
            if (t == default(TimeSpan))
            {
                t = new TimeSpan(0, 0, 0);
            }
            TimeSpan info;
            if (!TimeSpan.TryParse(o.ToString(string.Empty), out info))
            {
                info = t;
            }
            return info;
        }

        /// <summary>
        /// 将object转换为Guid类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <param name="t">默认值。</param>
        /// <returns>Guid。</returns>
        public static Guid ToGuid(this object o, Guid t = default(Guid))
        {
            Guid info;
            if (!Guid.TryParse(o.ToString(string.Empty), out info))
            {
                info = t;
            }
            return info;
        }

        private static Regex BoolRegex = new Regex("(?<info>(true|false))", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        /// <summary>
        /// 从object中获取bool类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <returns>bool。</returns>
        public static bool? GetBool(this object o)
        {
            bool info;
            if (!bool.TryParse(BoolRegex.Match(o.ToString(string.Empty)).Groups["info"].Value, out info))
            {
                return null;
            }
            return info;
        }

        private static Regex IntRegex = new Regex("(?<info>-?\\d+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        /// <summary>
        /// 从object中获取int类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <returns>int。</returns>
        public static int? GetInt(this object o)
        {
            int info;
            if (!int.TryParse(IntRegex.Match(o.ToString(string.Empty)).Groups["info"].Value, out info))
            {
                return null;
            }
            return info;
        }

        private static Regex DecimalRegex = new Regex("(?<info>-?\\d+(\\.\\d+)?)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        /// <summary>
        /// 从object中获取decimal类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <returns>decimal。</returns>
        public static decimal? GetDecimal(this object o)
        {
            decimal info;
            if (!decimal.TryParse(DecimalRegex.Match(o.ToString(string.Empty)).Groups["info"].Value, out info))
            {
                return null;
            }
            return info;
        }

        /// <summary>
        /// 从object中获取double类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <returns>double。</returns>
        public static double? GetDouble(this object o)
        {
            double info;
            if (!double.TryParse(DecimalRegex.Match(o.ToString(string.Empty)).Groups["info"].Value, out info))
            {
                return null;
            }
            return info;
        }

        /// <summary>
        /// 从object中获取正数信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <returns>decimal。</returns>
        public static decimal? GetPositiveNumber(this object o)
        {
            decimal info;
            if (!decimal.TryParse(DecimalRegex.Match(o.ToString(string.Empty)).Groups["info"].Value, out info))
            {
                return null;
            }
            return Math.Abs(info);
        }

        private static Regex DateTimeRegex = new Regex("(?<info>(((\\d+)[/年-](0?[13578]|1[02])[/月-](3[01]|[12]\\d|0?\\d)[日]?)|((\\d+)[/年-](0?[469]|11)[/月-](30|[12]\\d|0?\\d)[日]?)|((\\d+)[/年-]0?2[/月-](2[0-8]|1\\d|0?\\d)[日]?))(\\s((2[0-3]|[0-1]\\d)):[0-5]\\d:[0-5]\\d)?)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        /// <summary>
        /// 从object中获取DateTime?类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <returns>DateTime?。</returns>
        public static DateTime? GetDateTime(this object o)
        {
            DateTime info;
            if (!DateTime.TryParse(DateTimeRegex.Match(o.ToString(string.Empty)).Groups["info"].Value.Replace("年", "-").Replace("月", "-").Replace("/", "-").Replace("日", ""), out info))
            {
                return null;
            }
            return info;
        }

        private static Regex TimeSpanRegex = new Regex("(?<info>-?(\\d+\\.(([0-1]\\d)|(2[0-3])):[0-5]\\d:[0-5]\\d)|((([0-1]\\d)|(2[0-3])):[0-5]\\d:[0-5]\\d)|(\\d+))", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        /// <summary>
        /// 从object中获取TimeSpan?类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <returns>TimeSpan?。</returns>
        public static TimeSpan? GetTimeSpan(this object o)
        {
            TimeSpan info;
            if (!TimeSpan.TryParse(TimeSpanRegex.Match(o.ToString(string.Empty)).Groups["info"].Value, out info))
            {
                return null;
            }
            return info;
        }

        private static Regex GuidRegex = new Regex("(?<info>\\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\\}{0,1})", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        /// <summary>
        /// 从object中获取Guid?类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <returns>Guid?。</returns>
        public static Guid? GetGuid(this object o)
        {
            Guid info;
            if (!Guid.TryParse(GuidRegex.Match(o.ToString(string.Empty)).Groups["info"].Value, out info))
            {
                return null;
            }
            return info;
        }

        /// <summary>
        /// 将object转换为SqlServer中的DateTime?类型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <param name="t">默认值。</param>
        /// <returns>DateTime?。</returns>
        public static DateTime? GetSqlDateTime(this object o, DateTime t = default(DateTime))
        {
            DateTime info;
            if (!DateTime.TryParse(o.ToString(string.Empty), out info))
            {
                info = t;
            }
            if (info < new DateTime(1753, 1, 1) || info > new DateTime(9999, 12, 31))
            {
                return null;
            }
            return info;
        }

        /// <summary>
        /// 读取XElement节点的文本内容。
        /// </summary>
        /// <param name="xElement">XElement节点。</param>
        /// <param name="t">默认值。</param>
        /// <returns>文本内容。</returns>
        public static string Value(this XElement xElement, string t = default(string))
        {
            if (xElement == null)
            {
                return t;
            }
            else
            {
                return xElement.Value;
            }
        }

        /// <summary>
        /// 获取与指定键相关的值。
        /// </summary>
        /// <typeparam name="TKey">键类型。</typeparam>
        /// <typeparam name="TValue">值类型。</typeparam>
        /// <param name="dictionary">表示键/值对象的泛型集合。</param>
        /// <param name="key">键。</param>
        /// <param name="t">默认值。</param>
        /// <returns>值。</returns>
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue t = default(TValue))
        {
            TValue value = default(TValue);
            if (dictionary == null || key == null)
            {
                return t;
            }
            if (!dictionary.TryGetValue(key, out value))
            {
                value = t;
            }
            return value;
        }

        /// <summary>
        /// 获取与指定键相关或者第一个的值。
        /// </summary>
        /// <typeparam name="TKey">键类型。</typeparam>
        /// <typeparam name="TValue">值类型。</typeparam>
        /// <param name="dictionary">表示键/值对象的泛型集合。</param>
        /// <param name="key">键。</param>
        /// <param name="t">默认值。</param>
        /// <returns>值。</returns>
        public static TValue GetFirstOrDefaultValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue t = default(TValue))
        {
            TValue value = default(TValue);
            if (dictionary == null || key == null)
            {
                return t;
            }
            if (!dictionary.TryGetValue(key, out value))
            {
                if (dictionary.Count() == 0)
                {
                    value = t;
                }
                else
                {
                    value = dictionary.FirstOrDefault().Value;
                }
            }
            return value;
        }

        /// <summary>
        /// 获取具有指定 System.Xml.Linq.XName 的第一个（按文档顺序）子元素。
        /// </summary>
        /// <param name="xContainer">XContainer。</param>
        /// <param name="xName">要匹配的 System.Xml.Linq.XName。</param>
        /// <param name="t">是否返回同名默认值。</param>
        /// <returns>与指定 System.Xml.Linq.XName 匹配的 System.Xml.Linq.XElement，或者为 null。</returns>
        public static XElement Element(this XContainer xContainer, XName xName, bool t)
        {
            XElement info;
            if (xContainer == null)
            {
                info = null;
            }
            else
            {
                info = xContainer.Element(xName);
            }
            if (t && info == null)
            {
                info = new XElement(xName);
            }
            return info;
        }

        /// <summary>
        /// 按文档顺序返回此元素或文档的子元素集合。
        /// </summary>
        /// <param name="xContainer">XContainer。</param>
        /// <param name="t">是否返回非空默认值。</param>
        /// <returns>System.Xml.Linq.XElement 的按文档顺序包含此System.Xml.Linq.XContainer 的子元素，或者非空默认值。</returns>
        public static IEnumerable<XElement> Elements(this XContainer xContainer, bool t)
        {
            IEnumerable<XElement> info;
            if (xContainer == null)
            {
                info = null;
            }
            else
            {
                info = xContainer.Elements();
            }
            if (t && info == null)
            {
                info = new List<XElement>();
            }
            return info;
        }

        /// <summary>
        /// 按文档顺序返回此元素或文档的经过筛选的子元素集合。集合中只包括具有匹配 System.Xml.Linq.XName 的元素。
        /// </summary>
        /// <param name="xContainer">XContainer。</param>
        /// <param name="xName">要匹配的 System.Xml.Linq.XName。</param>
        /// <param name="t">是否返回非空默认值。</param>
        /// <returns>System.Xml.Linq.XElement 的按文档顺序包含具有匹配System.Xml.Linq.XName 的 System.Xml.Linq.XContainer 的子级，或者非空默认值。</returns>
        public static IEnumerable<XElement> Elements(this XContainer xContainer, XName xName, bool t)
        {
            IEnumerable<XElement> info;
            if (xContainer == null)
            {
                info = null;
            }
            else
            {
                info = xContainer.Elements(xName);
            }
            if (t && info == null)
            {
                info = new List<XElement>();
            }
            return info;
        }

        /// <summary>
        /// 删除html标签。
        /// </summary>
        /// <param name="html">输入的字符串。</param>
        /// <returns>没有html标签的字符串。</returns>
        public static string RemoveHTMLTags(this string html)
        {
            return Regex.Replace(Regex.Replace(Regex.Replace((html ?? string.Empty).Replace("&nbsp;", " ").Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace("\t", " "), "<\\/?[^>]+>", "\r\n"), "(\r\n)+", "\r\n"), "(\\s)+", " ").Trim();
        }

        /// <summary>
        /// 字符串转换为文件名。
        /// </summary>
        /// <param name="s">字符串。</param>
        /// <returns>文件名。</returns>
        public static string ToFileName(this string s)
        {
            return Regex.Replace(s.ToString(string.Empty), @"[\\/:*?<>|]", "_").Replace("\t", " ").Replace("\r\n", " ").Replace("\"", " ");
        }

        /// <summary>
        /// 获取星期一的日期。
        /// </summary>
        /// <param name="dateTime">日期。</param>
        /// <returns>星期一的日期。</returns>
        public static DateTime? GetMonday(this DateTime dateTime)
        {
            return dateTime.AddDays(-1 * (int)dateTime.AddDays(-1).DayOfWeek).ToString("yyyy-MM-dd").GetDateTime();
        }

        /// <summary>
        /// 获取默认非空字符串。
        /// </summary>
        /// <param name="s">首选默认非空字符串。</param>
        /// <param name="args">依次非空字符串可选项。</param>
        /// <returns>默认非空字符串。若无可选项则返回string.Empty。</returns>
        public static string DefaultStringIfEmpty(this string s, params string[] args)
        {
            if (string.IsNullOrEmpty(s))
            {
                foreach (string i in args)
                {
                    if (!string.IsNullOrEmpty(i) && !string.IsNullOrEmpty(i.Trim()))
                    {
                        return i;
                    }
                }
            }
            return (s ?? string.Empty);
        }

        /// <summary>
        /// 对 URL 字符串进行编码。
        /// </summary>
        /// <param name="s">要编码的文本。</param>
        /// <param name="regex">匹配要编码的文本。</param>
        /// <param name="encoding">指定编码方案的 System.Text.Encoding 对象。</param>
        /// <returns>一个已编码的字符串。</returns>
        public static string ToUrlEncodeString(this string s, Regex regex = default(Regex), Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            if (regex == null)
            {
                return HttpUtility.UrlEncode(s, encoding);
            }
            List<string> l = new List<string>();
            foreach (char i in s)
            {
                string t = i.ToString();
                l.Add(regex.IsMatch(t) ? HttpUtility.UrlEncode(t, encoding) : t);
            }
            return string.Join(string.Empty, l);
        }

        /// <summary>
        /// 对 URL 字符串进行编码。
        /// </summary>
        /// <param name="s">要编码的文本。</param>
        /// <param name="regex">匹配要编码的文本。</param>
        /// <param name="encoding">指定编码方案的 System.Text.Encoding 对象。</param>
        /// <returns>一个已编码的字符串。</returns>
        public static string ToUrlEncodeString(this string s, string regex, Encoding encoding = null)
        {
            return ToUrlEncodeString(s, new Regex(regex), encoding);
        }

        /// <summary>
        /// 将日期转换为UNIX时间戳字符串
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToUnixTimeStamp(this DateTime date)
        {
            DateTime startTime = TimeZoneInfo.ConvertTimeToUtc(new DateTime(1970, 1, 1));
            string timeStamp = date.Subtract(startTime).Ticks.ToString();
            return timeStamp.Substring(0, timeStamp.Length - 7);
        }

        private static Regex MobileRegex = new Regex("^1[3|4|5|7|8][0-9]\\d{4,8}$");
        private static Regex EmailRegex = new Regex("^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,3}){1,2})$");

        /// <summary>
        /// 判断当前字符串是否是移动电话号码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsMobile(this string mobile)
        {
            return MobileRegex.IsMatch(mobile);
        }

        /// <summary>
        /// 判断当前字符串是否为邮箱
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmail(this string email)
        {
            return EmailRegex.IsMatch(email);
        }
    }
}