using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public class KVMetaUpdateRequest : KVMetaExtensible
    {
        /// <summary>
        /// Gets or sets the update strategy.
        /// </summary>
        /// <value>
        /// The update strategy.
        /// </value>
        [JsonProperty("updateStrategy")]
        public KVMetaUpdateStrategy UpdateStrategy { get; set; }

        /// <summary>
        /// Merges to.
        /// </summary>
        /// <param name="kvMeta">The kv meta.</param>
        public void MergeTo(IKVMetaExtensible kvMeta)
        {
            if (kvMeta != null)
            {
                if (kvMeta.KVMeta == null)
                {
                    kvMeta.KVMeta = new KVMetaDictionary();
                }

                switch (UpdateStrategy)
                {
                    case KVMetaUpdateStrategy.Clear:
                        kvMeta.KVMeta.Clear();
                        break;
                    case KVMetaUpdateStrategy.Default:
                    case KVMetaUpdateStrategy.Overwrite:
                        kvMeta.KVMeta = KVMeta ?? new KVMetaDictionary();
                        break;
                    case KVMetaUpdateStrategy.MergeAll:
                        if (KVMeta.HasItem())
                        {
                            kvMeta.KVMeta.Merge(KVMeta, true);
                        }
                        break;
                    case KVMetaUpdateStrategy.MergeNew:
                        if (KVMeta.HasItem())
                        {
                            kvMeta.KVMeta.Merge(KVMeta, false);
                        }
                        break;
                }
            }
        }
    }
}