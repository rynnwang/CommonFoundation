using Newtonsoft.Json.Linq;

namespace Beyova.Web
{
    /// <summary>
    ///
    /// </summary>
    public static class ModelExtension
    {
        /// <summary>
        /// Standardizes web object. It would scan first level properties, and reset value to <c>null</c> if type is null or empty string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public static T StandardizeWebObject<T>(this T criteria)
            where T : class
        {
            if (criteria != null)
            {
                var jsonObject = JObject.FromObject(criteria);
                foreach (JProperty item in jsonObject.Properties())
                {
                    if (item.Value.Type == JTokenType.String)
                    {
                        var stringValue = item.Value.ToObject<string>();
                        item.Value = string.IsNullOrWhiteSpace(stringValue) ? null : stringValue.Trim();
                    }
                }

                return jsonObject.ToObject<T>();
            }

            return null;
        }
    }
}