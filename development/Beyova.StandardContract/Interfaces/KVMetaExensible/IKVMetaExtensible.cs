using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Interface IKVMetaExtensible is used in models, where needs to supprt un-predictable highly extensibility.
    /// Based on this interface, series of extension are supported, focus on data perspective (like DAL, Query, etc.)
    /// </summary>
    public interface IKVMetaExtensible
    {
        /// <summary>
        /// Gets or sets the kv meta.
        /// </summary>
        /// <value>
        /// The kv meta.
        /// </value>
        Dictionary<string, JValue> KVMeta { get; }
    }
}