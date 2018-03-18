using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public static class DataSecurityExtension
    {
        /// <summary>
        /// The encoding
        /// </summary>
        static private Encoding encoding = Encoding.UTF8;

        /// <summary>
        /// Encrypts the specified object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider">The provider.</param>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static byte[] Encrypt<T>(this IDataSecurityProvider provider, T obj)
        {
            return (provider != null && obj != null) ? provider.Encrypt(encoding.GetBytes(typeof(T) == typeof(string) ? obj.ToString() : obj.ToJson(false))) : null;
        }

        /// <summary>
        /// Encrypts as string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider">The provider.</param>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static string EncryptObjectAsString<T>(this IDataSecurityProvider provider, T obj)
        {
            return Encrypt(provider, obj).EncodeBase64();
        }

        /// <summary>
        /// Decrypts the specified bytes.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static T Decrypt<T>(this IDataSecurityProvider provider, byte[] bytes)
        {
            var originalString = (provider != null && bytes != null) ? encoding.GetString(provider.Decrypt(bytes)) : null;
            return typeof(T) == typeof(string) ? (T)(object)originalString : originalString.TryConvertJsonToObject<T>();
        }

        /// <summary>
        /// Decrypts the specified data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider">The provider.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static T DecryptFromString<T>(this IDataSecurityProvider provider, string data)
        {
            return Decrypt<T>(provider, data.DecodeBase64ToByteArray());
        }
    }
}
