using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Beyova.Http;

namespace Beyova.Api.RestApi
{
    /// <summary>
    /// Class HttpApiContextContainer
    /// </summary>
    public sealed class HttpApiContextContainer : HttpApiContextContainer<HttpRequest, HttpResponse>
    {
        /// <summary>
        /// The cached request body byte array
        /// </summary>
        private byte[] _cachedRequestBodyByteArray = null;

        #region Abstract Properties

        /// <summary>
        /// Gets the request all header keys.
        /// </summary>
        /// <value>
        /// The request all header keys.
        /// </value>
        public override IEnumerable<string> RequestAllHeaderKeys
        {
            get
            {
                return Request?.Headers?.AllKeys;
            }
        }

        /// <summary>
        /// Gets the raw URL.
        /// </summary>
        /// <value>
        /// The raw URL.
        /// </value>
        public override string RawUrl
        {
            get { return Request?.RawUrl; }
        }

        /// <summary>
        /// Gets the user agent.
        /// </summary>
        /// <value>
        /// The user agent.
        /// </value>
        public override string UserAgent { get { return Request?.UserAgent; } }

        /// <summary>
        /// Gets the user languages.
        /// </summary>
        /// <value>
        /// The user languages.
        /// </value>
        public override IEnumerable<string> UserLanguages { get { return Request?.UserLanguages; } }

        /// <summary>
        /// Gets the network protocol.
        /// </summary>
        /// <value>
        /// The network protocol.
        /// </value>
        public override string NetworkProtocol { get { return Request?.Url.Scheme; } }

        /// <summary>
        /// Gets the query string.
        /// </summary>
        /// <value>
        /// The query string.
        /// </value>
        public override NameValueCollection QueryString { get { return Request?.QueryString; } }

        /// <summary>
        /// Gets the request headers.
        /// </summary>
        /// <value>
        /// The request headers.
        /// </value>
        public override NameValueCollection RequestHeaders { get { return Request?.Headers; } }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public override Uri Url { get { return Request?.Url; } }

        /// <summary>
        /// Gets the HTTP method.
        /// </summary>
        /// <value>
        /// The HTTP method.
        /// </value>
        public override string HttpMethod { get { return Request?.HttpMethod; } }

        /// <summary>
        /// Gets the request body stream.
        /// </summary>
        /// <value>
        /// The request body stream.
        /// </value>
        public override Stream RequestBodyStream
        {
            get { return Request?.InputStream; }
        }

        /// <summary>
        /// Gets or sets the response status code.
        /// </summary>
        /// <value>
        /// The response status code.
        /// </value>
        public override HttpStatusCode ResponseStatusCode
        {
            get
            {
                return (HttpStatusCode)Response.StatusCode;
            }
            set
            {
                Response.StatusCode = (int)value;
            }
        }

        #endregion Abstract Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpApiContextContainer" /> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <param name="options">The options.</param>
        public HttpApiContextContainer(HttpRequest request, HttpResponse response, HttpContextOptions<HttpRequest> options = null)
                : base(request, response, options)
        {
        }

        /// <summary>
        /// Tries the get header.
        /// </summary>
        /// <param name="headerKey">The header key.</param>
        /// <returns></returns>
        public override string TryGetRequestHeader(string headerKey)
        {
            return Request.TryGetHeader(headerKey);
        }

        /// <summary>
        /// Sets the response header.
        /// </summary>
        /// <param name="headerName">Name of the header.</param>
        /// <param name="value">The value.</param>
        public override void SetResponseHeader(string headerName, string value)
        {
            Response.SafeSetHttpHeader(headerName, value);
        }

        /// <summary>
        /// Removes the response header.
        /// </summary>
        /// <param name="headerName">Name of the header.</param>
        public override void RemoveResponseHeader(string headerName)
        {
            if (!string.IsNullOrWhiteSpace(headerName) && (Response?.Headers?.AllKeys?.Contains(headerName, false) ?? false))
            {
                Response.Headers.Remove(headerName);
            }
        }

        /// <summary>
        /// Writes the response body.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="contentType">Type of the content.</param>
        public override void WriteResponseBody(byte[] bytes, string contentType)
        {
            if (bytes != null)
            {
                WriteResponseBody(bytes.ToStream(), contentType);
            }
        }

        /// <summary>
        /// Writes the response body.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">Type of the content.</param>
        public override void WriteResponseBody(Stream stream, string contentType)
        {
            if (Response != null && stream != null)
            {
                stream.CopyTo(Response.OutputStream);

                if (!string.IsNullOrWhiteSpace(contentType))
                {
                    Response.ContentType = contentType;
                }
            }
        }

        /// <summary>
        /// Writes the response gzip body.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="contentType">Type of the content.</param>
        public override void WriteResponseGzipBody(byte[] bytes, string contentType)
        {
            if (Response != null)
            {
                Response.Filter = new System.IO.Compression.GZipStream(Response.Filter, System.IO.Compression.CompressionMode.Compress);
                Response.Headers.Remove(HttpConstants.HttpHeader.ContentEncoding);
                Response.AppendHeader(HttpConstants.HttpHeader.ContentEncoding, HttpConstants.HttpValues.GZip);
            }
            if (!string.IsNullOrWhiteSpace(contentType))
            {
                Response.SafeSetHttpHeader(HttpConstants.HttpHeader.ContentType, contentType);
            }
            Response.OutputStream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes the response deflate body.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="contentType">Type of the content.</param>
        public override void WriteResponseDeflateBody(byte[] bytes, string contentType)
        {
            if (Response != null)
            {
                Response.Filter = new System.IO.Compression.DeflateStream(Response.Filter, System.IO.Compression.CompressionMode.Compress);
                Response.Headers.Remove(HttpConstants.HttpHeader.ContentEncoding);
                Response.AppendHeader(HttpConstants.HttpHeader.ContentEncoding, HttpConstants.HttpValues.Deflate);
            }
            if (!string.IsNullOrWhiteSpace(contentType))
            {
                Response.SafeSetHttpHeader(HttpConstants.HttpHeader.ContentType, contentType);
            }
            Response.OutputStream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes the response gzip body.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">Type of the content.</param>
        public override void WriteResponseGzipBody(Stream stream, string contentType)
        {
            WriteResponseGzipBody(stream.ReadStreamToBytes(), contentType);
        }

        /// <summary>
        /// Writes the response deflate body.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">Type of the content.</param>
        public override void WriteResponseDeflateBody(Stream stream, string contentType)
        {
            WriteResponseDeflateBody(stream.ReadStreamToBytes(), contentType);
        }

        /// <summary>
        /// Reads the request body.
        /// </summary>
        /// <returns></returns>
        public override byte[] ReadRequestBody()
        {
            // Regarding input stream can be read only once, need to cache it in case to be re-called in difference places.
            if (_cachedRequestBodyByteArray == null)
            {
                _cachedRequestBodyByteArray = Request?.InputStream.ReadStreamToBytes(true);
            }

            return _cachedRequestBodyByteArray;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="cookieKey">The cookie key.</param>
        /// <returns></returns>
        public override string GetCookieValue(string cookieKey)
        {
            return Request?.Cookies.TryGetValue(cookieKey);
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <param name="cookieKey">The cookie key.</param>
        /// <returns></returns>
        public override IEnumerable<string> GetCookieValues(string cookieKey)
        {
            return (Request?.Cookies?.Get(cookieKey)?.Values as IEnumerable<string>)?.Select(x => x.ToUrlDecodedText());
        }
    }
}