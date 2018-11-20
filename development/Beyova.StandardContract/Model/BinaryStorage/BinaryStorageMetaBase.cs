using System;
using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    /// Class BinaryStorageMetaData.
    /// </summary>
    public class BinaryStorageMetaBase : BinaryStorageIdentifier
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// <remarks>
        /// http://www.w3.org/wiki/Evolution/MIME
        /// </remarks>
        /// </summary>
        /// <value>
        /// The MIME.
        /// </value>
        [JsonProperty(PropertyName = "contentType")]
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the MIME.
        /// <remarks>
        /// http://www.w3.org/wiki/Evolution/MIME
        /// </remarks>
        /// </summary>
        /// <value>The MIME.</value>
        [JsonIgnore]
        [Obsolete]
        public string Mime { get { return ContentType; } set { ContentType = value; } }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>The length.</value>
        [JsonProperty(PropertyName = "length")]
        public long? Length { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// <remarks>It is used when storage is image or video.</remarks>
        /// </summary>
        /// <value>The width.</value>
        [JsonProperty(PropertyName = "width")]
        public int? Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// <remarks>It is used when storage is image or video.</remarks>
        /// </summary>
        /// <value>The height.</value>
        [JsonProperty(PropertyName = "height")]
        public int? Height { get; set; }

        /// <summary>
        /// Gets or sets the duration.
        /// <remarks>It is used when storage is audio or video. Unit: second.</remarks>
        /// </summary>
        /// <value>The duration.</value>
        [JsonProperty(PropertyName = "duration")]
        public int? Duration { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryStorageMetaBase"/> class.
        /// </summary>
        /// <param name="metaBase">The meta base.</param>
        public BinaryStorageMetaBase(BinaryStorageMetaBase metaBase)
            : this(metaBase as BinaryStorageIdentifier)
        {
            if (metaBase != null)
            {
                this.Name = metaBase.Name;
                this.ContentType = metaBase.ContentType;
                this.Length = metaBase.Length;
                this.Width = metaBase.Width;
                this.Height = metaBase.Height;
                this.Duration = metaBase.Duration;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryStorageMetaBase"/> class.
        /// </summary>
        public BinaryStorageMetaBase() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryStorageMetaBase"/> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        public BinaryStorageMetaBase(BinaryStorageIdentifier identifier)
        {
            if (identifier != null)
            {
                this.Container = identifier.Container;
                this.Identifier = identifier.Identifier;
            }
        }
    }
}