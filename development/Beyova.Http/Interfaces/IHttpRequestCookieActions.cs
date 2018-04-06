using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova.Http
{
    /// <summary>
    /// Interface IHttpRequestCookieActions
    /// </summary>
    public interface IHttpRequestCookieActions
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="cookieKey">The cookie key.</param>
        /// <returns></returns>
        string GetCookieValue(string cookieKey);

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <param name="cookieKey">The cookie key.</param>
        /// <returns></returns>
        IEnumerable<string> GetCookieValues(string cookieKey);
    }
}
