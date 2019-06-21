using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class BinaryStorageMetaData.
    /// </summary>
    public class BinaryStorageMetaData : BinaryStorageMetaBase, IOwnerIdentifiable, IKVMetaExtensible
    {
        /// <summary>
        /// Gets or sets the hash.
        /// </summary>
        /// <value>The hash.</value>
        [JsonProperty(PropertyName = "hash")]
        public CryptoKey Hash { get; set; }

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
        /// Gets or sets the kv meta.
        /// </summary>
        /// <value>
        /// The kv meta.
        /// </value>
        [JsonProperty(PropertyName = "kvMeta")]
        public KVMetaDictionary KVMeta { get; set; }

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
                Container = identifier.Container;
                Identifier = identifier.Identifier;
            }
        }
    }
}