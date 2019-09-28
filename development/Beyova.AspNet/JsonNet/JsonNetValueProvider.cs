using Newtonsoft.Json.Linq;
using System;
using System.Web.Mvc;

namespace Beyova.Web
{
    /// <summary>
    ///
    /// </summary>
    public class JsonNetValueProvider : IValueProvider
    {
        /// <summary>
        /// The json token
        /// </summary>
        private JToken _jsonToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetValueProvider" /> class.
        /// </summary>
        /// <param name="jsonToken">The json token.</param>
        public JsonNetValueProvider(JToken jsonToken)
        {
            _jsonToken = jsonToken;
        }

        /// <summary>
        /// Determines whether the specified prefix contains prefix.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns>
        ///   <c>true</c> if the specified prefix contains prefix; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsPrefix(string prefix)
        {
            return true;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public ValueProviderResult GetValue(string key)
        {
            var result = (String.IsNullOrWhiteSpace(key) ? null : _jsonToken.SelectToken(key)) ?? _jsonToken;
            return new JsonNetValueProviderResult(result, key, null);
        }
    }
}