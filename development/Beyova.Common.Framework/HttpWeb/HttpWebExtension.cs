using System.Net.Http;
using System.Web;

namespace Beyova.Http
{
    /// <summary>
    ///
    /// </summary>
    public static class HttpWebExtension
    {
        /// <summary>
        /// Gets the client ip.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        internal static string GetClientIp(this HttpRequestMessage request)
        {
            const string httpContextKey = "MS_HttpContext";
            if (request.Properties.ContainsKey(httpContextKey))
            {
                return ((HttpContextWrapper)request.Properties[httpContextKey]).Request.UserHostAddress;
            }

            return null;
        }
    }
}