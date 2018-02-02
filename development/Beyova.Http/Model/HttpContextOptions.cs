using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova.Http
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpContextOptions<TRequest>
    {
        /// <summary>
        /// Gets or sets a value indicating whether [cookie compatible].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [cookie compatible]; otherwise, <c>false</c>.
        /// </value>
        public bool CookieCompatible { get; set; }

        /// <summary>
        /// Gets or sets the language parameter key.
        /// </summary>
        /// <value>
        /// The language parameter key.
        /// </value>
        public string LanguageParameterKey { get; set; }

        /// <summary>
        /// Gets or sets the incoming HTTP request extensible.
        /// </summary>
        /// <value>
        /// The incoming HTTP request extensible.
        /// </value>
        public IIncomingHttpRequestExtensible<TRequest> IncomingHttpRequestExtensible { get; set; }

        /// <summary>
        /// Gets or sets the maximum size of the request body.
        /// </summary>
        /// <value>
        /// The maximum size of the request body.
        /// </value>
        public long? RequestBodyMaxSize { get; set; }
    }
}
