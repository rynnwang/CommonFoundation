using System.Net.Http;

namespace Beyova.Api.RestApi
{
    /// <summary>
    /// Class RestApiRouter, which deeply integrated with <see cref="ContextHelper"/> for common usage.
    /// </summary>
    public sealed class RestApiRouter : RestApiRouter<HttpRequestMessage, HttpResponseMessage>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiRouter"/> class.
        /// </summary>
        public RestApiRouter()
                    : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiRouter" /> class.
        /// </summary>
        /// <param name="defaultApiSettings">The default API settings.</param>
        /// <param name="allowOptions">if set to <c>true</c> [allow options].</param>
        public RestApiRouter(RestApiSettings defaultApiSettings, bool allowOptions = false)
                    : base(defaultApiSettings, allowOptions)
        {
        }

        #endregion Constructor       
    }
}