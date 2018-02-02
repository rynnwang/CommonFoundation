using System.IO;
using System.Net;

namespace Beyova.Http
{
    /// <summary>
    /// Interface IHttpResponseApiActions
    /// </summary>
    public interface IHttpResponseApiActions
    {
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
