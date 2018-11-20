using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class GlobalCultureResourceCollection
    /// </summary>
    public sealed class GlobalCultureResourceCollection
    {
        /// <summary>
        /// The resources
        /// </summary>
        internal Dictionary<string, GlobalCultureResource> _resources = new Dictionary<string, GlobalCultureResource>();

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public string Source { get; private set; }

        /// <summary>
        /// Prevents a default instance of the <see cref="GlobalCultureResourceHub" /> class from being created.
        /// </summary>
        /// <param name="source">The source.</param>
        internal GlobalCultureResourceCollection(string source)
        {
            this.Source = source;
        }

        /// <summary>
        /// Gets the resource by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="typeRequired">The type required.</param>
        /// <returns></returns>
        public string GetResourceByKey(string key, GlobalCultureResourceType? typeRequired = null)
        {
            GlobalCultureResource outValue = null;
            if (!string.IsNullOrWhiteSpace(key) && _resources.TryGetValue(key, out outValue))
            {
                if ((!typeRequired.HasValue || typeRequired.Value == outValue.Type))
                {
                    return outValue.Resource;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// To the json.
        /// </summary>
        /// <returns></returns>
        public JToken ToJson()
        {
            return _resources.ToJson(false);
        }
    }
}