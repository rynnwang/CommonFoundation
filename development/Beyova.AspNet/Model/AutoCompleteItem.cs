using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Beyova.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class AutoCompleteItem
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Froms the friendly identifier.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static AutoCompleteItem FromFriendlyIdentifier(FriendlyIdentifier item)
        {
            return item == null ? null : new AutoCompleteItem
            {
                Text = item.Name,
                Value = item.Key.ToString()
            };
        }

        /// <summary>
        /// Froms the friendly identifier.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static List<AutoCompleteItem> FromFriendlyIdentifier(List<FriendlyIdentifier> items)
        {
            return items.ConvertAll(FromFriendlyIdentifier);
        }
    }
}