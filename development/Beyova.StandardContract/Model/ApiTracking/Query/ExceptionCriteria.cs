using System;
using Newtonsoft.Json;

namespace Beyova.Diagnostic
{
    /// <summary>
    /// Class ExceptionCriteria.
    /// </summary>
    public class ExceptionCriteria : GlobalApiUniqueIdentifier, ICriteria, IStampCriteria
    {
        #region Property

        /// <summary>
        /// Gets or sets the major code.
        /// </summary>
        /// <value>The major code.</value>
        [JsonProperty(PropertyName = "majorCode")]
        public ExceptionCode.MajorCode? MajorCode { get; set; }

        /// <summary>
        /// Gets or sets the minor code.
        /// </summary>
        /// <value>The minor code.</value>
        [JsonProperty(PropertyName = "minorCode")]
        public string MinorCode { get; set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        [JsonProperty(PropertyName = "key")]
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets from stamp.
        /// </summary>
        /// <value>From stamp.</value>
        [JsonProperty(PropertyName = "fromStamp")]
        public DateTime? FromStamp { get; set; }

        /// <summary>
        /// Gets or sets to stamp.
        /// </summary>
        /// <value>To stamp.</value>
        [JsonProperty(PropertyName = "toStamp")]
        public DateTime? ToStamp { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the keyword.
        /// </summary>
        /// <value>
        /// The keyword.
        /// </value>
        [JsonProperty(PropertyName = "keyword")]
        public string Keyword { get; set; }

        /// <summary>
        /// Gets or sets the event key.
        /// </summary>
        /// <value>
        /// The event key.
        /// </value>
        [JsonProperty(PropertyName = "eventKey")]
        public string EventKey { get; set; }

        /// <summary>
        /// Gets or sets the operator keyword.
        /// </summary>
        /// <value>
        /// The operator keyword.
        /// </value>
        [JsonProperty(PropertyName = "operatorCredential")]
        public string OperatorCredential { get; set; }

        /// <summary>
        /// Gets or sets the type of the exception.
        /// </summary>
        /// <value>
        /// The type of the exception.
        /// </value>
        [JsonProperty(PropertyName = "exceptionType")]
        public string ExceptionType { get; set; }

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
        /// Initializes a new instance of the <see cref="ExceptionCriteria" /> class.
        /// </summary>
        /// <param name="exceptionBase">The exception base.</param>
        public ExceptionCriteria(ExceptionBase exceptionBase)
            : base(exceptionBase)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionCriteria"/> class.
        /// </summary>
        public ExceptionCriteria()
            : this(null)
        {
        }
    }
}