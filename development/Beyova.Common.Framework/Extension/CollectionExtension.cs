using System.Web;

namespace Beyova
{
    public static partial class CollectionExtension
    {
        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="cookieCollection">The cookie collection.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string TryGetValue(this HttpCookieCollection cookieCollection, string key)
        {
            string result = null;

            if (cookieCollection != null && !string.IsNullOrWhiteSpace(key))
            {
                var cookie = cookieCollection[key];

                if (cookie != null)
                {
                    result = cookie.Value;
                }
            }

            return result;
        }
    }
}