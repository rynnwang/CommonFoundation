using System.Web;

namespace Beyova.Http
{
    /// <summary>
    ///
    /// </summary>
    public class HttpRequestExtensible : IIncomingHttpRequestExtensible<HttpRequest>
    {
        /// <summary>
        /// Gets the client ip address.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public string GetClientIpAddress(HttpRequest request)
        {
            return request?.UserHostAddress;
        }

        /// <summary>
        /// Determines whether the specified request is local.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>true</c> if the specified request is local; otherwise, <c>false</c>.
        /// </returns>
        public bool IsLocal(HttpRequest request)
        {
            return request?.IsLocal ?? false;
        }
    }
}