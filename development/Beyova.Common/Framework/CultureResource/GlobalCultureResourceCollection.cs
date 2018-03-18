using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Beyova
{
    /// <summary>
    /// Class GlobalResourceCollection. Only resources defined via <see cref="BeyovaCultureResourceAttribute"/> would be provided here.
    /// </summary>
    public sealed class GlobalCultureResourceCollection : II18NResourceCollection
    {
        /// <summary>
        /// The culture based resources
        /// </summary>
        internal Dictionary<CultureInfo, Dictionary<string, GlobalCultureResource>> cultureBasedResources = new Dictionary<CultureInfo, Dictionary<string, GlobalCultureResource>>();

        /// <summary>
        /// Gets or sets the default culture information.
        /// </summary>
        /// <value>The default culture information.</value>
        public CultureInfo DefaultCultureInfo { get; internal set; }

        /// <summary>
        /// Prevents a default instance of the <see cref="GlobalCultureResourceCollection"/> class from being created.
        /// </summary>
        internal GlobalCultureResourceCollection()
        {
        }

        /// <summary>
        /// Determines whether [has culture resource] [the specified culture information].
        /// </summary>
        /// <param name="cultureInfo">The culture information.</param>
        /// <returns><c>true</c> if [has culture resource] [the specified culture information]; otherwise, <c>false</c>.</returns>
        public bool HasCultureResource(CultureInfo cultureInfo)
        {
            return cultureInfo != null && cultureBasedResources.ContainsKey(cultureInfo);
        }

        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="cultureInfo">The culture information.</param>
        /// <param name="languageCompatibility">The language compatibility.</param>
        /// <returns>
        /// System.Object.
        /// </returns>
        public string GetResourceString(string resourceKey, CultureInfo cultureInfo = null, bool languageCompatibility = true)
        {
            if (string.IsNullOrWhiteSpace(resourceKey))
            {
                return null;
            }

            cultureInfo = cultureInfo ?? ContextHelper.CurrentCultureInfo ?? DefaultCultureInfo;

            if (cultureInfo == null) return string.Empty;

            Dictionary<string, GlobalCultureResource> hitDictionary = null;
            if (!cultureBasedResources.TryGetValue(cultureInfo, out hitDictionary))
            {
                if (languageCompatibility)
                {
                    if (cultureInfo.Name.Contains("-"))
                    {
                        var parentCultureInfo = cultureInfo?.Name.SubStringBeforeFirstMatch('-').AsCultureInfo();

                        if (parentCultureInfo != null)
                        {
                            cultureBasedResources.TryGetValue(parentCultureInfo, out hitDictionary);
                        }
                    }
                }

                if (hitDictionary == null)
                {
                    cultureBasedResources.TryGetValue(DefaultCultureInfo, out hitDictionary);
                }
            }

            if (hitDictionary != null)
            {
                GlobalCultureResource result;
                return hitDictionary.TryGetValue(resourceKey, out result) ? result?.Resource : null;
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the available culture information.
        /// </summary>
        /// <value>The available culture information.</value>
        public ICollection<CultureInfo> AvailableCultureInfo
        {
            get
            {
                return cultureBasedResources.Keys;
            }
        }
    }
}