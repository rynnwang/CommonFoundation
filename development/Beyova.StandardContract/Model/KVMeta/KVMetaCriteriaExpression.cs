using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public class KVMetaCriteriaExpression : IKVMetaCriteriaExpression
    {
        /// <summary>
        /// Gets or sets the item left.
        /// </summary>
        /// <value>
        /// The item left.
        /// </value>
        [JsonProperty("itemLeft")]
        public string ItemLeft { get; set; }

        /// <summary>
        /// Gets or sets the item right.
        /// </summary>
        /// <value>
        /// The item right.
        /// </value>
        [JsonProperty("itemRight")]
        public JValue ItemRight { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        [JsonProperty("operator")]
        public string Operator { get; set; }

        /// <summary>
        /// Computes the specified json.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Compute(JObject json)
        {
            return json.Compute(ItemLeft, Operator, ItemRight);
        }
    }
}