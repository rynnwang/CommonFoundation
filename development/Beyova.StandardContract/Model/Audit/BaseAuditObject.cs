using System;
using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TContent">The type of the content.</typeparam>
    public abstract class BaseAuditObject<TContent> : BaseAuditObject<Guid?, TContent>, IIdentifier
    {
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TAuditKey">The type of the audit key.</typeparam>
    /// <typeparam name="TContent">The type of the content.</typeparam>
    public abstract class BaseAuditObject<TAuditKey, TContent>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [JsonProperty("key")]
        public TAuditKey Key { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        [JsonProperty("entityType")]
        public string EntityType { get; set; }

        /// <summary>
        /// Gets or sets the primary key.
        /// </summary>
        /// <value>The primary key.</value>
        [JsonProperty("primaryKey")]
        public string PrimaryKey { get; set; }

        /// <summary>
        /// Gets or sets the secondary key.
        /// </summary>
        /// <value>
        /// The secondary key.
        /// </value>
        [JsonProperty("secondaryKey")]
        public string SecondaryKey { get; set; }

        /// <summary></summary>
        /// <value></value>
        [JsonProperty("methodName")]
        public string MethodName { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        [JsonProperty("content")]
        public TContent Content { get; set; }

        /// <summary>
        /// Gets or sets the stamp.
        /// </summary>
        /// <value>
        /// The stamp.
        /// </value>
        [JsonProperty("stamp")]
        public DateTime? Stamp { get; set; }

        /// <summary>
        /// Gets or sets the operator key.
        /// </summary>
        /// <value>
        /// The operator key.
        /// </value>
        [JsonProperty("operatorKey")]
        public string OperatorKey { get; set; }
    }
}