using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    /// Class GlobalCultureResource.
    /// </summary>
    internal class GlobalCultureResource
    {
        /// <summary>
        /// Gets or sets the resource.
        /// </summary>
        /// <value>The resource.</value>
        public string Resource { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [JsonConverter(typeof(EnumStringConverter))]
        public GlobalCultureResourceType Type { get; set; }
    }
}