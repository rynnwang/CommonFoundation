using System;
using Beyova.Diagnostic;
using Beyova.Http;

namespace Beyova.Api.RestApi
{
    /// <summary>
    /// Class HttpContextContainer
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public abstract class HttpApiContextContainer<TRequest, TResponse> : HttpContextContainer<TRequest, TResponse>, IHttpResponseActions, IHttpRequestCookieActions
    {
        #region Runtime Context properties

        /// <summary>
        /// Gets the runtime context.
        /// </summary>
        /// <value>
        /// The runtime context.
        /// </value>
        public RuntimeContext RuntimeContext { get; internal set; }

        /// <summary>
        /// Gets or sets the entry stamp.
        /// </summary>
        /// <value>
        /// The entry stamp.
        /// </value>
        public DateTime? EntryStamp { get; set; }

        /// <summary>
        /// Gets or sets the exit stamp.
        /// </summary>
        /// <value>
        /// The exit stamp.
        /// </value>
        public DateTime? ExitStamp { get; set; }

        /// <summary>
        /// Gets or sets the API event.
        /// </summary>
        /// <value>
        /// The API event.
        /// </value>
        public ApiEventLog ApiEvent { get; protected set; }

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public RestApiSettings Settings { get; set; }

        /// <summary>
        /// Gets or sets the trace identifier.
        /// </summary>
        /// <value>
        /// The trace identifier.
        /// </value>
        public string TraceId { get; set; }

        /// <summary>
        /// Gets or sets the trace sequence.
        /// </summary>
        /// <value>
        /// The trace sequence.
        /// </value>
        public int? TraceSequence { get; set; }

        /// <summary>
        /// Gets or sets the base exception.
        /// </summary>
        /// <value>
        /// The base exception.
        /// </value>
        public BaseException BaseException { get; set; }

        #endregion Runtime Context properties

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpApiContextContainer{TRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <param name="options">The options.</param>
        protected HttpApiContextContainer(TRequest request, TResponse response, HttpContextOptions<TRequest> options)
            : base(request, response, options)
        {
        }

        /// <summary>
        /// Collects the tracking information.
        /// </summary>
        public void CollectTrackingInfo(long bodyLength)
        {
            if (Settings != null && Settings.ApiTracking != null && Settings.TrackingEvent && RuntimeContext != null && !RuntimeContext.OmitApiEvent)
            {
                ApiEvent = new ApiEventLog
                {
                    RawUrl = RawUrl,
                    EntryStamp = EntryStamp,
                    TraceId = TraceId,
                    // If request came from ApiTransport or other proxy ways, ORIGINAL stands for the IP ADDRESS from original requester.
                    IpAddress = TryGetRequestHeader(Settings?.OriginalIpAddressHeaderKey.SafeToString(HttpConstants.HttpHeader.ORIGINAL)).SafeToString(ClientIpAddress),
                    CultureCode = UserLanguages.SafeFirstOrDefault(),
                    ContentLength = bodyLength,
                    OperatorCredential = ContextHelper.CurrentCredential as BaseCredential,
                    ServiceIdentifier = RuntimeContext.ApiInstance?.GetType()?.Name,
                    ServerIdentifier = EnvironmentCore.MachineName
                };

                var clientIdentifierHeaderKey = Settings.ClientIdentifierHeaderKey;
                if (!string.IsNullOrWhiteSpace(clientIdentifierHeaderKey))
                {
                    ApiEvent.ClientIdentifier = TryGetRequestHeader(clientIdentifierHeaderKey);
                }
            }

            if (!string.IsNullOrWhiteSpace(TraceId))
            {
                ApiTraceContext.Initialize(TraceId, TraceSequence, EntryStamp);
            }
        }
    }
}