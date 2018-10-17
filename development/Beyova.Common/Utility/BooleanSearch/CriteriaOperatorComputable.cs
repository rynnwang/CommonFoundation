using Newtonsoft.Json.Linq;

namespace Beyova.BooleanSearch
{
    /// <summary>
    /// Class CriteriaOperatorComputable.
    /// </summary>
    public class CriteriaOperatorComputable : ICriteriaOperatorComputable
    {
        /// <summary>
        /// Gets or sets the item1.
        /// </summary>
        /// <value>
        /// The item1.
        /// </value>
        public string ItemLeft { get; set; }

        /// <summary>
        /// Gets or sets the item2.
        /// </summary>
        /// <value>
        /// The item2.
        /// </value>
        public string ItemRight { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        public ComputeOperator Operator { get; set; }

        /// <summary>
        /// Computes the specified json.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns><c>true</c> if compute as true, <c>false</c> otherwise.</returns>
        public bool Compute(JObject json)
        {
            return BooleanSearchCore.BooleanCompute(this, json);
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns><c>true</c> if validation passed, <c>false</c> otherwise.</returns>
        public bool Validate()
        {
            return BooleanSearchCore.IsKeyValid(this);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return (!string.IsNullOrWhiteSpace(ItemLeft) && !string.IsNullOrWhiteSpace(ItemRight))
                ? string.Format(BooleanSearchCore.expressionFormat, BooleanSearchCore.StringFieldToExpressionString(ItemLeft), Operator.ToString(), BooleanSearchCore.StringFieldToExpressionString(ItemRight))
                : string.Empty;
        }
    }
}