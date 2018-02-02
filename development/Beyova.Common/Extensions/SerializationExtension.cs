using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Beyova.ExceptionSystem;

namespace Beyova
{
    /// <summary>
    /// Extension class for mail.
    /// </summary>
    public static class SerializationExtension
    {
        /// <summary>
        /// The formatter
        /// </summary>
        private static readonly BinaryFormatter formatter = new BinaryFormatter();

        /// <summary>
        /// Serializes to stream.
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize.</param>
        /// <returns>MemoryStream.</returns>
        /// <exception cref="OperationFailureException">SerializeToStream</exception>
        public static MemoryStream SerializeToStream(this object objectToSerialize)
        {
            try
            {
                objectToSerialize.CheckNullObject(nameof(objectToSerialize));

                using (var memoryStream = new MemoryStream())
                {
                    formatter.Serialize(memoryStream, objectToSerialize);
                    return memoryStream;
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(objectToSerialize);
            }
        }

        /// <summary>
        /// Serializes to byte array.
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] SerializeToByteArray(this object objectToSerialize)
        {
            return SerializeToStream(objectToSerialize).ToArray();
        }

        /// <summary>
        /// Serializes to base64 string.
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="OperationFailureException">SerializeToBase64String</exception>
        public static string SerializeToBase64String(this object objectToSerialize)
        {
            try
            {
                byte[] byteArray = SerializeToByteArray(objectToSerialize);
                return Convert.ToBase64String(byteArray, 0, byteArray.Length);
            }
            catch (Exception ex)
            {
                throw ex.Handle(objectToSerialize);
            }
        }

        /// <summary>
        /// Deserializes to object.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="closeStream">if set to <c>true</c> [close stream].</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="OperationFailureException">DeserializeToObject</exception>
        public static object DeserializeToObject(this MemoryStream stream, bool closeStream = true)
        {
            try
            {
                stream.CheckNullObject(nameof(stream));
                return formatter.Deserialize(stream);
            }
            catch (Exception ex)
            {
                throw ex.Handle(closeStream);
            }
            finally
            {
                if (stream != null && closeStream)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        /// Deserializes from base64 string to object.
        /// </summary>
        /// <param name="base64String">The base64 string.</param>
        /// <returns>System.Object.</returns>
        public static object DeserializeFromBase64StringToObject(this string base64String)
        {
            try
            {
                base64String.CheckEmptyString(nameof(base64String));

                var byteArray = Convert.FromBase64String(base64String);
                return byteArray.DeserializeToObject();
            }
            catch (Exception ex)
            {
                throw ex.Handle(base64String);
            }
        }

        /// <summary>
        /// Deserializes to object.
        /// </summary>
        /// <param name="byteArray">The byte array.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="OperationFailureException">DeserializeToObject</exception>
        public static object DeserializeToObject(this byte[] byteArray)
        {
            try
            {
                byteArray.CheckNullObject(nameof(byteArray));
                using (var memoryStream = new MemoryStream(byteArray, 0, byteArray.Length))
                {
                    return memoryStream.DeserializeToObject();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        /// <summary>
        /// Deserializes to object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns>T.</returns>
        public static T DeserializeToObject<T>(this MemoryStream stream)
        {
            var obj = stream.DeserializeToObject(true);

            if (obj != null)
            {
                return (T)obj;
            }

            return default(T);
        }

        /// <summary>
        /// Deserializes to object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes">The bytes.</param>
        /// <returns>T.</returns>
        public static T DeserializeToObject<T>(this byte[] bytes)
        {
            var obj = bytes.DeserializeToObject();

            if (obj != null)
            {
                return (T)obj;
            }

            return default(T);
        }

        /// <summary>
        /// Deserailizes to object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="base64String">The base64 string.</param>
        /// <returns>T.</returns>
        public static T DeserializeToObject<T>(string base64String)
        {
            if (!string.IsNullOrWhiteSpace(base64String))
            {
                byte[] byteArray = Convert.FromBase64String(base64String);
                return byteArray.DeserializeToObject<T>();
            }

            return default(T);
        }
    }
}