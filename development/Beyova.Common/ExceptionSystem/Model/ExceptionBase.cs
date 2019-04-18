using Newtonsoft.Json;

namespace Beyova.Diagnostic
{
    /// <summary>
    /// Class ExceptionBase.
    /// </summary>
    public class ExceptionBase : GlobalApiUniqueIdentifier
    {
        #region Property

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the target site.
        /// </summary>
        /// <value>The target site.</value>
        [JsonProperty(PropertyName = "targetSite")]
        public string TargetSite { get; set; }

        /// <summary>
        /// Gets or sets the stack trace.
        /// </summary>
        /// <value>The stack trace.</value>
        [JsonProperty(PropertyName = "stackTrace")]
        public string StackTrace { get; set; }

        /// <summary>
        /// Gets or sets the type of the exception.
        /// </summary>
        /// <value>The type of the exception.</value>
        [JsonProperty(PropertyName = "exceptionType")]
        public string ExceptionType { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the event key. Primarily for ApiEvent Key. It can be any other system event key.
        /// </summary>
        /// <value>
        /// The event key.
        /// </value>
        [JsonProperty(PropertyName = "eventKey")]
        public string EventKey { get; set; }

        /// <summary>
        /// Gets or sets the operator credential.
        /// </summary>
        /// <value>The operator credential.</value>
        [JsonProperty(PropertyName = "operatorCredential")]
        public BaseCredential OperatorCredential { get; set; }

        /// <summary>
        /// Gets or sets the raw URL.
        /// </summary>
        /// <value>
        /// The raw URL.
        /// </value>
        [JsonProperty(PropertyName = "rawUrl")]
        public string RawUrl { get; set; }

        #endregion Property

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionBase" /> class.
        /// </summary>
        /// <param name="exceptionBase">The exception base.</param>
        public ExceptionBase(ExceptionBase exceptionBase = null)
            : base(exceptionBase)
        {
            if (exceptionBase != null)
            {
                Message = exceptionBase.Message;
                RawUrl = exceptionBase.RawUrl;
                TargetSite = exceptionBase.TargetSite;
                StackTrace = exceptionBase.StackTrace;
                Source = exceptionBase.Source;
                OperatorCredential = exceptionBase.OperatorCredential;
                EventKey = exceptionBase.EventKey;
            }
        }
    }
}