using System;
using StackExchange.Redis.Extensions.Core;

namespace Beyova.Cache
{
    /// <summary>
    ///
    /// </summary>
    public static class StackExchangeRedisExtension
    {
        /// <summary>
        /// Replaces the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client">The client.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="utcExpiredStamp">The UTC expired stamp.</param>
        /// <returns></returns>
        public static bool Replace<T>(this ICacheClient client, string key, T value, DateTime? utcExpiredStamp)
        {
            return (client == null || string.IsNullOrWhiteSpace(key)) ? false : Act(key, value, utcExpiredStamp, client.Replace, client.Replace);
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client">The client.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="utcExpiredStamp">The UTC expired stamp.</param>
        /// <returns></returns>
        public static bool Add<T>(this ICacheClient client, string key, T value, DateTime? utcExpiredStamp)
        {
            return (client == null || string.IsNullOrWhiteSpace(key)) ? false : Act(key, value, utcExpiredStamp, client.Add, client.Add);
        }

        /// <summary>
        /// Acts the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="utcExpiredStamp">The UTC expired stamp.</param>
        /// <param name="predict1">The predict1.</param>
        /// <param name="predict2">The predict2.</param>
        /// <returns></returns>
        private static bool Act<T>(string key, T value, DateTime? utcExpiredStamp, Func<string, T, DateTimeOffset, bool> predict1, Func<string, T, bool> predict2)
        {
            return utcExpiredStamp.HasValue ? predict1(key, value, utcExpiredStamp.Value) : predict2(key, value);
        }
    }
}