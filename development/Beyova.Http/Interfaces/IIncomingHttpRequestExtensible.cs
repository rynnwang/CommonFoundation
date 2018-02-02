using System.IO;
using System.Net;

namespace Beyova.Http
{
    /// <summary>
    /// Interface IIncomingHttpRequestExtensible, to be implemented by extension methods.
    /// </summary>
    public interface IIncomingHttpRequestExtensible<TRequest>
    {
        /// <summary>
        /// Gets the client ip address.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        string GetClientIpAddress(TRequest request);

        /// <summary>
        /// Determines whether this instance is local.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>true</c> if this instance is local; otherwise, <c>false</c>.
        /// </returns>
        bool IsLocal(TRequest request);
    }
}
