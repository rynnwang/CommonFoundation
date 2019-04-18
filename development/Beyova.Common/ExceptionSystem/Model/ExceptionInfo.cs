using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova.Diagnostic
{
    /// <summary>
    /// Class ExceptionInfo.
    /// </summary>
    public class ExceptionInfo : ExceptionBase, IIdentifier, ILogBase, ICreatedStamp
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        [JsonProperty(PropertyName = "key")]
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the inner exception.
        /// </summary>
        /// <value>The inner exception.</value>
        [JsonProperty(PropertyName = "innerException")]
        public ExceptionInfo InnerException { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        [JsonProperty(PropertyName = "code")]
        public ExceptionCode Code { get; set; }

        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>The created stamp.</value>
        [JsonProperty(PropertyName = "createdStamp")]
        public DateTime? CreatedStamp { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        [JsonProperty(PropertyName = "data")]
        public JToken Data { get; set; }

        /// <summary>
        /// Gets or sets the scene.
        /// </summary>
        /// <value>The scene.</value>
        [JsonProperty(PropertyName = "scene")]
        public ExceptionScene Scene { get; set; }

        /// <summary>
        /// Gets or sets the hint.
        /// </summary>
        /// <value>The hint.</value>
        [JsonProperty(PropertyName = "hint")]
        public FriendlyHint Hint { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionInfo"/> class.
        /// </summary>
        public ExceptionInfo(ExceptionBase exceptionBase = null)
            : base(exceptionBase)
        {
            Key = Guid.NewGuid();
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}-{1}", Code, Message);
        }
    }
}