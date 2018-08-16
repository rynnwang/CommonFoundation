using System.IO;
using System.Net;

namespace Beyova.Http
{
    /// <summary>
    /// Interface IHttpResponseActions
    /// </summary>
    public interface IHttpResponseActions
    {
        /// <summary>
        /// Sets the response header.
        /// </summary>
        /// <param name="headerName">Name of the header.</param>
        /// <param name="value">The value.</param>
        void SetResponseHeader(string headerName, string value);

        /// <summary>
        /// Removes the response header.
        /// </summary>
        /// <param name="headerName">Name of the header.</param>
        void RemoveResponseHeader(string headerName);

        /// <summary>
        /// Gets or sets the response status code.
        /// </summary>
        /// <value>
        /// The response status code.
        /// </value>
        HttpStatusCode ResponseStatusCode { get; set; }

        /// <summary>
        /// Writes the response body.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="contentType">Type of the content.</param>
        void WriteResponseBody(byte[] bytes, string contentType);

        /// <summary>
        /// Writes the response body.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">Type of the content.</param>
        void WriteResponseBody(Stream stream, string contentType);

        /// <summary>
        /// Writes the response gzip body.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="contentType">Type of the content.</param>
        void WriteResponseGzipBody(byte[] bytes, string contentType);

        /// <summary>
        /// Writes the response deflate body.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="contentType">Type of the content.</param>
        void WriteResponseDeflateBody(byte[] bytes, string contentType);

        /// <summary>
        /// Writes the response gzip body.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">Type of the content.</param>
        void WriteResponseGzipBody(Stream stream, string contentType);

        /// <summary>
        /// Writes the response deflate body.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">Type of the content.</param>
        void WriteResponseDeflateBody(Stream stream, string contentType);
    }
}
