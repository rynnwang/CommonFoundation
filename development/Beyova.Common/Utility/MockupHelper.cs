using System;
using System.Collections.Generic;
using System.Linq;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public static class MockupHelper
    {
        /// <summary>
        /// The mockup data
        /// </summary>
        private static MatrixList<Type, object> _mockupData = new MatrixList<Type, object>();

        /// <summary>
        /// The locker
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// Ensures the exist container.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static List<object> EnsureExistContainer<T>()
            where T : class
        {
            var type = typeof(T);
            List<object> result = null;
            if (!_mockupData.TryGetValue(type, out result))
            {
                lock (locker)
                {
                    if (!_mockupData.TryGetValue(type, out result))
                    {
                        result = new List<object>();
                        _mockupData.Initial(type, result);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Existses this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool Exists<T>()
            where T : class
        {
            return _mockupData.ContainsKey(typeof(T));
        }

        /// <summary>
        /// Registers the specified objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects">The objects.</param>
        public static void Register<T>(params T[] objects)
            where T : class
        {
            if (objects.HasItem())
            {
                var container = EnsureExistContainer<T>();
                container.AddRange(objects as IEnumerable<object>);
            }
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetAll<T>()
            where T : class
        {
            return EnsureExistContainer<T>().AsCovariance<T>();
        }

        /// <summary>
        /// Gets the by key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static T GetByKey<T>(Guid key)
            where T : class, IIdentifier
        {
            return EnsureExistContainer<T>().AsNotNullAll(x =>
            {
                return ((x as IIdentifier)?.Key == key) ? x as T : default(T);
            }).FirstOrDefault();
        }

        /// <summary>
        /// Gets the by code.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static T GetByCode<T>(string code)
           where T : class, ICodeIdentifier
        {
            return EnsureExistContainer<T>().AsNotNullAll(x =>
            {
                return (!string.IsNullOrWhiteSpace(code) && string.Equals((x as ICodeIdentifier)?.Code, code, StringComparison.OrdinalIgnoreCase)) ? x as T : default(T);
            }).FirstOrDefault();
        }
    }
}
