using System;
using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    /// Class BinaryStorageMetaData.
    /// </summary>
    public class BinaryStorageMetaData : BinaryStorageMetaBase, IOwnerIdentifiable
    {
        /// <summary>
        /// Gets or sets the hash.
        /// </summary>
        /// <value>The hash.</value>
        [JsonProperty(PropertyName = "hash")]
        public string Hash { get; set; }

        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>The created stamp.</value>
        [JsonProperty(PropertyName = "createdStamp")]
        public DateTime? CreatedStamp { get; set; }

        /// <summary>
        /// Gets or sets the last updated stamp.
        /// </summary>
        /// <value>The last updated stamp.</value>
        [JsonProperty(PropertyName = "lastUpdatedStamp")]
        public DateTime? LastUpdatedStamp { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        [JsonProperty(PropertyName = "state")]
        public BinaryStorageState State { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the owner key.
        /// </summary>
        /// <value>The owner key.</value>
        [JsonProperty(PropertyName = "ownerKey")]
        public Guid? OwnerKey { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryStorageMetaData" /> class.
        /// </summary>
        public BinaryStorageMetaData() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryStorageMetaBase" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        public BinaryStorageMetaData(BinaryStorageIdentifier identifier)
        {
            if (identifier != null)
            {
                this.Container = identifier.Container;
                this.Identifier = identifier.Identifier;
            }
        }
    }
}