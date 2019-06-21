using System;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    public static class FunctionFactory
    {
        /// <summary>
        /// Gets the self.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static T GetSelf<T>(T obj)
        {
            return obj;
        }

        /// <summary>
        /// News the default object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T NewDefaultObject<T>()
        {
            return default(T);
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static Guid? GetKey<T>(T obj)
             where T : IIdentifier
        {
            return obj?.Key;
        }

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static string GetCode<T>(T obj)
             where T : ICodeIdentifier
        {
            return obj?.Code;
        }

    }
}