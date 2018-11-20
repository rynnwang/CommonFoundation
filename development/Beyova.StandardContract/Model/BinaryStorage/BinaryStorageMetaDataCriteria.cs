using System;
using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    /// Class BinaryStorageMetaDataCriteria.
    /// </summary>
    public class BinaryStorageMetaDataCriteria : BinaryStorageIdentifier, IOwnerIdentifiable
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the hash.
        /// </summary>
        /// <value>The hash.</value>
        [JsonProperty(PropertyName = "hash")]
        public string Hash { get; set; }

        /// <summary>
        /// Gets or sets the MIME.
        /// <remarks>
        /// http://www.w3.org/wiki/Evolution/MIME
        /// </remarks>
        /// </summary>
        /// <value>The MIME.</value>
        [JsonProperty(PropertyName = "contentType")]
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the MIME.
        /// </summary>
        /// <value>
        /// The MIME.
        /// </value>
        [JsonIgnore]
        [Obsolete]
        public string Mime { get; set; }

        /// <summary>
        /// Gets or sets the minimum length.
        /// </summary>
        /// <value>The minimum length.</value>
        [JsonProperty(PropertyName = "lengthFrom")]
        public long? LengthFrom { get; set; }

        /// <summary>
        /// Gets or sets the maximum length.
        /// </summary>
        /// <value>The maximum length.</value>
        [JsonProperty(PropertyName = "lengthTo")]
        public long? LengthTo { get; set; }

        /// <summary>
        /// Gets or sets the minimum width.
        /// </summary>
        /// <value>The minimum width.</value>
        [JsonProperty(PropertyName = "widthFrom")]
        public int? WidthFrom { get; set; }

        /// <summary>
        /// Gets or sets the maximum width.
        /// </summary>
        /// <value>The maximum width.</value>
        [JsonProperty(PropertyName = "widthTo")]
        public int? WidthTo { get; set; }

        /// <summary>
        /// Gets or sets the minimum height.
        /// </summary>
        /// <value>The minimum height.</value>
        [JsonProperty(PropertyName = "heightFrom")]
        public int? HeightFrom { get; set; }

        /// <summary>
        /// Gets or sets the maximum height.
        /// </summary>
        /// <value>The maximum height.</value>
        [JsonProperty(PropertyName = "heightTo")]
        public int? HeightTo { get; set; }

        /// <summary>
        /// Gets or sets the minimum duration.
        /// </summary>
        /// <value>The minimum duration.</value>
        [JsonProperty(PropertyName = "durationFrom")]
        public int? DurationFrom { get; set; }

        /// <summary>
        /// Gets or sets the maximum duration.
        /// </summary>
        /// <value>The maximum duration.</value>
        [JsonProperty(PropertyName = "durationTo")]
        public int? DurationTo { get; set; }

        /// <summary>
        /// Gets or sets the owner key.
        /// </summary>
        /// <value>The owner key.</value>
        [JsonProperty(PropertyName = "ownerKey")]
        public Guid? OwnerKey { get; set; }

        /// <summary>
        /// Gets or sets to stamp.
        /// </summary>
        /// <value>To stamp.</value>
        [JsonProperty(PropertyName = "toStamp")]
        public DateTime? ToStamp { get; set; }

        /// <summary>
        /// Gets or sets from stamp.
        /// </summary>
        /// <value>From stamp.</value>
        [JsonProperty(PropertyName = "fromStamp")]
        public DateTime? FromStamp { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        [JsonProperty(PropertyName = "count")]
        public int? Count { get; set; }
    }
}