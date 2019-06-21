using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class KVMetaExtensible : IKVMetaExtensible
    {
        /// <summary>
        /// Gets or sets the key-value meta.
        /// </summary>
        /// <value>
        /// The kv meta.
        /// </value>
        [JsonProperty(PropertyName = "kvMeta")]
        public KVMetaDictionary KVMeta { get; set; }
    }
}