using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;

namespace Beyova.Http
{
    /// <summary>
    /// Class HttpContextContainer
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public abstract class HttpContextContainer<TRequest, TResponse> : IHttpRequestActions, IHttpResponseActions, IHttpRequestCookieActions
    {
        /// <summary>
        /// The options
        /// </summary>
        protected HttpContextOptions<TRequest> _options;

        /// <summary>
        /// Gets or sets the request.
        /// </summary>
        /// <value>
        /// The request.
        /// </value>
        public TRequest Request { get; protected set; }

        /// <summary>
        /// Gets or sets the response.
        /// </summary>
        /// <value>
        /// The response.
        /// </value>
        public TResponse Response { get; protected set; }

        /// <summary>
        /// Determines whether this instance is local.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is local; otherwise, <c>false</c>.
        /// </returns>
        public bool IsLocal
        {
            get { return _options.IncomingHttpRequestExtensible?.IsLocal(Request) ?? false; }
        }

        /// <summary>
        /// Gets the client ip address.
        /// </summary>
        /// <value>
        /// The client ip address.
        /// </value>
        public string ClientIpAddress
        {
            get
            {
                return _options.IncomingHttpRequestExtensible?.GetClientIpAddress(Request);
            }
        }

        #region Abstract Properties

        /// <summary>
        /// Gets the request all header keys.
        /// </summary>
        /// <value>
        /// The request all header keys.
        /// </value>
        public abstract IEnumerable<string> RequestAllHeaderKeys { get; }

        /// <summary>
        /// Gets the raw URL.
        /// </summary>
        /// <value>
        /// The raw URL.
        /// </value>
        public abstract string RawUrl { get; }

        /// <summary>
        /// Gets the user agent.
        /// </summary>
        /// <value>
        /// The user agent.
        /// </value>
        public abstract string UserAgent { get; }

        /// <summary>
        /// Gets the user languages.
        /// </summary>
        /// <value>
        /// The user languages.
        /// </value>
        public abstract IEnumerable<string> UserLanguages { get; }

        /// <summary>
        /// Gets the network protocol.
        /// </summary>
        /// <value>
        /// The network protocol.
        /// </value>
        public abstract string NetworkProtocol { get; }

        /// <summary>
        /// Gets the query string.
        /// </summary>
        /// <value>
        /// The query string.
        /// </value>
        public abstract NameValueCollection QueryString { get; }

        /// <summary>
        /// Gets the request headers.
        /// </summary>
        /// <value>
        /// The request headers.
        /// </value>
        public abstract NameValueCollection RequestHeaders { get; }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public abstract Uri Url { get; }

        /// <summary>
        /// Gets the HTTP method.
        /// </summary>
        /// <value>
        /// The HTTP method.
        /// </value>
        public abstract string HttpMethod { get; }

        /// <summary>
        /// Gets or sets the request body stream.
        /// </summary>
        /// <value>
        /// The request body stream.
        /// </value>
        public abstract Stream RequestBodyStream { get; }

        /// <summary>
        /// Gets or sets the response status code.
        /// </summary>
        /// <value>
        /// The response status code.
        /// </value>
        public abstract HttpStatusCode ResponseStatusCode { get; set; }

        #endregion Abstract Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpContextContainer{TRequest, TResponse}" /> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <param name="options">The options.</param>
        protected HttpContextContainer(TRequest request, TResponse response, HttpContextOptions<TRequest> options)
        {
            request.CheckNullObject(nameof(request));
            Request = request;
            response.CheckNullObject(nameof(response));
            Response = response;
            _options = options ?? new HttpContextOptions<TRequest> { LanguageParameterKey = "lang" };
        }

        /// <summary>
        /// Tries the get header.
        /// </summary>
        /// <param name="headerKey">The header key.</param>
        /// <returns></returns>
        public abstract string TryGetRequestHeader(string headerKey);

        /// <summary>
        /// Reads the request body.
        /// </summary>
        /// <returns></returns>
        public abstract byte[] ReadRequestBody();

        /// <summary>
        /// Sets the response header.
        /// </summary>
        /// <param name="headerName">Name of the header.</param>
        /// <param name="value">The value.</param>
        public abstract void SetResponseHeader(string headerName, string value);

        /// <summary>
        /// Removes the response header.
        /// </summary>
        /// <param name="headerName">Name of the header.</param>
        public abstract void RemoveResponseHeader(string headerName);

        /// <summary>
        /// Writes the response body.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="contentType">Type of the content.</param>
        public abstract void WriteResponseBody(byte[] bytes, string contentType);

        /// <summary>
        /// Writes the response body.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">Type of the content.</param>
        public abstract void WriteResponseBody(Stream stream, string contentType);

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="cookieKey">The cookie key.</param>
        /// <returns></returns>
        public virtual string GetCookieValue(string cookieKey)
        {
            return GetCookieValues(cookieKey).SafeFirstOrDefault();
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <param name="cookieKey">The cookie key.</param>
        /// <returns></returns>
        public abstract IEnumerable<string> GetCookieValues(string cookieKey);

        /// <summary>
        /// Writes the response gzip body.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="contentType">Type of the content.</param>
        public abstract void WriteResponseGzipBody(byte[] bytes, string contentType);

        /// <summary>
        /// Writes the response deflate body.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="contentType">Type of the content.</param>
        public abstract void WriteResponseDeflateBody(byte[] bytes, string contentType);

        /// <summary>
        /// Writes the response gzip body.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">Type of the content.</param>
        public abstract void WriteResponseGzipBody(Stream stream, string contentType);

        /// <summary>
        /// Writes the response deflate body.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">Type of the content.</param>
        public abstract void WriteResponseDeflateBody(Stream stream, string contentType);
    }
}