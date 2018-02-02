using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova.Http
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHttpRequestActions
    {
        /// <summary>
        /// Tries the get request header.
        /// </summary>
        /// <param name="headerKey">The header key.</param>
        /// <returns></returns>
        string TryGetRequestHeader(string headerKey);

        /// <summary>
        /// Gets the HTTP method.
        /// </summary>
        /// <value>
        /// The HTTP method.
        /// </value>
        string HttpMethod { get; }

        /// <summary>
        /// Gets the raw URL.
        /// </summary>
        /// <value>
        /// The raw URL.
        /// </value>
        string RawUrl { get; }

        /// <summary>
        /// Reads the body of request.
        /// </summary>
        /// <returns></returns>
        byte[] ReadRequestBody();
    }
}
