namespace Beyova
{
    /// <summary>
    /// Class HttpConstants.
    /// </summary>
    public static class HttpConstants
    {
        /// <summary>
        /// The user password format
        /// </summary>
        public const string UserPasswordFormat = "{0}:{1}";

        /// <summary>
        /// Class HttpMethod.
        /// </summary>
        public static class HttpMethod
        {
            /// <summary>
            /// The get
            /// </summary>
            public const string Get = "GET";

            /// <summary>
            /// The post
            /// </summary>
            public const string Post = "POST";

            /// <summary>
            /// The put
            /// </summary>
            public const string Put = "PUT";

            /// <summary>
            /// The delete
            /// </summary>
            public const string Delete = "DELETE";

            /// <summary>
            /// The head
            /// </summary>
            public const string Head = "HEAD";

            /// <summary>
            /// The options
            /// </summary>
            public const string Options = "OPTIONS";

            /// <summary>
            /// The trace
            /// </summary>
            public const string Trace = "TRACE";

            /// <summary>
            /// The patch
            /// </summary>
            public const string Patch = "PATCH";

            /// <summary>
            /// The lock
            /// </summary>
            public const string Lock = "LOCK";

            /// <summary>
            /// The unlock
            /// </summary>
            public const string Unlock = "UNLOCK";

            /// <summary>
            /// The fetch
            /// </summary>
            public const string Fetch = "FETCH";
        }

        /// <summary>
        /// Class HttpHeader.
        /// </summary>
        public static class HttpHeader
        {
            /// <summary>
            /// The cookie
            /// </summary>
            public const string Cookie = "Cookie";

            /// <summary>
            /// The token
            /// </summary>
            public const string TOKEN = "X-BA-TOKEN";

            /// <summary>
            /// If modified since
            /// </summary>
            public const string IfModifiedSince = "If-Modified-Since";

            /// <summary>
            /// The last modified
            /// </summary>
            public const string LastModified = "Last-Modified";

            /// <summary>
            /// The client identifier
            /// </summary>
            public const string CLIENTIDENTIFIER = "X-BA-CLIENT-ID";

            /// <summary>
            /// The server exit time
            /// </summary>
            public const string SERVEREXITTIME = "X-SERVER-EXIT-UTC-TIME";

            /// <summary>
            /// The server entry time
            /// </summary>
            public const string SERVERENTRYTIME = "X-SERVER-ENTRY-UTC-TIME";

            /// <summary>
            /// </summary>
            public const string SERVERNAME = "X-SERVER-NAME";

            /// <summary>
            /// The original IP address
            /// </summary>
            public const string ORIGINAL = "X-BA-ORIGINAL";

            /// <summary>
            /// The trace ID
            /// </summary>
            public const string TRACEID = "X-BA-TRACE-ID";

            /// <summary>
            /// The debug ID
            /// </summary>
            public const string DEBUG = "X-BA-DEBUG-ID";

            /// <summary>
            /// The operator
            /// </summary>
            public const string OPERATOR = "X-BA-OPERATOR";

            /// <summary>
            /// The poweredby
            /// </summary>
            public const string POWEREDBY = "X-Powered-By";

            /// <summary>
            /// The beyova api version
            /// </summary>
            public const string BEYOVAAPIVERSION = "X-Beyova-Version";

            /// <summary>
            /// The trace sequence
            /// </summary>
            public const string TRACESEQUENCE = "X-BA-TRACE-SEQ";

            /// <summary>
            /// The content encoding
            /// </summary>
            public const string ContentEncoding = "Content-Encoding";

            /// <summary>
            /// The accept encoding
            /// </summary>
            public const string AcceptEncoding = "Accept-Encoding";

            /// <summary>
            /// The accept
            /// </summary>
            public const string Accept = "Accept";

            /// <summary>
            /// The host
            /// </summary>
            public const string Host = "Host";

            /// <summary>
            /// The authorization
            /// </summary>
            public const string Authorization = "Authorization";

            /// <summary>
            /// The referrer
            /// </summary>
            public const string Referrer = "Referrer";

            /// <summary>
            /// The user agent
            /// </summary>
            public const string UserAgent = "UserAgent";

            /// <summary>
            /// The content disposition
            /// </summary>
            public const string ContentDisposition = "Content-Disposition";

            /// <summary>
            /// The e tag
            /// </summary>
            public const string ETag = "ETag";

            /// <summary>
            /// The content m d5
            /// </summary>
            public const string ContentMD5 = "Content-MD5";

            /// <summary>
            /// The transfer encoding
            /// </summary>
            public const string TransferEncoding = "Transfer-Encoding";

            /// <summary>
            /// The content length
            /// </summary>
            public const string ContentLength = "Content-Length";

            /// <summary>
            /// The access control allow origin
            /// </summary>
            public const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";

            /// <summary>
            /// The access control allow headers
            /// </summary>
            public const string AccessControlAllowHeaders = "Access-Control-Allow-Headers";

            /// <summary>
            /// The access control allow methods
            /// </summary>
            public const string AccessControlAllowMethods = "Access-Control-Allow-Methods";

            /// <summary>
            /// The content type
            /// </summary>
            public const string ContentType = "Content-Type";

            /// <summary>
            /// The location
            /// </summary>
            public const string Location = "Location";
        }

        /// <summary>
        /// Class HttpProtocols.
        /// </summary>
        public static class HttpProtocols
        {
            /// <summary>
            /// The HTTP
            /// </summary>
            public const string Http = "http";

            /// <summary>
            /// The HTTP protocol
            /// </summary>
            public const string HttpProtocol = "http://";

            /// <summary>
            /// The HTTPS
            /// </summary>
            public const string Https = "https";

            /// <summary>
            /// The HTTPS protocol
            /// </summary>
            public const string HttpsProtocol = "https://";
        }

        /// <summary>
        /// Class HttpValues.
        /// </summary>
        public static class HttpValues
        {
            /// <summary>
            /// The GZIP
            /// </summary>
            public const string GZip = "gzip";

            /// <summary>
            /// The deflate
            /// </summary>
            public const string Deflate = "deflate";

            /// <summary>
            /// The localhost
            /// </summary>
            public const string Localhost = "localhost";

            /// <summary>
            /// The localhost ip v4
            /// </summary>
            public const string LocalhostIpV4 = "127.0.0.1";

            /// <summary>
            /// The localhost ip v6
            /// </summary>
            public const string LocalhostIpV6 = "::1";

            /// <summary>
            /// The powered by beyova
            /// </summary>
            public const string PoweredByBeyova = "Beyova";
        }

        /// <summary>
        /// Class QueryString.
        /// </summary>
        public static class QueryString
        {
            /// <summary>
            /// The language
            /// </summary>
            public const string Language = "language";
        }

        /// <summary>
        /// Class ContentType.
        /// </summary>
        public static class ContentType
        {
            /// <summary>
            /// The language
            /// </summary>
            public const string Json = "application/json";

            /// <summary>
            /// The HTML
            /// </summary>
            public const string Html = "text/html";

            /// <summary>
            /// The binary
            /// </summary>
            public const string BinaryDefault = "application/octet-stream";

            /// <summary>
            /// The zip file
            /// </summary>
            public const string ZipFile = "application/zip";

            /// <summary>
            /// The PNG image
            /// </summary>
            public const string PngImage = "image/png";

            /// <summary>
            /// The JPEG image
            /// </summary>
            public const string JpegImage = "image/jpeg";

            /// <summary>
            /// The GIF image
            /// </summary>
            public const string GifImage = "image/gif";

            /// <summary>
            /// The MP3 audio
            /// </summary>
            public const string Mp3Audio = "audio/mpeg";

            /// <summary>
            /// The MP4 video
            /// </summary>
            public const string Mp4Video= "video/mp4";

            /// <summary>
            /// The text
            /// </summary>
            public const string Text = "text/plain";

            /// <summary>
            /// The XML
            /// </summary>
            public const string Xml = "text/xml";

            /// <summary>
            /// The form submit
            /// </summary>
            public const string FormSubmit = "application/x-www-form-urlencoded";

            /// <summary>
            /// The apk
            /// </summary>
            public const string Apk = "application/vnd.android.package-archive";

            /// <summary>
            /// The ipa
            /// </summary>
            public const string Ipa = "application/vnd.iphone";

            /// <summary>
            /// The excel
            /// </summary>
            public const string Excel = "application/x-xls";

            /// <summary>
            /// The PDF
            /// </summary>
            public const string Pdf = "application/pdf";

            /// <summary>
            /// The CSS
            /// </summary>
            public const string Css = "text/css";

            /// <summary>
            /// The java script
            /// </summary>
            public const string JavaScript = "application/x-javascript";

            /// <summary>
            /// The markdown
            /// </summary>
            public const string Markdown = "text/markdown";

            /// <summary>
            /// The power point
            /// </summary>
            public const string PowerPoint = "application/vnd.ms-powerpoint";

            /// <summary>
            /// The power point x
            /// </summary>
            public const string PowerPointX = "application/vnd.openxmlformats-officedocument.presentationml.presentation";

            /// <summary>
            /// The excel x
            /// </summary>
            public const string ExcelX = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            /// <summary>
            /// The word
            /// </summary>
            public const string Word = "application/msword";

            /// <summary>
            /// The word x
            /// </summary>
            public const string WordX = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        }
    }
}