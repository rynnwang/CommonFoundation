using System.Net;
using System.Net.Http;

namespace Beyova.Api.RestApi
{
    /// <summary>
    /// Class ApiHandlerBase.
    /// </summary>
    public abstract class ApiHandlerBase : ApiHandlerBase<HttpRequestMessage, HttpResponseMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiHandlerBase" /> class.
        /// </summary>
        /// <param name="defaultApiSettings">The default API settings.</param>
        /// <param name="allowOptions">if set to <c>true</c> [allow options].</param>
        protected ApiHandlerBase(RestApiSettings defaultApiSettings, bool allowOptions = false) : base(defaultApiSettings, allowOptions)
        {
        }
    }
}