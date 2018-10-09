using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using Beyova;
using Beyova.ExceptionSystem;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// </summary>
    public static partial class DbObjectConvertionExtension
    {
        #region Object To XXX

        /// <summary>
        /// Objects to nullable enum.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static TEnum? ObjectToNullableEnum<TEnum>(this object data, TEnum? defaultValue = null)
            where TEnum : struct, IConvertible
        {
            int result;
            TEnum? returnObject;
            if (data == null || data == DBNull.Value || !int.TryParse(data.ToString(), out result))
            {
                returnObject = defaultValue;
            }
            else
            {
                returnObject = (TEnum)Enum.ToObject(typeof(TEnum), result);
            }

            return returnObject;
        }

        /// <summary>
        /// Objects to enum.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static TEnum ObjectToEnum<TEnum>(this object data, TEnum defaultValue = default(TEnum))
            where TEnum : struct, IConvertible
        {
            return ObjectToNullableEnum<TEnum>(data, defaultValue).Value;
        }

        /// <summary>
        /// To double.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Double.</returns>
        public static double ObjectToDouble(this object data, double defaultValue = 0)
        {
            double result;
            if (data == null || data == DBNull.Value || !double.TryParse(data.ToString(), out result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// DBs to float.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Single.</returns>
        public static float ObjectToFloat(this object data, float defaultValue = 0)
        {
            float result;
            if (data == null || data == DBNull.Value || !float.TryParse(data.ToString(), out result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// To the int32.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Int32.</returns>
        public static int ObjectToInt32(this object data, int defaultValue = 0)
        {
            int result;
            if (data == null || data == DBNull.Value || !int.TryParse(data.ToString(), out result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Databases to int64.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Int64.</returns>
        public static long ObjectToInt64(this object data, long defaultValue = 0)
        {
            long result;
            if (data == null || data == DBNull.Value || !long.TryParse(data.ToString(), out result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// To the nullable int32.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Nullable{System.Int32}.</returns>
        public static Int32? ObjectToNullableInt32(this object data, Int32? defaultValue = null)
        {
            int result;
            return (data == null || data == DBNull.Value || !Int32.TryParse(data.ToString(), out result)) ? defaultValue : result;
        }

        /// <summary>
        /// Databases to nullable int64.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Nullable&lt;Int64&gt;.</returns>
        public static Int64? ObjectToNullableInt64(this object data, Int64? defaultValue = null)
        {
            Int64 result;
            return (data == null || data == DBNull.Value || !Int64.TryParse(data.ToString(), out result)) ? defaultValue : result;
        }

        /// <summary>
        /// Databases to nullable float.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Nullable&lt;System.Single&gt;.</returns>
        public static float? ObjectToNullableFloat(this object data, float? defaultValue = null)
        {
            float result;
            return (data == null || data == DBNull.Value || !float.TryParse(data.ToString(), out result)) ? defaultValue : result;
        }

        /// <summary>
        /// Databases to nullable double.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Nullable&lt;System.Double&gt;.</returns>
        public static double? ObjectToNullableDouble(this object data, double? defaultValue = null)
        {
            double result;
            return (data == null || data == DBNull.Value || !double.TryParse(data.ToString(), out result)) ? defaultValue : result;
        }

        /// <summary>
        /// Databases to nullable decimal.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Nullable&lt;System.Decimal&gt;.</returns>
        public static decimal? ObjectToNullableDecimal(this object data, decimal? defaultValue = null)
        {
            decimal result;
            return (data == null || data == DBNull.Value || !decimal.TryParse(data.ToString(), out result)) ? defaultValue : result;
        }

        /// <summary>
        /// DBs to date time.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>System.Nullable{DateTime}.</returns>
        public static DateTime? ObjectToDateTime(this object data)
        {
            DateTime? result = null;

            if (data != null && data != DBNull.Value)
            {
                try
                {
                    result = Convert.ToDateTime(data);
                }
                catch
                {
                    result = data as DateTime?;
                }
            }

            if (result != null)
            {
                result = DateTime.SpecifyKind(result.Value, DateTimeKind.Utc);
            }

            return result;
        }

        /// <summary>
        /// To the date time.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>DateTime.</returns>
        public static DateTime ObjectToDateTime(this object data, DateTime defaultValue)
        {
            return ObjectToDateTime(data) ?? defaultValue;
        }

        /// <summary>
        /// Objects to date.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static Date? ObjectToDate(this object data)
        {
            return (Date?)data.ObjectToString();
        }

        /// <summary>
        /// Objects to date.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static Date ObjectToDate(this object data, Date defaultValue)
        {
            return ObjectToDate(data) ?? defaultValue;
        }

        /// <summary>
        /// To the date time.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="format">The format.</param>
        /// <param name="style">The style.</param>
        /// <returns>System.Nullable&lt;System.DateTime&gt;.</returns>
        public static DateTime? ToDateTime(this string data, string format = null, DateTimeStyles style = DateTimeStyles.AssumeUniversal)
        {
            DateTime dateTime;
            return DateTime.TryParseExact(data, format.SafeToString(StandardFormats.FullDateTimeTZFormat), CultureInfo.InvariantCulture, style, out dateTime) ? dateTime : null as DateTime?;
        }

        /// <summary>
        /// To the date time.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="format">The format.</param>
        /// <param name="style">The style.</param>
        /// <returns>System.DateTime.</returns>
        public static DateTime ToDateTime(this string data, DateTime defaultValue, string format = null, DateTimeStyles style = DateTimeStyles.AssumeUniversal)
        {
            return ToDateTime(data, format, style) ?? defaultValue;
        }

        /// <summary>
        /// To GUID.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Nullable{Guid}.</returns>
        public static Guid? ObjectToGuid(this object data, Guid? defaultValue = null)
        {
            Guid result;
            return (data == null || data == DBNull.Value || !Guid.TryParse(data.ToString(), out result)) ? defaultValue : result;
        }

        /// <summary>
        /// Databases the automatic decimal.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Decimal.</returns>
        public static decimal ObjectToDecimal(this object data, decimal defaultValue = 0)
        {
            decimal result;
            return (data == null || data == DBNull.Value || !decimal.TryParse(data.ToString(), out result)) ? defaultValue : result;
        }

        /// <summary>
        /// To nullable boolean
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The nullable boolean value.</returns>
        public static bool? ObjectToNullableBoolean(this object data, bool? defaultValue = null)
        {
            bool result;
            string dataString = data.SafeToString();
            int booleanInt;

            return
                int.TryParse(dataString, out booleanInt) ?
                Convert.ToBoolean(booleanInt)
                : ((data == null || data == DBNull.Value || !bool.TryParse(dataString, out result)) ? defaultValue : result);
        }

        /// <summary>
        /// DBs to boolean.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The boolean value. If failed to convert, return <c>false</c>.</returns>
        public static bool ObjectToBoolean(this object data)
        {
            return ObjectToNullableBoolean(data, false).Value;
        }

        /// <summary>
        /// Objects to bytes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static byte[] ObjectToBytes(this object data)
        {
            return (data == null || data == DBNull.Value) ? null : (byte[])data;
        }

        /// <summary>
        /// Objects to crypto key.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static CryptoKey ObjectToCryptoKey(this object data)
        {
            return new CryptoKey(ObjectToBytes(data));
        }

        /// <summary>
        /// To string.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.String.</returns>
        public static string ObjectToString(this object data, string defaultValue = StringConstants.EmptyString)
        {
            return (data == null || data == DBNull.Value) ? defaultValue : data.ToString();
        }

        /// <summary>
        /// Objects to json token.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// JToken.
        /// </returns>
        public static JToken ObjectToJToken(this object obj)
        {
            return obj.ObjectToString().ParseToJToken();
        }

        /// <summary>
        /// Objects to json object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static JObject ObjectToJObject(this object obj)
        {
            return obj.ObjectToString().ParseToJObject();
        }

        /// <summary>
        /// Objects to json object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static T ObjectToJsonObject<T>(this object obj)
        {
            var json = obj.ObjectToJToken();
            if (json == null || json.Type == JTokenType.Null)
            {
                return default(T);
            }
            else
            {
                return json.ToObject<T>();
            }
        }

        #endregion Object To XXX
    }
}