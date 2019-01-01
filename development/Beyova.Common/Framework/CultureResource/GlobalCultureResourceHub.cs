using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class GlobalResourceCollection. Only resources defined via <see cref="BeyovaCultureResourceAttribute"/> would be provided here.
    /// </summary>
    public sealed class GlobalCultureResourceHub : II18NResourceOperatable
    {
        /// <summary>
        /// The culture based resources
        /// </summary>
        internal Dictionary<CultureInfo, GlobalCultureResourceCollection> _cultureBasedResources = new Dictionary<CultureInfo, GlobalCultureResourceCollection>();

        /// <summary>
        /// Gets or sets the default culture information.
        /// </summary>
        /// <value>The default culture information.</value>
        public CultureInfo DefaultCultureInfo { get; internal set; }

        /// <summary>
        /// Prevents a default instance of the <see cref="GlobalCultureResourceHub"/> class from being created.
        /// </summary>
        internal GlobalCultureResourceHub()
        {
        }

        /// <summary>
        /// Determines whether [has culture resource] [the specified culture information].
        /// </summary>
        /// <param name="cultureInfo">The culture information.</param>
        /// <returns><c>true</c> if [has culture resource] [the specified culture information]; otherwise, <c>false</c>.</returns>
        public bool HasCultureResource(CultureInfo cultureInfo)
        {
            return cultureInfo != null && _cultureBasedResources.ContainsKey(cultureInfo);
        }

        /// <summary>
        /// Internals the culture thing.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="input">The input.</param>
        /// <param name="cultureInfo">The culture information.</param>
        /// <param name="languageCompatibility">if set to <c>true</c> [language compatibility].</param>
        /// <param name="delegateFunc">The delegate function.</param>
        /// <returns></returns>
        public TOutput InternalCultureThing<TInput, TOutput>(TInput input, CultureInfo cultureInfo, bool languageCompatibility, Func<GlobalCultureResourceCollection, TInput, TOutput> delegateFunc)
        {
            cultureInfo = cultureInfo ?? Framework.CurrentCultureInfo ?? DefaultCultureInfo;

            if (cultureInfo == null) return default(TOutput);

            GlobalCultureResourceCollection hitCollection = null;
            if (!_cultureBasedResources.TryGetValue(cultureInfo, out hitCollection))
            {
                if (languageCompatibility)
                {
                    if (cultureInfo.Name.Contains("-"))
                    {
                        var parentCultureInfo = cultureInfo?.Name.SubStringBeforeFirstMatch('-').AsCultureInfo();

                        if (parentCultureInfo != null)
                        {
                            _cultureBasedResources.TryGetValue(parentCultureInfo, out hitCollection);
                        }
                    }
                }

                if (hitCollection == null && DefaultCultureInfo != null)
                {
                    _cultureBasedResources.TryGetValue(DefaultCultureInfo, out hitCollection);
                }
            }

            if (hitCollection != null && delegateFunc != null)
            {
                return delegateFunc.Invoke(hitCollection, input);
            }

            return default(TOutput);
        }

        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="typeRequired">The type required.</param>
        /// <param name="cultureInfo">The culture information.</param>
        /// <param name="languageCompatibility">The language compatibility.</param>
        /// <returns>
        /// System.Object.
        /// </returns>
        public string GetResourceString(string resourceKey, GlobalCultureResourceType? typeRequired = null, CultureInfo cultureInfo = null, bool languageCompatibility = true)
        {
            return string.IsNullOrWhiteSpace(resourceKey) ? null : InternalCultureThing(new Tuple<string, GlobalCultureResourceType?>(resourceKey, typeRequired), cultureInfo, languageCompatibility, (coll, p) =>
               {
                   return coll?.GetResourceByKey(p?.Item1, p?.Item2);
               }).SafeToString(resourceKey);
        }

        /// <summary>
        /// Gets the available culture information.
        /// </summary>
        /// <value>The available culture information.</value>
        public ICollection<CultureInfo> AvailableCultureInfo
        {
            get
            {
                return _cultureBasedResources.Keys;
            }
        }

        /// <summary>
        /// Gets the json.
        /// </summary>
        /// <param name="cultureInfo">The culture information.</param>
        /// <param name="languageCompatibility">if set to <c>true</c> [language compatibility].</param>
        /// <returns></returns>
        public JToken GetJson(CultureInfo cultureInfo = null, bool languageCompatibility = true)
        {
            return InternalCultureThing<string, JToken>(null, cultureInfo, languageCompatibility, (coll, p) =>
             {
                 return coll?.ToJson();
             });
        }
    }
}